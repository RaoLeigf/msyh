using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade.Interface
{
    public interface IBillCodeRuleFacade
    {
        DataTable GetBillCodeRuleList(string c_btype);

        DataTable GetBillCodeRuleDetailList(string c_bcode);

        DataTable GetInfo(string loginid, long orgid);

        DataTable GetBillInfoHelp(string containerid);

        DataTable GetBillType(string busphid);

        string GetTypeName(string c_code);

        int Save(DataTable dt, string c_btype, DataTable deldt);

        int SaveDetails(DataTable dt, string loginid, long orgid);

        object SaveRuleDistribution(DataTable dt, DataTable checkdt, string[] strArray, string billRule_m_code);
    }
}
