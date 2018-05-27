using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using DoorBash.Persistence;
using Xunit;


namespace DoorBash.WebApi.Tests
{
    public class CategoryControllserTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public CategoryControllserTest(ServerClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Test_GetAllCategories()
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Category");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Category>>(responseString);

            Assert.NotNull(responseObject);
            Assert.True(responseObject.Any());
        }
    }
}
