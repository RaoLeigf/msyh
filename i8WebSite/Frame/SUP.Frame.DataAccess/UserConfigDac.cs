using System;
using NG3.Data;
using NG3.Data.Service;
using System.Data;

namespace SUP.Frame.DataAccess
{
    public static class UserConfigDac
    {

        /// <summary>
        /// 方案保存
        /// </summary>
        /// <param name="userid">用户phid</param>
        /// <param name="usertype">用户类型</param>
        /// <param name="dateflag">时间戳</param>
        /// <returns></returns>
        public static bool UserConfigSave(long userid, int usertype, string dateflag)
        {
            string sqlText = "select count(*) from fg3_userconfig where userid = '" + userid + "' and usertype = '" + usertype + "'";
            string obj = DbHelper.GetString(sqlText);
            if (obj != "0")
            {
                sqlText = "update fg3_userconfig set dateflag = '" + dateflag + "' where userid = '" + userid + "' and usertype = '" + usertype + "'";
                DbHelper.ExecuteNonQuery(sqlText);
                return true;
            }
            else
            {
                sqlText = "insert into fg3_userconfig(phid,userid,usertype,dateflag) values({0},{1},{2},{3})";

                long phid = GetBillId("fg3_userconfig", "phid");

                NGDataParameter[] dataparams = new NGDataParameter[4];
                dataparams[0] = new NGDataParameter("phid", DbType.Int64);
                dataparams[0].Value = phid;
                dataparams[1] = new NGDataParameter("userid", DbType.Int64);
                dataparams[1].Value = userid;
                dataparams[2] = new NGDataParameter("usertype", DbType.Byte);
                dataparams[2].Value = usertype;
                dataparams[3] = new NGDataParameter("dateflag", DbType.DateTime);
                dataparams[3].Value = dateflag;

                DbHelper.ExecuteNonQuery(sqlText, dataparams);
                return true;
            }
        }

        /// <summary>
        /// 获取用户对应角色
        /// </summary>
        /// <param name="userid">用户phid</param>
        public static long ActorGet(long userid)
        {
            object obj = DbHelper.ExecuteScalar("select desktop_actor from fg3_user where phid = '" + userid + "'");
            long iret = 0;
            if (obj == DBNull.Value || obj == null)
            {
                return 0;
            }
            else
            {
                long.TryParse(obj.ToString(), out iret);
                return iret;
            }           
        }

        private static long GetMaxID(string tableName)
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

        private static Int64 GetBillId(string tablename, string key)
        {
            try
            {
                var billNoCommon = new Enterprise3.WebApi.SDK.Common.BillNoHelper(); //new BillNoCommon();
                return billNoCommon.GetBillId(tablename, key);
            }
            catch (Exception ex)
            {
                throw new Exception("获取编码失败：" + ex.Message);
            }
        }

    }
}
