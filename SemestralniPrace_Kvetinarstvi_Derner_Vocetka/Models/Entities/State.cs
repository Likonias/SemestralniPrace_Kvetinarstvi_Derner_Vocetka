using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;

public class State
{
    public DateTime DateOfDelivery { get; set; }
    public DateTime DateOfBilling { get; set; }

    public State(DateTime dateOfDelivery, DateTime dateOfBilling)
    {
        DateOfDelivery = dateOfDelivery;
        DateOfBilling = dateOfBilling;
    }
}