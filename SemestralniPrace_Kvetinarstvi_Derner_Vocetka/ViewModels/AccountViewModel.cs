using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private AccountStore accountStore;
        private OracleDbUtil dbUtil;
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged(Name); } }
        private string email;
        public string Email { get { return email; } set { email = value; OnPropertyChanged(Email); } }
        private bool isCheckedPrivate;
        public bool IsCheckedPrivate
        {
            get { return isCheckedPrivate; }
            set
            {
                
                isCheckedPrivate = value;
                if (!isBoot)
                {
                    if (IsCheckedPrivate)
                    {
                        var parameters = new Dictionary<string, object>
                    {
                        { "p_id", accountStore.CurrentAccount.Id },
                    };
                        dbUtil.ExecuteStoredProcedureAsync("UDELEJ_PRIVATNIHO_ZAKAZNIKA", parameters);
                    }
                    else
                    {
                        var parameters = new Dictionary<string, object>
                    {
                        { "p_id", accountStore.CurrentAccount.Id },
                    };
                        dbUtil.ExecuteStoredProcedureAsync("UDELEJ_PUBLIC_ZAKAZNIKA", parameters);
                    }
                    navigateAccount.Navigate();
                }
                isBoot = false;
                OnPropertyChanged(nameof(IsCheckedPrivate));

            }
        }
        private bool isCustomer;
        public bool IsCustomer
        {
            get { return isCustomer; }
            set
            {
                isCustomer = value;
                OnPropertyChanged(nameof(IsCustomer));
            }
        }
        private bool isBoot;
        private INavigationService navigateAccount;
        public AccountViewModel(AccountStore accountStore, INavigationService navigateAccount)
        {
            dbUtil = new OracleDbUtil();
            this.navigateAccount = navigateAccount;
            isBoot = true;
            this.accountStore = accountStore;
            IsCustomer = accountStore.CurrentAccount.EmployeePosition == null;
            Name = accountStore.CurrentAccount.FirstName + " " + accountStore.CurrentAccount.LastName;
            Email = accountStore.CurrentAccount.Email;
            CheckIsPrivate();
        }

        private async void CheckIsPrivate()
        {
            if(IsCustomer)
            {
                var parameters = new Dictionary<string, object>
            {
                { "p_id", accountStore.CurrentAccount.Id },
            };
                DataTable isPrivate = await dbUtil.ExecuteCommandAsync("check_anonymous_zakaznik", parameters);
                DataRow firstRow = isPrivate.Rows[0];
                string str = firstRow["EXISTS_IN_ANONYMOUS_ZAKAZNICI"].ToString();
                if (str == "FALSE")
                {
                    IsCheckedPrivate = false;
                }
                else
                {
                    IsCheckedPrivate = true;
                }
            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
