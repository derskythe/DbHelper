using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Shared;

namespace DbHelperPostgre.Db;


public partial class Db
{
    public async Task<bool> CheckConnection()
    {
        try
        {
            var result = await SelectSingle(
                             "SELECT 1",
                             null,
                             Converter.ToInt
                         );

            return result > 0;
        }
        catch (Exception exp)
        {
            Log.Error(exp, exp.Message);
        }

        return false;
    }

    /*
    SELECT
    table_schema || '.' || table_name
    FROM
    information_schema.tables
    WHERE
    table_type = 'BASE TABLE'
    AND
    table_schema NOT IN ('pg_catalog', 'information_schema');
     */
    public async Task<List<string>> ListTables()
    {
        return await Many("SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema NOT IN('pg_catalog', 'information_schema') ORDER BY table_name", null, Converter.ToStringValue);
    }

    public async Task<List<string>> ListViews()
    {
        return await Many("SELECT table_name FROM information_schema.tables WHERE table_type = 'VIEW' AND table_schema NOT IN('pg_catalog', 'information_schema') ORDER BY table_name", null, Converter.ToStringValue);
    }

    public async Task<List<ProcedureInfo>> ListProcedures()
    {
        const string sql =
            "SELECT r.routine_name, r.data_type, r.specific_name FROM information_schema.routines r WHERE r.specific_schema NOT IN('pg_catalog', 'information_schema') ORDER BY r.routine_name";
        return await Many(sql, null, Converter.ToProcedureInfo);
    }

    public async Task<List<ParameterInfo>> ListProcedureParameters(string name)
    {
        var parameterList = new List<NpgsqlParameter>
        {
            GetParameter("@procName", name)
        };
        const string sql = @"SELECT r.routine_name, p.parameter_mode, p.parameter_name, p.data_type, p.ordinal_position
FROM information_schema.routines r
    LEFT JOIN information_schema.parameters p ON r.specific_name=p.specific_name
WHERE r.specific_schema NOT IN('pg_catalog', 'information_schema') AND r.specific_name = @procName and p.parameter_name is not null
ORDER BY r.routine_name, p.ordinal_position";

        return await Many(sql, parameterList, Converter.ToParameterInfo);
    }

    public async Task<List<ParameterInfo>> ListColumns(string tableName, ObjectType objectType)
    {
        var parameterList = new List<NpgsqlParameter>
        {
            GetParameter("@name", tableName)
        };
        var sql = objectType == ObjectType.Table ?
                  @"select
       c.column_name,
       c.data_type, c.ordinal_position
       from information_schema.tables t
    left join information_schema.columns c
              on t.table_schema = c.table_schema
              and t.table_name = c.table_name
where t.table_type = 'BASE TABLE' AND t.table_name = @name
      and t.table_schema not in ('information_schema', 'pg_catalog')
order by c.ordinal_position" :
                  @"select
       c.column_name,
       c.data_type, c.ordinal_position
       from information_schema.tables t
    left join information_schema.columns c
              on t.table_schema = c.table_schema
              and t.table_name = c.table_name
where t.table_type = 'VIEW' AND t.table_name = @name
      and t.table_schema not in ('information_schema', 'pg_catalog')
order by c.ordinal_position";

        return await Many(sql, parameterList, Converter.ToColumn);
    }
}
