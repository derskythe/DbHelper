using System.Collections.Generic;
using System.Text;
using DbHelperPostgre.Db;
using EnumsNET;
using NpgsqlTypes;
using Shared;

namespace DbHelperPostgre;

internal static class Utils {
  private const string SPACE = "    ";

  public static string
  GenerateSelectTableOrViewMethod(string className,
                                  IReadOnlyList<ParameterInfo> list,
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
        "var paramList = new List<NpgsqlParameter> { ");
    funcData.Append(SPACE).Append(
        "GetParameter(\"@id\", 0, NpgsqlDbType.Integer) };\r\n");
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

  public static string
  GeneratePlSqlProcedure(string selectedItem,
                         IReadOnlyList<ParameterInfo> list) {
    var str = new StringBuilder();
    str.Append("CREATE FUNCTION ").Append(selectedItem).Append("_SAVE(\r\n");
    var i = 0;

    var fieldInsertName = new StringBuilder();
    var fieldInsertValues = new StringBuilder();
    var fieldUpdate = new StringBuilder();
    foreach (var pair in list) {
      bool isId = pair.Name.IsEqual("id");
      str.Append(SPACE).Append("V_").Append(pair.Name);
      str.Append(SPACE).Append(isId ? " IN OUT " : " IN ");
      str.Append(SPACE)
          .Append(selectedItem)
          .Append('.')
          .Append(pair.Name)
          .Append("%TYPE");

      fieldInsertName.Append(SPACE).Append(pair.Name);
      if (pair.Name.IsEqual("INSERT_DATE")) {
        fieldInsertValues.Append(SPACE).Append(
            "timezone('Asia/Baku'::text, now())");
      } else {
        fieldInsertValues.Append(SPACE).Append(SPACE).Append("V_").Append(
            pair.Name);
      }

      if (!isId) {
        fieldUpdate.Append(SPACE)
            .Append(SPACE)
            .Append(pair.Name)
            .Append(" = ")
            .Append("V_")
            .Append(pair.Name);
        if (i < list.Count - 1) {
          str.Append(",\r\n");
          fieldInsertName.Append(",\r\n");
          fieldInsertValues.Append(",\r\n");
          fieldUpdate.Append(",\r\n");
        }
      } else {
        if (i < list.Count - 1) {
          str.Append(",\r\n");
          fieldInsertName.Append(",\r\n");
          fieldInsertValues.Append(",\r\n");
        }
      }

      i++;
    }

    str.Append(
        ") RETURNS INTEGER\r\nLANGUAGE plpgsql\r\nAS\r\n$$\r\nBEGIN\r\n");

    str.Append(SPACE).Append("IF V_ID IS NULL THEN\r\n");
    str.Append(SPACE)
        .Append("INSERT ")
        .Append("INTO ")
        .Append(selectedItem)
        .Append("\r\n(");
    str.Append(SPACE)
        .Append(fieldInsertName)
        .Append("\r\n)\r\nVALUES\r\n(\r\n");
    str.Append(SPACE)
        .Append(fieldInsertValues)
        .Append("\r\n) RETURNING id INTO v_id;\r\n");
    // ELSE
    // UPDATE STATEMENT
    str.Append(SPACE).Append("ELSE\r\n");
    str.Append(SPACE)
        .Append("UPDATE ")
        .Append(selectedItem)
        .Append("\r\nSET\r\n");
    str.Append(SPACE).Append(fieldUpdate).AppendLine("");
    str.Append(SPACE).Append("WHERE id = V_ID;\r\n");
    str.Append(SPACE).Append("END IF;\r\n");
    str.Append(SPACE).Append("END;\r\n$$;");
    var result = str.ToString();
    return result;
  }

#endregion

  public static string GenerateProcedure(string selectedItem, string returnType,
                                         IReadOnlyList<ParameterInfo> paramList,
                                         bool radioSeparateChecked) {
    var funcData = new StringBuilder();
    if (!string.IsNullOrEmpty(returnType) && !returnType.IsEqual("void")) {
      funcData.Append("public async Task<")
          .Append(returnType.GetNetType())
          .Append("> ");
    } else {
      funcData.Append("public async Task ");
    }

    funcData.Append(selectedItem.ToUpperCamelCase(true));

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
      funcData.Append("var paramList = new List<NpgsqlParameter>\r\n{\r\n");
      foreach (var info in paramList) {
        if (!info.InParam) {
          funcData.Append("GetParameterOut(\"@")
              .Append(info.Name)
              .Append("\", ")
              .Append(info.DbType.GetDbParamType())
              .Append(", ");
          funcData.Append("null, ParameterDirection.Output");
        } else {
          funcData.Append("GetParameter(\"@").Append(info.Name).Append("\", ");
          if (radioSeparateChecked) {
            funcData.Append(info.Name.ToLowerCamelCase(false));
          } else {
            funcData.Append("item.").Append(info.Name);
          }

          funcData.Append(", ").Append(info.DbType.GetDbParamType());
        }

        funcData.Append("),\r\n");
      }

      funcData.Append("};\r\n\r\n");
    }

    if (!string.IsNullOrEmpty(returnType) && !returnType.IsEqual("void")) {
      funcData.Append("return await ExecuteScalar<")
          .Append(returnType.GetNetType())
          .Append(">(\"");
    } else {
      funcData.Append("await ExecuteNonQuery(\"");
    }

    funcData.Append(selectedItem);
    funcData.Append(paramList.Count > 0 ? "\", paramList);\r\n}"
                                        : "\", null);\r\n}");

    return funcData.ToString();
  }

  public static string GenerateClassData(string className,
                                         IReadOnlyList<ParameterInfo> list) {
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

  public static string GetNpgsqlDbType(this string type) {
    return type switch { "long" => "NpgsqlDbType.BigInt",
                         "byte[]" => "NpgsqlDbType.Bytea",
                         "int" => "NpgsqlDbType.Integer",
                         "decimal" => "NpgsqlDbType.Numeric",
                         "string" => "NpgsqlDbType.Varchar",
                         "DateTime" => "NpgsqlDbType.Timestamp",
                         "bool" => "NpgsqlDbType.Bit",
                         "double" => "NpgsqlDbType.Double",
                         "float" => "NpgsqlDbType.Real",
                         "short" => "NpgsqlDbType.SmallInt",
                         _ => string.Empty };
  }

  public static string GetClassName(this string value) {
    if (value.StartsWith("get")) {
      return value.Substring(3).ToUpperCamelCase(true);
    }

    if (value.StartsWith("list")) {
      return value.Substring(4).ToUpperCamelCase(true);
    }

    return value.ToUpperCamelCase(true);
  }

  public static string GetDbParamType(this string dbType) {
    dbType = dbType.ToUpperInvariant();
    if (dbType.IsEqual("CHARACTER VARYING")) {
      return "NpgsqlDbType.Varchar";
    }

    if (dbType.IsEqual("ARRAY")) {
      return "NpgsqlDbType.Array | NpgsqlDbType.Integer";
    }

    if (dbType.IsEqual("timestamp without time zone")) {
      return "NpgsqlDbType.Timestamp";
    }

    if (dbType.IsEqual("timestamp with time zone")) {
      return "NpgsqlDbType.TimestampTz";
    }

    if (dbType.IsEqual("time without time zone")) {
      return "NpgsqlDbType.Time";
    }

    if (dbType.IsEqual("time with time zone")) {
      return "NpgsqlDbType.TimeTz";
    }

    foreach (var name in Enums.GetNames(typeof(NpgsqlDbType))) {
      if (name.IsEqual(dbType)) {
        return "NpgsqlDbType." + name;
      }
    }

    return "dynamic";
  }
}
