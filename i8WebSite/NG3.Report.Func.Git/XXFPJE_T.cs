using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Report.Func.Core.Entity;

namespace NG3.Report.Func.Git
{
    /// <summary>
    /// 销售发票金额含税
    /// </summary>
    public class XXFPJE_T :XXFPJE
    {
        public override FuncCalcResult CalcFuncValue()
        {
            FuncCalcResult result = new FuncCalcResult();
            result.Status = EnumFuncActionStatus.Success;
            result.Value = "40.50";
            return result;
        }

        
        public override FuncInfo PreHandle()
        {
            return base.PreHandle();
        }

        
    }
}
