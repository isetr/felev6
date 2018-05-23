using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;

namespace DoorBash.Desktop.Model
{
    public enum OrderListFlag
    {
        ALL,
        FINISHED,
        NOT_FINISHED
    }

    public interface IDoorBashService
    {
        bool IsUserLoggedIn { get; }
        Task<IEnumerable<Order>> LoadOrdersAsync(string searchName = null, string searchAdress = null, OrderListFlag flag = OrderListFlag.ALL);
        Task<IEnumerable<Item>> LoadItems(int id);
        Task<IEnumerable<Category>> LoadCategories();
        Task<bool> AddNewItem(ItemDto item);
        Task<bool> LoginAsync(string name, string password);
        Task<bool> LogoutAsync();
    }
}
