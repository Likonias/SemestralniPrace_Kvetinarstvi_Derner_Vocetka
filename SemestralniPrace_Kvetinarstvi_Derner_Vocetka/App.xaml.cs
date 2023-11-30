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
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly AccountStore accountStore;
        private readonly AddressStore addressStore;
        
        private readonly NavigationServiceManager serviceManager;

        public App()
        {
            serviceManager = new NavigationServiceManager();
            
            // Register other navigation services here
            navigationStore = new NavigationStore();
            modalNavigationStore = new ModalNavigationStore();
            accountStore = new AccountStore();
            addressStore = new AddressStore();
            CreateNavigationBarViewModel(); 
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService mainNavigationService = CreateMainNavigationService();
            mainNavigationService.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(navigationStore, modalNavigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService CreateMainNavigationService()
        {
            return new LayoutNavigationService<MainViewModel>(navigationStore, () => new MainViewModel(CreateLoginNavigationService()), CreateNavigationBarViewModel);
        }

        private INavigationService CreateLoginNavigationService()
        {
            return new ModalNavigationService<LoginViewModel>(modalNavigationStore, () => new LoginViewModel(accountStore, new CloseModalNavigationService(modalNavigationStore)));
        }

        private INavigationService CreateRegisterNavigationService()
        {
            return new NavigationService<RegisterViewModel>(navigationStore, () => new RegisterViewModel(CreateMainNavigationService()));
        }

        private INavigationService CreateAccountNavigationService()
        {
            return new LayoutNavigationService<AccountViewModel>(navigationStore, () => new AccountViewModel(accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateFlowersNavigationService()
        {
            return new LayoutNavigationService<FlowersViewModel>(navigationStore, () => new FlowersViewModel(), CreateNavigationBarViewModel);
        }

        private INavigationService CreateAddressesNavigationService()
        {
            return new LayoutNavigationService<AddressViewModel>(navigationStore, () => new AddressViewModel(CreateAddressesFormNavigationService(), addressStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateAddressesFormNavigationService()
        {
            return new ModalNavigationService<AddressFormViewModel>(modalNavigationStore, () => new AddressFormViewModel(new CloseModalNavigationService(modalNavigationStore), addressStore));
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            UpdateServiceManager();
            return new NavigationBarViewModel(accountStore, serviceManager);
        }


        private void UpdateServiceManager()
        {
            serviceManager.ClearNavigationService();
            serviceManager.RegisterNavigationService<LoginViewModel>(CreateLoginNavigationService());
            serviceManager.RegisterNavigationService<RegisterViewModel>(CreateRegisterNavigationService());
            serviceManager.RegisterNavigationService<AccountViewModel>(CreateAccountNavigationService());
            serviceManager.RegisterNavigationService<FlowersViewModel>(CreateFlowersNavigationService());
            serviceManager.RegisterNavigationService<AddressViewModel>(CreateAddressesNavigationService());
        }

    }
}
