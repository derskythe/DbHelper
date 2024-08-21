using System;
using System.Collections.Generic;
using System.Data;
using NLog;
using Oracle.ManagedDataAccess.Client;
using Shared;
// ReSharper disable UnusedMember.Local

namespace DbHelperOracle.Db;


internal static class OracleDb
{
    private static string _ConnectionString;

    // ReSharper disable InconsistentNaming
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void Init(
        string hostName,
        string user,
        string pass,
        string dbase,
        string port,
        bool perfCounter = false
    )
    {
        _ConnectionString =
            $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={hostName})(PORT={port})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={dbase})));User Id={user};Password={pass};Min Pool Size=1;Max Pool Size=150;Pooling=True;Validate Connection=true;Connection Lifetime=300;Connection Timeout=300;";

        //$"User Id={user};Password={pass};Data Source={hostName}:1521/{dbase}";
        //$"Data Source={hostName}:1521/XE;Persist Security Info=True;User ID={user};Password={pass}";
        /*_ConnectionString = "User ID=" + user + ";Data Source=" + dbase + ";Password=" + pass +
                            ";Min Pool Size=1;Max Pool Size=150;Pooling=True;" +
                            "Validate Connection=true;Connection Lifetime=300;Connection Timeout=300";*/

        Log.Debug(_ConnectionString);
    }

    public static bool CheckConnection()
    {
        OracleConnection connection = null;

        try
        {
            connection = GetConnection();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT 1 FROM dual";
            cmd.ExecuteScalar();

            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return false;
    }

    private static OracleConnection GetConnection()
    {
        if (string.IsNullOrEmpty(_ConnectionString))
        {
            throw new Exception("Connection not initialized!");
        }

        var connection = new OracleConnection(_ConnectionString);
        connection.Open();

        return connection;
    }

    private static int? GetInt(object value)
    {
        return value != DBNull.Value ? Convert.ToInt32(value) : null;
    }

    private static string GetString(this object order)
    {
        return order != DBNull.Value ? Convert.ToString(order) : null;
    }

    private static DateTime? GetDateTime(object order)
    {
        return order != DBNull.Value ? Convert.ToDateTime(order) : null;
    }

    private static int? GetInt(OracleDataReader reader, string order)
    {
        return reader[order] != DBNull.Value ? Convert.ToInt32(reader[order]) : null;
    }

    private static string GetString(OracleDataReader reader, string order)
    {
        return reader[order] != DBNull.Value ? Convert.ToString(reader[order]) : null;
    }

    private static DateTime? GetDateTime(OracleDataReader reader, string order)
    {
        return reader[order] != DBNull.Value ? Convert.ToDateTime(reader[order]) : null;
    }

    public static List<ComboboxItem> ListViews()
    {
        OracleConnection connection = null;
        var result = new List<ComboboxItem>();

        try
        {
            connection = new OracleConnection(_ConnectionString);
            connection.Open();
            const string sql = "SELECT view_name FROM user_views UNION SELECT table_name view_name FROM user_tables";

            using var command = new OracleCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.BindByName = true;

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var item = reader["view_name"].GetString();

                    result.Add(
                        new ComboboxItem
                    {
                        Id = item,
                        Value = item,
                        AdditionalData = string.Empty,
                        ObjectType = ObjectType.View
                    }
                    );
                }
            }
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return result;
    }

    public static List<ComboboxItem> ListTables()
    {
        OracleConnection connection = null;
        var result = new List<ComboboxItem>();

        try
        {
            connection = new OracleConnection(_ConnectionString);
            connection.Open();
            const string sql = "SELECT table_name FROM user_tables";

            using var command = new OracleCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.BindByName = true;

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var item = reader.GetString(0);

                    if (!string.IsNullOrEmpty(item))
                    {
                        result.Add(new ComboboxItem
                        {
                            Id = item,
                            Value = item,
                            AdditionalData = string.Empty,
                            ObjectType = ObjectType.Table
                        }
                                  );
                    }
                }
            }
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return result;
    }

    public static List<ComboboxItem> ListProcedures(string ownerName)
    {
        OracleConnection connection = null;
        var result = new List<ComboboxItem>();

        try
        {
            connection = new OracleConnection(_ConnectionString);
            connection.Open();

            const string sql =
                "SELECT p.OBJECT_NAME,p.PROCEDURE_NAME FROM SYS.ALL_PROCEDURES p WHERE p.OBJECT_NAME = p.OBJECT_NAME AND p.OBJECT_TYPE = 'PROCEDURE' AND p.OWNER = :ownerName AND PROCEDURE_NAME IS NULL ORDER BY p.OBJECT_NAME,p.PROCEDURE_NAME";

            using var command = new OracleCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.BindByName = true;
            command.Parameters.Add("ownerName", OracleDbType.Varchar2, ParameterDirection.Input).Value = ownerName.ToUpperInvariant();

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var item = GetString(reader["OBJECT_NAME"]);

                    result.Add(new ComboboxItem
                    {
                        Id = item,
                        Value = item,
                        ClearName = item,
                        AdditionalData = string.Empty,
                        ObjectType = ObjectType.Procedure
                    }
                              );
                }
            }
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return result;
    }

    public static List<ComboboxItem> ListPackages(string ownerName)
    {
        OracleConnection connection = null;
        var result = new List<ComboboxItem>();

        try
        {
            connection = new OracleConnection(_ConnectionString);
            connection.Open();

            const string sql =
                "SELECT p.OBJECT_NAME,p.PROCEDURE_NAME FROM SYS.ALL_PROCEDURES p WHERE p.OBJECT_NAME = p.OBJECT_NAME AND p.OBJECT_TYPE = 'PACKAGE' AND p.OWNER = :ownerName AND PROCEDURE_NAME IS NOT NULL ORDER BY p.OBJECT_NAME,p.PROCEDURE_NAME";

            using var command = new OracleCommand();

            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.BindByName = true;
            command.Parameters.Add("ownerName", OracleDbType.Varchar2, ParameterDirection.Input).Value = ownerName.ToUpperInvariant();

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var objectName = GetString(reader["OBJECT_NAME"]);
                    var procedureName = GetString(reader["PROCEDURE_NAME"]);

                    result.Add(new ComboboxItem
                    {
                        Id = $"{objectName}.{procedureName}",
                        Value = $"{objectName}.{procedureName}",
                        AdditionalData = objectName,
                        ClearName = procedureName,
                        ObjectType = ObjectType.Package
                    }
                              );
                }
            }
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return result;
    }

    public static ProcedureInfo ListProcedureParameters(ComboboxItem desiredProcedure)
    {
        OracleConnection connection = null;
        var result = new ProcedureInfo(0, desiredProcedure.AdditionalData, desiredProcedure.ClearName);

        try
        {
            connection = new OracleConnection(_ConnectionString);
            connection.Open();

            if (string.IsNullOrEmpty(desiredProcedure.AdditionalData))
            {
                const string sql =
                    "SELECT t.ARGUMENT_NAME, t.in_out, t.DATA_TYPE FROM SYS.ALL_ARGUMENTS t WHERE OBJECT_NAME = :procName AND package_name IS NULL ORDER BY t.SEQUENCE";

                using var command = new OracleCommand();
                command.CommandText = sql;

                command.CommandType = CommandType.Text;
                command.Connection = connection;
                command.BindByName = true;

                //command.Parameters.Add("packageName", OracleDbType.Varchar2, ParameterDirection.Input).Value = package;
                command.Parameters
                .Add("procName", OracleDbType.Varchar2, ParameterDirection.Input)
                .Value = desiredProcedure.ClearName;

                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    var i = 0;

                    while (reader.Read())
                    {
                        var name = GetString(reader["ARGUMENT_NAME"]);

                        var info = new ParameterInfo(
                            name,
                            reader["DATA_TYPE"].GetString(),
                            reader["in_out"].GetString().IsEqual("IN"),
                            i++,
                            reader["DATA_TYPE"].GetString().GetNetType(),
                            name.ToUpperCamelCase(false),
                            name.ToLowerCamelCase(false)
                        );

                        result.AddParam(info);
                    }
                }
            }
            else
            {
                const string sql =
                    "SELECT "                                                           +
                    " t.ARGUMENT_NAME, t.in_out, t.DATA_TYPE FROM SYS.ALL_ARGUMENTS t " +
                    " WHERE OBJECT_NAME = :procName AND package_name = :packageName ORDER BY t.SEQUENCE";

                using var command = new OracleCommand();
                command.CommandText = sql;

                command.CommandType = CommandType.Text;
                command.Connection = connection;
                command.BindByName = true;

                command.Parameters
                .Add("procName", OracleDbType.Varchar2, ParameterDirection.Input)
                .Value = desiredProcedure.ClearName;

                command.Parameters
                .Add("packageName", OracleDbType.Varchar2, ParameterDirection.Input)
                .Value = desiredProcedure.AdditionalData;

                using var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    var i = 0;

                    while (reader.Read())
                    {
                        var name = GetString(reader["ARGUMENT_NAME"]);

                        var info = new ParameterInfo(
                            name,
                            reader["DATA_TYPE"].GetString(),
                            reader["in_out"].GetString().IsEqual("IN"),
                            i++,
                            reader["DATA_TYPE"].GetString().GetNetType(),
                            name.ToUpperCamelCase(false),
                            name.ToLowerCamelCase(false)
                        );

                        result.AddParam(info);
                    }
                }
            }
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return result;
    }

    public static ProcedureInfo ListProcedureParameters(string ownerName, string package, string procedureName)
    {
        OracleConnection connection = null;
        var result = new ProcedureInfo(0, package, procedureName);

        try
        {
            connection = new OracleConnection(_ConnectionString);
            connection.Open();
            using var command = new OracleCommand();
            command.BindByName = true;
            command.CommandType = CommandType.Text;

            if (!string.IsNullOrEmpty(package))
            {
                const string sql =
                    "SELECT t.ARGUMENT_NAME, t.in_out, t.DATA_TYPE FROM SYS.ALL_ARGUMENTS t WHERE package_name = :packageName AND OBJECT_NAME = :procName AND owner = :owner AND ARGUMENT_NAME IS NOT NULL ORDER BY t.SEQUENCE";

                command.CommandText = sql;

                command.Parameters
                .Add("packageName", OracleDbType.Varchar2, ParameterDirection.Input)
                .Value = package;
            }
            else
            {
                const string sql =
                    "SELECT t.ARGUMENT_NAME, t.in_out, t.DATA_TYPE FROM SYS.ALL_ARGUMENTS t WHERE package_name IS NULL AND OBJECT_NAME = :procName AND owner = :owner AND ARGUMENT_NAME IS NOT NULL ORDER BY t.SEQUENCE";

                command.CommandText = sql;
            }

            command.Connection = connection;

            //command.Parameters.Add("packageName", OracleDbType.Varchar2, ParameterDirection.Input).Value = package;
            command.Parameters
            .Add("procName", OracleDbType.Varchar2, ParameterDirection.Input)
            .Value = procedureName;

            command.Parameters
            .Add("owner", OracleDbType.Varchar2, ParameterDirection.Input)
            .Value = ownerName;

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                var i = 0;

                while (reader.Read())
                {
                    var name = GetString(reader["ARGUMENT_NAME"]);

                    var info = new ParameterInfo(
                        name,
                        reader["DATA_TYPE"].GetString(),
                        reader["in_out"].GetString().IsEqual("IN"),
                        i++,
                        reader["DATA_TYPE"].GetString().GetNetType(),
                        name.ToUpperCamelCase(false),
                        name.ToLowerCamelCase(false)
                    );

                    result.AddParam(info);
                }
            }
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return result;
    }

    public static List<KeyValuePair<string, string>> ListColumns(string tableOrView)
    {
        OracleConnection connection = null;
        List<KeyValuePair<string, string>> result = null;

        try
        {
            connection = new OracleConnection(_ConnectionString);
            connection.Open();
            const string sql = "SELECT t.column_name, t.DATA_TYPE FROM user_tab_cols t WHERE table_name = :viewName";

            using var command = new OracleCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.BindByName = true;
            command.Parameters.Add("viewName", OracleDbType.Varchar2, ParameterDirection.Input).Value = tableOrView;

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                result = new List<KeyValuePair<string, string>>();

                while (reader.Read())
                {
                    result.Add(new KeyValuePair<string, string>(
                                   reader["column_name"].GetString(),
                                   reader["DATA_TYPE"].GetString()
                               )
                              );
                }
            }
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        return result;
    }
}
