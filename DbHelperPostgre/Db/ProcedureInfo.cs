using System.Runtime.Serialization;

namespace DbHelperPostgre.Db;


[DataContract]
public record ProcedureInfo
{
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string DbType { get; set; }

    [DataMember]
    public string NetType { get; set; }

    [DataMember]
    public string SpecificName { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, DbType: {DbType}, NetType: {NetType}, SpecificName: {SpecificName}";
    }
}
