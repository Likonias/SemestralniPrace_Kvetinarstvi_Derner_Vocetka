using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class CloseModalNavigationService : INavigationService
    {
        private readonly ModalNavigationStore modalNavigationStore;

        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore)
        {
            this.modalNavigationStore = modalNavigationStore;
        }

        public void Navigate()
        {
            modalNavigationStore.Close();
        }
    }
}
