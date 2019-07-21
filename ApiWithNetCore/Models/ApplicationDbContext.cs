

namespace ApiWithNetCore.Models
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    //esta es la clase donde crearemos nuestras tablas 
    public class ApplicationDbContext:DbContext
    {
        //aqui estamos creando la tabla 
       public  DbSet<Country> Countries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }
    }
}
