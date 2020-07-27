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
    public partial class FriendsListPage : ContentPage
    {
        private FriendsListPageViewModel _viewModel;
        private MainPageViewModel _parent;
        public FriendsListPage()
        {
            _parent = Shell.Current.BindingContext as MainPageViewModel;
            BindingContext = _viewModel = new FriendsListPageViewModel(_parent);
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            _viewModel.Update();
            _viewModel.AddFriendButtonShower(AddFriendButton);
        }

        public async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CollectionView)sender).SelectedItem != null)
            {
                var friendId = ((GasUser)e.CurrentSelection.FirstOrDefault())?.UserId;
                await Shell.Current.GoToAsync($"frienddetails?id={friendId}");
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}