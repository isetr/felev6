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
    [Route("api/Category")]
    public class CategoryController : Controller
    {
        private readonly DoorBashDbContext context;

        public CategoryController(DoorBashDbContext context)
        {
            this.context = context;
        }
        
        // GET: api/Category
        [HttpGet]
        public IEnumerable<Category> GetLists()
        {
            return context.Categories;
        }
    }
}
