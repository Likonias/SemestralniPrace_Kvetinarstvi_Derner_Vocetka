using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;

public class Billing
{
    public int Id { get; set; }
    public BillingTypeEnum BillingType { get; set; }
    public string? Note { get; set; }

    public Billing(int id, BillingTypeEnum billingType, string? note)
    {
        Id = id;
        BillingType = billingType;
        Note = note;
    }
}
