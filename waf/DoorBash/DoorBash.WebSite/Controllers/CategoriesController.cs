using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DoorBash.Persistence;
using DoorBash.WebSite.Services;

namespace DoorBash.WebSite.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DoorBashServices doorBashServices;

        public CategoriesController(DoorBashServices services)
        {
            doorBashServices = services;
        }

        public IActionResult Index()
        {
            return View(doorBashServices.GetCategories());
        }

        public IActionResult Category(int? id, string searchString)
        {
            if(id == null)
            {
                return BadRequest();
            }

            var category = doorBashServices.GetCategoryById((int)id);
            if(category == null)
            {
                return NotFound();
            }

            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                category.Items =
                    category.Items
                        .Where(i => i.Name.Contains(searchString))
                        .ToList();
            }
            return View(category);
        }

        public IActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var item = doorBashServices.GetItemById((int)id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("AddToCart")]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCartConfirmed(int id)
        {
            var item = doorBashServices.GetItemById((int)id);
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            var price = HttpContext.Session.GetObjectFromJson<int>("Price");

            if(cart == null)
            {
                cart = new List<Item>();
                price = 0;
            }

            if (price + item.Price <= 20000)
            {
                cart.Add(item);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                HttpContext.Session.SetObjectAsJson("Price", price + item.Price);
            }

            return RedirectToAction(nameof(CartController.Index));
        }
    }
}