using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Password { get; set; }

        public Customer(string firstName, string lastName, string email, string tel, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Tel = tel;
            Password = password;
        }
    }
}
