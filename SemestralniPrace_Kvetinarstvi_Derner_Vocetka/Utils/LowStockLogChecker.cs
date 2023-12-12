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
        ////todo finish checking stock
        //private Timer timer;
        //private OracleDbUtil dbUtil;

        //public LowStockLogChecker()
        //{
        //    dbUtil = new OracleDbUtil();
        //    timer = new Timer(CheckLowStockLog, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        //}

        //private void CheckLowStockLog(object state)
        //{
        //    // Perform the logic to check for updates in LowStockLog table
        //    // For example, you could fetch the latest entries or compare timestamps

        //    // Here's an example fetching the data from LowStockLog
        //    var latestData = dbUtil.GetLowStockLogForItemAsync(itemId).Result;

        //    if (latestData != null)
        //    {
        //        // Process the data or perform actions based on the updates
        //        // Example: Display the data or trigger actions
        //        Console.WriteLine("LowStockLog table updated:");
        //        foreach (DataRow row in latestData.Rows)
        //        {
        //            // Process each row of data if needed
        //            // Example: Console.WriteLine(row["Item_ID"]);
        //        }
        //    }
        //    else
        //    {
        //        // Handle if the fetch operation failed
        //        Console.WriteLine("Fetching data from LowStockLog failed.");
        //    }
        //}

        //// Add a method to stop the cycle if needed
        //public void StopChecking()
        //{
        //    timer?.Change(Timeout.Infinite, 0);
        //}
    }
}
