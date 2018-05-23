using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;

namespace DoorBash.Desktop.Model
{
    public class DoorBashServices : IDoorBashService
    {
        private readonly HttpClient client;

        private bool isUserLoggedIn;
        public bool IsUserLoggedIn => isUserLoggedIn;

        public DoorBashServices(string baseAddress)
        {
            isUserLoggedIn = false;
            client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public async Task<IEnumerable<Order>> LoadOrdersAsync(string searchName = null, string searchAdress = null, OrderListFlag flag = OrderListFlag.ALL)
        {
            HttpResponseMessage res;

            Func<OrderListFlag, string> getListing = new Func<OrderListFlag, string> (f =>
                {
                    switch (f)
                    {
                        case OrderListFlag.ALL:
                            return "all";
                        case OrderListFlag.FINISHED:
                            return "finished";
                        case OrderListFlag.NOT_FINISHED:
                            return "notfinished";
                    }
                    return "all";
                }
            );

            if(!String.IsNullOrEmpty(searchName))
            {
                res = await client.GetAsync("api/Orders/?flag=" + getListing.Invoke(flag) + "?name=" + searchName);

                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<IEnumerable<Order>>();
                }

                throw new NetworkException("Service returned response: " + res.StatusCode);
            }
            else if (!String.IsNullOrEmpty(searchAdress))
            {
                res = await client.GetAsync("api/Orders/?flag=" + getListing.Invoke(flag) + "?address=" + searchName);

                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<IEnumerable<Order>>();
                }

                throw new NetworkException("Service returned response: " + res.StatusCode);
            }
            else
            {
                res = await client.GetAsync("api/Orders/?flag=" + getListing.Invoke(flag));

                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadAsAsync<IEnumerable<Order>>();
                }

                throw new NetworkException("Service returned response: " + res.StatusCode);
            }
        }

        public async Task<IEnumerable<Item>> LoadItems(int id)
        {
            HttpResponseMessage res = await client.GetAsync("api/Orders/Items/" + id);

            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadAsAsync<IEnumerable<Item>>();
            }

            throw new NetworkException("Service returned response: " + res.StatusCode);

        }

        public async Task<IEnumerable<Category>> LoadCategories()
        {
            HttpResponseMessage res = await client.GetAsync("api/Categories");

            if(res.IsSuccessStatusCode)
            {
                return await res.Content.ReadAsAsync<IEnumerable<Category>>();
            }

            throw new NetworkException("Service returned response: " + res.StatusCode);
        }

        public async Task<bool> AddNewItem(ItemDto item)
        {
            HttpResponseMessage res = await client.PostAsJsonAsync("api/Items/New", item);

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned response" + res.StatusCode);
        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            UserDto user = new UserDto { Username = name, Password = password };
            HttpResponseMessage res = await client.PostAsJsonAsync("api/Account/Login", user);

            if(res.IsSuccessStatusCode)
            {
                isUserLoggedIn = true;
                return true;
            }

            if(res.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response" + res.StatusCode);
        }

        public async Task<bool> LogoutAsync()
        {
            HttpResponseMessage res = await client.PostAsJsonAsync("api/Account/Signout", "");

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned response" + res.StatusCode);
        }
    }
}
