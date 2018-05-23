using System;
using System.Windows.Controls;
using DoorBash.Desktop.Model;
using DoorBash.Persistence;
using DoorBash.Persistence.DTOs;

namespace DoorBash.Desktop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IDoorBashService model;

        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }

        public string UserName { get; set; }

        public event EventHandler ExitApplication;
        public event EventHandler LoginSuccess;
        public event EventHandler LoginFailed;

        public LoginViewModel(IDoorBashService model)
        {
            if(model == null)
                throw new ArgumentNullException("model");

            this.model = model;
            UserName = String.Empty;

            ExitCommand = new DelegateCommand(param => OnExitApplication());

            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                bool result = await model.LoginAsync(UserName, passwordBox.Password);

                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Unexpected error! ({ex.Message})");
            }
        }
        
        private void OnLoginSuccess()
        {
            LoginSuccess?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }

    }
}
