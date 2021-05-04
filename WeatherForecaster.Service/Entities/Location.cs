using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecaster.Service
{
    public class Location
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        public double Latitude { get; set; }    
        public double Longitude { get; set; }

        public virtual ICollection<Forecast> Forecasts { get; set; }
    }
}
