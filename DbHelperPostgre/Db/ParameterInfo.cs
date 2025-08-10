using System.Runtime.Serialization;

namespace DbHelperPostgre.Db;


[DataContract]
public sealed record ParameterInfo
{
    [DataMember]
    public string DbType { get; set; }

    [DataMember]
    public bool InParam { get; set; }

    [DataMember]
    public int Index { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string NetType { get; set; }

    public override string ToString()
    {
        return $"DbType: {DbType}, InParam: {InParam}, Index: {Index}, Name: {Name}, NetType: {NetType}";
    }
}
