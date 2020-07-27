using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.ViewModels
{
    public class FriendDetailsViewModel : INotifyPropertyChanged
    {
        private GasUser _friend;
        private string _friendsWorkoutsString;
        private bool _noWorkoutsLabelVisibility = false;
        private ObservableCollection<Workout> _friendsWorkouts;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FriendDetailsViewModel(GasUser friend)
        {
            _friend = friend;
            FriendsWorkoutsString = GetFriendsWorkoutsString(friend.Username);
            FriendsWorkouts = GetFriendsWorkouts();
        }

        private string GetFriendsWorkoutsString(string username)
        {
            return $"{username}'s Workouts";
        }

        private ObservableCollection<Workout> GetFriendsWorkouts()
        {
            using (GasContext context = new GasContext())
            {
                context.SetupServer();
                GasContextController contextController = new GasContextController(context);
                GasWorkoutsController workoutsController = contextController.GasWorkoutsController;
                IList<Workout> friendsWorkouts = workoutsController.GetUsersWorkouts(_friend.UserId).ToList();
                friendsWorkouts = friendsWorkouts.OrderBy(w => w.Date).ToList();
                IList<Workout> sortedWorkouts = friendsWorkouts.Reverse().ToList();
                if (sortedWorkouts.Count == 0)
                {
                    NoWorkoutsLabelVisibility = true;
                }
                else
                {
                    NoWorkoutsLabelVisibility = false;
                }
                return new ObservableCollection<Workout>(sortedWorkouts);
            }
        }

        public string FriendsWorkoutsString
        {
            get => _friendsWorkoutsString;
            set
            {
                _friendsWorkoutsString = value;
                NotifyPropertyChanged();
            }
        }

        public bool NoWorkoutsLabelVisibility
        {
            get => _noWorkoutsLabelVisibility;
            set
            {
                _noWorkoutsLabelVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Workout> FriendsWorkouts
        {
            get => _friendsWorkouts;
            set
            {
                _friendsWorkouts = value;
                NotifyPropertyChanged();
            }
        }
    }
}
