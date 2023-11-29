using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels.Forms
{
    public class AddressFormViewModel
    {
        public RelayCommand BtnCancel { get; private set; }
        public string Street { get; set; }

        public AddressFormViewModel()
        {
            Street = "asdf";
            BtnCancel = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
            
        }

    }
}
