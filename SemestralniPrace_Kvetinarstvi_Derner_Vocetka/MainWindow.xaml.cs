﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        OracleConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            connection = GetConnection();
            connection.Open();
        }
        public static OracleConnection GetConnection()
        {
            
            string connectionString = "User Id=st67018;Password=abcde;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";
            return new OracleConnection(connectionString);
            
        }

        private string read(OracleDataReader reader, int columnIndex)
        {
            return reader.IsDBNull(columnIndex) ? "..." : reader.GetString(columnIndex);
        }
    }
}
