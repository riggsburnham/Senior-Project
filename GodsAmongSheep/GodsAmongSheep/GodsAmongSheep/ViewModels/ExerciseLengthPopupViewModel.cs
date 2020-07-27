using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace GodsAmongSheep.ViewModels
{
    public class ExerciseLengthPopupViewModel
    {
        private WorkoutType _exercise;
        private int _length;
        private AddWorkoutViewModel _parent;
        private ExerciseType _type;
        private Workout _workout;

        public ExerciseLengthPopupViewModel(Workout workout, ExerciseType type, AddWorkoutViewModel parent)
        {
            // TODO: prolly want to implement the is busy, look at AddWorkoutViewModel
            //AddExerciseCommand = new Command(
            //    async () => await AddExercise()
            //);
            _exercise = new WorkoutType{Exercise = type};
            _parent = parent;
            _type = type;
            _workout = workout;
            AddExerciseCommand = new Command(AddExercise);
        }

        public string ExerciseLengthLabel
            => $"Length of {_type.ToString()} Workout";

        public Command AddExerciseCommand { get; }

        // TODO: may need to be turned into a task once db is implemented, as tasks are what return 200, 300, 400, 500 values
        public void AddExercise()
        {
            _exercise.Length = _length;
            _workout.Workouts.Add(_exercise);
            _parent.Update();
            PopupNavigation.Instance.PopAsync(true);
        }

        //public async Task AddExercise()
        //{
        //    var test = _exercise;
        //    await PopupNavigation.Instance.PopAsync(true);
        //}

        public WorkoutType Exercise
        {
            get => _exercise;
            set => _exercise = value;
        }

        public int Length
        {
            get => _length;
            set => _length = value;
        }


    }
}
