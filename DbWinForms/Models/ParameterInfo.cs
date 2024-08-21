using System.Runtime.Serialization;

namespace DbWinForms.Models;

[DataContract]
public readonly record struct ParameterInfo {
  [DataMember]
  public int Index { get; }

  [DataMember]
  public string Name { get; }

  [DataMember]
  public string DbType { get; }

  [DataMember]
  public string NetType { get; }

  [DataMember]
  public bool InParam { get; }

  public ParameterInfo() {}

  public ParameterInfo(int index, string name, string dbType, string netType) {
    Index = index;
    Name = name;
    DbType = dbType;
    NetType = netType;
  }

  public ParameterInfo(int index, string name, string dbType, string netType,
                       bool inParam) {
    Index = index;
    Name = name;
    DbType = dbType;
    NetType = netType;
    InParam = inParam;
  }

  public override string ToString() {
    return $"DbType: {DbType}, InParam: {InParam}, Index: {Index}, Name: {Name}, NetType: {NetType}";
  }
}
