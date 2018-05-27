using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;
using DoorBash.Desktop.Model;

namespace DoorBash.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDoorBashService model;

        private ObservableCollection<Order> orders;
        private ObservableCollection<Item> items;
        private ObservableCollection<Category> categories;
        private string searchName;
        private string searchAddress;
        private ItemDto newItem;

        public DelegateCommand SelectCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand FinishCommand { get; set; }
        public DelegateCommand AddNewItemCommand { get; set; }
        
        public event EventHandler LogoutSuccess;

        public MainViewModel(IDoorBashService model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            this.model = model;

            NewItem = new ItemDto();

            LoadAsync();
            
            SelectCommand = new DelegateCommand(param => LoadItems(param));
            SearchCommand = new DelegateCommand(param => SearchOrder(param));
            FinishCommand = new DelegateCommand(param => FinishOrder(param));
            AddNewItemCommand = new DelegateCommand(param => AddNewItem());
            LogoutCommand = new DelegateCommand(param => Logout());
        }

        public async void Logout()
        {
            try
            {
                await model.LogoutAsync();
                LogoutSuccess?.Invoke(this, EventArgs.Empty);
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Unexpected error! ({ex.Message})");
            }
        }

        public ItemDto NewItem
        {
            get => newItem;
            set
            {
                newItem = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                orders = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Item> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged();
            }
        }

        public string SearchName
        {
            get => searchName;
            set
            {
                searchName = value;
                OnPropertyChanged();
            }
        }

        public string SearchAddress
        {
            get => searchAddress;
            set
            {
                searchAddress = value;
                OnPropertyChanged();
            }
        }

        public async void LoadItems(object param)
        {
            try
            {
                if(param!= null)
                    Items = new ObservableCollection<Item>(await model.LoadItems(((Order)param).Id));
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void LoadAsync()
        {
            try
            {
                Categories = new ObservableCollection<Category>(await model.LoadCategories());
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void FinishOrder(object param)
        {
            try
            {
                await model.FinishOrder((int)param);
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void SearchOrder(object param)
        {
            try
            {
                if(Orders != null)
                    Orders.Clear();
                Orders = new ObservableCollection<Order>(await model.LoadOrdersAsync(SearchName, SearchAddress, (int)param));
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }

        public async void AddNewItem()
        {
            try
            { 
                await model.AddNewItem(NewItem);
                NewItem = new ItemDto();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }
    }
}
