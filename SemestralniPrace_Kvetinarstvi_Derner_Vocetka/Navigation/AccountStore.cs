﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class AccountStore
    {

        private Account currentAccount;

        public Account CurrentAccount 
        { 
            get => currentAccount;  
            set 
            { 
                currentAccount = value; 
            }
        }

    }
}
