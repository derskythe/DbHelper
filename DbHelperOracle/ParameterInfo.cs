namespace DbHelperOracle
{
    class ParameterInfo
    {
        public string DbName { get; set; }

        public string DbType { get; set; }

        public bool InParam { get; set; }

        public int Index { get; set; }

        public string NetType { get; set; }

        public string Name { get; set; }

        public string NameLowerCamelCase { get; set; }

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
}
