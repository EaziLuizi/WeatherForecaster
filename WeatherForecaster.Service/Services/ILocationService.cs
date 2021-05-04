using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecaster.Service.Models;

namespace WeatherForecaster.Service.Services
{
    public interface ILocationService
    {
        public Task<IEnumerable<Location>> GetLocationsAsync();
        public Task<Location> GetLocationAsync(int id);
        public Task<Location> CreateLocationAsync(Location location);
        public Task UpdateLocationAsync(Location location);
        public Task DeleteLocationAsync(int id);

        public Task<OpenWeatherRoot> GetForecastsAsync(double latitude, double longitude);


        public Task UpdateForecastsAsync(int locationId);
    }
}
