﻿using NG3.Report.Func.Core.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Interface
{
    public interface IFuncCalc
    {

        //计算函数值
        FuncCalcResult GetValue();
    }
}
