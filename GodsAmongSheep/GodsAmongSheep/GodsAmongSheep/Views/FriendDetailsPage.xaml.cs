using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Id", "id")]
    public partial class FriendDetailsPage : ContentPage
    {
        private MainPageViewModel _parent = Shell.Current.BindingContext as MainPageViewModel;
        private FriendDetailsViewModel _viewModel;
        public FriendDetailsPage()
        {
            InitializeComponent();
        }

        public string Id
        {
            set
            {
                GasUser friend = _parent.User.FriendList.FirstOrDefault(f => f.UserId == Convert.ToInt32(Uri.UnescapeDataString(value)));
                BindingContext = _viewModel = new FriendDetailsViewModel(friend);
            }
        }

        public async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CollectionView)sender).SelectedItem != null)
            {
                int? workoutId = ((Workout)e.CurrentSelection.FirstOrDefault())?.WorkoutId;
                //CompetitorWorkout competitorWorkout = ((CompetitorWorkout)e.CurrentSelection.FirstOrDefault());
                await Shell.Current.GoToAsync($"competitorworkoutdetails?id={workoutId}");
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}