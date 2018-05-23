using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoorBash.Persistence;

namespace DoorBash.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController
    {
        private readonly DoorBashDbContext context;

        public OrdersController(DoorBashDbContext context)
        {
            this.context = context;
        }

        // GET: api/Orders/?flag=all?name=someone?address=somewhere
        [HttpGet]
        public IEnumerable<Order> GetLists([FromQuery] int flag, [FromQuery] string name, [FromQuery] string address)
        {
            return context.Orders.OrderBy(l => l.Sent);
        }

        // GET: api/Orders/Items/5
        [HttpGet("Items/{id}")]
        public IEnumerable<Item> Items([FromRoute] int id)
        {
            return context.Orders.Where(o => o.Id == id).SelectMany(o => o.Items).Select(o => o.Item);
        }

    }
}
