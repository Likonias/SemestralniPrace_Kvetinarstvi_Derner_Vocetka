using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressTypeFormViewModel : ViewModelBase
    {
        //private readonly AddressTypeStore addressTypeStore;
        //private readonly INavigationService closeNavSer;
        //private readonly AddressTypeRepository addressTypeRepository;

        //public RelayCommand BtnCancel { get; private set; }
        //public RelayCommand BtnOk { get; private set; }

        //private string _addressType;
        //public string AddressType
        //{
        //    get => _addressType;
        //    set
        //    {
        //        _addressType = value;
        //        OnPropertyChanged(nameof(AddressType));
        //    }
        //}

        //private string errorMessage;
        //public string ErrorMessage
        //{
        //    get => errorMessage;
        //    set
        //    {
        //        errorMessage = value;
        //        OnPropertyChanged(nameof(ErrorMessage));
        //    }
        //}

        //public AddressTypeFormViewModel(INavigationService closeModalNavigationService, AddressTypeStore addressTypeStore)
        //{
        //    closeNavSer = closeModalNavigationService;
        //    BtnCancel = new RelayCommand(Cancel);
        //    BtnOk = new RelayCommand(Ok);
        //    this.addressTypeStore = addressTypeStore;
        //    addressTypeRepository = new AddressTypeRepository();
        //    if (addressTypeStore.AddressType != null) { InitializeAddressType(); }
        //}

        //private void Cancel()
        //{
        //    closeNavSer.Navigate();
        //}

        //private void Ok()
        //{
        //    if (CheckAddressType())
        //    {
        //        if (addressTypeStore.AddressType == null)
        //        {
        //            addressTypeRepository.Add(new AddressType(0, AddressType));
        //        }
        //        else
        //        {
        //            addressTypeRepository.Update(new AddressType(addressTypeStore.AddressType.Id, AddressType));
        //        }

        //        closeNavSer.Navigate();
        //    }
        //    else
        //    {
        //        ErrorMessage = "Adding failed!";
        //    }
        //}

        //private bool CheckAddressType()
        //{
        //    return !string.IsNullOrEmpty(AddressType);
        //}

        //private void InitializeAddressType()
        //{
        //    _addressType = addressTypeStore.AddressType.addressType;
        //}
    }
}
