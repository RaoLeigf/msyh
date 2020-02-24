using NG3.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SUP.Frame.DataAccess
{
    public class SchemeAllocationDac
    {
        public DataTable GetUserSchemeAllocation()
        {
            long userid = NG3.AppInfoBase.UserID;
            StringBuilder strbuilder = new StringBuilder();
            DataTable dt = new DataTable();
            strbuilder.Append("SELECT c.phid,c.userid,c.userno,c.username,c.usertype,c.dateflag,d.roleno,d.rolename FROM ");
            strbuilder.Append(" (select a.phid,a.userid, b.userno,b.username,a.usertype,a.dateflag from fg3_userconfig a  ");
            strbuilder.Append(" left JOIN FG3_USER b ON a.USERID=b.PHID and a.USERTYPE=0 and b.STATUS IN(1,3)) c ");
            strbuilder.Append(" LEFT JOIN FG3_ROLE d ON c.userid=d.phid AND c.usertype=1 ");
            dt = DbHelper.GetDataTable(strbuilder.ToString());
            return dt;
        }

        public DataTable GetRoleList()
        {
            try
            {
                long userid = NG3.AppInfoBase.UserID;
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select phid, roleno,rolename from fg3_role");
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetUserList()
        {
            try
            {
                long userid = NG3.AppInfoBase.UserID;
                StringBuilder strbuilder = new StringBuilder();
                DataTable dt = new DataTable();
                strbuilder.Append("select phid,userno,username from fg3_user where status in(1,3)");
                dt = DbHelper.GetDataTable(strbuilder.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CopyIndividualSchema(string oriuserid, string oriusertype, string userid, string usertype)
        {
            try
            {
                object bytes = GetUserConfigMframeBfile(oriuserid, oriusertype);

                if (bytes == null) return false;

                string sqlText = "select count(*) from fg3_userconfig_mframe where userid = '" + userid + "' and usertype = '" + usertype + "'";
                string obj = NG3.Data.Service.DbHelper.GetString(sqlText).ToString();

                string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (obj != "0")
                {
                    sqlText = "update fg3_userconfig_mframe set bfile = {0},dateflag = {1} where userid = '" + userid + "' and usertype = '" + usertype + "'";
                    NG3.Data.NGDataParameter[] dataparams = new NG3.Data.NGDataParameter[2];
                    dataparams[0] = new NG3.Data.NGDataParameter("bfile", DbType.Binary);
                    dataparams[0].Value = bytes;
                    dataparams[1] = new NG3.Data.NGDataParameter("dateflag", DbType.DateTime);
                    dataparams[1].Value = dateflag;

                    NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlText, dataparams);
                }
                else
                {
                    sqlText = "insert into fg3_userconfig_mframe(phid,userid,usertype,bfile,dateflag,istemplate,remarks) values({0},{1},{2},{3},{4},{5},{6})";

                    long phid = GetMaxIDEx("fg3_userconfig_mframe");

                    NG3.Data.NGDataParameter[] dataparams = new NG3.Data.NGDataParameter[7];
                    dataparams[0] = new NG3.Data.NGDataParameter("phid", DbType.Int64);
                    dataparams[0].Value = ++phid;
                    dataparams[1] = new NG3.Data.NGDataParameter("userid", DbType.Int64);
                    dataparams[1].Value = userid;
                    dataparams[2] = new NG3.Data.NGDataParameter("usertype", DbType.Byte);
                    dataparams[2].Value = usertype;
                    dataparams[3] = new NG3.Data.NGDataParameter("bfile", DbType.Binary);
                    dataparams[3].Value = bytes;
                    dataparams[4] = new NG3.Data.NGDataParameter("dateflag", DbType.DateTime);
                    dataparams[4].Value = dateflag;
                    dataparams[5] = new NG3.Data.NGDataParameter("istemplate", DbType.String);
                    dataparams[5].Value = "0";
                    dataparams[6] = new NG3.Data.NGDataParameter("remarks", DbType.String);
                    dataparams[6].Value = "";

                    NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlText, dataparams);
                }

                sqlText = "select count(*) from fg3_userconfig where userid = '" + userid + "' and usertype = '" + usertype + "'";
                obj = NG3.Data.Service.DbHelper.GetString(sqlText).ToString();

                if (obj != "0")
                {
                    sqlText = "update fg3_userconfig set dateflag = '" + dateflag + "' where userid = '" + userid + "' and usertype = '" + usertype + "'";
                    NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlText);
                }
                else
                {
                    sqlText = "insert into fg3_userconfig(phid,userid,usertype,dateflag) values({0},{1},{2},{3})";

                    long phid = GetMaxIDEx("fg3_userconfig");

                    NG3.Data.NGDataParameter[] dataparams = new NG3.Data.NGDataParameter[4];
                    dataparams[0] = new NG3.Data.NGDataParameter("phid", DbType.Int64);
                    dataparams[0].Value = ++phid;
                    dataparams[1] = new NG3.Data.NGDataParameter("userid", DbType.Int64);
                    dataparams[1].Value = userid;
                    dataparams[2] = new NG3.Data.NGDataParameter("usertype", DbType.Byte);
                    dataparams[2].Value = usertype;
                    dataparams[3] = new NG3.Data.NGDataParameter("dateflag", DbType.DateTime);
                    dataparams[3].Value = dateflag;

                    NG3.Data.Service.DbHelper.ExecuteNonQuery(sqlText, dataparams);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public object GetUserConfigMframeBfile(string oriuserid, string oriusertype)
        {
            return DbHelper.ExecuteScalar("select bfile from fg3_userconfig_mframe where userid = '" + oriuserid + "' and usertype = '" + oriusertype + "'");
        }


        public DataTable DeleteIndividualSchema(string phid)
        {
            DataTable dt = GetUserConfigIdTypeByPhid(phid);
            NG3.Data.Service.DbHelper.ExecuteNonQuery("delete from fg3_userconfig where phid = '" + phid + "'");
            NG3.Data.Service.DbHelper.ExecuteNonQuery("delete from fg3_userconfig_mframe where userid = '" + dt.Rows[0]["userid"] + "' and usertype = '" + dt.Rows[0]["usertype"] + "'");
            return dt;
        }

        public DataTable GetUserConfigIdTypeByPhid(string phid)
        {
            return NG3.Data.Service.DbHelper.GetDataTable("select userid, usertype from fg3_userconfig where phid = '" + phid + "'");
        }

        public long GetMaxIDEx(string tableName)
        {
            string sql = "select max(phid) from " + tableName;
            object obj = DbHelper.ExecuteScalar(sql);

            long iret = 0;
            if (obj != null && obj != DBNull.Value)
            {
                long.TryParse(obj.ToString(), out iret);
            }
            return iret;
        }
    }
}
