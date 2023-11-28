using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
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
        public OccasionTypeEnum OccasionType { get; set; }
        public string? Note { get; set; }

        public Occasion(int id, OccasionTypeEnum occasionType, string? note)
        {
            Id = id;
            OccasionType = occasionType;
            Note = note;
        }
    }
}
