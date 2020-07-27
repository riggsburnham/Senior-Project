using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class RegisterPageViewModel
    {
        private MainPageViewModel _parent;
        private GasUsersController _gasUserController;
        private string _username;
        private string _password;
        private string _confirmPassword;
        public RegisterPageViewModel(MainPageViewModel parent)
        {
            _parent = parent;
            _gasUserController = parent._gasContextController.GasUsersController;
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
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = value;
        }

        public ICommand RegisterCommand { get; }

        public void Register()
        {
            var valid = true;
            if (Username.Contains(" "))
            {
                Application.Current.MainPage.DisplayAlert("Failed to register!", "Username cannot have a space in it!", "Back");
            }
            else if (Password != ConfirmPassword)
            {
                Application.Current.MainPage.DisplayAlert("Failed to register!", "Passwords don't match!", "Back");
                valid = false;
            }
            else if (Password.Length <= 5)
            {
                Application.Current.MainPage.DisplayAlert("Password is too short!", "Please enter a password that is at least 5 characters long", "Back");
                valid = false;
            }
            else if (Password.All(char.IsLetterOrDigit))
            {
                Debug.WriteLine("Password does not contain a special character!");
                Application.Current.MainPage.DisplayAlert("Password does not contain a special character!", 
                    "Please include at least 1 special character in your password", "Back");
                valid = false;
            }
            else if (!Password.Any(char.IsDigit))
            {
                Debug.WriteLine("Password does not contain a digit!");
                Application.Current.MainPage.DisplayAlert("Password does not contain a digit!",
                    "Please include at least 1 digit in your password", "Back");
                valid = false;
            }
            else if (_gasUserController.FindGasUser(Username) != null)
            {
                Debug.WriteLine("Username already exists!");
                Application.Current.MainPage.DisplayAlert("Username already exists!",
                    "Please create a different username", "Back");
                valid = false;
            }

            if (valid)
            {
                // salt and hash the password...
                var salt = CreateSalt(16);
                var hashedPassword = GenerateSHA256Hash(this.Password, salt);

                // register the user...
                var user = new GasUser()
                {
                    Username = this.Username,
                    Password = hashedPassword,
                    Salt = salt
                };

                _gasUserController.CreateGasUser(user);
                Shell.Current.Navigation.PopAsync();
            }
        }

        public string CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
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
