using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class InvoiceStore
    {
        private Action invoiceChanged;
        private Models.Invoice invoice;

        public Models.Invoice Invoice
        {
            get { return invoice; }
            set
            {
                invoice = value;
                OnCurrentInvoiceChanged();
            }
        }

        public void OnCurrentInvoiceChanged()
        {
            invoiceChanged?.Invoke();
        }
    }
}
