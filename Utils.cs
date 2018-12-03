using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DbHelper
{
    internal static class Utils
    {
        public static string GenerateViewFunction(string className,
                                            List<KeyValuePair<string, string>> list,
                                            string selectedItem)
        {
            StringBuilder funcData = new StringBuilder();

            funcData.Append("public static List<")
                    .Append(className)
                    .Append("> List")
                    .Append(className)
                    .Append("()\n{");
            funcData.Append("OracleConnection connection = null;\n");
            funcData.Append("var result = new List<").Append(className).Append(">();\n");
            funcData.Append("try\n{\nconnection = new OracleConnection(_ConnectionString);\n");
            funcData.Append("connection.Open();\n\nusing (var command = new OracleCommand())\n{\n");
            funcData.Append("command.CommandText =");
            funcData.Append("\"SELECT ");
            int i = 0;
            foreach (var pair in list)
            {
                funcData.Append("t.").Append(pair.Key);
                i++;
                if (i < list.Count)
                {
                    funcData.Append(", ");
                }
            }

            funcData.Append(" FROM ").Append(selectedItem).Append(" t\";\n");
            funcData.Append("command.CommandType = CommandType.Text;\n");
            funcData.Append("command.Connection = connection;\ncommand.BindByName = true;\n\n");
            funcData.Append("using (var reader = command.ExecuteReader())\n{\n");
            funcData.Append("if (reader.HasRows)\n");
            funcData.Append("{\n");            
            funcData.Append("while (reader.Read())\n{\nvar item = new ")
                    .Append(className)
                    .Append("\n{\n");
            foreach (var pair in list)
            {
                var paramName = ToUpperCamelCase(pair.Key, true);
                var funcName = GetNetType(pair.Value);
                funcData.Append(paramName)
                        .Append(" = ")
                        
                        .Append("reader[\"")
                        .Append(pair.Key)
                        .Append("\"]")
                        .Append(".Get")
                        .Append(char.ToUpperInvariant(funcName[0]))
                        .Append(funcName.Substring(1))
                        .Append("(),\n");
            }

            funcData.Append("};\n").Append("result.Add(item);\n");
            funcData.Append("}\n}\n}\n}\n}\nfinally\n{\n");
            funcData.Append(
                "if (connection != null)\n{\nconnection.Close();\nconnection.Dispose();\n}\n}\nreturn result;\n}\n");
            return funcData.ToString();
        }

        public static string GenerateClassData(string className, List<KeyValuePair<string, string>> list)
        {
            var classData = new StringBuilder();

            classData.Append("[Serializable, XmlRoot(\"").Append(className).Append("\")]\n");
            classData.Append("[DataContract(Name = \"")
                     .Append(className)
                     .Append("\", Namespace = \"urn: Quetzalcoatlus\")]\n");
            classData.Append("public class ").Append(className).Append("\n{");
            List<string> upperList = new List<string>();
            foreach (var pair in list)
            {
                var upper = ToUpperCamelCase(pair.Key, true);
                classData.Append("[XmlElement]\n");
                classData.Append("[DataMember]\n");
                classData.Append("public ")
                         .Append(GetNetType(pair.Value))
                         .Append(" ")
                         .Append(upper)
                         .Append(" { get; set; }\n\n");
                upperList.Add(upper);
            }

            classData.Append("public ").Append(className).Append("()\n{\n}\n");
            classData.Append("public ").Append(className).Append("(\n");

            List<string> lowerList = new List<string>();
            int i = 0;
            foreach (var pair in list)
            {
                var lower = ToLowerCamelCase(pair.Key, true);
                classData.Append(GetNetType(pair.Value))
                         .Append(" ")
                         .Append(lower);
                i++;
                lowerList.Add(lower);
                if (i < list.Count)
                {
                    classData.Append(",\n");
                }
            }

            classData.Append(")\n{\n");
            i = 0;
            foreach (var pair in list)
            {
                classData.Append(upperList[i])
                         .Append(" = ")
                         .Append(lowerList[i])
                         .Append(";\n");
                i++;
            }

            classData.Append("}\n\n");

            classData.Append("public override string ToString()\n{\nreturn string.Format(\"");
            i = 0;
            foreach (var pair in upperList)
            {
                classData.Append(pair)
                         .Append(": ")
                         .Append("{")
                         .Append(i)
                         .Append("}, ");
                i++;
            }

            classData.Append("\",\n");
            i = 0;
            foreach (var pair in upperList)
            {
                classData.Append(pair);
                i++;
                if (i < list.Count)
                {
                    classData.Append(",");
                }
            }

            classData.Append(");\n}\n}\n");
            return classData.ToString();
        }

        public static string GetNetType(string oracleType)
        {
            oracleType = oracleType.ToUpperInvariant();
            if (oracleType.Contains("NUMBER(15)"))
            {
                return "long";
            }

            if (oracleType.Contains("NUMBER(5)"))
            {
                return "int";
            }

            if (oracleType.Contains("NUMBER"))
            {
                return "int";
            }

            if (oracleType.Contains("VARCHAR2") || oracleType.Contains("NVARCHAR2"))
            {
                return "string";
            }

            if (oracleType.Contains("TIMESTAMP") || oracleType.Contains("DATE"))
            {
                return "DateTime";
            }

            if (oracleType.Contains("BLOB") || oracleType.Contains("CLOB"))
            {
                return "byte[]";
            }

            return "object";
        }

        public static string GetOracleType(string type)
        {
            switch (type)
            {
                case "long":
                    return "OracleDbType.Int64";

                case "int":
                    return "OracleDbType.Int32";

                case "decimal":
                    return "OracleDbType.Decimal";

                case "string":
                    return "OracleDbType.Varchar2";

                case "DateTime":
                    return "OracleDbType.TimeStamp";

                case "byte[]":
                    return "OracleDbType.Blob";

                default:
                    return string.Empty;
            }
        }

        public static string ToUpperCamelCase(string value, bool cleanVar, bool manyType = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            var s = cleanVar
                ? value.ToLowerInvariant().Split('_')
                : value.Substring(2).ToLowerInvariant().Split('_');
            var str = new StringBuilder();
            foreach (var word in s)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    str.Append(char.ToUpperInvariant(word[0])).Append(word.Substring(1));
                }
            }

            var result = str.ToString();           
            if (manyType)
            {
                var r = new Regex(@"^([a-z0-9]+)es$", RegexOptions.IgnoreCase);
                if (r.IsMatch(result))
                {
                    result = r.Match(result).Groups[1].Value;
                }
                else
                {
                    r = new Regex(@"^([a-z0-9]+)s$", RegexOptions.IgnoreCase);
                    if (r.IsMatch(result))
                    {
                        result = r.Match(result).Groups[1].Value;
                    }
                }
            }

            return result;
        }

        public static string ToLowerCamelCase(string value, bool cleanVar, bool manyType = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            var s = cleanVar
                ? value.ToLowerInvariant().Split('_')
                : value.Substring(2).ToLowerInvariant().Split('_');
            var str = new StringBuilder();
            int i = 0;
            foreach (var word in s)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    if (i == 0)
                    {
                        str.Append(char.ToLowerInvariant(word[0])).Append(word.Substring(1));
                    }
                    else
                    {
                        str.Append(char.ToUpperInvariant(word[0])).Append(word.Substring(1));
                    }

                    i++;
                }
            }

            var result = str.ToString();
            if (manyType)
            {
                var r = new Regex(@"^([a-z0-9]+)es$", RegexOptions.IgnoreCase);
                if (r.IsMatch(result))
                {
                    result = r.Match(result).Groups[1].Value;
                }
                else
                {
                    r = new Regex(@"^([a-z0-9]+)s$", RegexOptions.IgnoreCase);
                    if (r.IsMatch(result))
                    {
                        result = r.Match(result).Groups[1].Value;
                    }
                }
            }

            return result;
        }
    }
}
