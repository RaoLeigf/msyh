using NG3.Report.Func.Core.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Report.Func.Core.Entity;

namespace NG3.Report.Func.Core
{
    public abstract class DefaultFunc : IFunction, IFuncCalc
    {
        private FuncInfo func;
        public FuncInfo Func
        {
            get
            {
                return func;
            }

            set
            {
                func = value;
            }
        }

        public abstract FuncCalcResult GetValue();

        public virtual FuncInfo PreHandle()
        {
            //预解析主要是设置参数
            return func;
        }
        
    }

}
