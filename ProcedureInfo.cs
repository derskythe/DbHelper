using System;
using System.Collections.Generic;
using System.Linq;

namespace DbHelper
{
    internal class ProcedureInfo
    {
        private int _Index;

        public int Count { get; set; }

        public string PackageName { get; set; }

        public string ProcedureName { get; set; }

        public List<List<ParameterInfo>> ParamList { get; }

        public void AddParam(ParameterInfo info)
        {
            if (ParamList.Count > 0 && ParamList[0].Any(item => item.Name == info.Name))
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

        public ProcedureInfo(int count, string packageName, string procedureName)
        {
            ParamList = new List<List<ParameterInfo>>();
            Count = count;
            PackageName = packageName;
            ProcedureName = procedureName;
        }
    }
}
