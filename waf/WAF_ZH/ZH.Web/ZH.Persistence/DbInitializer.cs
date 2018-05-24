using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ZH.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(ZHDbContext context)
        {
            context.Database.EnsureCreated();

            if(context.Courses.Any())
            {
                return;
            }

            IList<Course> courses = new List<Course>
            {
                new Course
                {
                    Name = "WAF"
                },
                new Course
                {
                    Name = "EVA"
                },
                new Course
                {
                    Name = "ORSI"
                }
            };

            courses.ToList().ForEach(c => context.Courses.Add(c));

            context.SaveChanges();
        }
    }
}
