using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecaster.Service;
using WeatherForecaster.Service.DAL;
using WeatherForecaster.Service.Models;
using WeatherForecaster.Service.Services;
using Xunit;

namespace WeatherForecaster.Tests
{
    public class LocationTestsMoq
    {
        ILocationService LocationService;
        AppDbContext Context;

        public LocationTestsMoq()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            options.Freeze();
            Context = new AppDbContext(options);
        }

        private void PopulateContextWithLocations()
        {
            Context.Locations.AddRange(TestData.AllLocations());
            Context.SaveChanges();

            LocationService = new LocationService(Context);
        }

        private void EmptyLocationsFromContext()
        {
            LocationService = new LocationService(Context);
        }

        [Fact]
        public async Task GetLocations_AllTestDataCount_Should_Return6()
        {
            PopulateContextWithLocations();

            var actual = await LocationService.GetLocationsAsync();

            Assert.Equal(TestData.AllLocations().Count, actual.ToList().Count);
        }

        [Fact]
        public async Task GetLocations_AllTestData_Should_ReturnAllLocations()
        {
            PopulateContextWithLocations();

            var actual = await LocationService.GetLocationsAsync();

            Assert.Equal(TestData.AllLocations(), actual.ToList());
        }

        [Fact]
        public async Task GetLocation_Should_ReturnLocation4With0Forecasts()
        {
            PopulateContextWithLocations();

            var expected = new Location()
            {
                Id = 4,
                Name = "Location4",
                Latitude = -12.3406789,
                Longitude = -21.34567891,
                Forecasts = new List<Forecast>(),
            };

            var actual = await LocationService.GetLocationAsync(expected.Id);
            //Assert.Equal(expected, actual);

            var expectedSerialized = JsonConvert.SerializeObject(expected);
            var actualSerialized = JsonConvert.SerializeObject(actual);

            Assert.Equal(expectedSerialized, actualSerialized);
        }


        [Fact]
        public async Task GetLocation_Should_ReturnLocation2NoLat()
        {
            PopulateContextWithLocations();

            var expected = new Location()
            {
                Id = 2,
                Name = "Location2",
                Longitude = -21.04567891,
                Forecasts = new List<Forecast>()
            };

            var actual = await LocationService.GetLocationAsync(expected.Id);

            var expectedSerialized = JsonConvert.SerializeObject(expected);
            var actualSerialized = JsonConvert.SerializeObject(actual);

            Assert.Equal(expectedSerialized, actualSerialized);
        }

        [Fact]
        public async Task CreateLocation_Should_ReturnLocationWithId1()
        {
            EmptyLocationsFromContext();

            var newLocation = new Location()
            {
                Name = "New Location",
                Latitude = -21.04567891,
                Longitude = -21.34567891,
                Forecasts = null
            };

            var mockLocationService = new Mock<LocationService>(Context) { CallBase = true };

            mockLocationService.Setup(x => x.GetForecastsAsync(It.IsAny<double>(), It.IsAny<double>()))
                .Returns((double i, double j) => Task.FromResult(TestData.GetOpenWeatherRootData()));

            LocationService = mockLocationService.Object;
            var actual = await LocationService.CreateLocationAsync(newLocation);

            Assert.NotNull(actual);
            Assert.Equal(1, actual.Id);

        }

        [Fact]
        public async Task CreateLocation_Should_ThrowInvalidOperationExceptionWhenLocationAlreadyExists()
        {
            PopulateContextWithLocations();

            var newLocation = new Location()
            {
                Id = 1,
                Name = "New Location",
                Latitude = -21.04567891,
                Longitude = -21.34567891,
                Forecasts = null
            };

            var mockLocationService = new Mock<LocationService>(Context) { CallBase = true };

            mockLocationService.Setup(x => x.GetForecastsAsync(It.IsAny<double>(), It.IsAny<double>()))
                .Returns((double i, double j) => Task.FromResult(TestData.GetOpenWeatherRootData()));

            LocationService = mockLocationService.Object;

            await Assert.ThrowsAsync<InvalidOperationException>(() => LocationService.CreateLocationAsync(newLocation));
        }

        [Fact]
        public async Task UpdateLocation_Should_UpdateLocationDetails()
        {
            PopulateContextWithLocations();
            var location3 = Context.Locations.FirstOrDefault(x => x.Id == 3);
            location3.Name = "Updated Name";
            location3.Latitude = 0.123;
            location3.Longitude = 0.456;

            await LocationService.UpdateLocationAsync(location3);

            var location3Actual = Context.Locations.AsNoTracking().FirstOrDefault(x => x.Id == 3);


            Assert.Equal(location3.Name, location3Actual.Name);
            Assert.Equal(location3.Latitude, location3Actual.Latitude);
            Assert.Equal(location3.Longitude, location3Actual.Longitude);
        }


        [Fact]
        public async Task UpdateLocation_Should_ThrowIfLocationDoesNotExist()
        {
            PopulateContextWithLocations();

            var newLocation = new Location()
            {
                Id = 101,
                Name = "New Location",
                Latitude = -21.04567891,
                Longitude = -21.34567891,
                Forecasts = null
            };

            await Assert.ThrowsAsync<Exception>(() => LocationService.UpdateLocationAsync(newLocation));            
        }

        [Fact]
        public async Task DeleteLocation_Should_DeleteLocationFromContext()
        {
            PopulateContextWithLocations();

            var location2 = Context.Locations.FirstOrDefault(x => x.Id == 2);

            await LocationService.DeleteLocationAsync(location2.Id);

            var location2Actual = Context.Locations.FirstOrDefault(x => x.Id == 2);

            Assert.Null(location2Actual);
        }

        [Fact]
        public async Task DeleteLocation_Should_ThrowIfLocationDoesNotExist()
        {
            PopulateContextWithLocations();

            await Assert.ThrowsAsync<Exception>(() => LocationService.DeleteLocationAsync(101));
        }

    }
}
