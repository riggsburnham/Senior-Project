using System.Collections.Generic;
using System.Linq;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.ViewModels
{
    public class CompetitorWorkoutDetailsPageViewModel
    {
        private readonly Workout _workout;
        private readonly GasUser _user;
        private readonly string _competitorUsername;
        private readonly string _date;
        private readonly string _description;
        private IList<string> _displayWorkout;

        public CompetitorWorkoutDetailsPageViewModel(Workout workout)
        {
            _workout = workout;
            _date = _workout.GetDate;
            _description = _workout.Description;
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasUsersController usersController = contextController.GasUsersController;
                _user = usersController.FindGasUser(_workout.GasUserId);
                _competitorUsername = _user.Username;
                GasWorkoutsController workoutsController = contextController.GasWorkoutsController;
                _workout.Workouts = workoutsController.GetWorkoutsWorkoutTypes(_workout.WorkoutId).ToList();
            }
        }

        public string CompetitorUsername => _competitorUsername;

        public string Date => _date;

        public string Description => _description;

        public List<string> DisplayWorkouts
            => WorkoutTypesToListDisplay();

        private List<string> WorkoutTypesToListDisplay()
        {
            var result = new List<string>();
            result.Add(DisplayTotalLength);
            foreach (var type in _workout.Workouts)
            {
                result.Add($"{type.Exercise.ToString()} Workout Length: {type.Length.ToString()} Minutes");
            }
            return result;
        }

        private string DisplayTotalLength
            => $"Total Workout Length: {GetTotalLengthString()}";

        private string GetTotalLengthString()
        {
            int totalLength = 0;
            foreach (WorkoutType workout in _workout.Workouts)
            {
                totalLength += workout.Length;
            }
            return $"{totalLength.ToString()} Minutes";
        }
    }
}
