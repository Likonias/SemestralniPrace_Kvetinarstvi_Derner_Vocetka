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

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

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

            

            if (Password != null)
            {

                bool passwordOk = PasswordHash.IsPasswordCorrect(PasswordHash.PasswordHashing(Password), PasswordHash.PasswordHashing(PasswordCheck));

                string emailToValidate = "example@email.com";

                CustomerRepository customerRepository = new CustomerRepository();
                bool isEmailViable = await dbUtil.ExecuteStoredEmailBooleanFunctionAsync("validateEmail", Email);
                bool isEmailAvailable = await dbUtil.ExecuteStoredEmailBooleanFunctionAsync("emailExists", Email);


                if (isEmailAvailable && isEmailViable && passwordOk)
                {
                    await customerRepository.Add(new Customer(0, FirstName, LastName, Email, Tel, PasswordHash.PasswordHashing(Password)));
                    closeNavSer.Navigate();
                }
                else if (isEmailAvailable)
                {
                    ErrorMessage = "Incorrect Email!";
                }
                else
                {
                    ErrorMessage = "Wrong data input!";
                }
            }
            else
            {
                ErrorMessage = "Passwords dont match!";
            }
            
            
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(FirstName) ;
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(LastName);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(Email);
            }
        }

        private string _tel;
        public string Tel
        {
            get { return _tel; }
            set
            {
                _tel = value;
                OnPropertyChanged(Tel);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(Password);
            }
        }

        private string _passwordCheck;
        public string PasswordCheck
        {
            get { return _passwordCheck; }
            set
            {
                _passwordCheck = value;
                OnPropertyChanged(PasswordCheck);
            }
        }

    }
}
