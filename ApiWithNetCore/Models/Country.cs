

namespace ApiWithNetCore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    public class Country
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        //aqui estamos enlazando las proviencias con sus repetivas ciudades
        public List<City> Cities { get; set; }
        public Country()
        {
            Cities = new List<City>();
        }
    }
}
