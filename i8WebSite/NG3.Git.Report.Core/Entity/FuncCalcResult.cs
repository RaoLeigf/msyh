using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Entity
{
    public class FuncCalcResult
    {
        public string Value { set; get; }

        //返回值的数据类型
        public EnumFuncDataType ResultDataType { set; get; }
        public EnumFuncActionStatus Status { set; get; }
        //计算失败的信息
        public FuncFault Fault { set; get; }
    }
}
