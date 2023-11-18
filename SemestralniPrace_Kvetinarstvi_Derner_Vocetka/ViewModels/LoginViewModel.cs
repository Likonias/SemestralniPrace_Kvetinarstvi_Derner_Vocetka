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

        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }

        private OracleDbUtil dbUtil; // Use your OracleDbUtil class for database operations

        public LoginViewModel(Navigation navigation)
        {
            dbUtil = new OracleDbUtil(); // Initialize the database utility with the connection string
            CancelCommand = new NavigateCommand<MainViewModel>(navigation, () => new MainViewModel(navigation));
        }

        private bool CanLogin(object parameter)
        {
            // Add validation logic here if needed
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }

        private void Login(object parameter)
        {
            // Authenticate the user here, e.g., by checking the credentials against a database
            if (AuthenticateUser(Email, Password))
            {
                // Successful login
                MessageBox.Show("Login successful.");
                // Navigate to the next view or perform other actions as needed
            }
            else
            {
                ErrorMessage = "Invalid email or password. Please try again.";
            }
        }

        private bool AuthenticateUser(string email, string password)
        {
            // Use the ExecuteQuery method from OracleDbUtil to retrieve user data
            string query = "SELECT Jmeno, Prijmeni FROM Zakaznici WHERE Email = :email AND Heslo = :password";
            var parameters = new List<OracleParameter>
            {
                new OracleParameter("email", email),
                new OracleParameter("password", password)
            };

            DataTable result = dbUtil.ExecuteQuery(query, parameters);
            if (result != null && result.Rows.Count > 0)
            {
                // Successfully authenticated, you can load customer data from the database into your Customer model
                string firstName = result.Rows[0]["FirstName"].ToString();
                string lastName = result.Rows[0]["LastName"].ToString();

                // Instantiate a Customer model
                Customer authenticatedCustomer = new Customer(firstName, lastName, email, "", password);
                // You can now work with the authenticatedCustomer object
                return true;
            }

            return false;
        }
    }
}