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

        if (msSqlDbType.Contains("BIGINT"))
        {
            return "long";
        }

        if (msSqlDbType.Contains("SMALLINT"))
        {
            return "short";
        }

        if (msSqlDbType.Contains("BIT"))
        {
            return "bool";
        }

        if (msSqlDbType.Contains("TINYINT"))
        {
            return "int";
        }

        if (msSqlDbType.Contains("INT"))
        {
            return "int";
        }

        if (msSqlDbType.Contains("DECIMAL")
            || msSqlDbType.Contains("MONEY")
            || msSqlDbType.Contains("SMALLMONEY"))
        {
            return "decimal";
        }

        if (msSqlDbType.Contains("VARCHAR")
            || msSqlDbType.Contains("NVARCHAR")
            || msSqlDbType.Contains("CHAR")
            || msSqlDbType.Contains("NCHAR")
            || msSqlDbType.Contains("NTEXT")
            || msSqlDbType.Contains("TEXT"))
        {
            return "string";
        }

        if (msSqlDbType.Contains("DATETIMEOFFSET"))
        {
            return "DateTimeOffset";
        }

        if (msSqlDbType.Contains("DATE")
            || msSqlDbType.Contains("DATETIME")
            || msSqlDbType.Contains("TIME"))
        {
            return "DateTime";
        }

        if (msSqlDbType.Contains("VARBINARY")
            || msSqlDbType.Contains("BINARY")
            || msSqlDbType.Contains("TIMESTAMP")
            || msSqlDbType.Contains("IMAGE"))
        {
            return "byte[]";
        }

        if (msSqlDbType.Contains("REAL"))
        {
            return "float";
        }

        if (msSqlDbType.Contains("FLOAT"))
        {
            return "double";
        }

        if (msSqlDbType.Contains("VARIANT"))
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
