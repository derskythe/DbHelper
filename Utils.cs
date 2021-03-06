﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace DbHelper
{
    internal static class Utils
    {
        private const string SPACE = "    ";

        public static string GenerateOldViewFunction(string className,
                                            List<KeyValuePair<string, string>> list,
                                            string selectedItem)
        {
            var funcData = new StringBuilder();

            funcData.Append("public static List<").Append(className).Append("> List").Append(className).Append("()\r\n{");
            funcData.Append(SPACE).Append("OracleConnection connection = null;\r\n");
            funcData.Append(SPACE).Append("var result = new List<").Append(className).Append(">();\r\n");
            funcData.Append(SPACE).Append("try\r\n{\r\n");
            funcData.Append(SPACE).Append(SPACE).Append("connection = new OracleConnection(_ConnectionString);\r\n");
            funcData.Append(SPACE).Append(SPACE).Append("connection.Open();\r\n\r\n");
            funcData.Append(SPACE).Append(SPACE).Append("const string sql = \"SELECT ");

            var i = 0;
            foreach (var pair in list)
            {
                funcData.Append("t.").Append(pair.Key);
                i++;
                if (i < list.Count)
                {
                    funcData.Append(", ");
                }
            }

            funcData.Append(" FROM ").Append(selectedItem).Append(" t\";\r\n");

            funcData.Append(SPACE).Append(SPACE).Append("using (var command = new OracleCommand())\r\n{\r\n");
            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append("command.CommandText = sql;");

            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append("command.CommandType = CommandType.Text;\r\n");
            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append("command.Connection = connection;\r\ncommand.BindByName = true;\r\n\r\n");
            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append("using (var reader = command.ExecuteReader())\r\n{\r\n");
            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append("if (reader.HasRows)\r\n");
            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append("{\r\n");
            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append("while (reader.Read())\r\n{\r\nvar item = new ").Append(className).Append("\r\n{\r\n");
            foreach (var pair in list)
            {
                var paramName = ToUpperCamelCase(pair.Key, true);
                var funcName = GetNetType(pair.Value);
                funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append(paramName)
                        .Append(" = ")
                        .Append("reader[\"")
                        .Append(pair.Key)
                        .Append("\"]")
                        .Append(".Get")
                        .Append(char.ToUpperInvariant(funcName[0]))
                        .Append(funcName.Substring(1))
                        .Append("(),\r\n");
            }

            funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append("};\r\n").Append("result.Add(item);\r\n");
            funcData.Append("}\r\n}\r\n}\r\n}\r\n}\r\nfinally\r\n{\r\n");
            funcData.Append(
                "if (connection != null)\r\n{\r\nconnection.Close();\r\nconnection.Dispose();\r\n}\r\n}\r\nreturn result;\r\n}\r\n");
            return funcData.ToString();
        }

        public static string GenerateViewFunction(string className, List<KeyValuePair<string, string>> list, string selectedItem)
        {
            var funcData = new StringBuilder();

            funcData.Append("public static async Task<List<").Append(className).Append(">> List").Append(className).Append("()\r\n{");

            funcData.Append(SPACE).Append(SPACE).Append("const string query = \"SELECT ");
            var i = 0;
            foreach (var pair in list)
            {
                funcData.Append("t.").Append(pair.Key);
                i++;
                if (i < list.Count)
                {
                    funcData.Append(", ");
                }
            }
            funcData.Append(" FROM ").Append(selectedItem).Append(" t\";\r\n\r\n");
            funcData.Append(SPACE).Append("var paramList = new List<DbParam>\r\n{};\r\n");
            funcData.Append("return await SelectMany(query, paramList, To")
                    .Append(className)
                    .Append(");\r\n}\r\n\r\n");

            funcData.Append("public static ").Append(className).Append(" To").Append(className).Append("(DbDataReader reader)\r\n{\r\n");
            funcData.Append("var result = new ").Append(className).Append("\r\n{\r\n");
            foreach (var pair in list)
            {
                var paramName = ToUpperCamelCase(pair.Key, true);
                var funcName = GetNetType(pair.Value);
                funcData.Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append(SPACE).Append(paramName)
                        .Append(" = ")
                        .Append("reader[\"")
                        .Append(pair.Key)
                        .Append("\"]")
                        .Append(".Get")
                        .Append(char.ToUpperInvariant(funcName[0]))
                        .Append(funcName.Substring(1))
                        .Append("(),\r\n");
            }

            funcData.Append("};\r\nreturn result;\r\n}");

            return funcData.ToString();
        }

        public static string GeneratePlSqlProcedure(string selectedItem, List<KeyValuePair<string, string>> list)
        {
            var str = new StringBuilder();
            str.Append("PROCEDURE SAVE_").Append(selectedItem).Append("(\r\n");
            var i = 0;


            var fieldInsertName = new StringBuilder();
            var fieldInsertValues = new StringBuilder();
            var fieldUpdate = new StringBuilder();
            foreach (var pair in list)
            {
                str.Append("V_").Append(pair.Key);
                str.Append(pair.Key.IsEqual("id") ? " IN OUT " : " IN ");
                str.Append(selectedItem).Append(".").Append(pair.Key).Append("%TYPE");
                fieldInsertName.Append(pair.Key);
                fieldInsertValues.Append("V_").Append(pair.Key);
                fieldUpdate.Append(pair.Key).Append(" = ").Append("V_").Append(pair.Key);
                if (i < list.Count - 1)
                {
                    str.Append(",\r\n");
                    fieldInsertName.Append(",\r\n");
                    fieldInsertValues.Append(",\r\n");
                    fieldUpdate.Append(",\r\n");
                }

                i++;
            }

            str.Append(") IS\r\n");
            str.Append(
                "EXP_CUSTOM EXCEPTION;\r\nPRAGMA EXCEPTION_INIT(EXP_CUSTOM, -20001);\r\nV_CODE NUMBER;\r\nV_ERRM VARCHAR2(255);\r\nBEGIN\r\n"
            );
            // IF V_ID IS NULL THEN
            // INSERT STATEMENT
            str.Append("IF V_ID IS NULL THEN\r\n");
            str.Append("SELECT SEQ_").Append(selectedItem).Append("_ID.NEXTVAL INTO V_ID FROM DUAL;\r\n");
            str.Append("INSERT INTO ").Append(selectedItem).Append("\r\n(");
            str.Append(fieldInsertName).Append("\r\n)\r\nVALUES\r\n(\r\n");
            str.Append(fieldInsertValues).Append("\r\n);\r\n");
            // ELSE
            // UPDATE STATEMENT
            str.Append("ELSE\r\n");
            str.Append("UPDATE ").Append(selectedItem).Append("\r\nSET\r\n");
            str.Append(fieldUpdate);
            str.Append("\r\nWHERE id = V_ID;\r\nEND IF;\r\n");
            // EXCEPTION
            str.Append(
                   "EXCEPTION\r\nWHEN OTHERS THEN\r\nROLLBACK;\r\nV_CODE:= SQLCODE;\r\nV_ERRM:= SUBSTR(SQLERRM, 1, 255);\r\nRAISE_APPLICATION_ERROR(-20001, V_CODE || CHR(10) || V_ERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || CHR(10));\r\nEND "
               )
               //.Append("SAVE_")
               //.Append(selectedItem)
               .Append(";\r\n");

            var result = str.ToString();
            return result;
        }

        public static string GenerateOldProcudure(string selectedItem, bool radioSeparateChecked)
        {
            var spl = selectedItem.Split('.');
            var packageName = spl[0];
            var procName = spl[1];

            var list = OracleDb.ListProcedureParameters(packageName, procName);

            var str = new StringBuilder();
            var methodHeader = "public static void " +
                               ToUpperCamelCase(packageName, true) +
                               ToUpperCamelCase(procName, true) + "(\r\n";
            foreach (var paramList in list.ParamList)
            {
                str.Append(methodHeader);
                var i = 0;
                if (radioSeparateChecked)
                {
                    foreach (var info in paramList)
                    {
                        str.Append(info.NetType).Append(" ").Append(info.NameLowerCamelCase);

                        i++;
                        if (i < paramList.Count)
                        {
                            str.Append(",\r\n");
                        }
                    }
                }
                else
                {
                    str.Append("FooClass item");
                }

                str.Append(")\r\n{\r\nOracleConnection connection = null;\r\n");
                str.Append("try\r\n{\r\nconnection = GetConnection();\r\n");
                str.Append("const string cmdText =\r\n\"begin ")
                   .Append(packageName)
                   .Append(".")
                   .Append(procName)
                   .Append("(");
                i = 0;
                foreach (var info in paramList)
                {
                    str.Append(info.DbName).Append(" => :").Append(info.DbName);

                    i++;
                    if (i < paramList.Count)
                    {
                        str.Append(",");
                    }
                }

                str.Append("); end;\";\r\n\r\n");
                str.Append("using (var cmd = new OracleCommand(cmdText, connection))\r\n{\r\n");
                var hasOutputParam = false;
                foreach (var info in paramList)
                {
                    str.Append("cmd.Parameters.Add(\"").Append(info.DbName);
                    str.Append("\", ").Append(info.NetType.GetOracleType()).Append(", ");
                    if (info.InParam)
                    {
                        str.Append("ParameterDirection.Input).Value = ");
                        if (radioSeparateChecked)
                        {
                            str.Append(info.NameLowerCamelCase).Append(";\r\n");
                        }
                        else
                        {
                            str.Append("item.").Append(info.Name).Append(";\r\n");
                        }
                    }
                    else
                    {
                        hasOutputParam = true;
                        str.Append("ParameterDirection.Output);\r\n");
                    }
                }

                str.Append("cmd.ExecuteNonQuery();\r\n");

                if (hasOutputParam)
                {
                    i = 0;
                    foreach (var info in paramList)
                    {
                        if (!info.InParam)
                        {
                            str.Append("var var").Append(i).Append(" = ");
                            str.Append("cmd.Parameters[\"").Append(info.DbName).Append("\"]");
                            str.Append(
                                   ".Value == null ? 0 : ((OracleDecimal)"
                               )
                               .Append("cmd.Parameters[\"")
                               .Append(info.DbName)
                               .Append("\"].Value).ToInt64();\r\n");
                        }
                    }
                }

                str.Append(
                    "}\r\n}\r\nfinally\r\n{\r\nif (connection != null)\r\n{\r\nconnection.Close();\r\nconnection.Dispose();\r\n}\r\n}\r\n}\r\n\r\n\r\n"
                );
            }

            return str.ToString();
        }

        public static string GenerateProcedure(string selectedItem, bool radioSeparateChecked)
        {
            var spl = selectedItem.Split('.');
            var packageName = spl[0];
            var procName = spl[1];

            var list = OracleDb.ListProcedureParameters(packageName, procName);

            var str = new StringBuilder();
            var methodHeader = "public static async Task " +
                               ToUpperCamelCase(packageName, true) +
                               ToUpperCamelCase(procName, true) + "(\r\n";
            foreach (var paramList in list.ParamList)
            {
                str.Append(methodHeader);
                var i = 0;
                if (radioSeparateChecked)
                {
                    foreach (var info in paramList)
                    {
                        str.Append(info.NetType).Append(" ").Append(info.NameLowerCamelCase);

                        i++;
                        if (i < paramList.Count)
                        {
                            str.Append(",\r\n");
                        }
                    }
                }
                else
                {
                    str.Append("FooClass item");
                }

                str.Append(")\r\n{\r\n");
                str.Append("var paramList = new List<DbParam> {\r\n");
                foreach (var info in paramList)
                {
                    str.Append("new DbParam(\"").Append(info.DbName).Append("\", ").Append(info.NetType.GetOracleType()).Append(", ");
                    if (!info.InParam)
                    {
                        str.Append("null, ParameterDirection.Output");
                    }
                    else
                    {
                        if (radioSeparateChecked)
                        {
                            str.Append(info.NameLowerCamelCase);
                        }
                        else
                        {
                            str.Append("item.").Append(info.Name);
                        }
                    }

                    str.Append("),\r\n");
                }
                str.Append("};\r\n\r\n");
                str.Append("await ExecuteNonQuery(\"")
                   .Append(packageName)
                   .Append(".")
                   .Append(procName)
                   .Append("\", paramList);\r\n}");
            }

            return str.ToString();
        }

        public static string GenerateClassData(string className, List<KeyValuePair<string, string>> list)
        {
            var classData = new StringBuilder();

            classData.Append("[Serializable, XmlRoot(\"").Append(className).Append("\")]\r\n");
            classData.Append("[DataContract(Name = \"")
                     .Append(className)
                     .Append("\")]\r\n");
            classData.Append("public class ").Append(className).Append("\r\n{");
            var upperList = new List<string>();
            foreach (var pair in list)
            {
                var upper = ToUpperCamelCase(pair.Key, true);
                classData.Append(SPACE).Append("[XmlElement]\r\n");
                classData.Append(SPACE).Append("[DataMember]\r\n");
                classData.Append(SPACE).Append("public ")
                         .Append(GetNetType(pair.Value))
                         .Append(" ")
                         .Append(upper)
                         .Append(" { get; set; }\r\n\r\n");
                upperList.Add(upper);
            }

            classData.Append(SPACE).Append("public ").Append(className).Append("()\r\n").Append(SPACE).Append("{}\r\n");
            classData.Append(SPACE).Append("public ").Append(className).Append("(\r\n");

            var lowerList = new List<string>();
            var i = 0;
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
                    classData.Append(",\r\n");
                }
            }

            classData.Append(")\r\n{\r\n");
            i = 0;
            foreach (var _ in list)
            {
                classData.Append(upperList[i])
                         .Append(" = ")
                         .Append(lowerList[i])
                         .Append(";\r\n");
                i++;
            }

            classData.Append("}\r\n\r\n");

            classData.Append(SPACE).Append("public override string ToString()\r\n{\r\nreturn string.Format(\"");
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

            classData.Append("\",\r\n");
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

            classData.Append(");\r\n}\r\n}\r\n");
            return classData.ToString();
        }

        public static string GetNetType(this string oracleType)
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

        private static string GetOracleType(this string type)
        {
            return type switch
            {
                "long" => "OracleDbType.Int64",
                "int" => "OracleDbType.Int32",
                "decimal" => "OracleDbType.Decimal",
                "string" => "OracleDbType.Varchar2",
                "DateTime" => "OracleDbType.TimeStamp",
                "byte[]" => "OracleDbType.Blob",
                _ => string.Empty
            };
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
            var i = 0;
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

        public static bool IsEqual(this string value1, string value2)
        {
            if (string.IsNullOrEmpty(value1))
            {
                return false;
            }

            return value1.Equals(value2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
