﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class AddressStore
    {
        private Action addressChanged;
        private Address address;

        public Address Address { get { return address; } set {  address = value; OnCurrentAddressChanged(); } }

        private void OnCurrentAddressChanged()
        {
            addressChanged?.Invoke();
        }
    }
}
