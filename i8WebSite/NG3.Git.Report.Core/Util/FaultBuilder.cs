using NG3.Report.Func.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Util
{
    public static class FaultBuilder
    {
        public static FuncFault Fault(string faultString,string detail )
        {
            return new  FuncFault
            {
                FaultCode = "-1",
                Faultstring = faultString,
                Detail = detail
            };
        }
    }
}
