using SUP.Frame.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Rule
{
    public class ApplyCheckRule
    {

        private ApplyCheckDac dac = null;

        public ApplyCheckRule()
        {
            dac = new ApplyCheckDac();
        }

        public bool DeleteApplyCheck(string _ucode, string _ocode, string _logid, string paramvalue, string receiver)
        {
            try
            {
                _ucode = _ucode == "" ? NG3.AppInfoBase.UCode : _ucode;
                _ocode = _ocode == "" ? NG3.AppInfoBase.OCode : _ocode;
                _logid = _logid == "" ? NG3.AppInfoBase.LoginID : _logid;
               
                return dac.DeleteApplyCheck(_ucode, _ocode, _logid,paramvalue, receiver);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
