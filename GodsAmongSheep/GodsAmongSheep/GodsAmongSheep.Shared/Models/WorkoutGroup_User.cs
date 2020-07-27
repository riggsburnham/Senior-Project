using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GodsAmongSheep.Shared.Models
{
    public class WorkoutGroup_User
    {
        [ForeignKey("WorkoutGroup")]
        public int WorkoutGroupId { get; set; }
        public WorkoutGroup WorkoutGroup { get; set; }

        [ForeignKey("GasUser")]
        public int GasUserId { get; set; }
        public GasUser GasUser { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime AcceptedDateTime { get; set; }
    }
}
