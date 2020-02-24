using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Bill.Base
{
    public class ApplyCheck
    {
        private ApplyCheckRule rule = null;

        public ApplyCheck()
        {
            rule = new ApplyCheckRule();
        }

        public bool DeleteApplyCheck(string _ucode, string _ocode, string _logid, string paramvalue, string receiver)
        {
            return rule.DeleteApplyCheck(_ucode, _ocode, _logid, paramvalue, receiver);
        }
    }
}
