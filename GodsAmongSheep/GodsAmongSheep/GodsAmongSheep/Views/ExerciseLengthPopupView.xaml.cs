using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseLengthPopupView : PopupPage
    {
        private ExerciseLengthPopupViewModel _viewModel;
        public ExerciseLengthPopupView(Workout workout, ExerciseType type, AddWorkoutViewModel parent)
        {
            InitializeComponent();
            _viewModel = new ExerciseLengthPopupViewModel(workout, type, parent);
            BindingContext = _viewModel;
        }
    }
}