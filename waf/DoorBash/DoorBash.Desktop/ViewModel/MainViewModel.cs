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

        public DelegateCommand SelectCommand { get; set; }

        public MainViewModel(IDoorBashService model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            this.model = model;

            LoadAsync();
            SelectCommand = new DelegateCommand(LoadItems);
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

        public async void LoadItems(object param)
        {
            try
            {
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
                Orders = new ObservableCollection<Order>(await model.LoadOrdersAsync());
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Váratlan hiba történt! ({ex.Message})");
            }
        }
    }
}
