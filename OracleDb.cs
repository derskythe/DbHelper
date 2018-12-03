using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms.VisualStyles;
using NLog;
using Oracle.ManagedDataAccess.Client;

namespace DbHelper
{
    internal static class OracleDb
    {
        private static string _ConnectionString;
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void Init(string hostName,
                                string user,
                                string pass,
                                string dbase,
                                string port,
                                bool perfCounter = false)
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
                using (OracleCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 FROM dual";
                    cmd.ExecuteScalar();
                }

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
            if (String.IsNullOrEmpty(_ConnectionString))
            {
                throw new Exception("Connection not initialized!");
            }

            var connection = new OracleConnection(_ConnectionString);
            connection.Open();

            return connection;
        }

        private static int? GetInt(object value)
        {
            return value != DBNull.Value ? (int?)Convert.ToInt32(value) : null;
        }

        private static String GetString(object order)
        {
            return order != DBNull.Value ? Convert.ToString(order) : null;
        }

        private static DateTime? GetDateTime(object order)
        {
            return order != DBNull.Value ? Convert.ToDateTime(order) : (DateTime?)null;
        }

        private static int? GetInt(OracleDataReader reader, string order)
        {
            return reader[order] != DBNull.Value ? (int?)Convert.ToInt32(reader[order]) : null;
        }

        private static String GetString(OracleDataReader reader, string order)
        {
            return reader[order] != DBNull.Value ? Convert.ToString(reader[order]) : null;
        }

        private static DateTime? GetDateTime(OracleDataReader reader, String order)
        {
            return reader[order] != DBNull.Value ? Convert.ToDateTime(reader[order]) : (DateTime?)null;
        }

        public static List<string> ListViews()
        {
            OracleConnection connection = null;
            List<String> result = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                using (var command = new OracleCommand())
                {
                    command.CommandText =
                        "select view_name from user_views";
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    command.BindByName = true;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            result = new List<String>();
                            while (reader.Read())
                            {
                                result.Add(GetString(reader["view_name"]));
                            }
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

        public static List<string> ListTables()
        {
            OracleConnection connection = null;
            List<String> result = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                using (var command = new OracleCommand())
                {
                    command.CommandText =
                        "select table_name from user_tables";
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    command.BindByName = true;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            result = new List<String>();
                            while (reader.Read())
                            {
                                var str = reader.GetString(0);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    result.Add(str);
                                }
                            }
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

        public static List<KeyValuePair<String, String>> ListPackages(string ownerName)
        {
            OracleConnection connection = null;
            List<KeyValuePair<String, String>> result = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                using (var command = new OracleCommand())
                {
                    command.CommandText =
                        "SELECT a.OBJECT_NAME,p.PROCEDURE_NAME FROM SYS.ALL_OBJECTS a, SYS.ALL_PROCEDURES p WHERE a.OBJECT_NAME = p.OBJECT_NAME AND a.OBJECT_TYPE = 'PACKAGE' AND a.OWNER = :ownerName AND p.PROCEDURE_NAME IS NOT NULL ORDER BY a.OBJECT_NAME,p.PROCEDURE_NAME";
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    command.BindByName = true;
                    command.Parameters.Add("ownerName", OracleDbType.Varchar2, ParameterDirection.Input).Value = ownerName.ToUpperInvariant();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            result = new List<KeyValuePair<String, String>>();
                            while (reader.Read())
                            {
                                result.Add(new KeyValuePair<string, string>(
                                               GetString(reader["OBJECT_NAME"]),
                                               GetString(reader["PROCEDURE_NAME"])));
                            }
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

        public static ProcedureInfo ListProcedureParameters(string package, String procedureName)
        {
            OracleConnection connection = null;
            var result = new ProcedureInfo(0, package, procedureName);
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                using (var command = new OracleCommand())
                {
                    command.CommandText =
                        "SELECT t.ARGUMENT_NAME, t.in_out, t.DATA_TYPE FROM SYS.ALL_ARGUMENTS t WHERE PACKAGE_NAME = :packageName AND OBJECT_NAME = :procName ORDER BY t.SEQUENCE";
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    command.BindByName = true;
                    command.Parameters.Add("packageName", OracleDbType.Varchar2, ParameterDirection.Input).Value = package;
                    command.Parameters
                           .Add("procName", OracleDbType.Varchar2, ParameterDirection.Input)
                           .Value = procedureName;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int i = 0;
                            while (reader.Read())
                            {
                                var name = GetString(reader["ARGUMENT_NAME"]);
                                var info = new ParameterInfo(
                                    name,
                                    GetString(reader["DATA_TYPE"]),
                                    GetString(reader["in_out"]) == "IN",
                                    i++,
                                    Utils.GetNetType(GetString(reader["DATA_TYPE"])),
                                    Utils.ToUpperCamelCase(name, false),
                                    Utils.ToLowerCamelCase(name, false)
                                );
                                result.AddParam(info);
                            }
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

        public static List<KeyValuePair<String, String>> ListColumns(string tableOrView)
        {
            OracleConnection connection = null;
            List<KeyValuePair<String, String>> result = null;
            try
            {
                connection = new OracleConnection(_ConnectionString);
                connection.Open();

                using (var command = new OracleCommand())
                {
                    command.CommandText =
                        "SELECT t.column_name, t.DATA_TYPE FROM user_tab_cols t WHERE table_name = :viewName";
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;
                    command.BindByName = true;
                    command.Parameters.Add("viewName", OracleDbType.Varchar2, ParameterDirection.Input).Value = tableOrView;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            result = new List<KeyValuePair<String, String>>();
                            while (reader.Read())
                            {
                                result.Add(new KeyValuePair<string, string>(
                                               GetString(reader["column_name"]),
                                               GetString(reader["DATA_TYPE"])));

                            }
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
    }
}
