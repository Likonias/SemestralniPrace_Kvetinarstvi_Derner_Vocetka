﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views;
using System;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;
        private readonly AccountStore accountStore;
        
        public App()
        {
            navigationStore = new NavigationStore();
            accountStore = new AccountStore();
            CreateNavigationBarViewModel(); 
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService<MainViewModel> mainNavigationService = CreateMainNavigationService();
            mainNavigationService.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService<MainViewModel> CreateMainNavigationService()
        {
            return new LayoutNavigationService<MainViewModel>(navigationStore, () => new MainViewModel(CreateLoginNavigationService()), CreateNavigationBarViewModel);
        }

        private INavigationService<LoginViewModel> CreateLoginNavigationService()
        {
            return new NavigationService<LoginViewModel>(navigationStore, () => new LoginViewModel(CreateMainNavigationService()));
        }


        private INavigationService<RegisterViewModel> CreateRegisterNavigationService()
        {
            return new NavigationService<RegisterViewModel>(navigationStore, () => new RegisterViewModel(CreateMainNavigationService()));
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            return new NavigationBarViewModel(accountStore, CreateLoginNavigationService(), CreateRegisterNavigationService());
        }
    }
}
