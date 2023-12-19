using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class BillingStore
    {
        private Action billingChanged;
        private Billing billing;

        public Billing Billing { get { return billing; } set { billing = value; OnCurrentBillingChanged(); } }

        public BillingStore()
        {
            billing = null;
        }

        public void OnCurrentBillingChanged()
        {
            billingChanged?.Invoke();
        }

        public BillingTypeEnum BillingType
        {
            get { return billing?.BillingType ?? BillingTypeEnum.Cash; }
            set
            {
                if (billing != null)
                {
                    billing.BillingType = value;
                    OnCurrentBillingChanged();
                }
            }
        }

        public string Note
        {
            get { return billing?.Note; }
            set
            {
                if (billing != null)
                {
                    billing.Note = value;
                    OnCurrentBillingChanged();
                }
            }
        }

        public void Subscribe(Action action)
        {
            billingChanged += action;
        }

        public void Unsubscribe(Action action)
        {
            billingChanged -= action;
        }
    }
}
