using System;
using System.Configuration;
using System.Windows;

using DoorBash.Desktop.Model;
using DoorBash.Desktop.View;
using DoorBash.Desktop.ViewModel;
using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;

namespace DoorBash.Desktop
{
    public partial class App : Application
    {
        private IDoorBashService service;
        private LoginViewModel loginViewModel;
        private MainViewModel mainViewModel;
        private MainWindow mainWindow;
        private LoginWindow loginWindow;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            service = new DoorBashServices(ConfigurationManager.AppSettings["baseAddress"]);

            loginViewModel = new LoginViewModel(service);

            loginViewModel.ExitApplication += ViewModel_ExitApplication;
            loginViewModel.MessageApplication += ViewModel_MessageApplication;
            loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            loginViewModel.LoginFailed += ViewModel_LoginFailed;

            loginWindow = new LoginWindow
            {
                DataContext = loginViewModel
            };
            loginWindow.Show();
        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (service.IsUserLoggedIn)
            {
                await service.LogoutAsync();
            }
        }

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            mainViewModel = new MainViewModel(service);
            mainViewModel.MessageApplication += ViewModel_MessageApplication;
            mainViewModel.LogoutSuccess += ViewModel_Logout;

            mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();
            loginWindow.Close();
        }

        private void ViewModel_Logout(object sender, EventArgs e)
        {
            loginWindow = new LoginWindow
            {
                DataContext = loginViewModel
            };
            loginViewModel = new LoginViewModel(service);

            loginViewModel.ExitApplication += ViewModel_ExitApplication;
            loginViewModel.MessageApplication += ViewModel_MessageApplication;
            loginViewModel.LoginSuccess += ViewModel_LoginSuccess;
            loginViewModel.LoginFailed += ViewModel_LoginFailed;
            loginWindow.Show();
            mainWindow.Close();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Invalid username or password!", "Login", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Login", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
