using System;
using GodsAmongSheep.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GodsAmongSheep.Shared;
using GodsAmongSheep.Shared.Controllers;
using GodsAmongSheep.Views;

namespace GodsAmongSheep
{
    public partial class App : Application
    {
        public GasContext _gasContext;
        public GasContextController _gasContextController;
        public App()
        {
            _gasContext = new GasContext();
            _gasContext.SetupServer();
            _gasContextController = new GasContextController(_gasContext);
            MainPage = new AppShell(_gasContextController);
            InitializeComponent();
        }

        //protected override void RegisterTypes(IContainerRegistry containerRegistry)
        //{

        //}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
