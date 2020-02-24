using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Entity
{
    public class FuncFault
    {

        // 错误代码         
        private string faultcode = "";
        
         // 错误         
        private string faultstring = "错误";
        
         // 详细描述         
        private string detail = "";
        
         // 错误原因         
        private string reason = "";

        // 错误角色         
        private string faultactor = "";


        public string FaultCode
        {
            set { faultcode = value; }
            get { return faultcode; }
        }

        public string Faultstring
        {
            set { faultstring = value; }
            get { return faultstring; }
        }

        public string Detail
        {
            set { detail = value; }
            get { return detail; }
        }

        public string Reason
        {
            set { reason = value; }
            get { return reason; }
        }

        public string Faultactor
        {
            set { faultactor = value; }
            get { return faultactor; }
        }
    }
}
