using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using Xamarin.Forms;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.ViewModels
{
    public class CreateWorkoutGroupsPageViewModel
    {
        private WorkoutGroupsPageViewModel _parent;
        private string _workoutGroupNameEntry;

        public ObservableCollection<SelectableData<GasUser>> Friends { get; set; }
        //public ObservableCollection<GasUser> Friends { get; set; }

        public class SelectableData<T>
        {
            public T Data { get; set; }
            public bool Selected { get; set; }
        }

        public CreateWorkoutGroupsPageViewModel(WorkoutGroupsPageViewModel parent)
        {
            _parent = parent;
            InitializeFriends();
            CreateWorkoutGroupCommand = new Command(CreateWorkoutGroup);
        }

        public Command CreateWorkoutGroupCommand { get; }

        public string WorkoutGroupNameEntry
        {
            get => _workoutGroupNameEntry;
            set => _workoutGroupNameEntry = value;
        }

        private async void CreateWorkoutGroup()
        {
            var competitors = new List<GasUser>();
            foreach (var friend in Friends)
            {
                if (friend.Selected)
                {
                    competitors.Add(friend.Data);
                }
            }

            var workoutGroup = new WorkoutGroup()
            {
                CreatorId = _parent.Parent.User.UserId,
                WorkoutGroupName = WorkoutGroupNameEntry
            };

            // TODO: need to call the database and create the workout group, this should allow you to get the correct Id for the workout group...
            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var workoutGroupController = contextController.GasWorkoutGroupsController;
                workoutGroupController.CreateGasWorkoutGroup(workoutGroup);
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                GasGroupChatsController groupChatsController = contextController.GasGroupChatsController;
                GroupChat gc = new GroupChat()
                {
                    WorkoutGroupId = workoutGroup.WorkoutGroupId,
                    WorkoutGroupName = workoutGroup.WorkoutGroupName
                };
                await groupChatsController.CreateGasGroupChat(gc);
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var workoutGroupController = contextController.GasWorkoutGroupsController;

                // add the creator to the workoutgroup_users table
                var workoutGroup_User = new WorkoutGroup_User()
                {
                    WorkoutGroupId = workoutGroup.WorkoutGroupId,
                    GasUserId = _parent.Parent.User.UserId,
                    //WorkoutGroup = workoutGroup,
                    //GasUser = _parent.Parent.User,
                    IsAccepted = true,
                    AcceptedDateTime = DateTime.Now
                };
                await workoutGroupController.CreateGasWorkoutGroup_User(workoutGroup_User);

                // add all competitors to the workoutgroup_users table
                foreach (var user in competitors)
                {
                    // in here the workoutGroupId should never change from the original id,
                    // each time we want to add the new userid to the workoutgroup_user for a proper join table
                    workoutGroup_User.GasUserId = user.UserId;
                    //workoutGroup_User.GasUser = user;
                    workoutGroup_User.IsAccepted = false;
                    workoutGroup_User.AcceptedDateTime = DateTime.MinValue;
                    //workoutGroup_User.WorkoutGroup = workoutGroup;
                    workoutGroup_User.WorkoutGroupId = workoutGroup.WorkoutGroupId;
                    await workoutGroupController.CreateGasWorkoutGroup_User(workoutGroup_User);
                }
            }
            await Shell.Current.Navigation.PopAsync();
        }

        private void InitializeFriends()
        {
            var user = _parent.Parent.User;
            Friends = new ObservableCollection<SelectableData<GasUser>>();
            if (user == null)
            {
                var lonely = new GasUser() { Username = "Please Log In" };
                //Friends.Append(lonely);
                var selectableFriend = new SelectableData<GasUser> { Data = lonely, Selected = false };
                Friends.Add(selectableFriend);
                return;
            }
            //Friends = new ObservableCollection<GasUser>();
            using (var context = new GasContext())
            {
                context.SetupServer(); 
                var contextController = new GasContextController(context);
                var friendsController = contextController.GasFriendsController;
                var friends = friendsController.FindFriendsByGasUser(user);
                foreach (var f in friends)
                {
                    var friend = friendsController.FindFriendsGasUserByFriend(user, f);
                    // TODO: also test putting in the constructor of a user to add its workouts and friends if they have any
                    //Friends.Add(friend);
                    var selectableFriend = new SelectableData<GasUser> {Data = friend, Selected = false};
                    Friends.Add(selectableFriend);
                }
            }

            if (!Friends.Any())
            {
                // give a template friend
                var lonely = new GasUser(){Username = "Its Lonely here..."};
                //Friends.Add(lonely);
                var selectableFriend = new SelectableData<GasUser> { Data = lonely, Selected = false };
                Friends.Add(selectableFriend);
            }
        }
    }
}
