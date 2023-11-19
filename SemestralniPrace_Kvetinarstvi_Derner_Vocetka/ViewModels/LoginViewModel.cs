using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System.ComponentModel;
using System.Windows.Input;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Services;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string email;
        private string password;
        private string errorMessage;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }
        
        public RelayCommand LoginCommand { get; }
        public ICommand CancelCommand { get; }

        private OracleDbUtil dbUtil; // Use your OracleDbUtil class for database operations
        public LoginViewModel(Navigation navigation)
        {
            dbUtil = new OracleDbUtil(); // Initialize the database utility with the connection string
            LoginCommand = new RelayCommand(Login);
            CancelCommand = new NavigateCommand<MainViewModel>(navigation, () => new MainViewModel(navigation));
        }

        private void Login()
        {

        }

    }
}