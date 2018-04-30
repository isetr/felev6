using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoorBash.Persistence;
using DoorBash.WebSite.Services;

namespace DoorBash.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoorBashServices doorBashServices;

        public HomeController(DoorBashServices services)
        {
            doorBashServices = services;
        }

        public IActionResult Index()
        {
            var items = doorBashServices.GetTopItems();

            return View(items);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
