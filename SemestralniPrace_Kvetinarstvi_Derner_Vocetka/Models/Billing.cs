using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;

public class Billing
{
    public BillingType BillingType { get; set; }
    public string Note { get; set; }

    public Billing(BillingType billingType, string note)
    {
        BillingType = billingType;
        Note = note;
    }
}
