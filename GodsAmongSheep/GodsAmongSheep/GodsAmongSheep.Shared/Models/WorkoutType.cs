using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GodsAmongSheep.Shared.Models
{
    public class WorkoutType
    {
        [Key]
        public int WorkoutTypeId { get; set; }

        // foreign key
        public int WorkoutId { get; set; }

        [Required]
        public ExerciseType Exercise { get; set; }

        [Required]
        public int Length { get; set; }
    }

    public enum ExerciseType
    {
        Legs = 1,
        Arms = 2,
        Chest = 3,
        Shoulder = 4,
        Back = 5,
        Abs = 6,
        Cardio = 7,
        Sports = 8
    }
}
