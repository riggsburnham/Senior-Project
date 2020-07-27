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
    public partial class WorkoutDetailsPage : ContentPage
    {
        private MainPageViewModel _parent = Shell.Current.BindingContext as MainPageViewModel;
        private WorkoutDetailsViewModel _viewModel;
        private Workout _workout;

        public string Id
        {
            set
            {
                _workout = _parent.User.Workouts.FirstOrDefault(w => w.WorkoutId == Convert.ToInt32(Uri.UnescapeDataString(value)));
                _viewModel = new WorkoutDetailsViewModel(_workout, _parent);
                //BindingContext = _workout;
                BindingContext = _viewModel;
            }
        }
        public WorkoutDetailsPage()
        {
            InitializeComponent();
        }
    }
}