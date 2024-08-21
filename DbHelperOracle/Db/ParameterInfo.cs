namespace DbHelperOracle.Db;


internal struct ParameterInfo
{
    public string DbName { get; init; }

    public string DbType { get; init; }

    public bool InParam { get; init; }

    public int Index { get; init; }

    public string NetType { get; init; }

    public string Name { get; init; }

    public string NameLowerCamelCase { get; init; }

    public ParameterInfo(string dbName,
                         string dbType,
                         bool inParam,
                         int index,
                         string netType,
                         string name,
                         string nameLowerCamelCase)
    {
        DbName = dbName;
        DbType = dbType;
        InParam = inParam;
        Index = index;
        NetType = netType;
        Name = name;
        NameLowerCamelCase = nameLowerCamelCase;
    }

    public override string ToString()
    {
        return $"{nameof(DbName)}: {DbName}, {nameof(DbType)}: {DbType}, {nameof(InParam)}: {InParam}, {nameof(Index)}: {Index}, {nameof(NetType)}: {NetType}, {nameof(Name)}: {Name}, {nameof(NameLowerCamelCase)}: {NameLowerCamelCase}";
    }
}
