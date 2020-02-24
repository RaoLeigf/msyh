using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.DataAccess;
using NG3.Data.Service;

namespace SUP.Common.Rule
{
    public class PersonRule
    {
        private PersonDac personDac;
        public PersonRule()
        {
            personDac = new PersonDac();
        }

        /// <summary>
        /// 获取人员列表数据
        /// </summary>
        /// <param name="defaultFilter"></param>
        /// <param name="sqlFilter"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="oCode"></param>
        /// <param name="leaf"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string defaultFilter, string sqlFilter, int pageSize, int PageIndex, ref int totalRecord, string oCode, bool leaf, string getType)
        {
            DataTable tmpDT = new DataTable();
            string sqlString = "", tmpFilter = sqlFilter;
            sqlFilter = string.IsNullOrEmpty(sqlFilter) ? "" : ("where  " + sqlFilter);
            switch (getType)
            {
                case "actor":
                    if (oCode == "××％#$%(*)")
                    {
                        sqlString = "select secuser.logid cno,secuser.u_name cname,'2' ctype,bopomofo from secuser LEFT OUTER JOIN hr_epm_main ON secuser.hrno = hr_epm_main.cno" + (string.IsNullOrEmpty(defaultFilter) ? "" : (" where  " + defaultFilter));
                    }
                    else
                    {
                        sqlString = "select DISTINCT secuser.logid cno,secuser.u_name cname,'2' ctype,bopomofo from secuser " +
                        "LEFT OUTER JOIN fg_useractor on  fg_useractor.userid = secuser.logid " +
                        "LEFT OUTER JOIN hr_epm_main ON secuser.hrno = hr_epm_main.cno " +
                        "where fg_useractor.actorid = '" + oCode + "'" + (string.IsNullOrEmpty(defaultFilter) ? "" : (" and  " + defaultFilter));
                    }
                    break;
                case "ugroup":
                    if (oCode == "allugroup")
                    {
                        sqlString = "select secuser.logid cno,secuser.u_name cname,'2' ctype,bopomofo from secuser LEFT OUTER JOIN hr_epm_main ON secuser.hrno = hr_epm_main.cno" + (string.IsNullOrEmpty(defaultFilter) ? "" : (" where  " + defaultFilter));
                    }
                    else
                    {
                        sqlString = "select DISTINCT secuser.logid cno,secuser.u_name cname,'2' ctype,bopomofo from fg_groupuser " +
                        " LEFT OUTER JOIN secuser on fg_groupuser.userid = secuser.logid " +
                        " LEFT OUTER JOIN hr_epm_main ON secuser.hrno = hr_epm_main.cno " +
                        " where fg_groupuser.groupid = '" + oCode + "'" + (string.IsNullOrEmpty(defaultFilter) ? "" : (" and  " + defaultFilter));
                    }
                    break;
                case "selfgroup":
                    sqlString = GetSqlStringBySelfGroup(oCode);
                    break;
                case "online":
                    return GetOnLine(ref totalRecord, tmpFilter);
                case "outer":
                    sqlString = GetOuterString(oCode);
                    break;
                default:
                    return tmpDT;
            }
            tmpDT = personDac.GetUserList(string.Format("select * from ({0}) tmpTable {1}", sqlString, sqlFilter), pageSize, PageIndex, ref totalRecord);
            return tmpDT;
        }

        /// <summary>
        /// 获取自定义联系人查询串
        /// <param name="ccode"></param>
        /// </summary>
        /// <returns></returns>
        private string GetSqlStringBySelfGroup(string ccode)
        {
            string LogId = NG3.AppInfoBase.LoginID,
                   lj = personDac.IsOracle() ? "||" : "+";
            string swhere = "fg_msg_linkmangroup_dtl.ccode ='" + ccode + "'";
            string inSql = string.Format(@"select distinct cno from fg_msg_linkmangroup where fillemp ='{0}' or (cno in 
                        (select groupid from user_grouprgts where usertype='1' and userid='{0}' 
                        union select groupid from user_grouprgts where usertype='7' AND userid IN 
                        (SELECT actorid from fg_useractor where userid='{0}') 
                        union select groupid from user_grouprgts where usertype='0' AND EXISTS
                        (select logid from secuser where logid='{0}' and deptno IN (select ocode from fg_orgrelatitem 
                        where relatindex LIKE (select DISTINCT relatindex FROM fg_orgrelatitem where ocode=user_grouprgts.userid 
                        AND user_grouprgts.usertype='0' AND fg_orgrelatitem.relatid IN(SELECT fg_orgrelat.relatid from 
                        fg_orgrelat where  fg_orgrelat.attrcode='18')){1}'%'))))", LogId, lj);
            if (ccode == "allselfgroup")
            {
                swhere = string.Format("(fg_msg_linkmangroup.fillemp='{0}' or fg_msg_linkmangroup.cno in ({1}))", LogId, inSql);
            }
            string sqlString = string.Format(@"select distinct secuser.logid cno,secuser.u_name cname, '2' ctype,hr_epm_main.bopomofo
                        from fg_msg_linkmangroup_dtl
                        LEFT OUTER JOIN fg_msg_linkmangroup ON fg_msg_linkmangroup.ccode=fg_msg_linkmangroup_dtl.ccode
                        LEFT OUTER JOIN secuser ON linkmanid = secuser.logid
                        LEFT OUTER JOIN hr_epm_main ON  secuser.hrno = hr_epm_main.cno 
                        WHERE linkmantype IN ('1','5','7') AND {0} 
                        UNION 
                        select distinct hr_epm_main.cno,hr_epm_main.cname, '1' ctype,hr_epm_main.bopomofo
                        from fg_msg_linkmangroup_dtl
                        LEFT OUTER JOIN fg_msg_linkmangroup ON fg_msg_linkmangroup.ccode=fg_msg_linkmangroup_dtl.ccode
                        LEFT OUTER JOIN hr_epm_main ON  linkmanid = hr_epm_main.cno 
                        WHERE linkmantype IN ('2','4') AND {0}", swhere);
            return sqlString;
        }

        /// <summary>
        /// 在线用户
        /// </summary>
        /// <param name="totalRecord"></param>
        /// <param name="tmpFilter"></param>
        /// <returns></returns>
        private DataTable GetOnLine(ref int totalRecord, string tmpFilter)
        {
            DataTable tmpDT = UserOnlineInfor.Instance.GetDt();
            DataTable resultDT = new DataTable();
            resultDT.Columns.Add("cno");
            resultDT.Columns.Add("cname");
            resultDT.Columns.Add("ctype");
            resultDT.Columns.Add("bopomofo");
            DataRow ddr;
            foreach (DataRow dr in tmpDT.Rows)
            {
                ddr = resultDT.NewRow();
                ddr["cno"] = dr["UserID"];
                ddr["cname"] = dr["UserName"];
                ddr["ctype"] = "2";
                ddr["bopomofo"] = "";
                resultDT.Rows.Add(ddr);
            }
            resultDT.DefaultView.RowFilter = tmpFilter;
            totalRecord = resultDT.Rows.Count;
            return resultDT;
        }

        /// <summary>
        /// 1_员工，2_用户，3_联盟体，4_外部联系人，5_客户，6_UIC会员,7_分销商
        /// </summary>
        /// <param name="code"></param>
        /// <param name="tmpFilter"></param>
        /// <returns></returns>
        private string GetOuterString(string code)
        {
            string sqlString = "";
            switch (code)
            {
                #region 联盟体
                case "allubemp":
                    sqlString = "select cno, cname, '3' ctype, dbo.fun_getPY(cname) bopomofo from hr_ube_main";
                    break;
                #endregion
                #region 外部联系人
                case "alloutemp":
                    sqlString = "select ccode cno, cname, '4' ctype, dbo.fun_getPY(cname) bopomofo from fg_outcontact_info";
                    break;
                #endregion
                #region 客户
                case "allkehemp":
                    sqlString = "select c_code cno,c_name cname, '5' ctype, dbo.fun_getPY(c_name) bopomofo from uv_crm_custom where uv_crm_custom.c_code is not null and uv_crm_custom.c_name is not null";
                    break;
                #endregion
                #region UIC
                case "alluicemp":
                    sqlString = "select userid cno,username cname, '6' ctype, dbo.fun_getPY(username) bopomofo from wsp_user";
                    break;
                #endregion
                #region 分销商
                case "allfxsemp":
                    sqlString = "SELECT distinct enterprise.compno cno,enterprise.compname cname,'7' ctype,dbo.fun_getPY(enterprise.compname) bopomofo from fg_customfile,enterprise"
                                + " where enterprise.compno = fg_customfile.compno and fg_customfile.ocode ='" + NG3.AppInfoBase.OCode + "' and"
                                + " (fg_customfile.customattr='O'  or fg_customfile.customattr='UO') and"
                                + " (fg_customfile.accstop=0 or fg_customfile.accstop is null)  and enterprise.compno is not null and enterprise.compname is not null";
                    break;
                #endregion
            }
            return sqlString;
        }
    }
}
