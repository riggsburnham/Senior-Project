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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql($"server={_serverName};database=gasPremium;user=GasAdmin;password=Excalibur1337!");
            optionsBuilder.UseMySql($"server=gasdb.c8xtvsksifgi.us-west-2.rds.amazonaws.com;port=3306;database=GasDB;user=GasAdmin;password=Excalibur1337!");



            //optionsBuilder.UseMySql($"server={_serverName};database=GodsAmongSheepDB;user=GasAdmin;password=Excalibur1337!");
            //optionsBuilder.UseMySql($"server={_serverName};database=gas_testing;user=GasAdmin;password=Excalibur1337!");
            //optionsBuilder.UseMySql($"server={_serverName};database=test;user=testAdmin;password=password");
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //optionsBuilder.EnableSensitiveDataLogging(true);
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
