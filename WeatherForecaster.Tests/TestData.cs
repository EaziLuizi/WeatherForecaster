using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecaster.Service;
using WeatherForecaster.Service.Models;

namespace WeatherForecaster.Tests
{
    public class TestData
    {
        public static Location Location1NoForecasts = new Location()
        {
            Id = 1,
            Name = "Location1",
            Latitude = 12.3456789,
            Longitude = 21.34567891,
            Forecasts = null
        };

        public static Location Location2NoLat = new Location()
        {
            Id = 2,
            Name = "Location2",
            Longitude = -21.04567891,
            Forecasts = null
        };

        public static Location Location3NoLon = new Location()
        {
            Id = 3,
            Name = "Location3",
            Latitude = 12.3056789,
        };

        public static Location Location4With0Forecasts = new Location()
        {
            Id = 4,
            Name = "Location4",
            Latitude = -12.3406789,
            Longitude = -21.34567891,
            Forecasts = new List<Forecast>(),
        };

        public static Location Location5With3DayForecasts = new Location()
        {
            Id = 5,
            Name = "Location5",
            Latitude = 12.3456789,
            Forecasts = new List<Forecast>()
            {
                new Forecast(){ Id = 1, DateTime = new DateTime(2021, 01, 01), Min = 11, FeelsLike = 16, Max = 21, LocationId = 5},
                new Forecast(){ Id = 2, DateTime = new DateTime(2021, 01, 02), Min = 12, FeelsLike = 17, Max = 22, LocationId = 5},
                new Forecast(){ Id = 3, DateTime = new DateTime(2021, 01, 03), Min = 13, FeelsLike = 18, Max = 23, LocationId = 5},
            },
        };

        public static Location Location6With5DayForecasts = new Location()
        {
            Id = 6,
            Name = "Location6",
            Latitude = 12.3456789,
            Forecasts = new List<Forecast>()
            {
                new Forecast(){ Id = 11, DateTime = new DateTime(2021, 01, 01), Min = 11, FeelsLike = 16, Max = 21, LocationId = 6},
                new Forecast(){ Id = 12, DateTime = new DateTime(2021, 01, 02), Min = 12, FeelsLike = 17, Max = 22, LocationId = 6},
                new Forecast(){ Id = 13, DateTime = new DateTime(2021, 01, 03), Min = 13, FeelsLike = 18, Max = 23, LocationId = 6},
                new Forecast(){ Id = 14, DateTime = new DateTime(2021, 01, 04), Min = 14, FeelsLike = 19, Max = 24, LocationId = 6},
                new Forecast(){ Id = 15, DateTime = new DateTime(2021, 01, 05), Min = 15, FeelsLike = 20, Max = 24, LocationId = 6},
            },
        };

        public static List<Location> AllLocations()
        {
            var locations = new List<Location>();
            locations.Add(Location1NoForecasts);
            locations.Add(Location2NoLat);
            locations.Add(Location3NoLon);
            locations.Add(Location4With0Forecasts);
            locations.Add(Location5With3DayForecasts);
            locations.Add(Location6With5DayForecasts);

            return locations;
        }

        public static OpenWeatherRoot GetOpenWeatherRootData()
        {
            var root = new OpenWeatherRoot()
            {
                cod = "200",
                list = new List<List>()
                {

                },               
            };

            root.list.Add(new List()
            {
                main = new Main()
                {

                },
                weather = new List<Weather>() { },
            });


            return root;
        }
    }
}
