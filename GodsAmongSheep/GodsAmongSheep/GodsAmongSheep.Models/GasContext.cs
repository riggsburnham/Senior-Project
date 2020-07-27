using Microsoft.EntityFrameworkCore;
using System;
using GodsAmongSheep.Models.Models;

namespace GodsAmongSheep.Models
{
    public class GasContext : DbContext
    {
        public virtual DbSet<Workout> Workouts { get; set; }
        public virtual DbSet<GasUser> GasUsers { get; set; }

        private string _serverName;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL($"server={_serverName};database=GodsAmongSheepDB;user=GasAdmin;password=Excalibur1337!");
        }

        public void SetupServer(string serverName)
        {
            _serverName = serverName;
        }

        // Function for testing...
        public void InsertData()
        {
            Database.EnsureCreated();

            var user = new GasUser()
            {
                Username = "Hestia"
            };
            var workout = new Workout()
            {
                Exercise = "Cardio",
                Length = 40,
                Date = DateTime.Now,
                GasUser = user,
                GasUserId = user.UserId
            };
            user.Workouts.Add(workout);
            GasUsers.Add(user);
            Workouts.Add(workout);

            // Saves changes
            SaveChanges();
        }
    }
}
