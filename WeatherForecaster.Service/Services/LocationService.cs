using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherForecaster.Service.DAL;
using WeatherForecaster.Service.Helpers;
using WeatherForecaster.Service.Models;

namespace WeatherForecaster.Service.Services
{

    public class LocationService : ILocationService
    {

        private readonly IOptions<CustomConfig> _config;
        private readonly AppDbContext _context;

        public LocationService(AppDbContext context, IOptions<CustomConfig> config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            var locations = await _context.Locations
                .Include(x => x.Forecasts)
                .ToListAsync();

            foreach (var loc in locations)
            {
                loc.Forecasts = loc.Forecasts.OrderBy(x => x.DateTime).ToList();
            }

            return locations;
        }

        public async Task<Location> GetLocationAsync(int id)
        {
            return await _context.Locations
                .Include(x => x.Forecasts)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            location.Name = TruncateString(location.Name, 200);

            // call weather app and save data
            var forecasts = await GetForecastsAsync(location.Latitude, location.Longitude);

            location.Forecasts = new List<Forecast>();
            foreach (var v in forecasts.list.OrderBy(x => x.dt))
            {
                location.Forecasts.Add(new Forecast()
                {
                    DateTime = DateTimeOffset.FromUnixTimeSeconds(v.dt).DateTime,
                    Min = v.main.temp_min,
                    Max = v.main.temp_max,
                    FeelsLike = v.main.feels_like,
                    Icon = v.weather.FirstOrDefault()?.icon
                });
            }

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return location;
        }

        public async Task UpdateLocationAsync(Location location)
        {
            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("ERROR - Location NOT updated, please ensure it exists and try again.");
            }
        }

        public async Task DeleteLocationAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                throw new Exception("ERROR - Location NOT found");
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<OpenWeatherRoot> GetForecastsAsync(double latitude, double longitude)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.openweathermap.org");
                var response = await client.GetAsync($"/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={_config?.Value?.OpenWeatherMap_API_Key}&units=metric");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                dynamic d = JsonConvert.DeserializeObject(json);

                OpenWeatherRoot deserialized = JsonConvert.DeserializeObject<OpenWeatherRoot>(json);
                return deserialized;
            }
        }

        private string TruncateString(string input, int maxLength)
        {
            if (input.Length <= maxLength) return input;
            return input.Substring(0, maxLength);
        }

        public Task UpdateForecastsAsync(int locationId)
        {
            //TODO ... Everyday, the scheduler should update any stored locations forecasts for the next 7 days
            throw new NotImplementedException();
        }
    }
}
