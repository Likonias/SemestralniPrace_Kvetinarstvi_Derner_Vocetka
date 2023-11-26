﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

public class OracleDbUtil
{
    // Same connection string as before
    private string connectionString = "User Id=st67018;Password=abcde;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";

    public async Task<bool> TestConnectionAsync()
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public async Task<DataTable> ExecuteQueryAsync(string query, List<OracleParameter> parameters = null)
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
                    await connection.OpenAsync();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable)); // Asynchronous fill operation
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

    public async Task<int> ExecuteNonQueryAsync(string query, List<OracleParameter> parameters = null)
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
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
                catch
                {
                    return -1;
                }
            }
        }
    }

    public async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, List<OracleParameter> parameters = null)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                try
                {
                    await connection.OpenAsync();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable)); // Asynchronous fill operation
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

}