using System;
using System.Collections.Generic;
using System.Text;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.ViewModels
{
    public class WorkoutDetailsViewModel
    {
        private string _length;
        private Workout _workout;
        private string _date;
        private string _description;
        private MainPageViewModel _parent;

        public WorkoutDetailsViewModel(Workout workout, MainPageViewModel parent)
        {
            _parent = parent;
            _workout = workout;
            _length = GetTotalLengthString();
            _date = workout.GetDate;
            _description = workout.Description;
        }
        public List<string> DisplayWorkouts
            => WorkoutTypesToListDisplay();

        private List<string> WorkoutTypesToListDisplay()
        {
            var workouts = _parent._gasContextController.GasWorkoutsController.GetWorkoutsWorkoutTypes(_workout.WorkoutId);
            _workout.Workouts = workouts;

            var result = new List<string>();
            result.Add(DisplayTotalLength);
            foreach (var type in _workout.Workouts)
            {
                result.Add($"{type.Exercise.ToString()} Workout Length: {type.Length.ToString()} Minutes");
            }

            return result;
        }

        public string GetTotalLengthString()
        {
            var workouts = _parent._gasContextController.GasWorkoutsController.GetWorkoutsWorkoutTypes(_workout.WorkoutId);
            int totalLength = 0;
            foreach (var workout in workouts)
            {
                totalLength += workout.Length;
            }
            return $"{totalLength.ToString()} Minutes";
        }

        public string DisplayTotalLength
            => $"Total Workout Length: {TotalLengthString}";

        public string TotalLengthString => _length;
        public string Date => _date;
        public string Description => _description;
    }
}
