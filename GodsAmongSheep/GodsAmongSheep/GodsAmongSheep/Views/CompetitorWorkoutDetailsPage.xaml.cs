using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Models;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Shared.Models;
using GodsAmongSheep.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Id", "id")]
    public partial class CompetitorWorkoutDetailsPage : ContentPage
    {
        private Workout _workout;
        private CompetitorWorkout _cWorkout;
        private CompetitorWorkoutDetailsPageViewModel _viewModel;
        public CompetitorWorkoutDetailsPage()
        {
            InitializeComponent();
        }

        public string Id
        {
            set
            {
                using (GasContext context = new GasContext())
                {
                    context.SetupServer();
                    GasContextController contextController = new GasContextController(context);
                    GasWorkoutsController workoutsController = contextController.GasWorkoutsController;
                    _workout = workoutsController.FindWorkout(Convert.ToInt32(Uri.UnescapeDataString(value)));
                }
                BindingContext = _viewModel = new CompetitorWorkoutDetailsPageViewModel(_workout);
            }
        }
    }
}