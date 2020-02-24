using System;
using System.Data;
using System.Text;
using NG3.Data.Service;
using SUP.Common.Base;

namespace SUP.Frame.DataAccess
{
    public class HrRightApplyDac
    {

        public string GetHridByUserid(string userid)
        {
            return DbHelper.GetString("select hrid from fg3_user where phid=" + userid);
        }

        public DataTable GetUnderHrUserid()
        {
            return DbHelper.GetDataTable("select phid from fg3_user where hrid in (select empno from fg_underling_dtl where phid_underling in " 
                + "(select phid from fg_underling where hrno in(select hrid from fg3_user where phid = " + NG3.AppInfoBase.UserID +" )))");
        }

        public DataTable GetQueryHrRightApplyDt(string clientJsonQuery)
        {
            string sql = "select * from fg3_hrrightapply";
            if (!string.IsNullOrEmpty(clientJsonQuery))
            {
                string query = string.Empty;
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientJsonQuery, string.Empty, ref query);
                if (!string.IsNullOrEmpty(query))
                {
                    sql += " where " + query;
                }
                sql += " order by filldate desc";
                return DbHelper.GetDataTable(sql, p);
            }
            else
            {
                sql += " order by filldate desc";
                return DbHelper.GetDataTable(sql);
            }
        }

        public DataTable GetHrRightApplyDt(string phid)
        {
            return DbHelper.GetDataTable("select * from fg3_hrrightapply where phid = " + phid);
        }

        public string GetHrRightApplyBillNo(string phid)
        {
            return DbHelper.GetString("select billno from fg3_hrrightapply where phid = " + phid);
        }

        public string GetUserNameById(string userid)
        {
            return DbHelper.GetString("select username from fg3_user where phid = " + userid);
        }

        public DataTable GetHrNameDeptDt(string hrid)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("select hr_epm_main.cname, fg_orglist.oname from hr_epm_main, fg_orglist ");
            strbuilder.Append("where fg_orglist.phid = hr_epm_main.dept and hr_epm_main.phid = " + hrid);
            return DbHelper.GetDataTable(strbuilder.ToString());
        }

        public string GetHrStation(string hrid)
        {
            return DbHelper.GetString("select fg_ogm_station.cname from hr_epm_station,fg_ogm_station where hr_epm_station.station = fg_ogm_station.phid " 
                + "and hr_epm_station.assigntype='0' and hr_epm_station.must_phid =" + hrid);
        }

        public DataTable GetApplicantDt(string billno)
        {
            return DbHelper.GetDataTable("select * from fg3_hrrightapplicant where billno = '" + billno + "'");
        }

        public string GetApplicantDeptId(long hrid)
        {
            return DbHelper.GetString("select dept from hr_epm_main where phid =" + hrid);
        }

        public string GetUserIdByHrid(string hrid)
        {
            return DbHelper.GetString("select phid from fg3_user where hrid=" + hrid);
        }

        public DataTable GetOrgIdName(long userid)
        {
            return DbHelper.GetDataTable("select distinct fg3_userorg.orgid, fg_orglist.oname, relatindex from fg3_userorg, fg_orglist, fg_orgrelatitem"
                + " where fg3_userorg.orgid = fg_orglist.phid and userid = " + userid + " and org_id = fg3_userorg.orgid and relatid = 'lg' order by relatindex");
        }

        public string GetUserOrgCount(string userid, string orgid)
        {
            return DbHelper.GetString("select count(*) from fg3_userorg where orgid=" + orgid + " and userid =" + userid);
        }

        public DataTable GetOrgDt(string userid, string billno)
        {
            return DbHelper.GetDataTable("select * from fg3_hrrightorg where billno = '" + billno + "' and userid = " + userid);
        }

        public DataTable GetSelectOrgDt(string billno)
        {
            return DbHelper.GetDataTable("select * from fg3_hrrightorg where billno = '" + billno + "' and isselect = 1");
        }

        public DataTable GetUserRoleOrg(string userid, string orgid)
        {
            return DbHelper.GetDataTable("select fg3_role.phid,fg3_role.roleno,fg3_role.rolename from fg3_userroleorg, fg3_role where "
                 + "fg3_role.phid = fg3_userroleorg.roleid and fg3_userroleorg.userid =" + userid + " and fg3_userroleorg.orgid =" + orgid);
        }

        public bool CheckUserRoleOrg(string userid, string roleid, string orgid)
        {
            string result = DbHelper.GetString("select count(*) from fg3_userroleorg where userid = " + userid + " and roleid = " + roleid + " and orgid = " + orgid);
            if (result == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckUserRoleOrg(string userid, string roleid, string orgid, string billno)
        {
            string result = DbHelper.GetString("select count(*) from fg3_hrrightrole where userid = " + userid + " and roleid = " + roleid + " and orgid = " + orgid + " and billno = '" + billno + "'");
            if (result == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InsertHrRightApplyDt(string phid, string billno, string billname, string remark)
        {
            DbHelper.ExecuteNonQuery("insert into fg3_hrrightapply(phid,billno,billname,fillpsnid,fillpsnname,filldate,checkstate,ischeck,remark) values ("
                + phid + ",'" + billno + "','" + billname + "'," + NG3.AppInfoBase.UserID + ",'" + NG3.AppInfoBase.UserName + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "',0,0,'" + remark + "')");
        }

        public void UpdateHrRightApplyDt(string billno, string billname, string remark)
        {
            DbHelper.ExecuteNonQuery("update fg3_hrrightapply set billname = '" + billname + "', remark = '" + remark + "' where billno = '" + billno + "'");
        }

        public void DeleteHrRightApplyDts(string billno)
        {
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightapplicant where billno = '" + billno + "'");
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightorg where billno = '" + billno + "'");
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightrole where billno = '" + billno + "'");
        }

        public void InsertApplicantDt(long phid, string billno, string hrid, string userid, string hrname, string dept, string station, string remark)
        {
            DbHelper.ExecuteNonQuery("insert into fg3_hrrightapplicant(phid,billno,hrid,userid,hrname,dept,station,remark) values ("
                + phid + ",'" + billno + "'," + hrid + "," + userid + ",'" + hrname + "','" + dept + "','" + station + "','" + remark + "')");
        }

        public void DeleteApplicantDt(string billno)
        {
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightapplicant where billno = '" + billno + "'");
        }

        public void InsertOrgDt(long phid, string billno, string userid, string orgid, string orgname, int isselect, int fillpsnorg, int applicantorg)
        {
            DbHelper.ExecuteNonQuery("insert into fg3_hrrightorg(phid,billno,userid,orgid,orgname,isselect,fillpsnorg,applicantorg) values ("
                + phid + ",'" + billno + "'," + userid + "," + orgid + ",'" + orgname + "'," + isselect + "," + fillpsnorg + "," + applicantorg + ")");
        }

        public void DeleteOrgDt(string billno, string userid)
        {
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightorg where billno = '" + billno + "' and userid = " + userid);
        }

        public void InsertRoleDt(long phid, string billno, string userid, string orgid, string roleid, string rolename)
        {
            DbHelper.ExecuteNonQuery("insert into fg3_hrrightrole(phid,billno,userid,orgid,roleid,rolename) values ("
                + phid + ",'" + billno + "'," + userid + "," + orgid + "," + roleid + ",'" + rolename + "')");
        }

        public void DeleteRoleDt(string billno, string userid, string orgid)
        {
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightrole where billno = '" + billno + "' and userid = " + userid + " and orgid = " + orgid);
        }

        public void UpdateWorkFlowCheckState(string phid, string checkstate)
        {
            DbHelper.ExecuteNonQuery("update fg3_hrrightapply set checkstate = " + checkstate + " where phid = " + phid);
        }

        public void UpdateWorkFlowCheckPsnDate(string phid)
        {
            DbHelper.ExecuteNonQuery("update fg3_hrrightapply set ischeck = 1, checkpsn = '"
                + NG3.AppInfoBase.UserName + "', checkdate = '" + DateTime.Now.ToString("yyyy-MM-dd") + " ' where phid = " + phid);
        }

        public bool DeleteHrRightApply(string billno)
        {
            try
            {
                DbHelper.BeginTran();
                DbHelper.ExecuteNonQuery("delete from fg3_hrrightapply where billno = '" + billno + "'");
                DeleteHrRightApplyDts(billno);
                DbHelper.CommitTran();
                return true;
            }
            catch
            {
                DbHelper.RollbackTran();
                return false;
            }
        }

        public DataTable GetRoleDtByBillNo(string billno)
        {
            return DbHelper.GetDataTable("select * from fg3_hrrightrole where billno = '" + billno + "' order by userid, orgid");
        }

        public DataTable GetRoleDtByBillNoNUserId(string billno, string userid)
        {
            return DbHelper.GetDataTable("select * from fg3_hrrightrole where billno = '" + billno + "' and userid = " + userid);
        }

        public void DeleteRemovedOrgRoleDt(string billno, string userid)
        {
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightorg where billno = '" + billno + "' and userid = " + userid);
            DbHelper.ExecuteNonQuery("delete from fg3_hrrightrole where billno = '" + billno + "' and userid = " + userid);
        }
        
    }
}
