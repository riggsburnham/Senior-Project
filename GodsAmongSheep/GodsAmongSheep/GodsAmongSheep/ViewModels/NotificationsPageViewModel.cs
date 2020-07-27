using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.Views;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class NotificationsPageViewModel : INotifyPropertyChanged//INotifyCollectionChanged
    {
        private MainPageViewModel _parent;
        private ListView _friendRequestsListView;
        private Button _acceptFriendRequestBT;
        private Button _declineFriendRequestBT;
        private ListView _workoutGroupRequestsListView;
        private Button _acceptWorkoutGroupRequestBT;
        private Button _declineWorkoutGroupRequestBT;
        private ObservableCollection<GasUser> friendRequests;

        public ObservableCollection<GasUser> FriendRequests
        {
            get => friendRequests;
            set 
            { 
                friendRequests = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<WorkoutGroup> WorkoutGroupRequests { get; set; }

        public NotificationsPageViewModel(MainPageViewModel parent)
        {
            _parent = parent;
            InitializeFriendRequests();
            InitializeWorkoutGroupRequests();
            AcceptFriendRequestCommand = new Command(AcceptFriendRequest);
            DeclineFriendRequestCommand = new Command(DeclineFriendRequest);
            AcceptWorkoutGroupRequestCommand = new Command(AcceptWorkoutGroupRequest);
            DeclineWorkoutGroupRequestCommand = new Command(DeclineWorkoutGroupRequest);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void InitializeLV(ListView friendRequestsListView, ListView workoutGroupRequestsListView)
        {
            _friendRequestsListView = friendRequestsListView;
            _workoutGroupRequestsListView = workoutGroupRequestsListView;
            if (FriendRequests.Count() == 1 && FriendRequests[0].UserId == -1)
            {
                _friendRequestsListView.SelectionMode = ListViewSelectionMode.None;
            }
            else
            {
                _friendRequestsListView.SelectionMode = ListViewSelectionMode.Single;
            }
            if (WorkoutGroupRequests.Count() == 1 && WorkoutGroupRequests[0].WorkoutGroupId == -1)
            {
                _workoutGroupRequestsListView.SelectionMode = ListViewSelectionMode.None;
            }
            else
            {
                _workoutGroupRequestsListView.SelectionMode = ListViewSelectionMode.Single;
            }
        }

        public void InitializeBT(Button acceptFriendRequestBT, Button declineFriendRequestBT, Button acceptWorkoutGroupRequestBT, Button declineWorkoutGroupRequestBT)
        {
            _acceptFriendRequestBT = acceptFriendRequestBT;
            _declineFriendRequestBT = declineFriendRequestBT;
            _acceptWorkoutGroupRequestBT = acceptWorkoutGroupRequestBT;
            _declineWorkoutGroupRequestBT = declineWorkoutGroupRequestBT;

            
            if (FriendRequests.Count() == 1 && FriendRequests[0].UserId == -1)
            {
                _acceptFriendRequestBT.IsEnabled = false;
                _declineFriendRequestBT.IsEnabled = false;
            }
            else
            {
                // this else could be redundent but would prevent items from ever becoming unclickable when they should be clickable
                _acceptFriendRequestBT.IsEnabled = true;
                _declineFriendRequestBT.IsEnabled = true;
            }

            if (WorkoutGroupRequests.Count() == 1 && WorkoutGroupRequests[0].WorkoutGroupId == -1)
            {
                _acceptWorkoutGroupRequestBT.IsEnabled = false;
                _declineWorkoutGroupRequestBT.IsEnabled = false;
            }
            else
            {
                _acceptWorkoutGroupRequestBT.IsEnabled = true;
                _declineWorkoutGroupRequestBT.IsEnabled = true;
            }
        }

        private void InitializeFriendRequests()
        {
            var user = _parent.User;
            if (FriendRequests == null)
            {
                FriendRequests = new ObservableCollection<GasUser>();
            }
            else
            {
                FriendRequests.Clear();
            }
            if (user == null)
            {
                var tempMessage = new GasUser() { Username = "Please Log In", UserId = -1 };
                FriendRequests.Add(tempMessage);
                return;
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var friendsController = contextController.GasFriendsController;
                var friends = friendsController.FindFriendsByGasUser(user).ToArray();
                foreach (var friend in friends)
                {
                    // if they are already friends skip this friend
                    if (friend.IsAccepted) continue;
                    // if the user is the sender of the friend request skip this friend
                    if (friend.SenderId == user.UserId) continue;

                    // find the friends user by using the application user and the friend
                    var friendUser = friendsController.FindFriendsGasUserByFriend(user, friend);
                    FriendRequests.Add(friendUser);
                }
            }

            if (!FriendRequests.Any())
            {
                var loneley = new GasUser() { Username = "No Friend Requests...", UserId = -1};
                FriendRequests.Add(loneley);
            }
        }

        private void InitializeWorkoutGroupRequests()
        {
            var user = _parent.User;
            if (WorkoutGroupRequests == null)
            {
                WorkoutGroupRequests = new ObservableCollection<WorkoutGroup>();
            }
            else
            {
                WorkoutGroupRequests.Clear();
            }
            if (user == null)
            {
                var tempMessage = new WorkoutGroup() { WorkoutGroupName = "Please Log In", WorkoutGroupId = -1 };
                WorkoutGroupRequests.Add(tempMessage);
                return;
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var workoutGroupsController = contextController.GasWorkoutGroupsController;
                var workoutGroups = workoutGroupsController.FindUsersWorkoutGroups(user.UserId, false);
                foreach (var workoutGroup in workoutGroups)
                {
                    WorkoutGroupRequests.Add(workoutGroup);
                }
            }

            if (!WorkoutGroupRequests.Any())
            {
                var loneley = new WorkoutGroup() { WorkoutGroupName = "No Workout Group Requests...", WorkoutGroupId = -1 };
                WorkoutGroupRequests.Add(loneley);
            }
        }

        public Command AcceptFriendRequestCommand { get; }

        private void AcceptFriendRequest()
        {
            var user = _parent.User;
            if (_friendRequestsListView.SelectedItem == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to accept friend request!", "Please select a friend request to accept", "Back");
                return;
            }

            GasUser friendGasUser = _friendRequestsListView.SelectedItem as GasUser;

            // this is a cautionary if statement, should honestly never be hit. This could actually be hit if the user had their
            // account deleted, although users are currently unable to do so
            if (friendGasUser == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to find friend who sent the request!", "Something went wrong...", "Back");
                return;
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var gasFriendsController = contextController.GasFriendsController;
                var friend = gasFriendsController.FindFriend(user.UserId, friendGasUser.UserId);
                friend.IsAccepted = true;
                gasFriendsController.UpdateGasFriend(friend);

                InitializeFriendRequests();
            }
        }

        public Command DeclineFriendRequestCommand { get; }

        private void DeclineFriendRequest()
        {
            var user = _parent.User;
            if (_friendRequestsListView.SelectedItem == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to decline friend request!", "Please select a friend request to decline", "Back");
                return;
            }

            GasUser friendGasUser = _friendRequestsListView.SelectedItem as GasUser;

            // this is a cautionary if statement, should honestly never be hit. This could actually be hit if the user had their
            // account deleted, although users are currently unable to do so
            if (friendGasUser == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to find friend who sent the request!", "Something went wrong...", "Back");
                return;
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var gasFriendsController = contextController.GasFriendsController;
                var friend = gasFriendsController.FindFriend(user.UserId, friendGasUser.UserId);
                gasFriendsController.DeleteFriend(friend.FriendId);

                InitializeFriendRequests();
            }
        }

        public Command AcceptWorkoutGroupRequestCommand { get; }

        private void AcceptWorkoutGroupRequest()
        {
            var user = _parent.User;
            if (_workoutGroupRequestsListView.SelectedItem == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to accept workout group request!", "Please select a workout group request to accept", "Back");
                return;
            }

            WorkoutGroup workoutGroup = _workoutGroupRequestsListView.SelectedItem as WorkoutGroup;
            
            // this if would be hit if the workout group creator deleted the workoutgroup
            if (workoutGroup == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to find the workout group user!", "The workout group was most likely deleted by workout group creator", "Back");
                return;
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var gasWorkoutGroupsController = contextController.GasWorkoutGroupsController;
                var workoutGroupUser = gasWorkoutGroupsController.FindWorkoutGroup_User(workoutGroup.WorkoutGroupId, user.UserId);
                workoutGroupUser.IsAccepted = true;
                workoutGroupUser.AcceptedDateTime = DateTime.Now;
                gasWorkoutGroupsController.UpdateGasWorkoutGroup_User(workoutGroupUser);
                InitializeWorkoutGroupRequests();
            }
        }

        public Command DeclineWorkoutGroupRequestCommand { get; }

        private void DeclineWorkoutGroupRequest()
        {
            var user = _parent.User;
            if (_workoutGroupRequestsListView.SelectedItem == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to accept workout group request!", "Please select a workout group request to accept", "Back");
                return;
            }

            WorkoutGroup workoutGroup = _workoutGroupRequestsListView.SelectedItem as WorkoutGroup;

            // this if would be hit if the workout group creator deleted the workoutgroup
            // TODO: this could cause an error here, using the as keyword above may convert the old data found into a actual workoutGroup object
            // TODO: then this check wouldn't be hit and would procede to the next part where you will decline a workoutgroup that no longer exists
            if (workoutGroup == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to find the workout group user!", "The workout group was most likely deleted by workout group creator", "Back");
                return;
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var gasWorkoutGroupsController = contextController.GasWorkoutGroupsController;
                var wgu = gasWorkoutGroupsController.FindWorkoutGroup_User(workoutGroup.WorkoutGroupId, user.UserId);
                gasWorkoutGroupsController.DeleteGasWorkoutGroup_User(wgu);
                InitializeWorkoutGroupRequests();
            }
        }
    }
}
