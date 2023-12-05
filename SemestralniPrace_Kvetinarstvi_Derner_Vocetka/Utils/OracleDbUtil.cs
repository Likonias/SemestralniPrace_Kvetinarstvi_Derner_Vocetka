﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

public class OracleDbUtil
{
    // Connection string Derner
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

    public async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string, object> parameters = null)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param.Key, param.Value);
                    }
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

    public async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters = null)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param.Key, param.Value);
                    }
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

    public async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object> parameters = null)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param.Key, param.Value);
                    }
                }

                try
                {
                    await connection.OpenAsync();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));
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

    public async Task<bool> ExecuteStoredEmailBooleanFunctionAsync(string functionName, string parameter)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(null, connection))
            {
                command.CommandText = "BEGIN :result := " + functionName + "(:p_email); END;";

                command.Parameters.Add("result", OracleDbType.Int32, System.Data.ParameterDirection.ReturnValue);
                command.Parameters.Add("email", OracleDbType.Varchar2).Value = parameter;
                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    OracleDecimal oracleResult = (OracleDecimal)command.Parameters["result"].Value;
                    int validationResult = oracleResult.ToInt32();
                    
                    if(validationResult == 1){ 
                        return true;
                    }else
                    { return false; }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            
        }
    }
    
    public async Task<bool> ExecuteStoredValidateLoginFunctionAsync(string functionName, string emailToValidate, string passwordToValidate)
    {
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(null, connection))
            {
                //validateLogin
                command.CommandText = "BEGIN :result := " + functionName + "(:p_email, :p_password); END;";

                command.Parameters.Add("result", OracleDbType.Int32, ParameterDirection.ReturnValue);
                command.Parameters.Add("email", OracleDbType.Varchar2).Value = emailToValidate;
                command.Parameters.Add("password", OracleDbType.Varchar2).Value = passwordToValidate;
                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    OracleDecimal oracleResult = (OracleDecimal)command.Parameters["result"].Value;
                    int validationResult = oracleResult.ToInt32();
                    
                    if(validationResult == 1){ 
                        return true;
                    }else
                    { return false; }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            
        }
    }
    
    public async Task<Account> ExecuteGetAccountFunctionAsync(string functionName, string parameter)
    {

        EmployeePositionEnum? MapDatabaseValueToEnum(string databaseValue)
        {

            if (Enum.TryParse<EmployeePositionEnum>(databaseValue, out var returnedEnum))
            {
                return returnedEnum;
            }
            return null;
        }
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            using (OracleCommand command = new OracleCommand(null, connection))
            {
                //getUserByEmail
                command.CommandText = "BEGIN :result := " + functionName + "(:email); END;";

                command.Parameters.Add("result", OracleDbType.RefCursor, System.Data.ParameterDirection.ReturnValue);
                command.Parameters.Add("email", OracleDbType.Varchar2).Value = parameter;
                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    OracleRefCursor refCursor = (OracleRefCursor)command.Parameters["result"].Value;

                    OracleDataAdapter adapter = new OracleDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    
                    Account account = new Account();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        
                        account.Id = Convert.ToInt32(row["user_id"]);
                        account.FirstName = row["user_jmeno"].ToString();
                        account.LastName = row["user_prijmeni"].ToString();
                        account.Email = row["user_email"].ToString();
                        account.Phone = row["user_telefon"].ToString();
                        account.EmployeePosition = MapDatabaseValueToEnum(row["user_pozice"].ToString());
                    }

                    return account;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            
        }
    }
}