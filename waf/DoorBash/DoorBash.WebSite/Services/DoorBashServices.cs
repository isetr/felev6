using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DoorBash.Persistence;

namespace DoorBash.WebSite.Services
{
    public class DoorBashServices
    {
        private readonly DoorBashDbContext context;

        public enum DoorBashUpdateResult
        {
            Success,
            ConcurrencyError,
            DbError
        }

        public DoorBashServices(DoorBashDbContext context)
        {
            this.context = context;
        }

        public List<Item> GetItems(string searchString = null)
        {
            return context.Items
                .Where(i => i.Name.Contains(searchString ?? ""))
                .OrderBy(i => i.Name)
                .ToList();
        }

        public List<Item> GetTopItems()
        {
            return
                context.Orders
                    .Select(o => o.Items)
                    .SelectMany(i => i)
                    .GroupBy(i => i.ItemID)
                    .OrderByDescending(o => o.Count())
                    .Select(o => o.Select(i => i).FirstOrDefault())
                    .Take(10)
                    .Join(context.Items, (i => i.ItemID), (i => i.Id), ((l, r) => r))
                    .ToList();
        }

        public Item GetItemById(int id)
        {
            return context.Items
                .Where(i => i.Id == id)
                .FirstOrDefault();
        }

        public List<Category> GetCategories()
        {
            return context.Categories
                .ToList();
        }

        public Category GetCategoryById(int id)
        {
            return context.Categories
                .Include(c => c.Items)
                .Where(c => c.Id == id)
                .FirstOrDefault();
        }

        public Category GetCategory(string category)
        {
            return context.Categories
                .Include(c => c.Items)
                .Where(c => c.Name.Equals(category))
                .FirstOrDefault();
        }

        public DoorBashUpdateResult CreateOrder(Order order, List<Item> items)
        {
            try
            {
                context.Orders.Add(new Order()
                    {
                        Name = order.Name,
                        Address = order.Address,
                        Phone = order.Phone,
                        Done = false,
                        Items = items.Select(i => new OrderItem() { ItemID = i.Id }).ToList(),
                        Sent = System.DateTime.Now
                    });
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return DoorBashUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return DoorBashUpdateResult.DbError;
            }

            return DoorBashUpdateResult.Success;
        }

        public DoorBashUpdateResult UpdateOrder(Order order)
        {
            try
            {
                context.Update(order);
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return DoorBashUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return DoorBashUpdateResult.DbError;
            }

            return DoorBashUpdateResult.Success;
        }

        public DoorBashUpdateResult RemoveOrder(int id)
        {
            var order = context.Orders.Find(id);
            if (order == null)
                return DoorBashUpdateResult.DbError; ;

            try
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return DoorBashUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return DoorBashUpdateResult.DbError;
            }

            return DoorBashUpdateResult.Success;
        }
    }
}
