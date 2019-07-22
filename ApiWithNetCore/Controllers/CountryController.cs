

namespace ApiWithNetCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ApiWithNetCore.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Produces("application/json")]
    [Route("api/Country")]
    //se le agrega la autenticacion
    [Authorize (AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class CountryController : Controller
    {
        //aqui estamos usando inyecciones de dependencia para traer el la db
        private readonly ApplicationDbContext dbContext;
        public CountryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        //metodo get api/country
        [HttpGet]
        public IEnumerable<Country> Get()
        {
            return dbContext.Countries.ToList();
        }

        //metodo get con id
        [HttpGet("{Id}", Name ="Creado")]

        public ActionResult GetById(int Id) {
            var Country = dbContext.Countries.Include(x => x.Cities).FirstOrDefault(x => x.Id == Id);

            if (Country==null) {
                return NotFound();
            }
            return Ok(Country);
        }
        //metodo para insertar un pais
        [HttpPost]
        public ActionResult Post([FromBody] Country country)
        {
            if (ModelState.IsValid) {
                dbContext.Countries.Add(country);
                dbContext.SaveChanges();

                return new CreatedAtRouteResult("Creado", new { id = country.Id });
            }
            return BadRequest(ModelState);
        }
        //metodo para modificar county

        [HttpPut("{Id}")]

        public ActionResult put([FromBody] Country country, int Id)
        {
            if (country.Id!=Id)
            {
                return BadRequest();
            }
            dbContext.Entry(country).State = EntityState.Modified;
            dbContext.SaveChanges();
            return Ok();
        }
        //metodo de remove 

        [HttpDelete ("{Id}")]

        public ActionResult delete(int Id)
        {
            var country = dbContext.Countries.FirstOrDefault(x=>x.Id==Id);
            if (country==null)
            {
                return NotFound();
            }
            dbContext.Countries.Remove(country);
            dbContext.SaveChanges();
            return Ok(country);
        }

    }
}