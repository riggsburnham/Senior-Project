using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Models;
using GodsAmongSheep.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddWorkoutsPage : ContentPage
    {
        private AddWorkoutViewModel _viewModel;
        private MainPageViewModel _parent;
        public AddWorkoutsPage()
        {
            InitializeComponent();
            _viewModel = new AddWorkoutViewModel();
            BindingContext = _viewModel;
        }

        public AddWorkoutsPage(MainPageViewModel parent)
        {
            InitializeComponent();
            _parent = parent;
            _viewModel = new AddWorkoutViewModel(_parent);
            BindingContext = _viewModel;
        }
    }
}