using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;

public class Invoice
{
    public DateTime Date { get; set; }
    public double Price { get; set; }
    public byte[] InvoicePdf { get; set; }
}