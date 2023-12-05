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
        private readonly AddressTypeStore addressTypeStore;
        private readonly BillingStore billingStore;
        private readonly DeliveryMethodStore deliveryMethodStore;
        private readonly DeliveryStore deliveryStore;
        private readonly InPersonPickupStore inPersonPickupStore;
        private readonly InvoiceStore invoiceStore;
        private readonly OccasionStore occasionStore;
        private readonly OrderStatusStore orderStatusStore;

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
            return new LayoutNavigationService<FlowersViewModel>(navigationStore, () => new FlowersViewModel(CreateFlowersFormNavigationService(), flowerStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateFlowersFormNavigationService() {
            return new ModalNavigationService<FlowerFormViewModel>(modalNavigationStore, () => new FlowerFormViewModel(CreateCloseModalNavigationService(), flowerStore, CreateFlowersNavigationService()));
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
            return new ModalNavigationService<EmployeeFormViewModel>(modalNavigationStore, () => new EmployeeFormViewModel(employeeStore, CreateCloseModalNavigationService(), CreateEmployeeNavigationService()));
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
            return new ModalNavigationService<OtherGoodsFormViewModel>(modalNavigationStore, () => new OtherGoodsFormViewModel(CreateCloseModalNavigationService(), otherGoodsStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateAddressTypeNavigationService()
        {
            return new LayoutNavigationService<AddressTypeViewModel>(navigationStore, () => new AddressTypeViewModel(CreateAddressTypeFormNavigationService(), addressTypeStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateAddressTypeFormNavigationService()
        {
            return new ModalNavigationService<AddressTypeFormViewModel>(modalNavigationStore, () => new AddressTypeFormViewModel(addressTypeStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateBillingNavigationService()
        {
            return new LayoutNavigationService<BillingViewModel>(navigationStore, () => new BillingViewModel(CreateBillingFormNavigationService(), billingStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateBillingFormNavigationService()
        {
            return new ModalNavigationService<BillingFormViewModel>(modalNavigationStore, () => new BillingFormViewModel(billingStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateDeliveryMethodNavigationService()
        {
            return new LayoutNavigationService<DeliveryMethodViewModel>(navigationStore, () => new DeliveryMethodViewModel(CreateDeliveryMethodFormNavigationService(), deliveryStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateDeliveryMethodFormNavigationService()
        {
            return new ModalNavigationService<DeliveryMethodFormViewModel>(modalNavigationStore, () => new DeliveryMethodFormViewModel(deliveryStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateDeliveryNavigationService()
        {
            return new LayoutNavigationService<DeliveryViewModel>(navigationStore, () => new DeliveryViewModel(CreateDeliveryFormNavigationService(), deliveryStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateDeliveryFormNavigationService()
        {
            return new ModalNavigationService<DeliveryFormViewModel>(modalNavigationStore, () => new DeliveryFormViewModel(deliveryStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateInPersonPickupNavigationService()
        {
            return new LayoutNavigationService<InPersonPickupViewModel>(navigationStore, () => new InPersonPickupViewModel(CreateInPersonPicupFormNavigationService(), inPersonPickupStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateInPersonPicupFormNavigationService()
        {
            return new ModalNavigationService<InPersonPickupFormViewModel>(modalNavigationStore, () => new InPersonPickupFormViewModel(inPersonPickupStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateInvoiceNavigationService()
        {
            return new LayoutNavigationService<InvoiceViewModel>(navigationStore, () => new InvoiceViewModel(CreateInvoiceFormNavigationService(), invoiceStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateInvoiceFormNavigationService()
        {
            return new ModalNavigationService<InvoiceFormViewModel>(modalNavigationStore, () => new InvoiceFormViewModel(invoiceStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateOccasionNavigationService()
        {
            return new LayoutNavigationService<OccasionViewModel>(navigationStore, () => new OccasionViewModel(CreateOccasionFormNavigationService(), occasionStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOccasionFormNavigationService()
        {
            return new ModalNavigationService<OccasionFormViewModel>(modalNavigationStore, () => new OccasionFormViewModel(occasionStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateOrderStatusNavigationService()
        {
            return new LayoutNavigationService<OrderStatusViewModel>(navigationStore, () => new OrderStatusViewModel(CreateOrderStatusFormNavigationService(), orderStatusStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOrderStatusFormNavigationService()
        {
            return new ModalNavigationService<OrderStatusFormViewModel>(modalNavigationStore, () => new OrderStatusFormViewModel(orderStatusStore, CreateCloseModalNavigationService()));
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
            serviceManager.RegisterNavigationService<AddressTypeViewModel>(CreateAddressTypeNavigationService());
            serviceManager.RegisterNavigationService<AddressTypeFormViewModel>(CreateAddressTypeFormNavigationService());
            serviceManager.RegisterNavigationService<BillingViewModel>(CreateBillingNavigationService());
            serviceManager.RegisterNavigationService<BillingFormViewModel>(CreateBillingFormNavigationService());
            serviceManager.RegisterNavigationService<DeliveryMethodViewModel>(CreateDeliveryMethodNavigationService());
            serviceManager.RegisterNavigationService<DeliveryMethodFormViewModel>(CreateDeliveryMethodFormNavigationService());
            serviceManager.RegisterNavigationService<DeliveryViewModel>(CreateDeliveryNavigationService());
            serviceManager.RegisterNavigationService<DeliveryFormViewModel>(CreateDeliveryFormNavigationService());
            serviceManager.RegisterNavigationService<InPersonPickupViewModel>(CreateInPersonPickupNavigationService());
            serviceManager.RegisterNavigationService<InPersonPickupFormViewModel>(CreateInPersonPicupFormNavigationService());
            serviceManager.RegisterNavigationService<InvoiceViewModel>(CreateInvoiceNavigationService());
            serviceManager.RegisterNavigationService<InvoiceFormViewModel>(CreateInvoiceFormNavigationService());
            serviceManager.RegisterNavigationService<OccasionViewModel>(CreateOccasionNavigationService());
            serviceManager.RegisterNavigationService<OccasionFormViewModel>(CreateOccasionFormNavigationService());
            serviceManager.RegisterNavigationService<OrderStatusViewModel>(CreateOrderStatusNavigationService());
            serviceManager.RegisterNavigationService<OrderStatusFormViewModel>(CreateOrderStatusFormNavigationService());

        }

    }
}
