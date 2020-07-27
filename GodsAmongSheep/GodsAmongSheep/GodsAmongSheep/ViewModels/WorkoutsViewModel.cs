using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class WorkoutsViewModel : INotifyPropertyChanged
    {
        private MainPageViewModel _parent;
        private GasWorkoutsController _gasWorkoutsController;
        private ObservableCollection<Workout> _workouts = new ObservableCollection<Workout>();
        private bool _addAWorkoutVisibilityLabel = false;
        private bool _loginLabelVisibility = false;

        public ObservableCollection<Workout> Workouts { get; set; }

        public Command LoadWorkoutsCommand { get; set; }

        //public WorkoutsViewModel()
        //{
        //    //Workouts = new ObservableCollection<Workout>();
        //    //LoadWorkoutsCommand = new Command(ExecuteLoadWorkoutsCommand);
        //}

        public WorkoutsViewModel(MainPageViewModel parent)
        {
            _parent = parent;
            _gasWorkoutsController = _parent._gasContextController.GasWorkoutsController;
            Workouts = new ObservableCollection<Workout>();
            LoadWorkoutsCommand = new Command(ExecuteLoadWorkoutsCommand);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")     // CallerMemberName remembers the function of which called this so you dont need to pass the name of the value
        {                                                                   // Allows you to call this function w/o params look at line 36 that has been commented out
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));  // this is what allows ui to update when objects change
        }

        public ObservableCollection<Workout> GetWorkouts
        {
            get
            {
                // this will be called each time the home page is opened,
                // so clear past _workouts to avoid duplicates
                _workouts.Clear();
                AddAWorkoutLabelVisibility = false;
                LoginLabelVisibility = false;
                var sortedWorkouts = new List<Workout>();
                if (_parent.IsLoggedIn)
                {
                    var workouts = GetAllWorkouts.ToList();
                    foreach (var workout in workouts)
                    {
                        _workouts.Add(workout);
                    }
                    sortedWorkouts = _workouts.OrderBy(obj => obj.Date).ToList();

                    // reverse list to have most recent item at top of list
                    sortedWorkouts.Reverse();
                    if (sortedWorkouts.Count == 0)
                    {
                        AddAWorkoutLabelVisibility = true;
                    }
                }
                else
                {
                    LoginLabelVisibility = true;
                }
                return new ObservableCollection<Workout>(sortedWorkouts);
            }
        }

        public bool AddAWorkoutLabelVisibility
        {
            get => _addAWorkoutVisibilityLabel;
            set
            {
                _addAWorkoutVisibilityLabel = value;
                OnPropertyChanged();
            }
        }

        public bool LoginLabelVisibility
        {
            get => _loginLabelVisibility;
            set
            {
                _loginLabelVisibility = value;
                OnPropertyChanged();
            }
        }


        private IEnumerable<Workout> GetAllWorkouts => _gasWorkoutsController.GetUsersWorkouts(_parent.User.UserId);


        public void UpdateCV(CollectionView cv)
        {
            cv.ItemsSource = GetWorkouts;
        }

        public void ExecuteLoadWorkoutsCommand()
        {
            try
            {
                Workouts.Clear();
                var usersWorkouts = _gasWorkoutsController.GetUsersWorkouts(_parent.User.UserId).ToList();
                foreach (var workout in usersWorkouts)
                {
                    Workouts.Add(workout);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
