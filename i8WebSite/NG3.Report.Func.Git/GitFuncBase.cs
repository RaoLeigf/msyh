
using NG3.Report.Func.Core;
using NG3.Report.Func.Core.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Git
{
    public abstract class GitFuncBase : DefaultFunc
    {
        

        private bool CalcBaseFunc()
        {
            return true;
        }
        public override FuncCalcResult GetValue()        
        {
            bool b = CalcBaseFunc();
            if (!b) return null;


            return CalcFuncValue();
        }

        public abstract IDictionary<string,string> GetWhere(); 
        public abstract FuncCalcResult CalcFuncValue();
    }
}
