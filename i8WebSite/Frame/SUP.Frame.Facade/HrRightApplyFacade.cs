using SUP.Frame.DataAccess;
using SUP.Frame.Rule;
using NG3;
using System.Data;

namespace SUP.Frame.Facade
{
    public class HrRightApplyFacade : IHrRightApplyFacade
    {
        private HrRightApplyDac dac = null;
        private HrRightApplyRule rule = null;

        public HrRightApplyFacade()
        {
            dac = new HrRightApplyDac();
            rule = new HrRightApplyRule();
        }

        [DBControl]
        public string GetUserHrid(string userid)
        {
            return dac.GetHridByUserid(userid);
        }

        [DBControl]
        public DataTable GetHrRightApply(string clientJsonQuery)
        {
            return rule.GetHrRightApply(clientJsonQuery);
        }

        [DBControl]
        public DataTable GetHrRightApplyDt(string phid)
        {
            return dac.GetHrRightApplyDt(phid);
        }

        [DBControl]
        public DataTable GetApplicantInfo(string hrids)
        {
            return rule.GetApplicantInfo(hrids);
        }

        [DBControl]
        public DataTable GetEditApplicantInfo(string billno)
        {
            return dac.GetApplicantDt(billno);
        }

        [DBControl]
        public DataTable GetOrgInfo(string userid, string billno)
        {
            return rule.GetOrgInfo(userid, billno);
        }

        [DBControl]
        public DataTable GetRoleInfo(string fillpsnid,string userid, string orgid, string billno)
        {
            return rule.GetRoleInfo(fillpsnid, userid, orgid, billno);
        }

        [DBControl]
        public bool SaveHrRightApply(string phid, string billno, string billname, string remark, string applicantObj, string orgObj, string roleObj, string otype, string removeApplicant)
        {
            return rule.SaveHrRightApply(phid, billno, billname, remark, applicantObj, orgObj, roleObj, otype, removeApplicant);
        }

        [DBControl]
        public bool DeleteHrRightApply(string billno)
        {
            return dac.DeleteHrRightApply(billno);
        }

        [DBControl]
        public DataTable GetHrRightApplyPrintInfo(string billno)
        {
            return rule.GetHrRightApplyPrintInfo(billno);
        }

    }

    public interface IHrRightApplyFacade
    {
        string GetUserHrid(string userid);

        DataTable GetHrRightApply(string clientJsonQuery);

        DataTable GetHrRightApplyDt(string phid);

        DataTable GetApplicantInfo(string hrids);

        DataTable GetEditApplicantInfo(string billno);

        DataTable GetOrgInfo(string userid, string billno);

        DataTable GetRoleInfo(string fillpsnid, string userid, string orgid, string billno);

        bool SaveHrRightApply(string phid, string billno, string billname, string remark, string applicantObj, string orgObj, string roleObj, string otype, string removeApplicant);

        bool DeleteHrRightApply(string billno);

        DataTable GetHrRightApplyPrintInfo(string billno);
    }
    
}
