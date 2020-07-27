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
using GodsAmongSheep.Views;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class WorkoutGroupsPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel Parent { get; }

        private ListView _workoutGroupsListView;
        private ObservableCollection<WorkoutGroup> _workoutGroups = new ObservableCollection<WorkoutGroup>();

        public WorkoutGroupsPageViewModel(MainPageViewModel parent)
        {
            Parent = parent;
            WorkoutGroups = GetWorkoutGroups();
            NavigateToCreateWorkoutGroupPageCommand = new Command(NavigateToCreateWorkoutGroupPage);
            NavigateToManageWorkoutGroupPageCommand = new Command(NavigateToManageWorkoutGroupPage);
            LeaveSelectedWorkoutGroupCommand = new Command(LeaveSelectedWorkoutGroup);
            NavigateToWorkoutGroupPageCommand = new Command(NavigateToWorkoutGroupPage);
        }

        public ObservableCollection<WorkoutGroup> WorkoutGroups
        {
            get => _workoutGroups;
            set
            {
                _workoutGroups = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InitializeLV(ListView workoutGroupsListView)
        {
            _workoutGroupsListView = workoutGroupsListView;
        }

        private ObservableCollection<WorkoutGroup> GetWorkoutGroups()
        {
            var user = Parent.User;
            ObservableCollection<WorkoutGroup> workoutGroups = new ObservableCollection<WorkoutGroup>();
            if (user == null)
            {
                var tempMessage = new WorkoutGroup() { WorkoutGroupName = "Please Log In" };
                workoutGroups.Add(tempMessage);
                return workoutGroups;
            }

            using (var context = new GasContext())
            {
                context.SetupServer();
                var contextController = new GasContextController(context);
                var workoutGroupsController = contextController.GasWorkoutGroupsController;
                List<WorkoutGroup> workoutGroupsList = workoutGroupsController.FindUsersWorkoutGroups(user.UserId, true).ToList();
                foreach (WorkoutGroup wg in workoutGroupsList)
                {
                    workoutGroups.Add(wg);
                }
            }

            if (!workoutGroups.Any())
            {
                var lonely = new WorkoutGroup() {WorkoutGroupName = "Its Lonely here..."};
                workoutGroups.Add(lonely);
            }
            return workoutGroups;
        }

        public Command NavigateToCreateWorkoutGroupPageCommand { get; }

        public void NavigateToCreateWorkoutGroupPage()
        {
            Shell.Current.Navigation.PushAsync(new CreateWorkoutGroupPage(this));
        }

        public Command NavigateToManageWorkoutGroupPageCommand { get; }

        public void NavigateToManageWorkoutGroupPage()
        {
            WorkoutGroup workoutGroup = _workoutGroupsListView.SelectedItem as WorkoutGroup;
            if (workoutGroup == null)
            {
                // display error, that there is an issue with teh workoutgroup
                Application.Current.MainPage.DisplayAlert("Cannot Manage Workout Group!",
                    "Please select a workout group to manage", "Back");
            }
            else if (workoutGroup.CreatorId != Parent.User.UserId)
            {
                using (GasContext context = new GasContext())
                {
                    GasUser creator = new GasUser();
                    context.SetupServer();
                    GasContextController contextController = new GasContextController(context);
                    GasUsersController gasUsersController = contextController.GasUsersController;
                    creator = gasUsersController.FindGasUser(workoutGroup.CreatorId);
                    // display error, they dont have permission to modify
                    Application.Current.MainPage.DisplayAlert("Cannot Manage Workout Group!",
                        $"Only the creator \"{creator.Username}\" can manage \"{workoutGroup.WorkoutGroupName}\"", "Back");
                }
            }
            else
            {
                Shell.Current.Navigation.PushAsync(new ManageWorkoutGroupPage(this, workoutGroup));
            }
        }

        public Command LeaveSelectedWorkoutGroupCommand { get; }

        private void LeaveSelectedWorkoutGroup()
        {
            WorkoutGroup workoutGroup = _workoutGroupsListView.SelectedItem as WorkoutGroup;
            if (workoutGroup == null)
            {
                // display error, that there is an issue with teh workoutgroup
                Application.Current.MainPage.DisplayAlert("Cannot Leave Workout Group!",
                    "Please select a workout group to leave", "Back");
            }
            else if (workoutGroup.CreatorId == Parent.User.UserId)
            {
                Application.Current.MainPage.DisplayAlert("Cannot Manage Workout Group!",
                    $"You are the leader of the workout group \"{workoutGroup.WorkoutGroupName}\", " +
                    $"please either promote someone else to leader before leaving or delete the workout group", "Back");
            }
            else
            {
                using (GasContext context = new GasContext())
                {
                    context.SetupServer();
                    GasContextController contextController = new GasContextController(context);
                    GasWorkoutGroupsController workoutGroupsController = contextController.GasWorkoutGroupsController;
                    // TODO: still need to get the user to leave the workout group and then update the workout groups list...
                    WorkoutGroup_User wgu = workoutGroupsController.FindWorkoutGroup_User(workoutGroup.WorkoutGroupId, Parent.User.UserId);
                    workoutGroupsController.DeleteGasWorkoutGroup_User(wgu);
                    WorkoutGroups = GetWorkoutGroups();
;                }
            }
        }

        public Command NavigateToWorkoutGroupPageCommand { get; }

        private void NavigateToWorkoutGroupPage()
        {
            WorkoutGroup workoutGroup = _workoutGroupsListView.SelectedItem as WorkoutGroup;
            if (workoutGroup == null)
            {
                // display error, that there is an issue with teh workoutgroup
                Application.Current.MainPage.DisplayAlert("Cannot Navigate To Workout Group!",
                    "Please select a workout group to goto", "Back");
            }
            else
            {
                Shell.Current.Navigation.PushAsync(new WorkoutGroupPage(this, workoutGroup));
            }
        }
    }
}
