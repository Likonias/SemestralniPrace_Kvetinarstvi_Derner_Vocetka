using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Price { get; set; }
    public int OrderId { get; set; }
    public byte[] InvoicePdf { get; set; }
}