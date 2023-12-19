﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
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
    public class EmployeeViewModel : ViewModelBase
    {
        private INavigationService employeeForm;
        private INavigationService supervisorView;
        private EmployeeStore employeeStore;
        private DataTable tableData;

        public RelayCommand BtnAdd { get; }
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }
        public RelayCommand BtnSuperVisor { get; }
        public DataRowView SelectedItem { get; set; }
        
        public DataTable TableData
        {
            get { return tableData; } 
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        private EmployeeRepository employeeRepository;
        public EmployeeViewModel(INavigationService employeeForm, INavigationService supervisorView, EmployeeStore employeeStore)
        {
            BtnAdd = new RelayCommand(BtnAddPresseed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            BtnSuperVisor = new RelayCommand(BtnSupervisorClicked);
            this.employeeForm = employeeForm;
            this.employeeStore = employeeStore;
            this.supervisorView = supervisorView;
            tableData = new DataTable();
            employeeRepository = new EmployeeRepository();
            InitializeTableData();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                await employeeRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                employeeStore.Employee = await employeeRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                employeeForm.Navigate();
            }
        }

        private void BtnSupervisorClicked()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                employeeStore.Id = int.Parse(SelectedItem.Row[0].ToString());
                supervisorView.Navigate();
            }
        }

        private void BtnAddPresseed()
        {

            employeeStore.Employee = null;
            employeeForm.Navigate();

        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await employeeRepository.ConvertToDataTable();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                searchQuery = value;
                FilterTableData();
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private void FilterTableData()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                InitializeTableData(); // Reset to the original data if search query is empty
                return;
            }

            DataView dv = tableData.DefaultView;
            dv.RowFilter = $"FirstName LIKE '%{SearchQuery}%' OR LastName LIKE '%{SearchQuery}%' OR Email LIKE '%{SearchQuery}%' OR Tel LIKE '%{SearchQuery}%' OR Supervisor LIKE '%{SearchQuery}%' OR Position LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }

    }
}
