using System;
using System.Collections.Generic;
using GodsAmongSheep.Models;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.ViewModels;
using GodsAmongSheep.Views;
using Xamarin.Forms;

namespace GodsAmongSheep
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private MainPageViewModel _viewModel;
        private Dictionary<string, Type> _routes = new Dictionary<string, Type>();
        private GasContextController _gasContextController;
        public AppShell(GasContextController gasContextController)
        {
            InitializeComponent();
            RegisterRoutes();
            _gasContextController = gasContextController;
            _viewModel = new MainPageViewModel(_gasContextController);
            BindingContext = _viewModel;
        }

        // TODO: this could also prolly be moved to the constructor or MainPageViewModel...
        private void RegisterRoutes()
        {
            _routes.Add("workoutdetails", typeof(WorkoutDetailsPage));
            _routes.Add("competitorworkoutdetails", typeof(CompetitorWorkoutDetailsPage));
            _routes.Add("frienddetails", typeof(FriendDetailsPage));

            foreach (var route in _routes)                              // TODO: foreseeing multiple routes...
            {
                Routing.RegisterRoute(route.Key, route.Value);
            }
        }

        protected override void OnAppearing()
        {
            _viewModel.FlyOutItemShower(
                HomePageFlyoutItem, 
                NavigateToFriendsMenuItem, 
                AddWorkoutPageMenuItem,
                LogInPageMenuItem, 
                LogOutPageMenuItem
                );
            //_viewModel.FlyoutItemShower(HomePageFlyoutItem);
            //_viewModel.AcceptFriendRequestButtonShower(AcceptFriendRequestButton);
        }
    }
}
