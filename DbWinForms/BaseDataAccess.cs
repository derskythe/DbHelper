// ***********************************************************************
// Assembly         : Db
// Author           : p.g.parpura
// Created          : 04-30-2020
//
// Last Modified By : p.g.parpura
// Last Modified On : 08-31-2020
// ***********************************************************************
// <copyright file="BaseDataAccess.cs" company="Db">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Containers;
using Containers.Settings;
using Microsoft.Data.SqlClient;
using NLog;

namespace DbWinForms
{
    /// <summary>
    /// Class BaseDataAccess.
    /// </summary>
    public abstract partial class BaseDataAccess
    {
        // ReSharper disable once NotAccessedField.Local
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// The log
        /// </summary>
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        private string ConnectionString { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccess" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected BaseDataAccess(DbConfigOption configuration)
        {
            ConnectionString = configuration.ToString();
            var outputString = $"Server={configuration.HostName};" +
                               $"Database={configuration.ServiceName};" +
                               $"User Id={configuration.Username};" +
                               "Password=*****;" +
                               "Persist Security Info=True;" +
                               "Integrated Security=True;" +
                               "MultipleActiveResultSets=true;" +
                               "Trusted_Connection=False;";
            Log.Info(outputString);
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>SqlConnection.</returns>
        private SqlConnection GetConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        public async Task<bool> CheckConnection()
        {
            try
            {
                await ListTables();
                return true;
            }
            catch (Exception exp)
            {
                Log.Error(exp);
            }

            return false;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>DbCommand.</returns>
        private DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            var command = new SqlCommand(commandText, connection as SqlConnection)
            {
                CommandType = commandType
            };
            return command;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="value">The value.</param>
        /// <returns>SqlParameter.</returns>
        protected SqlParameter GetParameter(string parameter, object value)
        {
            var parameterObject = new SqlParameter(parameter, value ?? DBNull.Value)
            {
                Direction = ParameterDirection.Input
            };
            return parameterObject;
        }

        protected SqlParameter GetParameter(string parameter, SqlDbType dbType, object value)
        {
            var parameterObject = new SqlParameter(parameter, value ?? DBNull.Value)
            {
                Direction = ParameterDirection.Input,
                SqlDbType = dbType
            };
            return parameterObject;
        }

        /// <summary>
        /// Gets the parameter out.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="parameterDirection">The parameter direction.</param>
        /// <returns>SqlParameter.</returns>
        protected SqlParameter GetParameterOut(
            string parameter,
            SqlDbType type,
            object value = null,
            ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            var parameterObject = new SqlParameter(parameter, type);

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText ||
                type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            parameterObject.Value = value ?? DBNull.Value;

            return parameterObject;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>System.Int32.</returns>
        protected async Task ExecuteNonQuery(
            string procedureName,
            List<DbParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
        {
            SqlConnection connection = null;

            try
            {
                connection = GetConnection();
                var cmd = GetCommand(connection, procedureName, commandType);

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                          $"{ex.Message}. Failed to ExecuteNonQuery for {procedureName}, parameters: {parameters?.GetStringFromArray()}"
                         );
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        protected async Task<T1> ExecuteNonQuery<T1>(
            string procedureName,
            List<DbParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
            where T1 : IConvertible
        {
            SqlConnection connection = null;

            try
            {
                connection = GetConnection();
                string returnParam1 = null;
                var cmd = GetCommand(connection, procedureName, commandType);

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Output ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (string.IsNullOrEmpty(returnParam1))
                            {
                                returnParam1 = parameter.ParameterName;
                            }
                            else
                            {
                                throw new ArgumentException($"Too many OUTPUT params. Already set {returnParam1}");
                            }
                        }

                        cmd.Parameters.Add(parameter);
                    }
                }

                if (string.IsNullOrEmpty(returnParam1))
                {
                    throw new
                        ArgumentException($"Didn't find Output or InputOutput params. {nameof(returnParam1)}: {returnParam1}"
                                         );
                }

                await cmd.ExecuteNonQueryAsync();

                var returnValue1 = cmd.Parameters[returnParam1].Value;
                return ExtractValue<T1>(returnValue1);
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                          $"{ex.Message}. Failed to ExecuteNonQuery for {procedureName}, parameters: {parameters?.GetStringFromArray()}"
                         );
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        protected async Task<(T1, T2)> ExecuteNonQuery<T1, T2>(
            string procedureName,
            List<DbParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
            where T1 : IConvertible
            where T2 : IConvertible
        {
            SqlConnection connection = null;

            try
            {
                connection = GetConnection();
                string returnParam1 = null;
                string returnParam2 = null;
                var cmd = GetCommand(connection, procedureName, commandType);

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Output ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (string.IsNullOrEmpty(returnParam1))
                            {
                                returnParam1 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam2))
                            {
                                returnParam2 = parameter.ParameterName;
                            }
                            else
                            {
                                throw new
                                    ArgumentException($"Too many OUTPUT params. Already set {returnParam1}, {returnParam2}"
                                                     );
                            }
                        }

                        cmd.Parameters.Add(parameter);
                    }
                }

                if (string.IsNullOrEmpty(returnParam1) || string.IsNullOrEmpty(returnParam2))
                {
                    throw new
                        ArgumentException($"Didn't find Output or InputOutput params. {nameof(returnParam1)}: {returnParam1}, {nameof(returnParam2)}: {returnParam2}"
                                         );
                }

                await cmd.ExecuteNonQueryAsync();

                var returnValue1 = cmd.Parameters[returnParam1].Value;
                var returnValue2 = cmd.Parameters[returnParam2].Value;
                return (ExtractValue<T1>(returnValue1), ExtractValue<T2>(returnValue2));
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                          $"{ex.Message}. Failed to ExecuteNonQuery for {procedureName}, parameters: {parameters?.GetStringFromArray()}"
                         );
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        protected async Task<(T1, T2, T3)> ExecuteNonQuery<T1, T2, T3>(
            string procedureName,
            List<DbParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
            where T1 : IConvertible
            where T2 : IConvertible
            where T3 : IConvertible
        {
            SqlConnection connection = null;

            try
            {
                connection = GetConnection();
                string returnParam1 = null;
                string returnParam2 = null;
                string returnParam3 = null;
                var cmd = GetCommand(connection, procedureName, commandType);

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Output ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (string.IsNullOrEmpty(returnParam1))
                            {
                                returnParam1 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam2))
                            {
                                returnParam2 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam3))
                            {
                                returnParam3 = parameter.ParameterName;
                            }
                            else
                            {
                                throw new
                                    ArgumentException($"Too many OUTPUT params. Already set {returnParam1}, {returnParam2}, {returnParam3}"
                                                     );
                            }
                        }

                        cmd.Parameters.Add(parameter);
                    }
                }

                if (string.IsNullOrEmpty(returnParam1) || string.IsNullOrEmpty(returnParam2) ||
                    string.IsNullOrEmpty(returnParam3))
                {
                    throw new ArgumentException($"Didn't find Output or InputOutput params. " +
                                                $"{nameof(returnParam1)}: {returnParam1}, " +
                                                $"{nameof(returnParam2)}: {returnParam2}" +
                                                $"{nameof(returnParam3)}: {returnParam3}"
                                               );
                }

                await cmd.ExecuteNonQueryAsync();

                var returnValue1 = cmd.Parameters[returnParam1].Value;
                var returnValue2 = cmd.Parameters[returnParam2].Value;
                var returnValue3 = cmd.Parameters[returnParam3].Value;
                return (ExtractValue<T1>(returnValue1), ExtractValue<T2>(returnValue2), ExtractValue<T3>(returnValue3));
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                          $"{ex.Message}. Failed to ExecuteNonQuery for {procedureName}, parameters: {parameters?.GetStringFromArray()}"
                         );
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        protected async Task<(T1, T2, T3, T4, T5, T6)> ExecuteNonQuery<T1, T2, T3, T4, T5, T6>(
            string procedureName,
            List<DbParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
            where T1 : IConvertible
            where T2 : IConvertible
            where T3 : IConvertible
            where T4 : IConvertible
            where T5 : IConvertible
            where T6 : IConvertible
        {
            SqlConnection connection = null;

            try
            {
                connection = GetConnection();
                string returnParam1 = null;
                string returnParam2 = null;
                string returnParam3 = null;
                string returnParam4 = null;
                string returnParam5 = null;
                string returnParam6 = null;
                var cmd = GetCommand(connection, procedureName, commandType);

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Output ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {
                            if (string.IsNullOrEmpty(returnParam1))
                            {
                                returnParam1 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam2))
                            {
                                returnParam2 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam3))
                            {
                                returnParam3 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam4))
                            {
                                returnParam4 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam5))
                            {
                                returnParam5 = parameter.ParameterName;
                            }
                            else if (string.IsNullOrEmpty(returnParam6))
                            {
                                returnParam6 = parameter.ParameterName;
                            }
                            else
                            {
                                throw new
                                    ArgumentException($"Too many OUTPUT params. Already set {returnParam1}, {returnParam2}, {returnParam3}, {returnParam4}, {returnParam5}, {returnParam6}"
                                                     );
                            }
                        }

                        cmd.Parameters.Add(parameter);
                    }
                }

                if (string.IsNullOrEmpty(returnParam1) || string.IsNullOrEmpty(returnParam2) ||
                    string.IsNullOrEmpty(returnParam3))
                {
                    throw new ArgumentException($"Didn't find Output or InputOutput params. " +
                                                $"{nameof(returnParam1)}: {returnParam1}, " +
                                                $"{nameof(returnParam2)}: {returnParam2}" +
                                                $"{nameof(returnParam3)}: {returnParam3}" +
                                                $"{nameof(returnParam4)}: {returnParam4}" +
                                                $"{nameof(returnParam5)}: {returnParam5}" +
                                                $"{nameof(returnParam6)}: {returnParam6}"
                                               );
                }

                await cmd.ExecuteNonQueryAsync();

                var returnValue1 = cmd.Parameters[returnParam1].Value;
                var returnValue2 = cmd.Parameters[returnParam2].Value;
                var returnValue3 = cmd.Parameters[returnParam3].Value;
                var returnValue4 = cmd.Parameters[returnParam4].Value;
                var returnValue5 = cmd.Parameters[returnParam5].Value;
                var returnValue6 = cmd.Parameters[returnParam6].Value;
                return (
                           ExtractValue<T1>(returnValue1),
                           ExtractValue<T2>(returnValue2),
                           ExtractValue<T3>(returnValue3),
                           ExtractValue<T4>(returnValue4),
                           ExtractValue<T5>(returnValue5),
                           ExtractValue<T6>(returnValue6)
                       );
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                          $"{ex.Message}. Failed to ExecuteNonQuery for {procedureName}, parameters: {parameters?.GetStringFromArray()}"
                         );
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        protected async Task<object> ExecuteScalar(string procedureName, List<DbParameter> parameters)
        {
            object returnValue;
            SqlConnection connection = null;
            try
            {
                connection = GetConnection();
                var cmd = GetCommand(connection, procedureName, CommandType.StoredProcedure);

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                returnValue = await cmd.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                          $"{ex.Message}. Failed to ExecuteScalar for {procedureName}, parameters: {parameters?.GetStringFromArray()}"
                         );
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>DbDataReader.</returns>
        protected async Task<DbDataReader> GetDataReader(
            string procedureName,
            List<DbParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
        {
            DbDataReader ds;

            try
            {
                DbConnection connection = GetConnection();
                {
                    var cmd = GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    ds = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                          $"{ex.Message}. Failed to GetDataReader for {procedureName}, parameters: {parameters?.GetStringFromArray()}"
                         );
                throw;
            }

            return ds;
        }

        /// <summary>
        /// Manies the specified SQL value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlValue">The SQL value.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <param name="funcName">Name of the function.</param>
        /// <returns>Task&lt;List&lt;T&gt;&gt;.</returns>
        protected async Task<List<T>> Many<T>(
            string sqlValue,
            List<DbParameter> paramList,
            Func<DbDataReader, T> funcName)
        {
            var result = new List<T>();

            using (var dataReader = await GetDataReader(sqlValue, paramList))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        result.Add(funcName(dataReader));
                    }
                }
            }

            return result;
        }


        protected async Task<List<T>> Many<T>(
            string sqlValue,
            List<DbParameter> paramList,
            CommandType commandType,
            Func<DbDataReader, T> funcName)
        {
            var result = new List<T>();

            using (var dataReader = await GetDataReader(sqlValue, paramList, commandType))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        result.Add(funcName(dataReader));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Singles the specified SQL value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlValue">The SQL value.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <param name="funcName">Name of the function.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        protected async Task<T> Single<T>(string sqlValue, List<DbParameter> paramList, Func<DbDataReader, T> funcName)
        {
            using (var dataReader = await GetDataReader(sqlValue, paramList))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        return funcName(dataReader);
                    }
                }
            }

            return default;
        }

        private static T ExtractValue<T>(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default;
            }

            return (T) Convert.ChangeType(value, typeof(T));
        }
    }
}