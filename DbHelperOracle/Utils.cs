using System.Collections.Generic;
using System.Text;
using DbHelperOracle.Db;
using Shared;

// ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162

namespace DbHelperOracle;


internal static partial class Utils
{
    private const string SPACE = "    ";
    private const bool IS_STATIC = false;

    public static string GenerateViewFunction(string className, IReadOnlyList<KeyValuePair<string, string>> list, string selectedItem)
    {
        var funcData = new StringBuilder();

        if (IS_STATIC)
        {
            funcData.Append("public static async Task<List<").Append(className).Append(">> List").Append(className).Append("()\r\n{");
        }
        else
        {
            funcData.Append("public async Task<List<").Append(className).Append(">> List").Append(className).Append("()\r\n{");
        }

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

        funcData.Append(" FROM ").Append(selectedItem).Append(" t WHERE t.id = :id\";\r\n\r\n");
        funcData.Append(SPACE).Append("var paramList = new DbParam[]\r\n{\r\nnew DbParam(\"id\", OracleDbType.Int32, 0)\r\n};\r\n");

        funcData.Append("return await SelectMany(query, paramList, To")
        .Append(className)
        .Append(");\r\n}\r\n\r\n");

        funcData.Append("public static ").Append(className).Append(" To").Append(className).Append("(DbDataReader reader)\r\n{\r\n");
        funcData.Append("var result = new ").Append(className).Append("\r\n{\r\n");

        foreach (var pair in list)
        {
            var paramName = pair.Key.ToUpperCamelCase(true);
            var funcName = pair.Value.GetNetType();

            funcData.Append(SPACE)
            .Append(SPACE)
            .Append(SPACE)
            .Append(SPACE)
            .Append(SPACE)
            .Append(paramName)
            .Append(" = ");


            if (funcName.IsEqual("byte[]"))
            {
                funcData.Append("GetBlob(")
                .Append("reader[\"")
                .Append(pair.Key)
                .Append("\"]")
                .Append("),\r\n");
            }
            else
            {
                funcData.Append("reader[\"")
                .Append(pair.Key)
                .Append("\"]")
                .Append(".Get")
                .Append(char.ToUpperInvariant(funcName[0]))
                .Append(funcName[1..])
                .Append("(),\r\n");
            }
        }

        funcData.Append("};\r\nreturn result;\r\n}");

        return funcData.ToString();
    }

    public static string GeneratePlSqlProcedure(string selectedItem, IReadOnlyList<KeyValuePair<string, string>> list)
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
            str.Append(selectedItem).Append('.').Append(pair.Key).Append("%TYPE");
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
        str.Append("INSERT INTO").Append(' ').Append(selectedItem).Append("\r\n(");
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

    public static string GenerateProcedure(
        string ownerName,
        string packageName,
        string procName,
        bool radioSeparateChecked
    )
    {
        var list = OracleDb.ListProcedureParameters(ownerName, packageName, procName);

        var str = new StringBuilder();
        var upperPackageName = packageName.ToUpperCamelCase(true, false);
        var upperProcName = procName.ToUpperCamelCase(true, false);

        foreach (var paramList in list.ParamList)
        {
            var strParamList = new StringBuilder();
            var strArgumentsList = new StringBuilder();
            var listOutputTypes = new List<string>();

            foreach (var info in paramList)
            {
                strParamList.Append("new DbParam(\"").Append(info.DbName).Append("\", ").Append(info.NetType.GetOracleType()).Append(", ");

                if (!info.InParam)
                {
                    listOutputTypes.Add(info.NetType);
                    strParamList.Append("null, ParameterDirection.Output");
                }
                else
                {
                    if (radioSeparateChecked)
                    {
                        strArgumentsList.Append(info.NetType).Append(' ').Append(info.NameLowerCamelCase).AppendLine(",");
                        strParamList.Append(info.NameLowerCamelCase);
                    }
                    else
                    {
                        if (strArgumentsList.Length <= 0)
                        {
                            strArgumentsList.AppendLine("FooClass item, ");
                        }

                        strParamList.Append("item.").Append(info.Name);
                    }
                }

                strParamList.Append("),\r\n");
            }

            if (listOutputTypes.Count > 0)
            {
                if (listOutputTypes.Count > 1)
                {
                    str.Append("public static async Task<(");
                    str.Append(string.Join(',', listOutputTypes));
                    str.Append(")> ").Append(upperPackageName).Append(upperProcName).Append("(\r\n");
                }
                else
                {
                    str.Append("public static async Task<");
                    str.Append(string.Join(',', listOutputTypes));
                    str.Append("> ").Append(upperPackageName).Append(upperProcName).Append("(\r\n");
                }
            }
            else
            {
                str.Append("public static async Task ").Append(upperPackageName).Append(upperProcName).Append("(\r\n");
            }

            var args = strArgumentsList.ToString();
            str.Append(args.Substring(0, args.LastIndexOf(',')));

            str.Append(")\r\n{\r\n");
            str.Append("var paramList = new DbParam[] {\r\n");
            str.Append(strParamList);
            str.Append("};\r\n\r\n");

            if (listOutputTypes.Count > 0)
            {
                str.Append("var result = await ExecuteNonQuery<");
                str.Append(string.Join(',', listOutputTypes));

                str.Append(">(\"")
                .Append(string.IsNullOrEmpty(packageName) ? ownerName : ownerName + "." + packageName)
                .Append('.')
                .Append(procName)
                .AppendLine("\", paramList);\r\nreturn result;\r\n}\r\n");
            }
            else
            {
                str.Append("await ExecuteNonQuery(\"")
                .Append(string.IsNullOrEmpty(packageName) ? ownerName : ownerName + "." + packageName)
                .Append('.')
                .Append(procName)
                .AppendLine("\", paramList);\r\n}\r\n");
            }
        }

        str.AppendLine();

        return str.ToString();
    }

    public static string GenerateClassData(string className, IReadOnlyList<KeyValuePair<string, string>> list)
    {
        var classData = new StringBuilder();

        classData.Append("[Serializable, XmlRoot(\"").Append(className).Append("\")]\r\n");

        classData.Append("[DataContract(Name = \"")
        .Append(className)
        .Append("\")]\r\n");

        classData.Append("public sealed record ").Append(className).Append("\r\n{");
        var upperList = new List<string>();

        foreach (var pair in list)
        {
            var upper = pair.Key.ToUpperCamelCase(true, false);
            classData.Append(SPACE).Append("[XmlElement]\r\n");
            classData.Append(SPACE).Append("[DataMember]\r\n");

            classData.Append(SPACE)
            .Append("public ")
            .Append(pair.Value.GetNetType())
            .Append(' ')
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
            var lower = pair.Key.ToLowerCamelCase(true);

            classData.Append(pair.Value.GetNetType())
            .Append(' ')
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
            .Append('{')
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

        if (oracleType.Contains("NUMBER(15.0)"))
        {
            return "long";
        }

        if (oracleType.Contains("NUMBER(5.0)"))
        {
            return "int";
        }

        if (oracleType.Contains("NUMBER"))
        {
            return "long";
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
        "long"     => "OracleDbType.Int64",
        "int"      => "OracleDbType.Int32",
        "decimal"  => "OracleDbType.Decimal",
        "string"   => "OracleDbType.Varchar2",
        "DateTime" => "OracleDbType.TimeStamp",
        "byte[]"   => "OracleDbType.Blob",
        _          => string.Empty
    };
}
}
