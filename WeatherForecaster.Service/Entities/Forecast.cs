using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecaster.Service
{
    public class Forecast
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }
        public double FeelsLike { get; set; }
        [MaxLength(50)]
        public string Icon { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
