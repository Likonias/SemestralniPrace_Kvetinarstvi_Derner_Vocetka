﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class UserOtherViewModel : ViewModelBase
    {

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
        private OracleDbUtil dbUtil;

        public UserOtherViewModel()
        {
            this.dbUtil = new OracleDbUtil();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            //TableData = 
        }
    }
}