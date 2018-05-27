using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;

namespace DoorBash.Desktop.Model
{
    public interface IDoorBashService
    {
        bool IsUserLoggedIn { get; }
        Task<IEnumerable<Order>> LoadOrdersAsync(string searchName = null, string searchAdress = null, int flag = 0);
        Task<IEnumerable<Item>> LoadItems(int id);
        Task<IEnumerable<Category>> LoadCategories();
        Task<bool> FinishOrder(int id);
        Task<bool> AddNewItem(ItemDto item);
        Task<bool> LoginAsync(string name, string password);
        Task<bool> LogoutAsync();
    }
}
