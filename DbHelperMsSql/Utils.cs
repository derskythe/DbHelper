using System.Collections.Generic;
using System.Data;
using System.Text;
using DbWinForms.Models;
using EnumsNET;
using Shared;

namespace DbHelperMsSql;

internal static class Utils {
  private const string SPACE = "    ";

  public static string GenerateSelectTableOrViewMethod(string className,
                                                       List<ParameterInfo> list,
                                                       string selectedItem) {
    var funcData = new StringBuilder();

    funcData.Append("public async Task<List<")
        .Append(className)
        .Append(">> List")
        .Append(className)
        .Append("()\r\n{");

    funcData.Append(SPACE).Append(SPACE).Append(
        "const string query = \"SELECT ");
    var i = 0;
    foreach (var pair in list) {
      funcData.Append("t.").Append(pair.Name);
      i++;
      if (i < list.Count) {
        funcData.Append(", ");
      }
    }

    funcData.Append(" FROM ").Append(selectedItem).Append(" t\";\r\n\r\n");
    funcData.Append(SPACE).Append(
        "var paramList = new DbParameter[]\r\n{};\r\n");
    funcData.Append("return await Many(query, paramList, Converter.To")
        .Append(className)
        .Append(");\r\n}\r\n\r\n");

    funcData.Append("public static ")
        .Append(className)
        .Append(" To")
        .Append(className)
        .Append("(DbDataReader reader)\r\n{\r\n");
    funcData.Append("var result = new ").Append(className).Append("\r\n{\r\n");
    foreach (var pair in list) {
      var paramName = pair.Name.ToUpperCamelCase(true);
      funcData.Append(SPACE)
          .Append(SPACE)
          .Append(SPACE)
          .Append(SPACE)
          .Append(SPACE)
          .Append(paramName)
          .Append(" = ")
          .Append("reader[\"")
          .Append(pair.Name)
          .Append("\"]")
          .Append(".Get")
          .Append(char.ToUpperInvariant(pair.NetType[0]))
          .Append(pair.NetType.Substring(1))
          .Append("(),\r\n");
    }

    funcData.Append("};\r\nreturn result;\r\n}");

    return funcData.ToString();
  }

#region GeneratePlSqlProcedure

  public static string GeneratePlSqlProcedure(string selectedItem,
                                              List<ParameterInfo> list) {
    var str = new StringBuilder();
    str.Append("PROCEDURE SAVE_").Append(selectedItem).Append("(\r\n");
    var i = 0;

    var fieldInsertName = new StringBuilder();
    var fieldInsertValues = new StringBuilder();
    var fieldUpdate = new StringBuilder();
    foreach (var pair in list) {
      str.Append("V_").Append(pair.Name.ToLowerCamelCase(false));
      str.Append(pair.Name.IsEqual("id") ? " IN OUT " : " IN ");
      str.Append(selectedItem).Append('.').Append(pair.Name).Append("%TYPE");
      fieldInsertName.Append(pair.Name);
      fieldInsertValues.Append("V_").Append(pair.Name);
      fieldUpdate.Append(pair.Name).Append(" = ").Append("V_").Append(
          pair.Name);
      if (i < list.Count - 1) {
        str.Append(",\r\n");
        fieldInsertName.Append(",\r\n");
        fieldInsertValues.Append(",\r\n");
        fieldUpdate.Append(",\r\n");
      }

      i++;
    }

    str.Append(") IS\r\n");
    str.Append(
        "EXP_CUSTOM EXCEPTION;\r\nPRAGMA EXCEPTION_INIT(EXP_CUSTOM, " +
        "-20001);\r\nV_CODE NUMBER;\r\nV_ERRM VARCHAR2(255);\r\nBEGIN\r\n");
    // IF V_ID IS NULL THEN
    // INSERT STATEMENT
    str.Append("IF V_ID IS NULL THEN\r\n");
    str.Append("SELECT SEQ_")
        .Append(selectedItem)
        .Append("_ID.NEXTVAL INTO V_ID FROM DUAL;\r\n");
    str.Append("INSERT ").Append("INTO ").Append(selectedItem).Append("\r\n(");
    str.Append(fieldInsertName).Append("\r\n)\r\nVALUES\r\n(\r\n");
    str.Append(fieldInsertValues).Append("\r\n);\r\n");
    // ELSE
    // UPDATE STATEMENT
    str.Append("ELSE\r\n");
    str.Append("UPDATE ").Append(selectedItem).Append("\r\nSET\r\n");
    str.Append(fieldUpdate);
    str.Append("\r\nWHERE id = V_ID;\r\nEND IF;\r\n");
    // EXCEPTION
    str.Append("EXCEPTION\r\nWHEN OTHERS THEN\r\nROLLBACK;\r\nV_CODE:= " +
               "SQLCODE;\r\nV_ERRM:= SUBSTR(SQLERRM, 1, " +
               "255);\r\nRAISE_APPLICATION_ERROR(-20001, V_CODE || CHR(10) " +
               "|| V_ERRM || CHR(10) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE " +
               "|| CHR(10));\r\nEND ")
        //.Append("SAVE_")
        //.Append(selectedItem)
        .Append(";\r\n");

    var result = str.ToString();
    return result;
  }

#endregion

  public static string GenerateProcedure(string selectedItem,
                                         List<ParameterInfo> paramList,
                                         bool radioSeparateChecked) {
    var funcData = new StringBuilder();
    funcData.Append("public async Task ")
        .Append(selectedItem.ToUpperCamelCase(true));

    var i = 0;
    if (radioSeparateChecked) {
      if (paramList.Count > 3) {
        funcData.Append("(\r\n");
      } else {
        funcData.Append('(');
      }

      foreach (var info in paramList) {
        funcData.Append(info.NetType)
            .Append(' ')
            .Append(info.Name.ToLowerCamelCase(false));
        i++;
        if (i < paramList.Count) {
          funcData.Append(paramList.Count <= 3 ? ", " : ",\r\n");
        }
      }
    } else {
      funcData.Append("(FooClass item");
    }

    funcData.Append(")\r\n{\r\n");

    if (paramList.Count > 0) {
      funcData.Append("var paramList = new DbParameter[]\r\n{\r\n");
      foreach (var info in paramList) {
        if (!info.InParam) {
          funcData.Append("GetParameterOut(\"")
              .Append(info.Name)
              .Append("\", ")
              .Append(info.DbType.GetDbParamType())
              .Append(", ");
          funcData.Append("null, ParameterDirection.Output");
        } else {
          funcData.Append("GetParameter(\"")
              .Append(info.Name)
              .Append("\", ")
              .Append(info.DbType.GetDbParamType())
              .Append(", ");

          if (radioSeparateChecked) {
            funcData.Append(info.Name.ToLowerCamelCase(false));
          } else {
            funcData.Append("item.").Append(info.Name);
          }
        }

        funcData.Append("),\r\n");
      }

      funcData.Append("};\r\n\r\n");
    }

    funcData.Append("await ExecuteNonQuery(\"").Append(selectedItem);
    funcData.Append(paramList.Count > 0 ? "\", paramList);\r\n}"
                                        : "\", null);\r\n}");

    return funcData.ToString();
  }

  internal static string GenerateProcedure(string selectedItem,
                                           string className,
                                           List<ParameterInfo> paramList,
                                           List<ParameterInfo> returnedFields,
                                           bool radioSeparateChecked) {
    var funcData = new StringBuilder();
    var singleRecord = selectedItem.StartsWith("get");
    if (singleRecord) {
      funcData.Append("public async Task<")
          .Append(className)
          .Append("> ")
          .Append(selectedItem.ToUpperCamelCase(true));
    } else {
      funcData.Append("public async Task<List<")
          .Append(className)
          .Append(">> ")
          .Append(selectedItem.ToUpperCamelCase(true));
    }

    var i = 0;
    if (radioSeparateChecked) {
      if (paramList.Count > 3) {
        funcData.Append("(\r\n");
      } else {
        funcData.Append('(');
      }

      foreach (var info in paramList) {
        funcData.Append(info.NetType)
            .Append(' ')
            .Append(info.Name.ToLowerCamelCase(false));
        i++;
        if (i < paramList.Count) {
          funcData.Append(paramList.Count <= 3 ? ", " : ",\r\n");
        }
      }

      funcData.Append(")\r\n{\r\n");
    } else if (paramList.Count > 0) {
      funcData.Append("(FooClass item)\r\n{\r\n");
    }

    if (paramList.Count > 0) {
      funcData.Append("var paramList = new DbParameter[]\r\n{\r\n");
      foreach (var info in paramList) {
        if (!info.InParam) {
          funcData.Append("GetParameterOut(\"")
              .Append(info.Name)
              .Append("\", ")
              .Append(info.DbType.GetDbParamType())
              .Append(", ");
          funcData.Append("null, ParameterDirection.Output");
        } else {
          funcData.Append("GetParameter(\"")
              .Append(info.Name)
              .Append("\", ")
              .Append(info.DbType.GetDbParamType())
              .Append(", ");

          if (radioSeparateChecked) {
            funcData.Append(info.Name.ToLowerCamelCase(false));
          } else {
            funcData.Append("item.").Append(info.Name);
          }
        }

        funcData.Append("),\r\n");
      }

      funcData.Append("};\r\n\r\n");
    }

    funcData.Append("return await ");

    if (singleRecord) {
      funcData.Append("Single");
    } else {
      funcData.Append("Many");
    }

    funcData.Append("(\"")
        .Append(selectedItem)
        .Append("\", ")
        .Append(paramList.Count > 0 ? "paramList, " : "null, ")
        .Append("Converter.To")
        .Append(className)
        .Append(");\r\n}\r\n\r\n");

    funcData.Append("public static ")
        .Append(className)
        .Append(" To")
        .Append(className)
        .Append("(DbDataReader reader)\r\n{\r\n");
    funcData.Append("var result = new ").Append(className).Append("\r\n{\r\n");
    foreach (var pair in returnedFields) {
      var paramName = pair.Name.ToUpperCamelCase(true);
      funcData.Append(SPACE)
          .Append(SPACE)
          .Append(SPACE)
          .Append(SPACE)
          .Append(SPACE)
          .Append(paramName)
          .Append(" = ")
          .Append("reader[\"")
          .Append(pair.Name)
          .Append("\"]")
          .Append(".Get");
      if (pair.NetType == "byte[]") {
        funcData.Append("Bytes");
      } else {
        funcData.Append(char.ToUpperInvariant(pair.NetType[0]))
            .Append(pair.NetType.Substring(1));
      }

      funcData.Append("(),\r\n");
    }

    funcData.Append("};\r\nreturn result;\r\n}");

    return funcData.ToString();
  }

  public static string GenerateClassData(string className,
                                         List<ParameterInfo> list) {
    var classData = new StringBuilder();

    classData.Append("[Serializable]\r\n");
    classData.Append("[DataContract]\r\n");
    classData.Append("public sealed record ").Append(className).Append("\r\n{");
    var upperList = new List<string>();
    foreach (var pair in list) {
      var upper = pair.Name.ToUpperCamelCase(true);
      // classData.Append(SPACE).Append("[XmlElement]\r\n");
      classData.Append(SPACE).Append("[DataMember]\r\n");
      classData.Append(SPACE)
          .Append("public ")
          .Append(pair.NetType)
          .Append(' ')
          .Append(upper)
          .Append(" { get; set; }\r\n\r\n");
      upperList.Add(upper);
    }

    var i = 0;

    classData.Append(SPACE)
        .Append("public ")
        .Append(className)
        .Append("()\r\n")
        .Append(SPACE)
        .Append("{}\r\n");
    classData.Append(SPACE).Append("public ").Append(className).Append("(\r\n");

    var lowerList = new List<string>();

    foreach (var pair in list) {
      var lower = pair.Name.ToLowerCamelCase(true);
      classData.Append(pair.NetType).Append(' ').Append(lower);
      i++;
      lowerList.Add(lower);
      if (i < list.Count) {
        classData.Append(",\r\n");
      }
    }

    classData.Append(")\r\n{\r\n");
    i = 0;
    foreach (var _ in list) {
      classData.Append(upperList[i])
          .Append(" = ")
          .Append(lowerList[i])
          .Append(";\r\n");
      i++;
    }

    classData.Append("}\r\n\r\n");

    classData.Append(SPACE).Append(
        "public override string ToString()\r\n{\r\nreturn $\"");
    i = 0;
    foreach (var value in upperList) {
      classData.Append(value).Append(": ");

      if (list[i].NetType == "byte[]") {
        classData.Append('{').Append(value).Append(" != null").Append('}');
      } else {
        classData.Append('{').Append(value).Append('}');
      }

      if (i + 1 < upperList.Count) {
        classData.Append(", ");
      }

      i++;
    }

    classData.Append("\";\r\n}\r\n}\r\n");
    return classData.ToString();
  }

  private static string GetSqlDbType(this string type) {
    return type switch { "long" => "SqlDbType.BigInt",
                         "byte[]" => "SqlDbType.VarBinary",
                         "int" => "SqlDbType.Int",
                         "decimal" => "SqlDbType.Decimal",
                         "string" => "SqlDbType.VarChar",
                         "DateTime" => "SqlDbType.DateTime",
                         "bool" => "SqlDbType.Bit",
                         "DateTimeOffset" => "SqlDbType.DateTimeOffset",
                         "double" => "SqlDbType.Float",
                         "float" => "SqlDbType.Real",
                         "short" => "SqlDbType.SmallInt",
                         "TimeSpan" => "SqlDbType.Time",
                         "byte" => "SqlDbType.TinyInt",
                         _ => string.Empty };
  }

  public static string GetClassName(this string value) {
    if (value.StartsWith("get")) {
      return value.Substring(3).ToUpperCamelCase(true);
    } else if (value.StartsWith("list")) {
      return value.Substring(4).ToUpperCamelCase(true);
    } else {
      return value.ToUpperCamelCase(true);
    }
  }

  private static string GetDbParamType(this string msSqlDbType) {
    msSqlDbType = msSqlDbType.ToUpperInvariant();
    foreach (string name in Enums.GetNames(typeof(SqlDbType))) {
      if (name.ToUpperInvariant() == msSqlDbType) {
        return "SqlDbType." + name;
      }
    }

    return "dynamic";
  }
}
