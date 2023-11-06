using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }

        public Address(string street, string streetNumber, string zip, string city)
        {
            Street = street;
            StreetNumber = streetNumber;
            Zip = zip;
            City = city;
        }
    }
}
