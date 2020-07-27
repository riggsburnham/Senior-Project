using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodsAmongSheep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InviteFriendPopupView : PopupPage
    {
        private InviteFriendPopupViewModel _viewModel;
        private ManageWorkoutGroupViewModel _parent;
        public InviteFriendPopupView(ManageWorkoutGroupViewModel parent)
        {
            _parent = parent;
            BindingContext = _viewModel = new InviteFriendPopupViewModel(_parent);
            InitializeComponent();
        }
    }
}