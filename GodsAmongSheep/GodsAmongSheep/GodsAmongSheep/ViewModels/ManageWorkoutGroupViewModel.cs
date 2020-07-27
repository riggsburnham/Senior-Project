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
    public class ManageWorkoutGroupViewModel : INotifyPropertyChanged
    {
        private WorkoutGroupsPageViewModel _parent;
        private WorkoutGroup _workoutGroupToManage;
        private GasUser _applicationUser;

        private string _workoutGroupName;
        private ObservableCollection<GasUser> _competitors;
        private GasUser _selectedCompetitor;

        public ManageWorkoutGroupViewModel(WorkoutGroupsPageViewModel parent, WorkoutGroup workoutGroupToManage)
        {
            _parent = parent;
            _workoutGroupToManage = workoutGroupToManage;
            _applicationUser = parent.Parent.User;
            WorkoutGroupName = workoutGroupToManage.WorkoutGroupName;
            Competitors = InitializeCompetitors();
            NavigateToInviteFriendPopupViewCommand = new Command(async () => await NavigateToInviteFriendPopupView());
            KickFriendFromWorkoutGroupCommand = new Command(KickFriendFromWorkoutGroup);
            PromoteFriendToWorkoutGroupLeaderCommand = new Command(PromoteFriendToWorkoutGroupLeader);
            DeleteWorkoutGroupCommand = new Command(DeleteWorkoutGroup);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string WorkoutGroupName
        {
            get => _workoutGroupName;
            set
            {
                _workoutGroupName = value;
                NotifyPropertyChanged();
            }
        }

        public GasUser ApplicationUser => _applicationUser;
        public WorkoutGroup WorkoutGroupToManage => _workoutGroupToManage;

        public ObservableCollection<GasUser> Competitors
        {
            get => _competitors;
            set
            {
                _competitors = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<GasUser> InitializeCompetitors()
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasUsersController usersController = contextController.GasUsersController;




                GasWorkoutGroupsController workoutGroupsController = contextController.GasWorkoutGroupsController;
                IList<WorkoutGroup_User> workoutGroupUsers = 
                    workoutGroupsController.FindWorkoutGroupWorkoutGroup_Users(_workoutGroupToManage).ToList();
                ObservableCollection<GasUser> competitors = new ObservableCollection<GasUser>();
                foreach (WorkoutGroup_User wgu in workoutGroupUsers)
                {
                    if (wgu.GasUserId == _applicationUser.UserId) continue;
                    if(wgu.IsAccepted) competitors.Add(usersController.FindGasUser(wgu.GasUserId));
                }

                //ObservableCollection<GasUser> competitors = new ObservableCollection<GasUser>();
                //foreach (WorkoutGroup_User wgu in _workoutGroupToManage.Competitors)
                //{
                //    if (wgu.GasUserId == _applicationUser.UserId) continue;
                //    GasUser competitor = usersController.FindGasUser(wgu.GasUserId);
                //    competitors.Add(competitor);
                //}

                if (competitors.Count == 0)
                {
                    GasUser lonely = new GasUser{Username = "Its lonely here, invite a friend", UserId = -1};
                    competitors.Add(lonely);
                }

                return competitors;
            }
        }

        public GasUser SelectedCompetitor
        {
            get => _selectedCompetitor;
            set => _selectedCompetitor = value;
        }

        public Command NavigateToInviteFriendPopupViewCommand { get; }

        private async Task NavigateToInviteFriendPopupView()
        {
            await PopupNavigation.Instance.PushAsync(new InviteFriendPopupView(this));
        }

        public Command KickFriendFromWorkoutGroupCommand { get; }

        private void KickFriendFromWorkoutGroup()
        {
            // TODO: probably should put in a "are you sure" popup
            if (SelectedCompetitor == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to Kick Friend!", "Need to select a friend before you can kick one", "Back");
            }
            else
            {
                using (GasContext context = new GasContext())
                {
                    context.SetupServer();
                    GasContextController contextController = new GasContextController(context);
                    GasWorkoutGroupsController workoutGroupController = contextController.GasWorkoutGroupsController;
                    WorkoutGroup_User wgu = workoutGroupController.FindWorkoutGroup_User(WorkoutGroupToManage.WorkoutGroupId,
                        SelectedCompetitor.UserId);
                    workoutGroupController.DeleteGasWorkoutGroup_User(wgu);
                }
                Competitors = InitializeCompetitors();
            }
        }

        public Command PromoteFriendToWorkoutGroupLeaderCommand { get; }

        private void PromoteFriendToWorkoutGroupLeader()
        {
            // TODO: probably should put in a "are you sure" popup
            if (SelectedCompetitor == null)
            {
                Application.Current.MainPage.DisplayAlert("Failed to Promote Friend!", "Need to select a friend before you can promote one", "Back");
            }
            else
            {
                using (GasContext context = new GasContext())
                {
                    context.SetupServer();
                    GasContextController contextController = new GasContextController(context);
                    GasWorkoutGroupsController workoutGroupController = contextController.GasWorkoutGroupsController;
                    WorkoutGroupToManage.CreatorId = SelectedCompetitor.UserId;
                    workoutGroupController.UpdateGasWorkoutGroup(WorkoutGroupToManage);
                    // no longer the leader/creator this user should no longer be able to use this page for this workout group
                    Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }

        public Command DeleteWorkoutGroupCommand { get; }

        private void DeleteWorkoutGroup()
        {
            // TODO: probably should put in a "are you sure" popup
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasWorkoutGroupsController workoutGroupController = contextController.GasWorkoutGroupsController;
                workoutGroupController.DeleteGasWorkoutGroup(WorkoutGroupToManage.WorkoutGroupId);
                Application.Current.MainPage.Navigation.PopAsync();
            }
            
        }
    }
}
