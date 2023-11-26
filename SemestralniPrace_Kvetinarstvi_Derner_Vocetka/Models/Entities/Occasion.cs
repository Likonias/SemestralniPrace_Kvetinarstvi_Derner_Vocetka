using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities
{
    public class Occasion
    {
        public int Id { get; set; }
        public enum OccasionType
        {
            Wedding,
            Funeral,
            MothersDay,
            FathersDay,
            Birthday,
            ThanksGiving
        }
        public string? Note { get; set; }
    }
}
