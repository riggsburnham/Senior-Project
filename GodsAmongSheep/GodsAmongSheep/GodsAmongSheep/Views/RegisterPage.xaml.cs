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
    public partial class RegisterPage : ContentPage
    {
        private RegisterPageViewModel _viewModel;
        public RegisterPage(MainPageViewModel parent)
        {
            InitializeComponent();
            _viewModel = new RegisterPageViewModel(parent);
            BindingContext = _viewModel;
        }
    }
}