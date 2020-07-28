using System;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace GodsAmongSheep.Shared
{
    public class GasContext : DbContext
    {
        public virtual DbSet<Workout> Workouts { get; set; }
        public virtual DbSet<GasUser> Users { get; set; }

        public virtual DbSet<WorkoutType> WorkoutTypes { get; set; }

        public virtual DbSet<Friend> Friends { get; set; }

        public virtual DbSet<WorkoutGroup> WorkoutGroups { get; set; }

        public virtual DbSet<WorkoutGroup_User> WorkoutGroups_Users { get; set; }

        public virtual DbSet<GroupChat> GroupChats { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        private string _serverName;

        // TODO: need to set up database connection in order to run application
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql($"server=sampleServerName;port=1337;database=SampleDatabase;user=SampleUser;password=SamplePassword");
        }

        public void SetupServer(string serverName)
        {
            _serverName = serverName;
        }

        public void SetupServer()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    _serverName = "Localhost";
                    //_serverName = "192.168.137.1";
                    break;
                case Device.Android:
                    _serverName = "10.0.2.2";
                    break;
                default:
                    _serverName = "Localhost";
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<WorkoutGroup_User>(eb => { eb.HasNoKey(); });
            modelBuilder.Entity<WorkoutGroup_User>()
                .HasKey(wgu => new {wgu.GasUserId, wgu.WorkoutGroupId});
            modelBuilder.Entity<WorkoutGroup_User>()
                .HasOne(gu => gu.GasUser)
                .WithMany(wgu => wgu.WorkoutGroupUsers)
                .HasForeignKey(gu => gu.GasUserId);
            modelBuilder.Entity<WorkoutGroup_User>()
                .HasOne(wg => wg.WorkoutGroup)
                .WithMany(wgu => wgu.Competitors)
                .HasForeignKey(wg => wg.WorkoutGroupId);
        }
    }
}
