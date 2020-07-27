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
    public partial class NotificationsPage : ContentPage
    {
        private MainPageViewModel _parent;
        private NotificationsPageViewModel _viewModel;

        public NotificationsPage(MainPageViewModel parent)
        {
            _parent = parent;
            BindingContext = _viewModel = new NotificationsPageViewModel(_parent);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeLV(FriendRequestsLV, WorkoutGroupRequestsLV);
            _viewModel.InitializeBT(AcceptFriendRequestBT, DeclineFriendRequestBT,
                                    AcceptWorkoutGroupRequestBT, DeclineWorkoutGroupRequestBT);
        }
    }
}