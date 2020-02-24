using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Entity
{
    public class FuncTrackResult
    {
        //值
        public DataTable Value { set; get; }

        //自定义列的名称
        public NameValueCollection CustomColNames { set; get; }
        public string Status { set; get; }
        //失败的信息
        public FuncFault Fault { set; get; }
    }
    

}
