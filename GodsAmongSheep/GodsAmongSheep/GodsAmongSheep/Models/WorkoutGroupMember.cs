using System;
using System.Collections.Generic;
using System.Text;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.Models
{
    public class WorkoutGroupMember
    {
        public GasUser Member { get; set; }
        public DateTime AcceptedDateTime { get; set; }
    }
}
