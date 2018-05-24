using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZH.Persistence;

namespace ZH.Web.Services
{
    public class ZHServices
    {
        private readonly ZHDbContext context;

        public enum ZHUpdateResult
        {
            OK,
            DbError,
            ConcurrencyError
        }

        public ZHServices(ZHDbContext context)
        {
            this.context = context;
        }

        public List<Course> GetCourses()
        {
            return context.Courses.ToList();
        }

        public List<Article> GetArticles()
        {
            return context.Articles
                .OrderBy(a => a.Uploaded)
                .ToList();
        }

        public Article GetArticleById(int Id)
        {
            return context.Articles
                .Where(a => a.Id == Id)
                .FirstOrDefault();
        }

        public ZHUpdateResult IncDownload(int Id)
        {
            try
            {
                var item = context.Articles.Where(a => a.Id == Id).FirstOrDefault();
                item.Downloaded += 1;
                context.Update(item);
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return ZHUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return ZHUpdateResult.DbError;
            }

            return ZHUpdateResult.OK;
        }

        public ZHUpdateResult UploadArticle(Article article)
        {
            try
            {
                context.Add(article);
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return ZHUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return ZHUpdateResult.DbError;
            }

            return ZHUpdateResult.OK;
        }
    }
}
