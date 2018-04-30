using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DoorBash.Persistence;
using DoorBash.WebSite.Services;
using DoorBash.WebSite.Models;

namespace DoorBash.WebSite.Controllers
{
    public class CartController : Controller
    {
        private readonly DoorBashServices doorBashServices;

        public CartController(DoorBashServices services)
        {
            doorBashServices = services;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            var price = HttpContext.Session.GetObjectFromJson<int>("Price");
            if (cart == null)
            {
                cart = new List<Item>();
                price = 0;
            }
            ViewBag.Price = price;
            return View(cart);
        }

        public IActionResult Order()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            var price = HttpContext.Session.GetObjectFromJson<int>("Price");
            if (cart == null)
            {
                cart = new List<Item>();
                price = 0;
            }
            ViewBag.Price = price;
            ViewBag.Items = cart;
            return View();
        }

        [HttpPost, ActionName("Order")]
        [ValidateAntiForgeryToken]
        public IActionResult OrderConfirmed(OrderViewModel order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            var price = HttpContext.Session.GetObjectFromJson<int>("Price");
            if (cart == null)
                RedirectToAction(nameof(Index));
            ViewBag.Price = price;
            ViewBag.Items = cart;

            if (ModelState.IsValid)
            {
                Order ord =
                    new Persistence.Order
                    {
                        Name = order.FullName,
                        Address = order.Address,
                        Phone = order.Phone,
                        Done = false
                    };

                var result = doorBashServices.CreateOrder(ord, cart);

                if (result == DoorBashServices.DoorBashUpdateResult.Success)
                { 
                    HttpContext.Session.SetObjectAsJson("Cart", new List<Item>());
                    HttpContext.Session.SetObjectAsJson("Price", 0);

                    return RedirectToAction(nameof(HomeController.Index));
                }

                ModelState.AddModelError("", "Error while connecting to the database. Please try again!");
            }

            return View(order);
        }

        public IActionResult CancelOrder()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            var price = HttpContext.Session.GetObjectFromJson<int>("Price");
            if (cart == null)
            {
                cart = new List<Item>();
                price = 0;
            }
            ViewBag.Price = price;
            return View(cart);
        }

        [HttpPost, ActionName("CancelOrder")]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrderConfirmed()
        {
            HttpContext.Session.SetObjectAsJson("Cart", new List<Item>());
            HttpContext.Session.SetObjectAsJson("Price", 0);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveItem(int? id)
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


        [HttpPost, ActionName("RemoveItem")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItemConfirmed(int id)
        {
            var item = doorBashServices.GetItemById(id);
            var cart = HttpContext.Session.GetObjectFromJson<List<Item>>("Cart");
            var price = HttpContext.Session.GetObjectFromJson<int>("Price");

            if (cart == null)
            {
                return RedirectToAction(nameof(Index));
            }
            
            HttpContext.Session.SetObjectAsJson("Cart", cart.Where(i => i.Id != item.Id));
            HttpContext.Session.SetObjectAsJson("Price", price - item.Price);

            return RedirectToAction(nameof(Index));
        }
    }
}