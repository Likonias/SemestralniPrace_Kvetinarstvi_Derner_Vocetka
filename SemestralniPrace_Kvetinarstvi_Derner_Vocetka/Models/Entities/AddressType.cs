using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class AddressType
    {
        public int Id { get; set; }
        public AddressTypeEnum addressType { get; set; }
    }
}
