using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbHelperPostgre.Properties.SettingsElements;
using Newtonsoft.Json;
using NLog;
using Npgsql;
using NpgsqlTypes;
using Shared;

namespace DbHelperPostgre.Db
{
    public partial class Db
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
        public Db(DbConfigSettingsElement configuration)
        {
            ConnectionString = configuration.ToString();
            var outputString = $"Host={configuration.HostName};Port={configuration.Port};Database={configuration.Database};User Id={configuration.Username};Password=*******;";
            Log.Info(outputString);
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>SqlConnection.</returns>
        private NpgsqlConnection GetConnection()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>DbCommand.</returns>
        private DbCommand GetCommand(NpgsqlConnection connection, string commandText, CommandType commandType)
        {
            var command = new NpgsqlCommand(commandText, connection)
            {
                CommandType = commandType
            };
            return command;
        }

        /// <summary>Gets the parameter.</summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="sqlType">Type of the SQL.</param>
        /// <returns>NpgsqlParameter.</returns>
        protected static NpgsqlParameter GetParameter(string parameter, object value, NpgsqlDbType? sqlType = null)
        {
            NpgsqlParameter parameterObject;
            if (value != null)
            {
                var type = value.GetType();
                if (type.IsEnum)
                {
                    parameterObject = new NpgsqlParameter(parameter, (int)value);
                }
                else if (sqlType is NpgsqlDbType.Json && type != typeof(string))
                {
                    parameterObject = new NpgsqlParameter(parameter, JsonConvert.SerializeObject(value));
                }
                else
                {
                    parameterObject = new NpgsqlParameter(parameter, value);
                }
            }
            else
            {
                parameterObject = new NpgsqlParameter(parameter, DBNull.Value);
            }

            parameterObject.Direction = ParameterDirection.Input;
            if (sqlType != null)
            {
                parameterObject.NpgsqlDbType = (NpgsqlDbType)sqlType;
            }

            return parameterObject;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="sqlType">Type of the SQL.</param>
        /// <param name="logValues">if set to <c>true</c> [log values].</param>
        /// <returns>NpgsqlParameter.</returns>
        protected NpgsqlParameter GetParameter(string parameter, object value, NpgsqlDbType sqlType, bool logValues)
        {
            NpgsqlParameter parameterObject;
            if (value != null)
            {
                var type = value.GetType();
                if (type.IsEnum)
                {
                    parameterObject = new NpgsqlParameter(parameter, (int)value);
                }
                else if (sqlType is NpgsqlDbType.Json && type != typeof(string))
                {
                    parameterObject = new NpgsqlParameter(parameter, JsonConvert.SerializeObject(value));
                }
                else
                {
                    parameterObject = new NpgsqlParameter(parameter, value);
                }
            }
            else
            {
                parameterObject = new NpgsqlParameter(parameter, DBNull.Value);
            }

            parameterObject.Direction = ParameterDirection.Input;
            parameterObject.NpgsqlDbType = sqlType;

            if (logValues)
            {
                Log.Debug($"parameterObject type: {parameterObject.NpgsqlDbType}, Value: {parameterObject.Value}");
            }

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
        protected NpgsqlParameter GetParameterOut(
            string parameter,
            SqlDbType type,
            object value = null,
            ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            NpgsqlParameter parameterObject;
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    parameterObject = new NpgsqlParameter(parameter, (int)value);
                }
                else
                {
                    parameterObject = new NpgsqlParameter(parameter, value);
                }
            }
            else
            {
                parameterObject = new NpgsqlParameter(parameter, DBNull.Value);
            }

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
            List<NpgsqlParameter> parameters,
            CommandType commandType = CommandType.StoredProcedure)
        {
            NpgsqlConnection connection = null;

            try
            {
                connection = GetConnection();
                using (var cmd = GetCommand(connection, procedureName, commandType))
                {
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    await cmd.ExecuteNonQueryAsync();
                }
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
                    await connection.CloseAsync();
                }
            }
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        protected async Task<T> ExecuteScalar<T>(string procedureName, List<NpgsqlParameter> parameters)
        {
            object returnValue;
            NpgsqlConnection connection = null;
            try
            {
                connection = GetConnection();
                using (var cmd = GetCommand(connection, procedureName, CommandType.StoredProcedure))
                {
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = await cmd.ExecuteScalarAsync();
                }
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
                    await connection.CloseAsync();
                }
            }

            return (T)returnValue;
        }

        protected async Task<int> Count(string tableName, List<NpgsqlParameter> parameters)
        {
            int returnValue;
            NpgsqlConnection connection = null;
            try
            {
                connection = GetConnection();
                using (var cmd = GetCommand(connection, $"SELECT COUNT(*) FROM {tableName}", CommandType.Text))
                {
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    var rawValue = await cmd.ExecuteScalarAsync();
                    returnValue = rawValue != null ? Convert.ToInt32(rawValue) : 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                             $"{ex.Message}. Failed to Count for {tableName}, parameters: {parameters?.GetStringFromArray()}"
                            );
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    await connection.CloseAsync();
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
            List<NpgsqlParameter> parameters,
            CommandType commandType = CommandType.Text)
        {
            DbDataReader ds;

            try
            {
                var connection = GetConnection();
                {
                    using (var cmd = GetCommand(connection, procedureName, commandType))
                    {
                        if (parameters != null && parameters.Count > 0)
                        {
                            cmd.Parameters.AddRange(parameters.Select(x => x.Clone()).ToArray());
                        }

                        ds = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    }
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


        protected async Task<DbDataReader> GetDataReader(
            string procedureName,
            CommandType commandType = CommandType.TableDirect)
        {
            DbDataReader ds;

            try
            {
                var connection = GetConnection();
                {
                    var cmd = GetCommand(connection, procedureName, commandType);
                    ds = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{ex.Message}. Failed to GetDataReader for {procedureName}");
                throw;
            }

            return ds;
        }

        protected async Task<T> SelectSingle<T>(
            string cmdText,
            List<NpgsqlParameter> parameters,
            Func<DbDataReader, T> funcName)
        {
            using (var dataReader = await GetDataReader(cmdText, parameters))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        return funcName(dataReader);
                    }
                }
            }

            return default(T);
        }

        protected async Task<T> SelectSingle<T>(
            string cmdText,
            List<NpgsqlParameter> parameters,
            CommandType commandType = CommandType.Text)
        {
            using (var dataReader = await GetDataReader(cmdText, parameters, commandType))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        return dataReader.GetFieldValue<T>(0);
                    }
                }
            }

            return default(T);
        }

        protected async Task<List<T>> Many<T>(
            string cmdText,
            List<NpgsqlParameter> parameters,
            Func<DbDataReader, T> funcName)
        {
            var result = new List<T>();

            using (var dataReader = await GetDataReader(cmdText, parameters))
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

        private static (string where, List<NpgsqlParameter> paramList) AppendArrayParams<T>(
            string columnName,
            List<T> list,
            string paramPrefix = "apParam")
        {
            if (list == null || list.Count == 0)
            {
                return (string.Empty, new List<NpgsqlParameter>());
            }

            var fields = new List<NpgsqlParameter>();
            var str = new StringBuilder();
            str.Append(" ( ");
            for (var i = 0; i < list.Count; i++)
            {
                var paramName = $"@{paramPrefix}{i}";
                if (i != 0)
                {
                    str.Append(" OR ");
                }

                str.Append(columnName).Append(" = ").Append(paramName).Append(i);
                fields.Add(
                           GetParameter(paramName, list[i])
                          );
            }

            str.Append(" ) ");

            return (str.ToString(), fields);
        }
    }
}