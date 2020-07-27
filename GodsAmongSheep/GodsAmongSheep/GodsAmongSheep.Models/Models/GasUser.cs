using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GodsAmongSheep.Models.Models
{
    public class GasUser
    {
        // TODO: will need to figure out recovery info steps and necessary attributes'
        public GasUser()
        {
            Workouts = new List<Workout>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [DisplayName(displayName: "Username")]
        public string Username { get; set; }

        public virtual ICollection<Workout> Workouts { get; set; }
        //TODO: public List<List<Trophy>> Trophies = new List<List<Trophy>>();
        //TODO: public List<WorkoutGroup> WorkoutGroups = new List<WorkoutGroup>();
        //TODO: public List<GasUser> FriendList = new List<GasUser>();
    }
}
