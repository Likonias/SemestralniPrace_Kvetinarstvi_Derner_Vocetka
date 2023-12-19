using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InvoiceFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }

        private readonly InvoiceStore invoiceStore;
        private INavigationService closeNavSer;
        private Invoice invoice;
        private INavigationService openInvoiceViewModel;

        public string ErrorMessage { get; set; }
        public ObservableCollection<string> BillingTypeComboBoxItems { get; set; }

        public InvoiceFormViewModel(INavigationService closeModalNavigationService, InvoiceStore invoiceStore, INavigationService? openInvoiceViewModel)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            invoice = invoiceStore.Invoice;
            this.invoiceStore = invoiceStore;
            this.openInvoiceViewModel = openInvoiceViewModel;

            if (invoice != null)
            {
                InitializeInvoice();
            }

            BillingTypeComboBoxItems = new ObservableCollection<string>();
            PopulateBillingTypeComboBox();
        }

        private void PopulateBillingTypeComboBox()
        {
            BillingTypeComboBoxItems.Clear();

            foreach (BillingTypeEnum value in Enum.GetValues(typeof(BillingTypeEnum)))
            {
                BillingTypeComboBoxItems.Add(value.ToString());
            }
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (CheckInvoice())
            {
                InvoiceRepository invoiceRepository = new InvoiceRepository();

                if (invoiceStore.Invoice == null)
                {
                    invoice = new Invoice(0, Date, Price, OrderId, InvoicePdf);
                    await invoiceRepository.Add(invoice);
                }
                else
                {
                    invoice = new Invoice(invoiceStore.Invoice.Id, Date, Price, OrderId, InvoicePdf);
                    await invoiceRepository.Update(invoice);
                }

                closeNavSer.Navigate();
                openInvoiceViewModel.Navigate();
            }
            else
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckInvoice()
        {
            return Date != null || Price != 0 || OrderId != null || InvoicePdf != null;
        }

        private void InitializeInvoice()
        {
            _date = invoice.Date;
            _price = invoice.Price;
            _orderId = invoice.OrderId;
            _invoicePdf = invoice.InvoicePdf;
        }

        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private DateTime? _date;

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private double _price;

        public int? OrderId
        {
            get => _orderId;
            set
            {
                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }

        private int? _orderId;

        public byte[] InvoicePdf
        {
            get => _invoicePdf;
            set
            {
                _invoicePdf = value;
                OnPropertyChanged(nameof(InvoicePdf));
            }
        }

        private byte[] _invoicePdf;
    }
}
