using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using SUP.Frame.DataEntity;
using NG3;
using NG.KeepConn;

namespace SUP.Frame.DataAccess
{
    public class MenuConfigDac
    {
        public string Load(long userid)
        {
            StringBuilder strbuilder = new StringBuilder();            

            strbuilder.Append("select treetabconfig");
            strbuilder.Append(" from fg3_mainframe_individual");
            strbuilder.Append(" where userid = " + userid + " and usertype = 0 ");
            string treetabconfig = DbHelper.GetString(strbuilder.ToString());
            if (treetabconfig==""|| treetabconfig == null)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select treetabconfig");
                strbuilder.Append(" from fg3_mainframe_individual");
                strbuilder.Append(" where userid = " + userid + " and usertype = 1 ");
                treetabconfig = DbHelper.GetString(strbuilder.ToString());
            }
                return treetabconfig;

        }
        /// <summary>
        /// 保存用户自定义添加的管理对象
        /// </summary>
        /// <param name="treetabconfig"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool Save(string treetabconfig, long userid, long phid)
        {
            string sql= "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            if (obj == "0")
            {
                //Int64 masterid = this.GetMaxID("fg3_mainframe_individual") + 1;
                sql = "insert into fg3_mainframe_individual (phid,userid,treetabconfig,usertype) values ( " + phid + "," + userid + ",'" + treetabconfig + "',0)";                
            }
            else
            {
                sql = "update fg3_mainframe_individual set treetabconfig = '" + treetabconfig + "' where userid =" + userid + " and usertype = 0 ";
            }
            try
            {
                DbHelper.ExecuteScalar(sql);
                string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UserConfigDac.UserConfigSave(userid, 0, dateflag);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }            
        }

        public Int64 GetMaxID(string tableName)
        {
            string sql = "select max(phid) from " + tableName;
            object obj = DbHelper.ExecuteScalar(sql);

            Int64 iret = 0;
            if (obj != null && obj != DBNull.Value)
            {
                Int64.TryParse(obj.ToString(), out iret);
            }
            return iret;
        }

        public string LoadEnFuncTreeRight()
        {
            string right_custom = DbHelper.GetString("select info_int from fg_info where sysno ='right_custom'");
            string show_custom = DbHelper.GetString("select info_int from fg_info where sysno ='show_custom'");
            string showsysfunc = DbHelper.GetString("select info_int from fg_info where sysno ='showsysfunc'");
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(right_custom);
            strbuilder.Append(show_custom);
            strbuilder.Append(showsysfunc);
            return strbuilder.ToString();
        }

        public bool SaveDockControl(int isDockControl, long userid, long phid)
        {
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            if (obj == "0")
            {
                //Int64 masterid = this.GetMaxID("fg3_mainframe_individual") + 1;
                sql = "insert into fg3_mainframe_individual (phid,userid,isdockcontrol,usertype) values ( " + phid + "," + userid + ",'" + isDockControl + "',0)";                
            }
            else
            {
                sql = "update fg3_mainframe_individual set isdockcontrol = '" + isDockControl + "' where userid =" + userid + " and usertype = 0 ";
            }
            try
            {
                DbHelper.ExecuteScalar(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public string GetDockControl(long userid)
        {
            StringBuilder strbuilder = new StringBuilder();

            strbuilder.Append("select isdockcontrol");
            strbuilder.Append(" from fg3_mainframe_individual");
            strbuilder.Append(" where userid = " + userid + "");

            string isdockcontrol = DbHelper.GetString(strbuilder.ToString());
            return isdockcontrol;

        }

        public bool SaveUITheme(int UITheme, long userid, long phid)
        {
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            if (obj == "0")
            {
                //Int64 masterid = this.GetMaxID("fg3_mainframe_individual") + 1;
                sql = "insert into fg3_mainframe_individual (phid,userid,uitheme,usertype) values ( " + phid + "," + userid + ",'" + UITheme + "',0)";
            }
            else
            {
                sql = "update fg3_mainframe_individual set uitheme = '" + UITheme + "' where userid =" + userid + "and usertype = 0 ";   
            }
            try
            {
                DbHelper.ExecuteScalar(sql);
                string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UserConfigDac.UserConfigSave(userid, 0, dateflag);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public string GetUITheme(long userid)
        {
            StringBuilder strbuilder = new StringBuilder();

            strbuilder.Append("select uitheme");
            strbuilder.Append(" from fg3_mainframe_individual");
            strbuilder.Append(" where userid = " + userid + " and usertype = 0 ");

            string uitheme = DbHelper.GetString(strbuilder.ToString());
            if (uitheme == "" || uitheme == null)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select uitheme");
                strbuilder.Append(" from fg3_mainframe_individual");
                strbuilder.Append(" where userid = " + userid +" and usertype = 1 ");

                uitheme = DbHelper.GetString(strbuilder.ToString());
            }
                return uitheme;

        }

        //public bool UserConfigCopy(long fromUserId, int fromUserType, long toUserId, int toUserType)
        //{
        //    try
        //    {
        //        string sqlText = "select * from fg3_mainframe_individual where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
        //        DataTable dt = DbHelper.GetDataTable(sqlText);
        //        sqlText = "select count(*) from fg3_mainframe_individual where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
        //        string obj = DbHelper.GetString(sqlText);
        //        if (obj != "0")
        //        {
        //            sqlText = "delete from fg3_mainframe_individual where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
        //            DbHelper.ExecuteNonQuery(sqlText);
        //        }
        //        DataTable newTable = dt.Copy();
        //        Int64 masterid = this.GetMaxID("fg3_mainframe_individual");
        //        foreach (DataRow dr in newTable.Rows)
        //        {
        //            dr["phid"] = ++masterid;
        //            dr["userid"] = toUserId;
        //            dr["usertype"] = toUserType;
        //            dr.SetAdded();
        //        }

        //        DbHelper.Update(newTable, "select * from fg3_mainframe_individual");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public bool UserConfigCopy_DefaultTab(long fromUserId, int fromUserType, long toUserId, int toUserType)
        //{
        //    try
        //    {
        //        string sqlText = "select * from fg3_defaultopen_tab where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
        //        DataTable dt = DbHelper.GetDataTable(sqlText);

        //        sqlText = "select count(*) from fg3_defaultopen_tab where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
        //        string obj = DbHelper.GetString(sqlText);
        //        if (obj != "0")
        //        {
        //            sqlText = "delete from fg3_defaultopen_tab where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
        //            DbHelper.ExecuteNonQuery(sqlText);
        //        }
        //        DataTable newTable = dt.Copy();
        //        Int64 masterid = this.GetMaxID("fg3_defaultopen_tab");
        //        foreach (DataRow dr in newTable.Rows)
        //        {
        //            dr["phid"] = ++masterid;
        //            dr["userid"] = toUserId;
        //            dr["usertype"] = toUserType;
        //        }
        //        DbHelper.Update(newTable, "select * from fg3_defaultopen_tab");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public bool UserConfigCopy(long fromUserId, int fromUserType, long toUserId, int toUserType,List<long> phid)
        {
            try
            {
                string sqlText = "select * from fg3_mainframe_individual where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
                DataTable dt = DbHelper.GetDataTable(sqlText);

                sqlText = "select count(*) from fg3_mainframe_individual where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                string obj = DbHelper.GetString(sqlText);
                if (obj != "0")
                {
                    sqlText = "delete from fg3_mainframe_individual where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                    DbHelper.ExecuteNonQuery(sqlText);
                }
                DataTable newTable = dt.Clone();
                //Int64 masterid = this.GetMaxID("fg3_mainframe_individual");
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    newRow.BeginEdit();                  
                    newRow["phid"] = phid[i++];
                    newRow["userid"] = toUserId;
                    newRow["usertype"] = toUserType;
                    newRow["treetabconfig"] = dr["treetabconfig"];
                    newRow["individualsetting"] = dr["individualsetting"];
                    newRow["isdockcontrol"] = dr["isdockcontrol"];
                    newRow["uitheme"] = dr["uitheme"];
                    newRow.EndEdit();
                    newTable.Rows.Add(newRow);
                }

                DbHelper.Update(newTable, "select * from fg3_mainframe_individual");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UserConfigDel(long userid, int usertype)
        {
            try
            {
                string sqlText = "delete from fg3_mainframe_individual where userid = '" + userid + "' and usertype = '" + usertype + "'";
                DbHelper.ExecuteNonQuery(sqlText);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //public bool UserConfigDel_DefaultTab(long userid, int usertype)
        //{
        //    try
        //    {
        //        string sqlText = "delete from fg3_defaultopen_tab where userid = '" + userid + "' and usertype = '" + usertype + "'";
        //        DbHelper.ExecuteNonQuery(sqlText);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public void SaveRequestRecord(RequestRecordEntity record)
        {
            record.UserId = AppInfoBase.UserID;
            record.Guid = Guid.NewGuid().ToString();
            record.SN = NGCOM.Instance.SN;
            record.Frequency = 1;
            //不过滤userid
            string connstr = NG3.AppInfoBase.PubConnectString;
            try
            {
                DbHelper.Open(connstr);
                DbHelper.BeginTran(connstr);
                string sql = String.Format(@"select frequency from request_record where url ='{1}' and sn = '{3}'", record.UserId, record.Url,record.Moduleno,record.SN);            
                string obj = DbHelper.GetString(connstr,sql);
                int frequency = 0;
                int.TryParse(obj, out frequency);
                if (frequency > 0)
                {
                    sql = String.Format(@"update request_record set frequency = {0},userid = {1} where url ='{2}' and sn = '{4}'", (frequency+1),record.UserId, record.Url, record.Moduleno, record.SN);               
                }
                else
                {
                    sql = String.Format(@"insert into request_record (frequency,userid,url,moduleno,sn,guid) values({0},{1},'{2}','{3}','{4}','{5}')", record.Frequency , record.UserId, record.Url, record.Moduleno, record.SN,record.Guid);
                    //sql = "insert into fg3_mainframe_individual (phid,userid,treetabconfig,usertype) values ( " + phid + "," + userid + ",'" + treetabconfig + "',0)";
                } 
                DbHelper.ExecuteScalar(connstr,sql);
                DbHelper.CommitTran(connstr);
            }
            catch (Exception ex)
            {
                DbHelper.RollbackTran(connstr);
                throw ex;
            }
            finally
            {
                DbHelper.Close(connstr);
            }
        }
    }
}
