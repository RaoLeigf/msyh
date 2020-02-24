using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade.Interface
{
    public interface IApplyCheckFacade
    {
        bool ConfirmApplyCheck(string _ucode, string _ocode, string _logid, string _ccode, string paramvalue, string msgdescription,  DateTime sortdate, string receiver, string sender, string targetcboo);

        string ConfirmApplyCheck(string _ucode, string _ocode, string _logid, string _ccode, string paramvalue, string msgdescription, DateTime sortdate, string receiver, string sender, string targetcboo,out string msg);

        bool DeleteApplyCheck(string _ucode, string _ocode, string _logid,string paramvalue, string receiver);

    }
}
