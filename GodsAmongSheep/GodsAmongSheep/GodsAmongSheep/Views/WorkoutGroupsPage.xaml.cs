using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutGroupsPage : ContentPage
    {
        private MainPageViewModel _parent;
        private WorkoutGroupsPageViewModel _viewModel;
        public WorkoutGroupsPage(MainPageViewModel parent)
        {
            _parent = parent;
            BindingContext = _viewModel = new WorkoutGroupsPageViewModel(_parent);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeLV(WorkoutGroupsLV);
        }
    }
}