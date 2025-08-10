using System.Collections.Generic;
using System.Linq;

namespace DbHelperOracle.Db;


internal struct ProcedureInfo
(
    int count,
    string packageName,
    string procedureName
)
{
    private int _Index;

    public int Count { get; set; } = count;

    public string PackageName { get; set; } = packageName;

    public string ProcedureName { get; set; } = procedureName;

    public List<List<ParameterInfo>> ParamList { get; } = new();

    public void AddParam(ParameterInfo info)
    {
        if (ParamList.Count > 0
            && ParamList[0].Any(item => item.Name == info.Name))
        {
            if (ParamList[0].Count <= 1)
            {
                Count++;
            }

            _Index++;

            if (ParamList.Count < _Index + 1)
            {
                ParamList.Add(new List<ParameterInfo>());
            }

            ParamList[_Index].Add(info);
        }
        else
        {
            _Index = 0;

            if (ParamList.Count <= 0)
            {
                Count++;
                ParamList.Add(new List<ParameterInfo>());
            }

            ParamList[_Index].Add(info);
        }
    }

    public override string ToString()
    {
        return string.IsNullOrEmpty(PackageName)
                   ? ProcedureName
                   : $"{ProcedureName}.{ProcedureName}";
    }
}
