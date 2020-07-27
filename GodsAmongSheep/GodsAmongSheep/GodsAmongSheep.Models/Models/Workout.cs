using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GodsAmongSheep.Models.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        [Required]
        public string Exercise { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Length { get; set; }


        public GasUser GasUser { get; set; }

        public int GasUserId { get; set; }

        //TODO (need to create workoutType first): public List<WorkoutType> Workouts = new List<WorkoutType>();

        // public string Description { get; set; }

        // TODO  (move to api?): code like this may need to be moved to the API, as these aren't attributes of the model but using the model to get certain data...
        // TODO: public int GetNumWorkouts => Workouts.Count;
        // TODO (Move to api?): public string GetDate => Date.Date.ToString("d");

        // TODO: (move to api?)
        // public string GetLength
        //{
        //    get
        //    {
        //        var totalLength = 0;
        //        foreach (var workout in Workouts)
        //        {
        //            totalLength += workout.Length;
        //        }
        //        return $"{totalLength.ToString()} Minutes";
        //    }
        //}
    }
}
