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
    [Route("api/Items")]
    public class ItemsController : Controller
    {
        private readonly DoorBashDbContext context;

        public ItemsController(DoorBashDbContext context)
        {
            this.context = context;
        }

        // POST: api/Items
        [HttpPost("[action]")]
        public IActionResult New([FromBody] ItemDto item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (context.Items.Any(i => i.Name.Equals(item.Name)))
                        return BadRequest();

                    var newItem = new Item()
                    {
                        CategoryID = item.CategoryID,
                        Description = item.Description,
                        Hot = item.Hot,
                        Name = item.Name,
                        Price = item.Price,
                        Vegan = item.Vegan
                    };

                    context.Items.Add(newItem);

                    context.SaveChanges();
                }
                else
                {
                    return BadRequest();
                }
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
