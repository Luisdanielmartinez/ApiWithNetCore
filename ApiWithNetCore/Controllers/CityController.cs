using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWithNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWithNetCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Country/{CountryId}/City")]
    public class CityController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public CityController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        //metodo  get 
        [HttpGet]
        public IEnumerable<City> Get(int CityId)
        {
            return dbContext.Cities.Where(x=>x.Id==CityId).ToList();
        }

        //metodo get api/country
        [HttpGet]
        public IEnumerable<Country> Get()
        {
            return dbContext.Countries.ToList();
        }

        //metodo get con id
        [HttpGet("{Id}", Name = "CreadoCity")]

        public ActionResult GetById(int Id)
        {
            var City = dbContext.Cities.FirstOrDefault(x => x.Id == Id);

            if (City == null)
            {
                return NotFound();
            }
            return Ok(City);
        }
        //metodo para insertar un pais
        [HttpPost]
        public ActionResult Post([FromBody] City city)
        {
            if (ModelState.IsValid)
            {
                dbContext.Cities.Add(city);
                dbContext.SaveChanges();

                return new CreatedAtRouteResult("Creado", new { id = city.Id });
            }
            return BadRequest(ModelState);
        }
        //metodo para modificar county

        [HttpPut("{Id}")]

        public ActionResult put([FromBody] City city, int Id)
        {
            if (city.Id != Id)
            {
                return BadRequest();
            }
            dbContext.Entry(city).State = EntityState.Modified;
            dbContext.SaveChanges();
            return Ok();
        }
        //metodo de remove 

        [HttpDelete("{Id}")]

        public ActionResult delete(int Id)
        {
            var city = dbContext.Cities.FirstOrDefault(x => x.Id == Id);
            if (city == null)
            {
                return NotFound();
            }
            dbContext.Cities.Remove(city);
            dbContext.SaveChanges();
            return Ok(city);
        }
    }
}