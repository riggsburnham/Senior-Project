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
    public partial class CreateWorkoutGroupPage : ContentPage
    {
        private WorkoutGroupsPageViewModel _parent;
        private CreateWorkoutGroupsPageViewModel _viewModel;
        public CreateWorkoutGroupPage(WorkoutGroupsPageViewModel parent)
        {
            _parent = parent;
            BindingContext = _viewModel = new CreateWorkoutGroupsPageViewModel(_parent);
            InitializeComponent();
        }
    }
}