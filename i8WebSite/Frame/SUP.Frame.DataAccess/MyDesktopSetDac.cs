using NG3.Data;
using NG3.Data.Service;
using System;
using System.Data;

namespace SUP.Frame.DataAccess
{
    public class MyDesktopSetDac
    {

        public object GetMyDesktopIdByUserID(long userid)
        {
            object phid = DbHelper.ExecuteScalar("select phid from fg3_mydesktop where userid = '" + userid + "' and usertype = '0'");
            if (phid == null)
            {
                long desktopactor = UserConfigDac.ActorGet(userid);
                if (desktopactor == 0)
                {
                    return null;
                }
                else
                {
                    phid = DbHelper.ExecuteScalar("select phid from fg3_mydesktop where userid = '" + desktopactor + "' and usertype = '1'");
                }
            }
            return phid;
        }

        public object GetMyDesktopDataByPhid(long phid)
        {
            return DbHelper.ExecuteScalar("select data from fg3_mydesktop where phid = '" + phid + "'");
        }

        public object GetMyDesktopInfoData(long userid, int usertype)
        {
            string sqlText = "select data from fg3_mydesktop where userid = " + userid + " and usertype = " + usertype;
            return DbHelper.ExecuteScalar(sqlText);
        }

        public string GetMyDesktopInfoCount(long userid, int usertype)
        {
            string sqlText = "select count(*) from fg3_mydesktop where userid = " + userid + " and usertype = " + usertype;
            return DbHelper.GetString(sqlText);
        }

        public void UpdateMyDesktopInfo(byte[] buffer, long userid, int usertype)
        {
            string sqlText = "update fg3_mydesktop set data = {0} where userid = " + userid + " and usertype = " + usertype;
            NGDataParameter[] dataparams = new NGDataParameter[1];
            dataparams[0] = new NGDataParameter("data", DbType.Binary);
            dataparams[0].Value = buffer;

            DbHelper.ExecuteNonQuery(sqlText, dataparams);
        }

        public void InsertMyDesktopInfo(long phid, byte[] buffer, long userid, int usertype)
        {
            string sqlText = "insert into fg3_mydesktop(phid,data,userid,usertype) values({0},{1},{2},{3})";
            
            NGDataParameter[] dataparams = new NGDataParameter[4];
            dataparams[0] = new NGDataParameter("phid", DbType.Int64);
            dataparams[0].Value = phid;
            dataparams[1] = new NGDataParameter("data", DbType.Binary);
            dataparams[1].Value = buffer;
            dataparams[2] = new NGDataParameter("userid", DbType.String);
            dataparams[2].Value = userid;
            dataparams[3] = new NGDataParameter("usertype", DbType.Byte);
            dataparams[3].Value = usertype;

            DbHelper.ExecuteNonQuery(sqlText, dataparams);
        }
        
        public long GetMaxID(string tableName)
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

        public bool UserConfigDel(long userid, int usertype)
        {
            string sqlText = "delete from fg3_mydesktop where userid = '" + userid + "' and usertype = '" + usertype + "'";
            DbHelper.ExecuteNonQuery(sqlText);
            return true;
        }
        
    }
}
