using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Models;
using GodsAmongSheep.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginPageViewModel _viewModel;

        // TODO: currenty am calling this, left it in so code will execute, need to find a way to navigate
            // TODO: and to pass values at the same time while keep code in the view model 
        public LoginPage()
        {
            InitializeComponent();
            
            var test = BindingContext;
            
            //_viewModel = new LoginPageViewModel(Parent.BindingContext as MainPageViewModel);
        }

        public LoginPage(MainPageViewModel parent)
        {
            InitializeComponent();
            _viewModel = new LoginPageViewModel(parent);
            BindingContext = _viewModel;
        }
    }
}