using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class InviteFriendPopupViewModel : INotifyPropertyChanged
    {
        public class SelectableData<T>
        {
            public T Data { get; set; }
            public bool Selected { get; set; }
        }
        private ManageWorkoutGroupViewModel _parent;
        private GasUser _applicationUser;
        private bool _noMoreFriendsToInviteVisibility = false;

        private ObservableCollection<SelectableData<GasUser>> _friendsToInvite;

        public InviteFriendPopupViewModel(ManageWorkoutGroupViewModel parent)
        {
            _parent = parent;
            _applicationUser = _parent.ApplicationUser;
            FriendsToInvite = new ObservableCollection<SelectableData<GasUser>>();
            FriendsToInvite = InitializeFriendsToInvite();
            InviteFriendsCommand = new Command(InviteFriends);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<SelectableData<GasUser>> FriendsToInvite
        {
            get => _friendsToInvite;
            set
            {
                _friendsToInvite = value;
                NotifyPropertyChanged();
            }
        }

        public bool NoMoreFriendsToInviteVisibility
        {
            get => _noMoreFriendsToInviteVisibility;
            set
            {
                _noMoreFriendsToInviteVisibility = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<SelectableData<GasUser>> InitializeFriendsToInvite()
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasFriendsController friendsController = contextController.GasFriendsController;

                GasWorkoutGroupsController workoutGroupsController = contextController.GasWorkoutGroupsController;
                IList<WorkoutGroup_User> competitors =
                    workoutGroupsController.FindWorkoutGroupWorkoutGroup_Users(_parent.WorkoutGroupToManage).ToList();

                ObservableCollection<SelectableData<GasUser>> friendsToInvite = new ObservableCollection<SelectableData<GasUser>>();
                IList<Friend> friends = friendsController.FindFriendsByGasUser(_applicationUser).ToList();
                foreach(Friend f in friends)
                {
                    GasUser friend = friendsController.FindFriendsGasUserByFriend(_applicationUser, f);
                    bool alreadyInWorkoutGroup = false;
                    foreach (WorkoutGroup_User wgu in competitors)
                    {
                        if (wgu.GasUserId == friend.UserId)
                        {
                            // there in the wg already, don't display them...
                            alreadyInWorkoutGroup = true;
                            break;
                        }
                    }

                    if (!alreadyInWorkoutGroup)
                    {
                        SelectableData<GasUser> selectableFriend = new SelectableData<GasUser> { Data = friend, Selected = false };
                        friendsToInvite.Add(selectableFriend);
                    }
                }
                if (!friendsToInvite.Any())
                {
                    NoMoreFriendsToInviteVisibility = true;
                }
                else
                {
                    NoMoreFriendsToInviteVisibility = false;
                }
                return friendsToInvite;
            }
        }
        
        public Command InviteFriendsCommand { get; }

        private async void InviteFriends()
        {
            IList<GasUser> competitors = new List<GasUser>();
            foreach (var friend in FriendsToInvite)
            {
                if (friend.Selected)
                {
                    competitors.Add(friend.Data);
                }
            }
            using (var context = new GasContext())
            {
                context.SetupServer();
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                GasContextController contextController = new GasContextController(context);
                GasWorkoutGroupsController workoutGroupController = contextController.GasWorkoutGroupsController;

                WorkoutGroup_User workoutGroup_User = new WorkoutGroup_User();

                // add all competitors to the workoutgroup_users table
                foreach (var user in competitors)
                {
                    workoutGroup_User.GasUserId = user.UserId;
                    workoutGroup_User.IsAccepted = false;
                    workoutGroup_User.WorkoutGroupId = _parent.WorkoutGroupToManage.WorkoutGroupId;
                    context.Entry(workoutGroup_User).State = EntityState.Detached;
                    await workoutGroupController.CreateGasWorkoutGroup_User(workoutGroup_User);
                }
            }
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
