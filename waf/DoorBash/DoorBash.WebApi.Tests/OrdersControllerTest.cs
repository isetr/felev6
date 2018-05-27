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
    public class OrdersControllserTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public OrdersControllserTest(ServerClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Test_GetListItems_ReturnsAllOrders()
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Orders/?flag=0");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Order>>(responseString);

            Assert.NotNull(responseObject);
            Assert.False(responseObject.Any());
        }

        [Theory]
        [InlineData(1)]
        public async void Test_GetListItems_ReturnsAllItemsInOrder(int id)
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Orders/Items/" + id);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Item>>(responseString);

            Assert.NotNull(responseObject);
            Assert.False(responseObject.Any());
            Assert.Equal(_fixture.Context.OrderItem.Where(o => o.OrderID == id).Count(),responseObject.Count());
        }
    }

}
