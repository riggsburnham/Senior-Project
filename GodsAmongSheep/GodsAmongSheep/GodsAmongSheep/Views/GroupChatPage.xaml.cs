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
    public partial class GroupChatPage : ContentPage
    {
        private GroupChatPageViewModel _viewModel;
        private WorkoutGroupPageViewModel _parent;
        private WorkoutGroup _workoutGroup;
        public GroupChatPage(WorkoutGroupPageViewModel parent, WorkoutGroup workoutGroup)
        {
            _parent = parent;
            _workoutGroup = workoutGroup;
            BindingContext = _viewModel = new GroupChatPageViewModel(_parent, _workoutGroup);
            InitializeComponent();
        }
    }
}