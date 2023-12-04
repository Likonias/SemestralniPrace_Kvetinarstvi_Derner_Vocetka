using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class EmployeeFormViewModel : ViewModelBase
    {
        private EmployeeStore employeeStore;
        private INavigationService closeNavSer;
        private bool isUpdated;
        private OracleDbUtil dbUtil;
        public bool IsUpdated
        {
            get { return isUpdated; }
            set
            {
                isUpdated = value;
                OnPropertyChanged("IsUpdated");
            }
        }
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
        private Employee employee;
        private INavigationService createEmployeeView;
        public EmployeeFormViewModel(EmployeeStore employeeStore, INavigationService closeNavSer, INavigationService createEmployeeView)
        {
            this.employeeStore = employeeStore;
            this.closeNavSer = closeNavSer;
            this.createEmployeeView = createEmployeeView;
            employee = employeeStore.Employee;
            if (employee != null) { InitializeEmployee(); IsUpdated = false; } else { IsUpdated = true; }
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            dbUtil = new OracleDbUtil();
            PopulateComboBoxes();
        }

        private void PopulateComboBoxes()
        {
            
        }

        private void Cancel()
        {
            employeeStore.Employee = null;
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (Password != null || !IsUpdated)
            {
                EmployeeRepository employeeRepository = new EmployeeRepository();
                if (employeeStore.Employee == null)
                {
                    bool passwordOk = PasswordHash.IsPasswordCorrect(PasswordHash.PasswordHashing(Password), PasswordHash.PasswordHashing(PasswordCheck));

                    bool isEmailViable = await dbUtil.ExecuteStoredEmailBooleanFunctionAsync("validateEmail", Email);
                    bool isEmailAvailable = await dbUtil.ExecuteStoredEmailBooleanFunctionAsync("emailExists", Email);

                    //todo position not working populate combobox

                    if (isEmailAvailable && isEmailViable && passwordOk)
                    {
                        await employeeRepository.Add(new Employee(0, FirstName, LastName, Wage, Email, Tel, IdSupervisor, PasswordHash.PasswordHashing(Password), Position));
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
                    await employeeRepository.Update(employeeStore.Employee.Id, FirstName, LastName, Wage, Email, Tel, IdSupervisor, Position);
                    closeNavSer.Navigate();
                }
                createEmployeeView.Navigate();
            }
            else
            {
                ErrorMessage = "Passwords dont match!";
            }
        }
        private void InitializeEmployee()
        {
            _firstName = employee.FirstName;
            _lastName = employee.LastName;
            _wage = employee.Wage;
            _email = employee.Email;
            _tel = employee.Tel;
            _idSupervisor = employee.IdSupervisor;
            _position = employee.Position;
        }

        private string _firstName;
        private string _lastName;
        private double _wage;
        private string _email;
        private string? _tel;
        private int? _idSupervisor;
        private string _password;
        private string _passwordCheck;
        private EmployeePositionEnum _position;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public double Wage
        {
            get => _wage;
            set
            {
                if (_wage != value)
                {
                    _wage = value;
                    OnPropertyChanged(nameof(Wage));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string? Tel
        {
            get => _tel;
            set
            {
                if (_tel != value)
                {
                    _tel = value;
                    OnPropertyChanged(nameof(Tel));
                }
            }
        }

        public int? IdSupervisor
        {
            get => _idSupervisor;
            set
            {
                if (_idSupervisor != value)
                {
                    _idSupervisor = value;
                    OnPropertyChanged(nameof(IdSupervisor));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string PasswordCheck
        {
            get { return _passwordCheck; }
            set
            {
                _passwordCheck = value;
                OnPropertyChanged(PasswordCheck);
            }
        }

        public EmployeePositionEnum Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

    }
}
