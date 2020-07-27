using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GodsAmongSheep.Shared.Controllers;

namespace GodsAmongSheep.Shared.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Length { get; set; }

        // TODO: may want to modify this to be used as a getter, where it does what GetLength is doing...
        public List<WorkoutType> Workouts = new List<WorkoutType>();

        public GasUser GasUser { get; set; }

        public int GasUserId { get; set; }

        //public string GetDate => Date.Date.ToString("d");
        public string GetDate => Date.ToLongDateString();

        public string GetLength
        {
            get
            {
                using (var context = new GasContext())
                {
                    context.SetupServer();
                    var controller = new GasContextController(context);
                    var totalLength = 0;
                    foreach (var exercise in controller.GasWorkoutsController.GetWorkoutsWorkoutTypes(WorkoutId))
                    {
                        totalLength += exercise.Length;
                    }
                    return $"{totalLength.ToString()} Minutes";
                }

                //var totalLength = 0;
                //foreach (var workout in Workouts)
                //{
                //    totalLength += workout.Length;
                //}
                //return $"{totalLength.ToString()} Minutes";
            }
        }
    }
}
