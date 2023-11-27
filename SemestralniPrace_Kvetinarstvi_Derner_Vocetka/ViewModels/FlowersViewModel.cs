﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class FlowersViewModel : ViewModelBase
    {

        private OracleDbUtil dbUtil;
        private DataTable tableData;

        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        public FlowersViewModel()
        {
            
            
            dbUtil = new OracleDbUtil();
            InitializeTableData();
        }
        private async void InitializeTableData()
        {
            TableData = await GetTable();
        }

        private async Task<DataTable> GetTable()
        {
            AddressRepository addressRepository = new AddressRepository();
            //await addressRepository.Add(new Address(1, "chrudimská", "1144", "Pardubice", "53002", null, null, null));
            await addressRepository.GetAll();
            return addressRepository.DataTable;
        }
    }
}
