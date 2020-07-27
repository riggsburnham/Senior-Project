using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GodsAmongSheep.Models;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.Views;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class WorkoutGroupPageViewModel : INotifyPropertyChanged
    {
        
        #region private member variables
        private WorkoutGroupsPageViewModel _parent;
        private WorkoutGroup _currentWorkoutGroup;
        private IList<CompetitorWorkout> _competitorsWorkouts;
        private IList<WorkoutGroupMember> _workoutGroupMembers;
        #endregion

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        public WorkoutGroup CurrentWorkoutGroup
        {
            get => _currentWorkoutGroup;
            set
            {
                _currentWorkoutGroup = value;
                NotifyPropertyChanged();
            }
        }

        public string WorkoutGroupName => CurrentWorkoutGroup.WorkoutGroupName;

        public IList<CompetitorWorkout> CompetitorsWorkouts
        {
            get => _competitorsWorkouts;
            set
            {
                _competitorsWorkouts = value;
                NotifyPropertyChanged();
            }
        }

        public WorkoutGroupsPageViewModel Parent => _parent;
        #endregion

        #region Functions
        public WorkoutGroupPageViewModel(WorkoutGroupsPageViewModel parent, WorkoutGroup workoutGroup)
        {
            _parent = parent;
            CurrentWorkoutGroup = workoutGroup;
            _workoutGroupMembers = InitializeWorkoutGroupMembers();
            CompetitorsWorkouts = new List<CompetitorWorkout>();
            CompetitorsWorkouts = InitializeCompetitorsWorkouts();
            NavigateToGroupChatPageCommand = new Command(NavigateToGroupChatPage);
        }

        private IList<WorkoutGroupMember> InitializeWorkoutGroupMembers()
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasUsersController usersController = contextController.GasUsersController;
                IList<WorkoutGroupMember> competitors = new List<WorkoutGroupMember>();
                foreach (WorkoutGroup_User wgu in CurrentWorkoutGroup.Competitors)
                {
                    if (wgu.IsAccepted == false) continue;
                    GasUser user = usersController.FindGasUser(wgu.GasUserId);
                    WorkoutGroupMember wgm = new WorkoutGroupMember()
                    {
                        Member = user,
                        AcceptedDateTime = wgu.AcceptedDateTime
                    };
                    competitors.Add(wgm);
                }
                return competitors;
            }
        }

        private IList<CompetitorWorkout> InitializeCompetitorsWorkouts()
        {
            // find the workouts for all users who are in the group
            // only will grab the workouts where the workout date time is greater than the
            // users workoutgroup_user datetime
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasWorkoutsController workoutsController = contextController.GasWorkoutsController;
                IList<CompetitorWorkout> competitorsWorkouts = new List<CompetitorWorkout>();
                foreach (WorkoutGroupMember wgm in _workoutGroupMembers)
                { 
                    IList<Workout> workouts = workoutsController.GetUsersWorkouts(wgm.Member.UserId)
                        .Where(w => w.Date > wgm.AcceptedDateTime).ToList();
                    foreach (Workout w in workouts)
                    {
                        CompetitorWorkout cw = new CompetitorWorkout()
                        {
                            Competitor = wgm.Member,
                            CWorkout = w
                        };
                        competitorsWorkouts.Add(cw);
                    }
                }

                // reverse list to have most recent item at top of list
                IList<CompetitorWorkout> sortedCompetitorsWorkouts = new List<CompetitorWorkout>();
                sortedCompetitorsWorkouts = competitorsWorkouts.OrderBy(obj => obj.CWorkout.Date).ToList();
                return sortedCompetitorsWorkouts.Reverse().ToList();
            }
        }
        #endregion

        #region Commands
        public Command NavigateToGroupChatPageCommand { get; }

        private void NavigateToGroupChatPage()
        {
            Shell.Current.Navigation.PushAsync(new GroupChatPage(this, _currentWorkoutGroup));
        }
        #endregion
    }
}
