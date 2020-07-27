using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutsPage : ContentPage
    {
        private MainPageViewModel _parent;
        private WorkoutsViewModel _viewModel;

        public WorkoutsPage()
        {
            _parent = Shell.Current.BindingContext as MainPageViewModel;
            _viewModel = new WorkoutsViewModel(_parent);
            BindingContext = _viewModel;
            InitializeComponent();
        }

        public WorkoutsPage(MainPageViewModel parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _viewModel.UpdateCV(this.WorkoutsCollectionView);
        }

        public async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CollectionView)sender).SelectedItem != null)
            {
                var workoutId = ((Workout)e.CurrentSelection.FirstOrDefault())?.WorkoutId;      
                await Shell.Current.GoToAsync($"workoutdetails?id={workoutId}");            
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}