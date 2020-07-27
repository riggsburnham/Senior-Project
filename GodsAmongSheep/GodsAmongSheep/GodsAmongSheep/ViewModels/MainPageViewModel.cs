using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using GodsAmongSheep.Models;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Views;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private GasUser _user = new GasUser();
        private string _username = null;
        private bool _isLoggedIn = false;
        private NavigationPage _navigator = new NavigationPage();
        public GasContextController _gasContextController;

        public MainPageViewModel(GasContextController gasContextController)
        {
            // Init commands here...
            //NavigateToLoginPage = new Command(
            //    async () => await NavigateToLoginPage()
            //);
            _gasContextController = gasContextController;
            NavigateToLoginPageCommand = new Command(NavigateToLoginPage);
            NavigateToLogoutPageCommand = new Command(NavigateToLogoutPage);
            NavigateToAddWorkoutPageCommand = new Command(NavigateToAddWorkoutPage);
            NavigateToWorkoutsPageCommand = new Command(NavigateToWorkoutsPage);
            NavigateToWorkoutGroupsPageCommand = new Command(NavigateToWorkoutGroupsPage);
            NavigateToNotificationsPageCommand = new Command(NavigateToNotificationsPage);
            NavigateToFriendsPageCommand = new Command(NavigateToFriendsPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")     // CallerMemberName remembers the function of which called this so you dont need to pass the name of the value
        {                                                                   // Allows you to call this function w/o params look at line 36 that has been commented out
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));  // this is what allows ui to update when objects change
        }


        public void FlyOutItemShower(
            FlyoutItem homePage, 
            MenuItem friendsPage, 
            MenuItem addWorkoutPage,
            MenuItem logInPage, 
            MenuItem logOutPage)
        {
            if (IsLoggedIn)
            {
                homePage.IsEnabled = true;
                friendsPage.IsEnabled = true;
                addWorkoutPage.IsEnabled = true;
                logInPage.IsEnabled = false;
                logOutPage.IsEnabled = true;
            }
            else
            {
                homePage.IsEnabled = false;
                friendsPage.IsEnabled = false;
                addWorkoutPage.IsEnabled = false;
                logInPage.IsEnabled = true;
                logOutPage.IsEnabled = false;
            }
        }

        public GasUser User
        {
            get => _user;
            set
            {
                _user = value;
                if (_user != null)
                {
                    // the user has logged in...
                    _username = _user.Username;
                    _isLoggedIn = true;
                }
                else
                {
                    // the user has logged out, the empty user is now null though, need to recreate it...
                    _user = new GasUser();
                    _username = null;
                    _isLoggedIn = false;
                }
            }
        }

        public bool IsLoggedIn => _isLoggedIn;

        public string GetUserName
        {
            get
            {
                if (_user.Username != null)
                {
                    return $"Hello, {_user.Username}";
                }
                else
                {
                    return "Hello. Sign In";
                }
            }
        }

        public Command NavigateToLoginPageCommand { get; }
        public void NavigateToLoginPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.Navigation.PushAsync(new LoginPage(this));
        }

        public Command NavigateToLogoutPageCommand { get; }

        public void NavigateToLogoutPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.Navigation.PushAsync(new LogoutPage(this));
        }

        public Command NavigateToAddWorkoutPageCommand { get; }
        public void NavigateToAddWorkoutPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.Navigation.PushAsync(new AddWorkoutsPage(this));
        }

        public Command NavigateToWorkoutsPageCommand { get; }

        public void NavigateToWorkoutsPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.Navigation.PushAsync(new WorkoutsPage(this));
        }

        public Command NavigateToWorkoutGroupsPageCommand { get; }
        public void NavigateToWorkoutGroupsPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.Navigation.PushAsync(new WorkoutGroupsPage(this));
        }

        public Command NavigateToNotificationsPageCommand { get; }
        public void NavigateToNotificationsPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.Navigation.PushAsync(new NotificationsPage(this));
        }

        public Command NavigateToFriendsPageCommand { get; }

        public void NavigateToFriendsPage()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.Navigation.PushAsync(new FriendsListPage());
        }

        public void Update()
        {
            //_user = user;
            //_username = user.Username;
            OnPropertyChanged(nameof(GetUserName));
        }
    }
}
