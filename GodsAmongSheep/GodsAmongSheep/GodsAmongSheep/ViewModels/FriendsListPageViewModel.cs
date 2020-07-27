using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    // TODO: would  prolly be better to call this like the workout details page, instead of using the workout  id to find the details
        // TODO: use the user id to find the friends (details of the user)... this way would give access to display parts of the workouts dynamically hopefully...
    public class FriendsListPageViewModel : INotifyPropertyChanged
    {
        private MainPageViewModel _parent;
        private ObservableCollection<GasUser> _friends = new ObservableCollection<GasUser>();
        private bool _addFriendsButtonClicked = false;

        public FriendsListPageViewModel(MainPageViewModel parent)
        {
            _parent = parent;
            Friends = GetFriends();
            _parent.User.FriendList = Friends;
            NavigateToAddFriendPopupViewCommand = new Command(async () => await NavigateToAddFriendPopupView());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")     // CallerMemberName remembers the function of which called this so you dont need to pass the name of the value
        {                                                                                   // Allows you to call this function w/o params look at line 36 that has been commented out
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));      // this is what allows ui to update when objects change
        }

        public Command NavigateToAddFriendPopupViewCommand { get; }

        public ObservableCollection<GasUser> Friends
        {
            get => _friends;
            set
            {
                _friends = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<GasUser> GetFriends()
        {
            Friends.Clear();
            _parent.User.FriendList.Clear();
            IList<Friend> friends = new List<Friend>();
            ObservableCollection<GasUser> friendsGasUsers = new ObservableCollection<GasUser>();
            if (!_parent.IsLoggedIn)
            {
                var message = "Please Log In";
                GasUser lonely = new GasUser{Username = "Please Log In", UserId = -1};
                //Friends.Add(lonely);
                friendsGasUsers.Add(lonely);
                return friendsGasUsers;
            }

            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                var gasContextController = new GasContextController(context);
                var gasFriendsController = gasContextController.GasFriendsController;
                friends = gasFriendsController.FindFriendsByGasUser(_parent.User).ToList();
                if (!friends.Any())
                {
                    // template friend to show that you are lonely
                    var templateFriend = new GasUser(){Username = "It's lonely here... Add a friend" };
                    friendsGasUsers.Add(templateFriend);
                }
                else
                {
                    foreach (var friend in friends)
                    {
                        var friendsUser = gasFriendsController.FindFriendsGasUserByFriend(_parent.User, friend);
                        if (friend.IsAccepted)
                        {
                            friendsGasUsers.Add(friendsUser);
                        }
                    }
                }

                _parent.User.FriendList = friendsGasUsers;
                return friendsGasUsers;
            }
        }

        public void AddFriendButtonShower(Button addFriendButton)
        {
            addFriendButton.IsVisible = _parent.IsLoggedIn;
        }

        public void Update()
        {
            Friends = GetFriends();
        }

        public async Task NavigateToAddFriendPopupView()
        {
            await PopupNavigation.Instance.PushAsync(new AddFriendPopupView(this));
        }
    }
}
