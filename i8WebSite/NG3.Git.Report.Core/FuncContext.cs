using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core
{
    /// <summary>
    /// 跟报表相关的上下文数据,方便用于公式计算
    /// </summary>
    public class FuncContext
    {
        private string ocode;
        private string uyear;

        private DataTable orgdt;

        public string Ocode
        {
            get
            {
                return ocode;
            }

            set
            {
                ocode = value;
            }
        }

        public string Uyear
        {
            get
            {
                return uyear;
            }

            set
            {
                uyear = value;
            }
        }

        //public Int64 GetOrgId(string OrgCode)
        //{
            
        //}

    }
}
