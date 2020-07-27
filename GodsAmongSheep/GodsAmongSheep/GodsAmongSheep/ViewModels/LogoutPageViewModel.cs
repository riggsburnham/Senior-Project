using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class LogoutPageViewModel
    {
        private MainPageViewModel _parent;
        public LogoutPageViewModel(MainPageViewModel parent)
        {
            _parent = parent;
            LogoutCommand = new Command(Logout);
        }

        public ICommand LogoutCommand { get; }

        public void Logout()
        {
            _parent.User = null;
            _parent.Update();
            Shell.Current.Navigation.PopToRootAsync();
        }
    }
}
