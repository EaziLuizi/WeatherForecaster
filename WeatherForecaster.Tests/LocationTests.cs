using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecaster.Service;
using WeatherForecaster.Service.DAL;
using WeatherForecaster.Service.Services;
using Xunit;

namespace WeatherForecaster.Tests
{
    public class LocationTests
    {
        ILocationService LocationService;

        public LocationTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            options.Freeze();

            var context = new AppDbContext(options);

            context.Locations.AddRange(TestData.AllLocations());
            context.SaveChanges();

            LocationService = new LocationService(context);
        }

        #region Naming Conventions

        // MethodName_StateUnderTest_ExpectedResult
        // GetLocations_AllTestData_ReturnsAllLocations()         

        // Should_ExpectedBehavior_When_StateUnderTest
        // Should_ReturnAllTestData_When_Called


        #endregion

        [Fact]
        public async Task GetLocations_AllTestDataCount_Returns6()
        {
            var actual = await LocationService.GetLocationsAsync();

            Assert.Equal(TestData.AllLocations().Count, actual.ToList().Count);
        }


        [Fact]
        public async Task GetLocations_AllTestData_ReturnsAllLocations()
        {
            var actual = await LocationService.GetLocationsAsync();

            Assert.Equal(TestData.AllLocations(), actual.ToList());
        }

        [Fact]
        public async Task GetLocations_Should_ReturnLocation4With0Forecasts()
        {
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
    }
}
