using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared;


public static class Extensions
{
    public static string GetStringFromArray<T>(this IEnumerable<T> list)
    {
        if (list == null)
        {
            return string.Empty;
        }

        var fields = new StringBuilder();

        foreach (var item in list)
        {
            if (item != null)
            {
                fields.Append(item).Append('\n');
            }
        }

        return fields.ToString();
    }

    public static int GetInt<T>(this T source, int defaultValue = 0)
    where T : class
    {
        return source != DBNull.Value ? Convert.ToInt32(source) : defaultValue;
    }

    /// <summary>
    /// Gets the long.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.Int64.</returns>
    public static long GetLong<T>(this T source)
    where T : class
    {
        return source != DBNull.Value ? Convert.ToInt64(source) : 0L;
    }

    /// <summary>
    /// Gets the string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.String.</returns>
    public static string GetString<T>(this T source)
    where T : class
    {
        return source != DBNull.Value ? Convert.ToString(source) : string.Empty;
    }

    /// <summary>
    /// Gets the base64 string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.Byte[].</returns>
    public static byte[] GetBase64String<T>(this T source)
    where T : class
    {
        return source != DBNull.Value ? Convert.ToString(source).IsBase64() : null;
    }

    private static byte[] IsBase64(this string base64String)
    {
        // Credit: oybek https://stackoverflow.com/users/794764/oybek
        if (string.IsNullOrEmpty(base64String))
        {
            return null;
        }

        if (string.IsNullOrEmpty(base64String) ||
                base64String.Length % 4 != 0       ||
                base64String.Contains(' ')         ||
                base64String.Contains('\t')        ||
                base64String.Contains('\r')        ||
                base64String.Contains('\n'))
        {
            return Encoding.ASCII.GetBytes(base64String);
        }

        try
        {
            return Convert.FromBase64String(base64String);
        }
        catch (Exception)
        {
            // Handle the exception
        }

        return Encoding.ASCII.GetBytes(base64String);
    }

    /// <summary>
    /// Gets the date time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>DateTime.</returns>
    public static DateTime GetDateTime<T>(this T source)
    where T : class
    {
        return source != DBNull.Value ? Convert.ToDateTime(source) : DateTime.MinValue;
    }

    /// <summary>
    /// Gets the bool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool GetBool<T>(this T source)
    where T : class
    {
        return source != DBNull.Value && Convert.ToInt32(source) != 0;
    }

    /// <summary>
    /// Gets the decimal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <returns>System.Decimal.</returns>
    public static decimal GetDecimal<T>(this T source)
    where T : class
    {
        return source != DBNull.Value ? Convert.ToDecimal(source) : 0M;
    }

    /// <summary>
    /// Gets the BLOB.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.Byte[].</returns>
    public static byte[] GetBlob(object value)
    {
        return value != DBNull.Value ? (byte[])value : null;
    }

    public static bool IsEqual(this string value1, string value2)
    {
        return !string.IsNullOrEmpty(value1) &&
               value1.Equals(value2, StringComparison.InvariantCultureIgnoreCase);
    }

    public static string ToUpperCamelCase(this string value, bool cleanVar, bool manyType = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        string[] s;
        var valueUpper = value.ToLowerInvariant();

        if (valueUpper.StartsWith("v_"))
        {
            s = cleanVar ? valueUpper[2..].Split('_') : valueUpper[3..].Split('_');
        }
        else
        {
            s = cleanVar ? valueUpper.Split('_') : valueUpper[1..].Split('_');
        }

        var str = new StringBuilder();

        foreach (var word in s)
        {
            if (!string.IsNullOrEmpty(word))
            {
                str.Append(char.ToUpperInvariant(word[0])).Append(word[1..]);
            }
        }

        var result = str.ToString();

        if (!manyType)
        {
            return result;
        }

        var r = new Regex("^([a-z0-9]+)es$", RegexOptions.IgnoreCase);

        if (r.IsMatch(result))
        {
            result = r.Match(result).Groups[1].Value;
        }
        else
        {
            r = new Regex("^([a-z0-9]+)s$", RegexOptions.IgnoreCase);

            if (r.IsMatch(result))
            {
                result = r.Match(result).Groups[1].Value;
            }
        }

        return result;
    }

    public static string ToLowerCamelCase(this string value, bool cleanVar, bool manyType = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var s = cleanVar ? value.ToLowerInvariant().Split('_') : value[1..].ToLowerInvariant().Split('_');
        var str = new StringBuilder();
        var i = 0;

        foreach (var word in s)
        {
            if (!string.IsNullOrEmpty(word))
            {
                if (i == 0)
                {
                    str.Append(char.ToLowerInvariant(word[0])).Append(word[1..]);
                }
                else
                {
                    str.Append(char.ToUpperInvariant(word[0])).Append(word[1..]);
                }

                i++;
            }
        }

        var result = str.ToString();

        if (!manyType)
        {
            return result;
        }

        var r = new Regex("^([a-z0-9]+)es$", RegexOptions.IgnoreCase);

        if (r.IsMatch(result))
        {
            result = r.Match(result).Groups[1].Value;
        }
        else
        {
            r = new Regex("^([a-z0-9]+)s$", RegexOptions.IgnoreCase);

            if (r.IsMatch(result))
            {
                result = r.Match(result).Groups[1].Value;
            }
        }

        return result;
    }
}
