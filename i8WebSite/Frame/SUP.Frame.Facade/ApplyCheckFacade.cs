using NG3;
using SUP.Frame.DataAccess;
using SUP.Frame.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade
{
    public class ApplyCheckFacade: IApplyCheckFacade
    {
        private ApplyCheckDac dac = new ApplyCheckDac();

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool ConfirmApplyCheck(string _ucode, string _ocode, string _logid, string _ccode, string paramvalue, string msgdescription, DateTime sortdate, string receiver, string sender, string targetcboo)
        {
            return dac.ConfirmApplyCheck(_ucode, _ocode, _logid, _ccode, paramvalue, msgdescription,  sortdate, receiver, sender, targetcboo);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public string ConfirmApplyCheck(string _ucode, string _ocode, string _logid, string _ccode, string paramvalue, string msgdescription, DateTime sortdate, string receiver, string sender, string targetcboo,out string msg)
        {
            return dac.ConfirmApplyCheck(_ucode, _ocode, _logid, _ccode, paramvalue, msgdescription, sortdate, receiver, sender, targetcboo,out msg);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool DeleteApplyCheck(string _ucode, string _ocode, string _logid, string paramvalue, string receiver)
        {
            return dac.DeleteApplyCheck(_ucode, _ocode, _logid, paramvalue, receiver);
        }
    }
}
