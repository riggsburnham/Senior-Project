using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class AddFriendPopupViewModel
    {
        private string _searchUsername;
        private ListView _searchResultsLV;
        private ObservableCollection<string> _foundUsers = new ObservableCollection<string>();
        private IList<GasUser> _gasUsers = new List<GasUser>();
        private MainPageViewModel _root;
        private FriendsListPageViewModel _parent;

        public AddFriendPopupViewModel(ListView searchResultsLV, FriendsListPageViewModel parent, MainPageViewModel root)
        {
            _parent = parent;
            _root = root;
            _searchResultsLV = searchResultsLV;
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasUsersController gasUsersController = contextController.GasUsersController;
                _gasUsers = gasUsersController.GetGasUsers.ToList();
            }
            AddFriendCommand = new Command(AddFriend);
        }

        public string SearchUsername
        {
            get => _searchUsername;
            set
            {
                _searchUsername = value;
                _foundUsers.Clear();
                _searchResultsLV.ItemsSource = _foundUsers;
                if (_searchUsername.Length < 3) return;
                foreach (GasUser user in _gasUsers)
                {
                    // ignore yourself !!!
                    if (user.Username == _root.User.Username) continue;

                    // check to see if we are already a friend...
                    bool requestPending = false;
                    foreach (GasUser friendsGasUser in _parent.Friends)
                    {
                        if (friendsGasUser.Username == user.Username ||
                            friendsGasUser.UserId == user.UserId)
                        {
                            requestPending = true;
                            break;
                        }
                    }
                    if (requestPending) continue;
                    if (_searchUsername.Length > user.Username.Length) continue;
                    var userSubString = user.Username.Substring(0, _searchUsername.Length);
                    if (_searchUsername.ToLower() == userSubString.ToLower())
                    {
                        _foundUsers.Add(user.Username);
                    }
                }
                _searchResultsLV.ItemsSource = _foundUsers;
            } 
        }

        public Command AddFriendCommand { get; }

        public void AddFriend()
        {
            if (_searchResultsLV.SelectedItem == null) return;
            GasUser newFriend = _gasUsers.FirstOrDefault(user => user.Username == _searchResultsLV.SelectedItem.ToString());
            if (newFriend != null)
            {
                using (GasContext context = new GasContext())
                {
                    context.SetupServer();
                    GasContextController contextController = new GasContextController(context);
                    GasFriendsController gasFriendsController = contextController.GasFriendsController;

                    IList<Friend> friends = gasFriendsController.GetFriends.ToList();
                    Friend exists = friends.FirstOrDefault(f =>
                        f.SenderId == _root.User.UserId && f.RecipientId == newFriend.UserId ||
                        f.RecipientId == _root.User.UserId && f.SenderId == newFriend.UserId);
                    if (exists != null)
                    {
                        const string recipientMessage = "This user has already sent you a friend request, check notifications tab.";
                        const string senderMessage = "You have already sent this user a friend request, please wait for a response.";
                        if (exists.SenderId == _root.User.UserId)
                        {
                            Application.Current.MainPage.DisplayAlert("Failed to send friend request!",
                                senderMessage, "Back");
                        }
                        else if (exists.RecipientId == _root.User.UserId)
                        {
                            Application.Current.MainPage.DisplayAlert("Failed to send friend request!",
                                recipientMessage, "Back");
                        }
                    }
                    else
                    {
                        gasFriendsController.CreateFriend(_root.User, newFriend);
                        _parent.Update();
                    }
                }
                PopupNavigation.Instance.PopAsync(true);
            }
        }
    }
}
