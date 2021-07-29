using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using DbWinForms.Models;

namespace DbWinForms
{
    public abstract partial class BaseDataAccess
    {
        public async Task<List<string>> ListTables()
        {
            return await Many("SELECT NAME FROM SYS.TABLES", null, CommandType.Text, Converter.ToStringValue);
        }

        public async Task<List<string>> ListViews()
        {
            return await Many("SELECT NAME FROM SYS.VIEWS", null, CommandType.Text, Converter.ToStringValue);
        }

        public async Task<List<string>> ListProcedures()
        {
            return await Many("SELECT NAME FROM SYS.PROCEDURES", null, CommandType.Text, Converter.ToStringValue);
        }

        public async Task<List<ParameterInfo>> ListProcedureParameters(string name)
        {
            var parameterList = new List<DbParameter>
            {
                GetParameter("@procName", name)
            };
            const string sql = @"SELECT
                    pa.parameter_id AS order_num
                        , pa.name AS name
                        , UPPER(t.name) AS type
                        , t.max_length AS length
                        , pa.is_output
                    FROM sys.parameters AS pa
                        INNER JOIN sys.procedures AS p on pa.object_id = p.object_id
                    INNER JOIN sys.types AS t on pa.system_type_id = t.system_type_id AND pa.user_type_id = t.user_type_id
                    WHERE p.name = @procName";

            return await Many(sql, parameterList, CommandType.Text, Converter.ToParameterInfo);
        }

        public async Task<List<ParameterInfo>> ListProcedureColumns(string procedureName)
        {
            var parameterList = new List<DbParameter>
            {
                GetParameter("@procedureName", procedureName)
            };
            const string sql = "sp_describe_first_result_set @procedureName";
            return await Many(sql, parameterList, CommandType.Text, Converter.ToProcedureParameterInfo);
        }

        public async Task<List<ParameterInfo>> ListColumns(string tableName, bool isTable)
        {
            var parameterList = new List<DbParameter>
            {
                GetParameter("@name", tableName)
            };
            var sql = isTable ?
                @"SELECT
            c.column_id AS order_num
            , c.name AS name
            , UPPER(t.name) AS type
            , t.max_length AS length
        FROM sys.columns AS c
        INNER JOIN sys.tables AS p on c.object_id = p.object_id
        INNER JOIN sys.types AS t on c.system_type_id = t.system_type_id AND c.user_type_id = t.user_type_id
        WHERE p.name = @name" :
                @"SELECT
            c.column_id AS order_num
            , c.name AS name
            , UPPER(t.name) AS type
            , t.max_length AS length
        FROM sys.columns AS c
        INNER JOIN sys.views AS p on c.object_id = p.object_id
        INNER JOIN sys.types AS t on c.system_type_id = t.system_type_id AND c.user_type_id = t.user_type_id
        WHERE p.name = @name";

            return await Many(sql, parameterList, CommandType.Text, Converter.ToColumn);
        }
    }
}