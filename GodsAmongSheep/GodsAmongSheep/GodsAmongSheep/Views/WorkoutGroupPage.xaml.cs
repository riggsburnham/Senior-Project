using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Models;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutGroupPage : ContentPage
    {
        private WorkoutGroupPageViewModel _viewModel;
        private WorkoutGroupsPageViewModel _parent;
        private WorkoutGroup _workoutGroup;
        public WorkoutGroupPage(WorkoutGroupsPageViewModel parent, WorkoutGroup workoutGroup)
        {
            _parent = parent;
            _workoutGroup = workoutGroup;
            BindingContext = _viewModel = new WorkoutGroupPageViewModel(_parent, _workoutGroup);
            InitializeComponent();
        }
        public async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CollectionView)sender).SelectedItem != null)
            {
                int? workoutId = ((CompetitorWorkout)e.CurrentSelection.FirstOrDefault())?.CWorkout.WorkoutId;
                //CompetitorWorkout competitorWorkout = ((CompetitorWorkout)e.CurrentSelection.FirstOrDefault());
                await Shell.Current.GoToAsync($"competitorworkoutdetails?id={workoutId}");
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}