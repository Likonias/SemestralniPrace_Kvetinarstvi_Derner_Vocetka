using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public interface INavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        void Navigate();
    }
}