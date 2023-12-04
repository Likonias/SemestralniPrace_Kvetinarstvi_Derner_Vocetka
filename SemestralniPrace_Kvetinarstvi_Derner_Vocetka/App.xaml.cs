using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
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
        private readonly CustomerStore customerStore;
        private readonly EmployeeStore employeeStore;
        private readonly OrderStore orderStore;
        private readonly OtherGoodsStore otherGoodsStore;
        private readonly FlowerStore flowerStore;


        private readonly NavigationServiceManager serviceManager;

        public App()
        {
            serviceManager = new NavigationServiceManager();
            flowerStore = new FlowerStore();
            // Register other navigation services here
            navigationStore = new NavigationStore();
            modalNavigationStore = new ModalNavigationStore();
            accountStore = new AccountStore();
            addressStore = new AddressStore();
            customerStore = new CustomerStore();
            employeeStore = new EmployeeStore();
            orderStore = new OrderStore();
            otherGoodsStore = new OtherGoodsStore();
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
            return new ModalNavigationService<LoginViewModel>(modalNavigationStore, () => new LoginViewModel(accountStore, new CloseModalNavigationService(modalNavigationStore), CreateAccountNavigationService()));
        }

        private INavigationService CreateAccountNavigationService()
        {
            return new LayoutNavigationService<AccountViewModel>(navigationStore, () => new AccountViewModel(accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateFlowersNavigationService()
        {
            return new LayoutNavigationService<FlowersViewModel>(navigationStore, () => new FlowersViewModel(CreateFlowersNavigationService(), flowerStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateFlowersFormNavigationService() {
            return new ModalNavigationService<FlowerFormViewModel>(modalNavigationStore, () => new FlowerFormViewModel(accountStore, CreateCloseModalNavigationService(), flowerStore, CreateFlowersNavigationService()));
        }

        private INavigationService CreateAddressesNavigationService()
        {
            return new LayoutNavigationService<AddressViewModel>(navigationStore, () => new AddressViewModel(CreateAddressesFormNavigationService(), addressStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateAddressesFormNavigationService()
        {
            return new ModalNavigationService<AddressFormViewModel>(modalNavigationStore, () => new AddressFormViewModel(accountStore,CreateCloseModalNavigationService(), addressStore, CreateAddressesNavigationService()));
        }

        private INavigationService CreateCustomerNavigationService()
        {
            return new LayoutNavigationService<CustomerViewModel>(navigationStore, () => new CustomerViewModel(CreateCustomerFormNavigationService(), customerStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateCustomerFormNavigationService()
        {
            return new ModalNavigationService<CustomerFormViewModel>(modalNavigationStore, () => new CustomerFormViewModel(customerStore, CreateCloseModalNavigationService(), CreateCustomerNavigationService()));
        }

        private INavigationService CreateEmployeeNavigationService()
        {
            return new LayoutNavigationService<EmployeeViewModel>(navigationStore, () => new EmployeeViewModel(CreateEmployeeFormNavigationService(), employeeStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateEmployeeFormNavigationService()
        {
            return new ModalNavigationService<EmployeeFormViewModel>(modalNavigationStore, () => new EmployeeFormViewModel(employeeStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateOrderNavigationService()
        {
            return new LayoutNavigationService<OrderViewModel>(navigationStore, () => new OrderViewModel(CreateOrderFormNavigationService(), orderStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOrderFormNavigationService()
        {
            return new ModalNavigationService<OrderFormViewModel>(modalNavigationStore, () => new OrderFormViewModel(orderStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateOtherGoodsNavigationService()
        {
            return new LayoutNavigationService<OtherGoodsViewModel>(navigationStore, () => new OtherGoodsViewModel(CreateOtherGoodsFormNavigationService(), otherGoodsStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOtherGoodsFormNavigationService()
        {
            return new ModalNavigationService<OtherGoodsFormViewModel>(modalNavigationStore, () => new OtherGoodsFormViewModel(otherGoodsStore, CreateCloseModalNavigationService()));
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            UpdateServiceManager();
            return new NavigationBarViewModel(accountStore, serviceManager);
        }

        private INavigationService CreateCloseModalNavigationService()
        {
            return new CloseModalNavigationService(modalNavigationStore);
        }


        private void UpdateServiceManager()
        {
            serviceManager.ClearNavigationService();
            serviceManager.RegisterNavigationService<LoginViewModel>(CreateLoginNavigationService());
            serviceManager.RegisterNavigationService<MainViewModel>(CreateMainNavigationService());
            serviceManager.RegisterNavigationService<AccountViewModel>(CreateAccountNavigationService());
            serviceManager.RegisterNavigationService<FlowersViewModel>(CreateFlowersNavigationService());
            serviceManager.RegisterNavigationService<FlowerFormViewModel>(CreateFlowersFormNavigationService());
            serviceManager.RegisterNavigationService<AddressViewModel>(CreateAddressesNavigationService());
            serviceManager.RegisterNavigationService<CustomerViewModel>(CreateCustomerNavigationService());
            serviceManager.RegisterNavigationService<CustomerFormViewModel>(CreateCustomerFormNavigationService());
            serviceManager.RegisterNavigationService<EmployeeViewModel>(CreateEmployeeNavigationService());
            serviceManager.RegisterNavigationService<EmployeeFormViewModel>(CreateEmployeeFormNavigationService());
            serviceManager.RegisterNavigationService<OrderViewModel>(CreateOrderNavigationService());
            serviceManager.RegisterNavigationService<OrderFormViewModel>(CreateOrderFormNavigationService());
            serviceManager.RegisterNavigationService<OtherGoodsViewModel>(CreateOtherGoodsNavigationService());
            serviceManager.RegisterNavigationService<OtherGoodsFormViewModel>(CreateOtherGoodsFormNavigationService());
        }

    }
}
