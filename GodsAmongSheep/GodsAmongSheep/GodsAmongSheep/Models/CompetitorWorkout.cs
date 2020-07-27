using System;
using System.Collections.Generic;
using System.Text;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.Models
{
    public class CompetitorWorkout
    {
        public GasUser Competitor { get; set; }
        public Workout CWorkout { get; set; }
    }
}
