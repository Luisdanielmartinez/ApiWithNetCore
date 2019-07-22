
namespace ApiWithNetCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using ApiWithNetCore.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly IConfiguration _configuration;
        public AccountController(
           UserManager<ApplicationUser> UserManager,
            SignInManager<ApplicationUser> SignInManager,
            IConfiguration configuration)
        {
            _UserManager=UserManager;
            _SignInManager = SignInManager;
            _configuration = configuration;
        }
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        {
            if (ModelState.IsValid) {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return BuildToken(model);
                }
                else {
                    return BadRequest("UserName or Password Invalid");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route ("Login")]
        public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _SignInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(userInfo);
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Invalid login attemp");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        private IActionResult BuildToken(UserInfo userInfo) {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,userInfo.Email), 
                //esto es para general un token unico
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Llave_super_secreta"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //aqui vamos a crear el token 
            //aqui es cuando vamos a poner que expire el token
            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "yourdomain.com",//esta es la entidad que esta imitiendo el token
                audience: "yourdomain.com",
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration=expiration
            });
        }
    }
}