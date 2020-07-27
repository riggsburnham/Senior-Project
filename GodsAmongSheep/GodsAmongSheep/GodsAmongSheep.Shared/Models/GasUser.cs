using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GodsAmongSheep.Shared.Models
{
    public class GasUser
    {
        public GasUser()
        {
            Workouts = new List<Workout>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [DisplayName(displayName: "Username")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        public virtual ICollection<Workout> Workouts { get; set; }
        public ICollection<GasUser> FriendList = new List<GasUser>();
        public ICollection<WorkoutGroup_User> WorkoutGroupUsers { get; set; }

        //public List<List<Trophy>> Trophies = new List<List<Trophy>>();
        //public List<WorkoutGroup> WorkoutGroups = new List<WorkoutGroup>();
    }
}
