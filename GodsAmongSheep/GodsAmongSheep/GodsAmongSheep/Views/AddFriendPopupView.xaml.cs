using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFriendPopupView : PopupPage
    {
        private AddFriendPopupViewModel _viewModel;
        private MainPageViewModel _root;
        private FriendsListPageViewModel _parent;

        public AddFriendPopupView(FriendsListPageViewModel parent)
        {
            InitializeComponent();
            _root = Shell.Current.BindingContext as MainPageViewModel;
            _parent = parent;
            BindingContext = _viewModel = new AddFriendPopupViewModel(SearchResultsLV, _parent, _root);
        }
    }
}