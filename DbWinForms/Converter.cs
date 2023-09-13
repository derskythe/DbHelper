// ***********************************************************************
// Assembly         : Db
// Author           : p.g.parpura
// Created          : 08-19-2020
//
// Last Modified By : p.g.parpura
// Last Modified On : 09-29-2020
// ***********************************************************************
// <copyright file="Converter.cs" company="skif@skif.ws">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
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
        return new ParameterInfo
        {
            Index = row["order_num"].GetInt() - 1,
            Name = row["name"].GetString(),
            DbType = row["type"].GetString(),
            NetType = row["type"].GetString().GetNetType(),
            InParam = !row["is_output"].GetBool()
        };
    }

    public static ParameterInfo ToProcedureParameterInfo(DbDataReader row)
    {
        return new ParameterInfo
        {
            Index = row["column_ordinal"].GetInt() - 1,
            Name = row["name"].GetString(),
            DbType = row["system_type_name"].GetString(),
            NetType = row["system_type_name"].GetString().GetNetType(),
            InParam = true
        };
    }

    public static ParameterInfo ToColumn(DbDataReader row)
    {
        return new ParameterInfo
        {
            Index = row["order_num"].GetInt() - 1,
            Name = row["name"].GetString(),
            DbType = row["type"].GetString(),
            NetType = row["type"].GetString().GetNetType()
        };
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

        if (msSqlDbType.Contains("DECIMAL") || msSqlDbType.Contains("MONEY") || msSqlDbType.Contains("SMALLMONEY"))
        {
            return "decimal";
        }

        if (msSqlDbType.Contains("VARCHAR") || msSqlDbType.Contains("NVARCHAR") || msSqlDbType.Contains("CHAR") ||
            msSqlDbType.Contains("NCHAR")   || msSqlDbType.Contains("NTEXT")    || msSqlDbType.Contains("TEXT"))
        {
            return "string";
        }

        if (msSqlDbType.Contains("DATETIMEOFFSET"))
        {
            return "DateTimeOffset";
        }

        if (msSqlDbType.Contains("DATE") || msSqlDbType.Contains("DATETIME") || msSqlDbType.Contains("TIME"))
        {
            return "DateTime";
        }

        if (msSqlDbType.Contains("VARBINARY") || msSqlDbType.Contains("BINARY") ||
            msSqlDbType.Contains("TIMESTAMP") || msSqlDbType.Contains("IMAGE"))
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
        foreach (string name in Enum.GetNames(typeof(SqlDbType)))
        {
            if (name.ToUpperInvariant() == msSqlDbType)
            {
                return "SqlDbType." + name;
            }
        }

        return "dynamic";
    }
}