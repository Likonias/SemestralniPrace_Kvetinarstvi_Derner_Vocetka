using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;
        private readonly AccountStore accountStore;
        
        private readonly NavigationServiceManager serviceManager;

        public App()
        {
            serviceManager = new NavigationServiceManager();
            
            // Register other navigation services here
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

        private INavigationService<AccountViewModel> CreateAccountNavigationService()
        {
            return new LayoutNavigationService<AccountViewModel>(navigationStore, () => new AccountViewModel(accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService<FlowersViewModel> CreateFlowersNavigationService()
        {
            return new LayoutNavigationService<FlowersViewModel>(navigationStore, () => new FlowersViewModel(), CreateNavigationBarViewModel);
        }

        private INavigationService<AddressViewModel> CreateAddressesNavigationService()
        {
            return new LayoutNavigationService<AddressViewModel>(navigationStore, () => new AddressViewModel(), CreateNavigationBarViewModel);
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            UpdateServiceManager();
            return new NavigationBarViewModel(accountStore, serviceManager);
        }


        private void UpdateServiceManager()
        {
            serviceManager.ClearNavigationService();
            serviceManager.RegisterNavigationService(CreateLoginNavigationService());
            serviceManager.RegisterNavigationService(CreateRegisterNavigationService());
            serviceManager.RegisterNavigationService(CreateAccountNavigationService());
            serviceManager.RegisterNavigationService(CreateFlowersNavigationService());
            serviceManager.RegisterNavigationService(CreateAddressesNavigationService());
        }

    }
}
