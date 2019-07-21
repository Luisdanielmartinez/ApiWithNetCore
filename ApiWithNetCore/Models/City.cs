
namespace ApiWithNetCore.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class City
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string name { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [JsonIgnore]
        public Country country { get; set; }
    }
}
