using NG3.Data;
using NG3.Data.Service;
using SUP.Common.Base;
using System;
using System.Data;
using System.Text;

namespace SUP.Frame.DataAccess
{
    public class SysMaintainCallDac
    {

        public DataTable GetQuerySysMaintainCallDt(string clientJsonQuery)
        {
            string sql = "select * from fg3_sysmaintaincall";
            if (!string.IsNullOrEmpty(clientJsonQuery))
            {
                string query = string.Empty;
                clientJsonQuery = clientJsonQuery.Replace("\"state*str*eq*1\":\"1\"", "\"state*str*eq*1\":\"新增\"")
                    .Replace("\"state*str*eq*1\":\"2\"", "\"state*str*eq*1\":\"维护开始\"")
                    .Replace("\"state*str*eq*1\":\"3\"", "\"state*str*eq*1\":\"维护中\"")
                    .Replace("\"state*str*eq*1\":\"4\"", "\"state*str*eq*1\":\"维护完成\"");
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientJsonQuery, string.Empty, ref query);
                if (!string.IsNullOrEmpty(query))
                {
                    sql += " where " + query;
                }
                sql += " order by filldate desc";
                return DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString, sql, p);
            }
            else
            {
                sql += " order by filldate desc";
                return DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString, sql);
            }
        }

        public DataTable GetSysMaintainCallByPhid(string phid)
        {
            return DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString, "select * from fg3_sysmaintaincall where phid =" + phid);
        }

        public string CheckSysMaintainCallState()
        {
            return DbHelper.GetString(NG3.AppInfoBase.PubConnectString, "select count(*) from fg3_sysmaintaincall where state = '维护开始' or state = '维护中'");
        }

        public bool DelSysMaintainCall(string phid)
        {
            int iret = DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, "delete from fg3_sysmaintaincall where phid =" + phid);
            return iret == 1 ? true : false;
        }

        public string StartSysMaintainCall(string phid)
        {
            string count = CheckSysMaintainCallState();
            if (int.Parse(count) > 0) return "还有状态为维护开始/维护中的单据，不允许开始维护！";

            DataTable dt = DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString, "select starttime,preenddate from fg3_sysmaintaincall where phid =" + phid);
            DateTime startdate = DateTime.Now.AddMinutes(int.Parse(dt.Rows[0]["starttime"].ToString()));
            DateTime preenddate = Convert.ToDateTime(dt.Rows[0]["preenddate"].ToString());
            if (preenddate <= startdate) return "预计结束时间必须大于当前时间加上系统维护开始所选分钟！";
            int iret = DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, "update fg3_sysmaintaincall set state = '维护开始', startdate = '" + startdate + "' where phid =" + phid);
            return iret == 1 ? "Success" : "更新单据状态时出错！";
        }

        public bool CloseSysMaintainCall(string phid)
        {
            int iret = DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, "update fg3_sysmaintaincall set state = '维护完成', enddate = '" + DateTime.Now + "' where phid =" + phid);
            return iret == 1 ? true : false;
        }

        public DataTable GetUcodeList()
        {
            return DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString, "select ucode from ngusers");
        }

        public string GetUserConnectString(string ucode)
        {
            DBConnectionStringBuilder dBConnectionStringBuilder = new DBConnectionStringBuilder();

            string theResult = string.Empty;
            DataTable dtacc = DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString, "select ucode,uname,dbname,product from ngusers order by ucode ");

            if (dtacc != null && dtacc.Rows.Count > 0)
            {
                foreach (DataRow dr in dtacc.Rows)
                {
                    if (dr["ucode"].ToString() == ucode)
                    {
                        return dBConnectionStringBuilder.GetAccConnstringElement(0, dr["dbname"].ToString(), NG3.AppInfoBase.PubConnectString, out theResult);
                    }
                }
            }
            return string.Empty;
        }

        public DataTable GetUserList(string connectString, int pageSize, int pageIndex, string searchkey, ref int totalRecord)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select phid,hrid,userno,username from fg3_user");
            if (!string.IsNullOrEmpty(searchkey))
            {
                string[] keys = new string[] { };
                if (!string.IsNullOrWhiteSpace(searchkey))
                {
                    keys = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(searchkey);
                }

                sql.Append(" where ");
                for (int i = 0; i < keys.Length; i++)
                {
                    if (i != 0)
                    {
                        sql.Append(" or ");
                    }

                    sql.Append(" userno like '%" + keys[i] + "%' or username like '%" + keys[i] + "%'");
                }
            }

            string sortString = " phid asc";
            string strSql = PaginationAdapter.GetPageDataSql(sql.ToString(), pageSize, ref pageIndex, ref totalRecord, sortString, null, connectString);
            return DbHelper.GetDataTable(connectString, strSql);
        }

        public int InsertSysMaintainCall(string phid, string title, string starttime, string preenddate, string endtype, string enddate, string runinfo, string endinfo, string netfreecall, string netfreecallucode, string allowlogin)
        {
            IDataParameter[] parameterList;
            if (!string.IsNullOrEmpty(enddate))
            {
                parameterList = new IDataParameter[12];
            }
            else
            {
                parameterList = new IDataParameter[11];
            }
            parameterList[0] = new NGDataParameter("phid", DbType.Int64);
            parameterList[0].Value = long.Parse(phid);
            parameterList[1] = new NGDataParameter("title", DbType.AnsiString);
            parameterList[1].Value = title;
            parameterList[2] = new NGDataParameter("filldate", DbType.DateTime);
            parameterList[2].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            parameterList[3] = new NGDataParameter("starttime", DbType.Int16);
            parameterList[3].Value = int.Parse(starttime);
            parameterList[4] = new NGDataParameter("preenddate", DbType.DateTime);
            parameterList[4].Value = preenddate;
            parameterList[5] = new NGDataParameter("endtype", DbType.AnsiString);
            parameterList[5].Value = endtype;
            parameterList[6] = new NGDataParameter("runinfo", DbType.AnsiString);
            parameterList[6].Value = runinfo;
            parameterList[7] = new NGDataParameter("endinfo", DbType.AnsiString);
            parameterList[7].Value = endinfo;
            parameterList[8] = new NGDataParameter("netfreecall", DbType.AnsiString);
            parameterList[8].Value = netfreecall == "true" ? "1" : "0";
            parameterList[9] = new NGDataParameter("netfreecallucode", DbType.AnsiString);
            parameterList[9].Value = netfreecallucode;
            parameterList[10] = new NGDataParameter("allowlogin", DbType.AnsiString);
            parameterList[10].Value = allowlogin;
            if (!string.IsNullOrEmpty(enddate))
            {
                parameterList[11] = new NGDataParameter("enddate", DbType.DateTime);
                parameterList[11].Value = enddate;
                return DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, "insert into fg3_sysmaintaincall (phid, title, filldate, starttime, preenddate, endtype, runinfo, endinfo, netfreecall, netfreecallucode, allowlogin, enddate, state) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},'新增')", parameterList);
            }
            else
            {
                return DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, "insert into fg3_sysmaintaincall (phid, title, filldate, starttime, preenddate, endtype, runinfo, endinfo, netfreecall, netfreecallucode, allowlogin, state) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'新增')", parameterList);
            }
        }

        public int UpdateSysMaintainCall(string phid, string title, string starttime, string preenddate, string endtype, string enddate, string runinfo, string endinfo, string netfreecall, string netfreecallucode, string allowlogin)
        {
            netfreecall = netfreecall == "true" ? "1" : "0";
            return DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, "update fg3_sysmaintaincall set title = '" + title + "',starttime = " + starttime + ", preenddate = '" + preenddate + "', endtype = '" + endtype
                + "', enddate = '" + enddate + "', runinfo = '" + runinfo + "', endinfo = '" + endinfo + "', netfreecall = '" + netfreecall + "', netfreecallucode = '" + netfreecallucode + "', allowlogin = '" + allowlogin + "' where phid = " + phid);
        }

    }
}
