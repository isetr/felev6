using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Web.Helpers;
using ZH.Persistence;
using ZH.Web.Services;
using ZH.Web.Models;

namespace ZH.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ZHServices services;

        public HomeController(ZHServices services)
        {
            this.services = services;
        }

        public IActionResult Index(ArticleViewModel model)
        {
            ViewBag.Articles = services.GetArticles();
            ViewBag.Courses = services.GetCourses();

            if (model == null)
                return View();

            return View(model);
        }

        [HttpGet]
        public FileResult Download(int id)
        {
            var item = services.GetArticleById(id);
            services.IncDownload(id);
            byte[] file = System.IO.File.ReadAllBytes(item.File);
            string filename = Path.GetFileName(item.File);
            return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [HttpPost, ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(ArticleViewModel articleVM)
        {
            if (ModelState.IsValid)
            {

                IFormFile file = articleVM.File.First();
                
                var path = Path.GetTempFileName();

                var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                stream.Flush();
                stream.Close();

                Article article = new Article
                {
                    Name = articleVM.Name,
                    CourseID = articleVM.CourseID,
                    Uploaded = System.DateTime.Now,
                    Downloaded = 0,
                    File = path
                };

                var res = services.UploadArticle(article);

                if(res == ZHServices.ZHUpdateResult.OK)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }

                ModelState.AddModelError("", "Error while connecting to database");
            }

            return RedirectToAction(nameof(HomeController.Index), articleVM);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
