using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class AddWorkoutViewModel : INotifyPropertyChanged
    {
        private DateTime _date = DateTime.Now;
        private int _id = 1;
        private bool _isBusy = false;
        private string _exercise = "";
        private int _length = 0;
        private Workout _workout = new Workout();
        private MainPageViewModel _parent;
        private string _description;

        private GasUsersController _gasUsersController;
        private GasWorkoutsController _gasWorkoutsController;

        // grabs all exercise types and puts them into an iEnumerable
        //private List<ExerciseType> exerciseTypes = 
        //    Enum.GetValues(typeof(ExerciseType)).Cast<ExerciseType>().ToList();

        public AddWorkoutViewModel()
        {
            // SaveWorkoutCommand = new Command(SaveWorkout);   // could use command like this if SaveWorkout was not a task
            SaveWorkoutCommand = new Command(
                async () => await SaveWorkout(),
                () => !IsBusy
                );
        }

        public AddWorkoutViewModel(MainPageViewModel parent)
        {
            _parent = parent;
            // SaveWorkoutCommand = new Command(SaveWorkout);   // could use command like this if SaveWorkout was not a task
            SaveWorkoutCommand = new Command(
                async () => await SaveWorkout(),
                () => !IsBusy
            );
            _gasUsersController = _parent._gasContextController.GasUsersController;
            _gasWorkoutsController = _parent._gasContextController.GasWorkoutsController;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")     // CallerMemberName remembers the function of which called this so you dont need to pass the name of the value
        {                                                                   // Allows you to call this function w/o params look at line 36 that has been commented out
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));  // this is what allows ui to update when objects change
        }

        public Command SaveWorkoutCommand { get; }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                _workout.Date = _date;
                //OnPropertyChanged(nameof(Date));            // Date has been changed, notify things referencing date
                //OnPropertyChanged();
                //OnPropertyChanged(nameof(DisplayMessage));  // since date has been changed so will display message, notify things calling DisplayMessage
            }
        }

        public string Description
        {
            get => _description;
            set => _workout.Description = _description = value;
        }

        public int Length
        {
            get
            {
                // set length to 0, it will be recalculated...
                _length = 0;
                foreach(var exercise in _workout.Workouts)
                {
                    _length += exercise.Length;
                }
                return _length;
            }
            set
            {
                _length = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayWorkouts));
                OnPropertyChanged(nameof(DisplayTotalLength));
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                //OnPropertyChanged(nameof(Id));
                //if (id == 1)                                  // this is an example of having the application wait for w/e is 
                //{                                                     // currently executing, do something like this when calling 
                //    IsBusy = true;                                    // an api
                //}
                //else
                //{
                //    IsBusy = false;
                //}

                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayWorkouts));
                OnPropertyChanged(nameof(DisplayTotalLength));
            }
        }

        public Workout Workout
        {
            get => _workout;
            set
            {
                _workout = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayWorkouts));
                OnPropertyChanged(nameof(DisplayTotalLength));
            }
        }

        public string Exercise
        {
            get => _exercise;
            set { _exercise = value; }
        }

        public void Update()
        {
            OnPropertyChanged(nameof(DisplayWorkouts));
            OnPropertyChanged(nameof(DisplayTotalLength));
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                //OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged();
                SaveWorkoutCommand.ChangeCanExecute();
            }
        }

        public List<string> DisplayWorkouts
            => WorkoutTypesToListDisplay();

        public string DisplayTotalLength
            => $"Total Workout Length: {Length} Minutes";

        public ExerciseType Legs => ExerciseType.Legs;
        private bool _isLegsSwitchToggled = false;
        public bool LegsSwitchToggled
        {
            get => _isLegsSwitchToggled;
            set
            {
                _isLegsSwitchToggled = value;
                OnPropertyChanged(nameof(LegsSwitchToggled));
                HandleExerciseTypeToggle(Legs, _isLegsSwitchToggled);
            }
        }

        public ExerciseType Arms => ExerciseType.Arms;
        private bool _isArmsSwitchToggled = false;
        public bool ArmsSwitchToggled
        {
            get => _isArmsSwitchToggled;
            set
            {
                _isArmsSwitchToggled = value;
                OnPropertyChanged(nameof(ArmsSwitchToggled));
                HandleExerciseTypeToggle(Arms, _isArmsSwitchToggled);
            }
        }

        public ExerciseType Chest => ExerciseType.Chest;
        private bool _isChestSwitchToggled = false;
        public bool ChestSwitchToggled
        {
            get => _isChestSwitchToggled;
            set
            {
                _isChestSwitchToggled = value;
                OnPropertyChanged(nameof(ChestSwitchToggled));
                HandleExerciseTypeToggle(Chest, _isChestSwitchToggled);
            }
        }

        public ExerciseType Shoulder => ExerciseType.Shoulder;
        private bool _isShoulderSwitchToggled = false;
        public bool ShoulderSwitchToggled
        {
            get => _isShoulderSwitchToggled;
            set
            {
                _isShoulderSwitchToggled = value;
                OnPropertyChanged(nameof(ShoulderSwitchToggled));
                HandleExerciseTypeToggle(Shoulder, _isShoulderSwitchToggled);
            }
        }

        public ExerciseType Back => ExerciseType.Back;
        private bool _isBackSwitchToggled = false;
        public bool BackSwitchToggled
        {
            get => _isBackSwitchToggled;
            set
            {
                _isBackSwitchToggled = value;
                OnPropertyChanged(nameof(BackSwitchToggled));
                HandleExerciseTypeToggle(Back, _isBackSwitchToggled);
            }
        }

        public ExerciseType Abs => ExerciseType.Abs;
        private bool _isAbsSwitchToggled = false;
        public bool AbsSwitchToggled
        {
            get => _isAbsSwitchToggled;
            set
            {
                _isAbsSwitchToggled = value;
                OnPropertyChanged(nameof(AbsSwitchToggled));
                HandleExerciseTypeToggle(Abs, _isAbsSwitchToggled);
            }
        }

        public ExerciseType Cardio => ExerciseType.Cardio;
        private bool _isCardioSwitchToggled = false;
        public bool CardioSwitchToggled
        {
            get => _isCardioSwitchToggled;
            set
            {
                _isCardioSwitchToggled = value;
                OnPropertyChanged(nameof(CardioSwitchToggled));
                HandleExerciseTypeToggle(Cardio, _isCardioSwitchToggled);
            }
        }

        public ExerciseType Sports => ExerciseType.Sports;
        private bool _isSportsSwitchToggled = false;
        public bool SportsSwitchToggled
        {
            get => _isSportsSwitchToggled;
            set
            {
                _isSportsSwitchToggled = value;
                OnPropertyChanged(nameof(SportsSwitchToggled));
                HandleExerciseTypeToggle(Sports, _isSportsSwitchToggled);
            }
        }

        private void HandleExerciseTypeToggle(ExerciseType type, bool toggledValue)
        {
            if (toggledValue == true)
            {
                PopupNavigation.Instance.PushAsync(new ExerciseLengthPopupView(_workout, type, this));
            }
            else if (toggledValue == false)
            {
                WorkoutType result = null;
                bool found = false;
                foreach (var exercise in _workout.Workouts)
                {
                    if (exercise.Exercise == type)
                    {
                        found = true;
                        result = exercise;
                        break;
                    }
                }
                if (found == true)
                {
                    _workout.Workouts.Remove(result);
                    OnPropertyChanged(nameof(DisplayWorkouts));
                    OnPropertyChanged(nameof(DisplayTotalLength));
                }
            }
        }

        private int NumWorkouts()
        {
            return Workout.Workouts.Count;
        }

        private List<string> WorkoutTypesToListDisplay()
        {
            var result = new List<String>();
            result.Add(DisplayTotalLength);
            foreach (var type in _workout.Workouts)
            {
                result.Add($"{type.Exercise.ToString()} Workout Length: {type.Length.ToString()} Minutes");
            }
            return result;
        }

        public async Task SaveWorkout()
        {
            IsBusy = true;
            // check to see if they started with date default value, if so set workouts date to it,
            // never entered setter since they stuck with default value
            if (_workout.Date != _date)
            {
                _workout.Date = _date;
            }

            // TODO: right now the workouts Id is being manually set, once database controls the setting of id remove this code
            //if (_workout.Id == 0)
            //{
            //    _workout.Id = _parent.User.Workouts.Count + 1;
            //}

            _workout.Length = Length;
            _workout.GasUser = _parent.User;
            _workout.GasUserId = _parent.User.UserId;

            

            // TODO: this line of code here may cause duplication of data....
            _parent.User.Workouts.Add(_workout);

            
            _gasWorkoutsController.CreateGasWorkout(_workout);
            foreach (var type in _workout.Workouts)
            {
                type.WorkoutId = _workout.WorkoutId;
                _gasWorkoutsController.CreateGasWorkoutType(type);
            }
            _gasUsersController.FindGasUser(_parent.User.UserId).Workouts.Add(_workout);
            
            IsBusy = false;
            _parent.Update();
            await Shell.Current.Navigation.PopToRootAsync();
            //await Application.Current.MainPage.DisplayAlert("title string", "message string", "cancel string");
        }
    }
}
