using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace DbWinForms;


/// <summary>
/// Class Utils.
/// </summary>
internal static class Utils
{
    /// <summary>
    /// Gets the int.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.Int32.</returns>
    public static int GetInt<T>(this T source) where T : class
    {
        return source != DBNull.Value ? Convert.ToInt32(source) : 0;
    }

    /// <summary>
    /// Gets the long.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.Int64.</returns>
    public static long GetLong<T>(this T source) where T : class
    {
        return source != DBNull.Value ? Convert.ToInt64(source) : 0L;
    }

    /// <summary>
    /// Gets the string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.String.</returns>
    public static string GetString<T>(this T source) where T : class
    {
        return source != DBNull.Value ? Convert.ToString(source) : string.Empty;
    }

    /// <summary>
    /// Gets the date time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>DateTime.</returns>
    public static DateTime GetDateTime<T>(this T source) where T : class
    {
        return source != DBNull.Value ? Convert.ToDateTime(source) : DateTime.MinValue;
    }

    /// <summary>
    /// Gets the bool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns><c>true</c> if value, <c>false</c> otherwise.</returns>
    public static bool GetBool<T>(this T source) where T : class
    {
        return source != DBNull.Value && Convert.ToInt32(source) != 0;
    }

    /// <summary>
    /// Gets the decimal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.Decimal.</returns>
    public static decimal GetDecimal<T>(this T source) where T : class
    {
        return source != DBNull.Value ? Convert.ToDecimal(source) : 0M;
    }

    /// <summary>
    /// Gets the double.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.Double.</returns>
    public static double GetDouble<T>(this T source) where T : class
    {
        return source != DBNull.Value ? Convert.ToDouble(source) : 0F;
    }

    public static byte[] GetBytes<T>(this T source) where T : class
    {
        return source != null && source != DBNull.Value ? source as byte[] : null;
    }

    /// <summary>
    /// Determines whether the specified column name has column.
    /// </summary>
    /// <param name="dr">The dr.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns><c>true</c> if the specified column name has column; otherwise, <c>false</c>.</returns>
    public static bool HasColumn(this IDataRecord dr, string columnName)
    {
        for (int i = 0; i < dr.FieldCount; i++)
        {
            if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string GetDescription<T>(this T value)
    {
        try
        {
            if (value == null)
            {
                return string.Empty;
            }

            var field = value.GetType().GetField(value.ToString() ?? string.Empty);

            if (field == null)
            {
                return string.Empty;
            }

            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute ?
                value.ToString() :
                attribute.Description;
        }
        catch (Exception exp)
        {
            Debug.WriteLine(exp.Message);
        }

        return string.Empty;
    }

    /// <summary>
    /// Format array to string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <returns>System.String.</returns>
    public static string GetStringFromArray<T>(this IEnumerable<T> list)
    {
        var fields = new StringBuilder();

        if (list != null)
        {
            foreach (T item in list)
            {
                if (item != null)
                {
                    fields.Append(item).Append("\n");
                }
            }
        }

        return fields.ToString();
    }
}
