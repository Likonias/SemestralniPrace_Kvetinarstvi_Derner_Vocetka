using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

public class OracleDbUtil
{
    //Login string for Derner
    private string connectionString = "User Id=st67018;Password=abcde;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";

    public bool TestConnection()
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public DataTable ExecuteQuery(string query, List<OracleParameter> parameters = null)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                try
                {
                    connection.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
    }

    public int ExecuteNonQuery(string query, List<OracleParameter> parameters = null)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch
                {
                    return -1;
                }
            }
        }
    }
}