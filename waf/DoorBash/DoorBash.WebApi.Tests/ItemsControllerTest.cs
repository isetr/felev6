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
    public class ItemsControllserTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public ItemsControllserTest(ServerClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Test_PostItem_ShouldAddItem()
        {
            // Arrange
            Item item = new Item()
            {
                Name = "Test",
                CategoryID = 1,
                Description = "asd",
                Hot = true,
                Vegan = false,
                Price = 650
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PostAsync("api/Items/New", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(_fixture.Context.Items.FirstOrDefault(i => i.Name == "Test"));
        }

        [Fact]
        public async void Test_PostItem_DuplicateItem()
        {
            // Arrange
            Item item = new Item()
            {
                Name = "Test",
                CategoryID = 1,
                Description = "asd",
                Hot = true,
                Vegan = false,
                Price = 650
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PostAsync("api/Items/New", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void Test_PostItem_BadItem()
        {
            // Arrange
            Item item = new Item()
            {
                CategoryID = 1,
                Description = "asd",
                Hot = true
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PostAsync("api/Items/New", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
