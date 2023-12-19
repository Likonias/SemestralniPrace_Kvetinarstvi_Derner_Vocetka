using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils
{
    public class LowStockLogChecker
    {
        //todo finish checking stock
        private Timer timer;
        private OracleDbUtil dbUtil;
        private DataTable currentData;
        private DataTable latestData;
        private AccountStore accountStore;
        private INavigationService navigateLowStockLog;
        public DataTable TableData { get; set; }
        public LowStockLogChecker(AccountStore accountStore, INavigationService navigateLowStockLog)
        {
            dbUtil = new OracleDbUtil();
            this.navigateLowStockLog = navigateLowStockLog;
            this.accountStore = accountStore;
            TableData = new DataTable();
            InitializeData();
            CheckLowStockLog(null);
            timer = new Timer(CheckLowStockLog, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private async void InitializeData()
        {
            latestData = await dbUtil.ExecuteCommandAsync("GET_LOWSTOCKLOG");
            if(latestData.Rows.Count > 0)
            {
                TableData = new DataTable();
                TableData.Columns.Add("Name", typeof(string));
                TableData.Columns.Add("PreviousStock", typeof(int));
                TableData.Columns.Add("NewStock", typeof(int));
                TableData.Columns.Add("AlertDate", typeof(string));

                foreach (DataRow row in latestData.Rows)
                {
                    TableData.Rows.Add(
                        row["ITEM_NAME"].ToString(),
                        Convert.ToInt32(row["PREVIOUS_STOCK"]),
                        Convert.ToInt32(row["NEW_STOCK"]),
                        row["ALERT_DATE"].ToString()
                    );
                }
            }
        }

        private async void CheckLowStockLog(object state)
        {
            // Perform the logic to check for updates in LowStockLog table
            // For example, you could fetch the latest entries or compare timestamps
            
            currentData = latestData;
            if(currentData.Rows.Count == 0)
            {
                TableData = new DataTable();
            }
            // Here's an example fetching the data from LowStockLog
            latestData = await dbUtil.ExecuteCommandAsync("GET_LOWSTOCKLOG");

            if (latestData != null && accountStore.CurrentAccount != null && latestData.Rows.Count > 0)
            {
                if(CompareDataTableData(latestData, currentData) && CompareDataTableStructure(latestData, currentData))
                {
                    
                }
                else
                {
                    if(accountStore.CurrentAccount.EmployeePosition != null)
                    {
                        TableData = new DataTable();
                        TableData.Columns.Add("Name", typeof(string));
                        TableData.Columns.Add("PreviousStock", typeof(int));
                        TableData.Columns.Add("NewStock", typeof(int));
                        TableData.Columns.Add("AlertDate", typeof(string));

                        foreach (DataRow row in latestData.Rows)
                        {
                            TableData.Rows.Add(
                                row["ITEM_NAME"].ToString(),
                                Convert.ToInt32(row["PREVIOUS_STOCK"]),
                                Convert.ToInt32(row["NEW_STOCK"]),
                                row["ALERT_DATE"].ToString()
                            );
                        }
                        navigateLowStockLog.Navigate();
                    }
                    
                }
                
            }
        }

        public void StopChecking()
        {
            timer?.Change(Timeout.Infinite, 0);
        }

        static bool CompareDataTableStructure(DataTable dt1, DataTable dt2)
        {
            if (dt1.Columns.Count != dt2.Columns.Count)
                return false;

            foreach (DataColumn col in dt1.Columns)
            {
                if (!dt2.Columns.Contains(col.ColumnName))
                    return false;

                // Additional checks for data types, constraints, etc., if needed
            }

            return true;
        }

        // Method to compare the data (rows and columns) of two DataTables
        static bool CompareDataTableData(DataTable dt1, DataTable dt2)
        {
            if (dt1.Rows.Count != dt2.Rows.Count || dt1.Columns.Count != dt2.Columns.Count)
                return false;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                for (int j = 0; j < dt1.Columns.Count; j++)
                {
                    if (!dt1.Rows[i][j].Equals(dt2.Rows[i][j]))
                        return false;
                }
            }

            return true;
        }
    }
}
