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
    public partial class ManageWorkoutGroupPage : ContentPage
    {
        private ManageWorkoutGroupViewModel _viewModel;
        private WorkoutGroupsPageViewModel _parent;
        private WorkoutGroup _workoutGroupToManage;
        public ManageWorkoutGroupPage(WorkoutGroupsPageViewModel parent, WorkoutGroup workoutGroupToManage)
        {
            _parent = parent;
            _workoutGroupToManage = workoutGroupToManage;
            BindingContext = _viewModel = new ManageWorkoutGroupViewModel(_parent, _workoutGroupToManage);
            InitializeComponent();
        }
    }
}