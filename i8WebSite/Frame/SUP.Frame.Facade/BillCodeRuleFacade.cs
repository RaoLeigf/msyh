using SUP.Frame.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SUP.Frame.Rule;
using NG3;
using SUP.Frame.DataAccess;

namespace SUP.Frame.Facade
{
    public class BillCodeRuleFacade : IBillCodeRuleFacade
    {
        private BillCodeRuleDac dac = new BillCodeRuleDac();
        private BillCodeRuleRule rule = new BillCodeRuleRule();

        [DBControl]
        public DataTable GetBillCodeRuleList(string c_btype)
        {
            return dac.GetBillCodeRuleList(c_btype);
        }

        [DBControl]
        public DataTable GetBillCodeRuleDetailList(string c_bcode)
        {
            return dac.GetBillCodeRuleDetailList(c_bcode);
        }

        [DBControl]
        public DataTable GetInfo(string loginid, long orgid)
        {
            return dac.GetInfo(loginid, orgid);
        }

        [DBControl]
        public DataTable GetBillInfoHelp(string containerid)
        {
            return dac.GetBillInfoHelp(containerid);
        }

        [DBControl]
        public DataTable GetBillType(string busphid)
        {
            return dac.GetBillType(busphid);
        }

        [DBControl]
        public string GetTypeName(string c_code)
        {
            return dac.GetTypeName(c_code);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int Save(DataTable dt, string c_btype, DataTable deldt)
        {
            return dac.Save(dt, c_btype, deldt);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveDetails(DataTable dt, string loginid, long orgid)
        {
            return dac.SaveDetails(dt, loginid, orgid);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public object SaveRuleDistribution(DataTable dt, DataTable checkdt, string[] strArray, string billRule_m_code)
        {
            return dac.SaveRuleDistribution(dt, checkdt, strArray, billRule_m_code);
        }
    }
}
