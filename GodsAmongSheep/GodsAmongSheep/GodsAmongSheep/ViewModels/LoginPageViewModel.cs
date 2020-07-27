using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Views;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class LoginPageViewModel
    {
        private GasUser _user;
        private string _username;
        private GAS_Repos _repos = new GAS_Repos();
        private GasUsersController _gasUsersController;
        private MainPageViewModel _parent;

        // TODO: in the future this will need a salted hash...
        private string _password;

        public LoginPageViewModel(MainPageViewModel parent)
        {
            _user = parent.User;
            _parent = parent;
            _gasUsersController = _parent._gasContextController.GasUsersController;
            LoginCommand = new Command(Login);
            RegisterCommand = new Command(Register);
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public void Register()
        {
            // Navigate to register page...
            Shell.Current.Navigation.PushAsync(new RegisterPage(_parent));
        }

        public void Login()
        {
            var found = false;
            foreach (var user in _gasUsersController.GetGasUsers)
            {
                if (_username.ToLower() == user.Username.ToLower())
                {
                    var salt = user.Salt;
                    var hashedPassword = GenerateSHA256Hash(_password, salt);
                    if (hashedPassword == user.Password)
                    {
                        found = true;
                        _user = user;
                        _parent.User = _user;
                        _parent.Update();
                        Shell.Current.Navigation.PopToRootAsync();
                        return;
                    }
                }
            }

            if (!found)
            {
                Application.Current.MainPage.DisplayAlert("Failed to log in!", "Incorrect username and/or password", "Back");
            }
        }

        public string GenerateSHA256Hash(string input, string salt)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256HashString = new System.Security.Cryptography.SHA256Managed();
            var hash = sha256HashString.ComputeHash(bytes);
            return ByteArrayToHexString(hash);
        }

        public string ByteArrayToHexString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
