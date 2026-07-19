using System;
using System.Data;
using System.Data.Common;
using DbWinForms.Models;

namespace DbWinForms;


/// <summary>
/// Class Converter.
/// </summary>
public static class Converter
{
    public static int ToInt32(DbDataReader row)
    {
        return row[0].GetInt();
    }

    public static int? ToInt32Nullable(DbDataReader row)
    {
        return row[0].GetInt();
    }

    public static string ToStringValue(DbDataReader row)
    {
        return row[0].GetString();
    }

    public static ParameterInfo ToParameterInfo(DbDataReader row)
    {
        return new ParameterInfo(
            row["order_num"].GetInt() - 1,
            row["name"].GetString(),
            row["type"].GetString(),
            row["type"].GetString().GetNetType(),
            !row["is_output"].GetBool());
    }

    public static ParameterInfo ToProcedureParameterInfo(DbDataReader row)
    {
        return new ParameterInfo(
            row["column_ordinal"].GetInt() - 1,
            row["name"].GetString(),
            row["system_type_name"].GetString(),
            row["system_type_name"].GetString().GetNetType(),
            true);
    }

    public static ParameterInfo ToColumn(DbDataReader row)
    {
        return new ParameterInfo(
            row["order_num"].GetInt() - 1,
            row["name"].GetString(),
            row["type"].GetString(),
            row["type"].GetString().GetNetType());
    }

    public static string GetNetType(this string msSqlDbType)
    {
        msSqlDbType = msSqlDbType.ToUpperInvariant();

        if (msSqlDbType.Contains("BIGINT", StringComparison.OrdinalIgnoreCase))
        {
            return "long";
        }

        if (msSqlDbType.Contains("SMALLINT", StringComparison.OrdinalIgnoreCase))
        {
            return "short";
        }

        if (msSqlDbType.Contains("BIT", StringComparison.OrdinalIgnoreCase))
        {
            return "bool";
        }

        if (msSqlDbType.Contains("INT", StringComparison.OrdinalIgnoreCase))
        {
            return "int";
        }

        if (msSqlDbType.Contains("DECIMAL", StringComparison.OrdinalIgnoreCase)
            || msSqlDbType.Contains("MONEY", StringComparison.OrdinalIgnoreCase))
        {
            return "decimal";
        }

        if (msSqlDbType.Contains("CHAR", StringComparison.OrdinalIgnoreCase)
            || msSqlDbType.Contains("TEXT", StringComparison.OrdinalIgnoreCase))
        {
            return "string";
        }

        if (msSqlDbType.Contains("DATETIMEOFFSET", StringComparison.OrdinalIgnoreCase))
        {
            return "DateTimeOffset";
        }

        if (msSqlDbType.Contains("DATE", StringComparison.OrdinalIgnoreCase)
            || msSqlDbType.Contains("TIME", StringComparison.OrdinalIgnoreCase))
        {
            return "DateTime";
        }

        if (msSqlDbType.Contains("BINARY", StringComparison.OrdinalIgnoreCase)
            || msSqlDbType.Contains("IMAGE", StringComparison.OrdinalIgnoreCase))
        {
            return "byte[]";
        }

        if (msSqlDbType.Contains("REAL", StringComparison.OrdinalIgnoreCase))
        {
            return "float";
        }

        if (msSqlDbType.Contains("FLOAT", StringComparison.OrdinalIgnoreCase))
        {
            return "double";
        }

        if (msSqlDbType.Contains("VARIANT", StringComparison.OrdinalIgnoreCase))
        {
            return "object";
        }

        return "dynamic";
    }

    public static string GetDbParamType(this string msSqlDbType)
    {
        msSqlDbType = msSqlDbType.ToUpperInvariant();

        foreach (var name in Enum.GetNames(typeof(SqlDbType)))
        {
            if (name.ToUpperInvariant() == msSqlDbType)
            {
                return "SqlDbType." + name;
            }
        }

        return "dynamic";
    }
}
