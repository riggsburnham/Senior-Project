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
    public partial class LogoutPage : ContentPage
    {
        private LogoutPageViewModel _viewModel;

        public LogoutPage(MainPageViewModel parent)
        {
            InitializeComponent();
            _viewModel = new LogoutPageViewModel(parent);
            BindingContext = _viewModel;
        }
    }
}