using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GodsAmongSheep.Shared.Models
{
    public class WorkoutGroup
    {
        [Key]
        public int WorkoutGroupId { get; set; }

        [Required] 
        public string WorkoutGroupName { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [NotMapped]
        public ICollection<WorkoutGroup_User> Competitors { get; set; }

        // comma separated string containing all competitor ids
        //public string CompetitorIds;

        // comma separated string containing all workout ids
        //public string WorkoutIds;
        //TODO: public List<GasUser> PastGods;
    }
}
