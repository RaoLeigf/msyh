using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Interface;

namespace NG3.Report.Func.Git
{
    public class XXFPJE : GitFuncBase, IFuncResolve, IFuncTrack
    {
        public override FuncCalcResult CalcFuncValue()
        {
            FuncCalcResult result = new FuncCalcResult();
            result.Status = EnumFuncActionStatus.Success;
            result.Value = "48.50";
            return result;
        }

        public FuncResolveResult Resolve()
        {
            throw new NotImplementedException();
        }

        public FuncTrackResult Track()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string,string> GetWhere()
        {
            throw new NotImplementedException();
        }

        public override FuncInfo PreHandle()
        {
            return base.PreHandle();
        }
    }
}
