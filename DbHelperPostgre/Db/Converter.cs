using System.Data.Common;
using Shared;

namespace DbHelperPostgre.Db;


internal static class Converter
{
    public static int ToInt(DbDataReader row)
    {
        return row[0].GetInt();
    }

    public static string ToStringValue(DbDataReader row)
    {
        return row[0].GetString();
    }

    public static ParameterInfo ToParameterInfo(DbDataReader row)
    {
        return new ParameterInfo
        {
            Index = row["ordinal_position"].GetInt() - 1,
            Name = row["parameter_name"].GetString(),
            DbType = row["data_type"].GetString(),
            NetType = row["data_type"].GetString().GetNetType(),
            InParam = row["parameter_mode"].GetString().IsEqual("IN")
        };
    }

    public static string GetNetType(this string dbType)
    {
        dbType = dbType.ToUpperInvariant();

        if (dbType.Contains("INT8") || dbType.Contains("BIGINT"))
        {
            return "long";
        }

        if (dbType.Contains("INT2"))
        {
            return "short";
        }

        if (dbType.Contains("BOOL"))
        {
            return "bool";
        }

        if (dbType.Contains("INT4") || dbType.Contains("INTEGER"))
        {
            return "int";
        }

        if (dbType.Contains("NUMERIC") || dbType.Contains("MONEY"))
        {
            return "decimal";
        }

        if (dbType.Contains("VARCHAR") || dbType.Contains("CHARACTER VARYING") || dbType.Contains("TEXT"))
        {
            return "string";
        }

        if (dbType.Contains("INTERVAL"))
        {
            return "TimeSpan";
        }

        if (dbType.Contains("TIME") ||
            dbType.Contains("DATE") ||
            dbType.Contains("TIMESTAMP"))
        {
            return "DateTime";
        }

        if (dbType.Contains("BYTEA"))
        {
            return "byte[]";
        }

        if (dbType.Contains("FLOAT4"))
        {
            return "float";
        }

        if (dbType.Contains("FLOAT8"))
        {
            return "double";
        }

        if (dbType.Contains("ARRAY"))
        {
            return "Array";
        }

        if (dbType.Contains("UUID"))
        {
            return "Guid";
        }

        if (dbType.Contains("INET"))
        {
            return "IPAddress";
        }

        if (dbType.Contains("JSON"))
        {
            return "JSON";
        }

        if (dbType.Contains("VOID"))
        {
            return "void";
        }

        return "dynamic";
    }

    public static ProcedureInfo ToProcedureInfo(DbDataReader row)
    {
        return new ProcedureInfo
        {
            Name = row["routine_name"].GetString(),
            DbType = row["data_type"].GetString(),
            NetType = row["data_type"].GetString().GetNetType(),
            SpecificName = row["specific_name"].GetString()
        };
    }

    public static ParameterInfo ToColumn(DbDataReader row)
    {
        return new ParameterInfo
        {
            Index = row["ordinal_position"].GetInt() - 1,
            Name = row["column_name"].GetString(),
            DbType = row["data_type"].GetString(),
            NetType = row["data_type"].GetString().GetNetType(),
        };
    }
}
