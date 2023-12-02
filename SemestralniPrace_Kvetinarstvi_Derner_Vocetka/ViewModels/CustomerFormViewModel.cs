using Oracle.ManagedDataAccess.Client;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class CustomerFormViewModel : ViewModelBase
    {

        private CustomerStore customerStore;
        private INavigationService closeNavSer;
        private Customer customer;
        private OracleDbUtil dbUtil;
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }

        public string ErrorMessage { get; set; }

        public CustomerFormViewModel(CustomerStore customerStore, INavigationService closeNavSer)
        {
            this.customerStore = customerStore;
            this.closeNavSer = closeNavSer;
            customer = customerStore.Customer;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            dbUtil = new OracleDbUtil();
            
        }

        private void Cancel()
        {
            customerStore.Customer = null;
            closeNavSer.Navigate();
        }

        private void Ok()
        {
            OkAsync();
        }
        private async Task OkAsync()
        {
            customer = new Customer(0, FirstName, LastName, Email, Tel, PasswordHash.PasswordHashing(Password));
            CustomerRepository customerRepository = new CustomerRepository();
            //for validation Regex.IsMatch(email, pattern); string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            string emailToValidate = "example@email.com";
            //todo check if email belongs to someone already, should return bool
            bool returnTableBool = await dbUtil.ExecuteStoredEmailBooleanFunctionAsync("validateEmail", emailToValidate);
            bool isEmailAvailable = await dbUtil.ExecuteStoredEmailBooleanFunctionAsync("emailExists", emailToValidate);
            

            if(isEmailAvailable)
            {
                await customerRepository.Add(customer);
            }
            closeNavSer.Navigate();
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _tel;
        public string Tel
        {
            get { return _tel; }
            set
            {
                _tel = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _passwordCheck;
        public string PasswordCheck
        {
            get { return _passwordCheck; }
            set
            {
                _passwordCheck = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
