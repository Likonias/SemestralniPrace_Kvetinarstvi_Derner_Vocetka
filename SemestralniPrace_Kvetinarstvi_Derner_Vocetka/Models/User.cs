using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    interface User
    {
        string Name { set; get; }
        string Surname { set; get; }
        string Email { set; get; }
        string Password { set; get; }
        int PhoneNumber { set; get; }

    }
}
