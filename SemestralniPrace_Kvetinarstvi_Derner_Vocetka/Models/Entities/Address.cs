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
        public int Id { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int? EmployeeId {get; set;}
        public int? CustomerId { get; set; }
        public int? AddressTypeId { get; set; }

        public Address(int id, string street, string streetNumber, string city, string zip, int? employeeId, int? customerId, int? addressTypeId)
        {
            Id = id;
            Street = street;
            StreetNumber = streetNumber;
            City = city;
            Zip = zip;
            EmployeeId = employeeId;
            CustomerId = customerId;
            AddressTypeId = addressTypeId;
        }
    }
}
