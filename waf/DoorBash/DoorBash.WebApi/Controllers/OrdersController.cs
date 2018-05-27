using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;

namespace DoorBash.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController : Controller
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
            var res = context.Orders
                .OrderBy(l => l.Sent)
                .Where(o => o.Name.Contains(name ?? "") && o.Address.Contains(address ?? ""));

            switch (flag)
            {
                case 1:
                    return res.Where(o => o.Done);
                case 2:
                    return res.Where(o => !o.Done);
                default:
                    return res;
            }
        }

        // GET: api/Orders/Items/5
        [HttpGet("Items/{id}")]
        public IEnumerable<Item> Items(int id)
        {
            return context.Orders.Where(o => o.Id == id).SelectMany(o => o.Items).Select(o => o.Item);
        }

        // POST: api/Orders/Finish
        [HttpPost("[action]")]
        public IActionResult Finish([FromBody] int id)
        {
            try
            {
                var order = context.Orders.Where(o => o.Id == id).FirstOrDefault();
                if (order == null)
                    return NotFound();

                order.Done = true;
                order.Approved = System.DateTime.Now;

                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}
