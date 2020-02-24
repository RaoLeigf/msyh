using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using NG3;
using SUP.Frame.DataAccess;

namespace SUP.Frame.DataAccess
{
    public class IndividualNavigationDac
    {
        public DataTable LoadTree()
        {
            long userid = AppInfoBase.UserID;
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("select text from fg3_function_navigation where userid =" + userid + " and usertype = 0 ");
            DataTable text = DbHelper.GetDataTable(strbuilder.ToString());
            if (text == null || text.Rows.Count == 0)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select text from fg3_function_navigation where userid =" + userid + " and usertype = 1 ");
                text = DbHelper.GetDataTable(strbuilder.ToString());
            }
            return text;
        }
        public string Load(string text)
        {            
            long userid = AppInfoBase.UserID;
            StringBuilder strbuilder = new StringBuilder();
            if(text == "默认功能导航图")
            {
                strbuilder.Append("select svgconfig from fg3_function_navigation where userid =" + userid + " and usertype = 0 " + " and text is null");
            }
            else
            {
                strbuilder.Append("select svgconfig from fg3_function_navigation where userid =" + userid + " and usertype = 0 " + " and text = '" + text+"'");                
            }
            string svgConfig = DbHelper.GetString(strbuilder.ToString());
            if (svgConfig == null || svgConfig =="")
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                if (text == "默认功能导航图")
                {
                    strbuilder.Append("select svgconfig from fg3_function_navigation where userid =" + userid + " and usertype = 1 " + " and text is null");
                }
                else
                {
                    strbuilder.Append("select svgconfig from fg3_function_navigation where userid =" + userid + " and usertype = 1 " + " and text = '" + text+"'");
                }    
                svgConfig = DbHelper.GetString(strbuilder.ToString());
            }
            return svgConfig;
        }
        public string Save(string svgConfig, string text, string saveType, long phid)
        {
            long userid = AppInfoBase.UserID;
            //Int64 masterid = this.GetMaxID("fg3_function_navigation") + 1;
            string sql;
            if (saveType == "add")
            {
                sql = String.Format(@"select count(*) from fg3_function_navigation where userid ={0} and usertype = 0 and text ='{1}'", userid,text);
                string obj = DbHelper.GetString(sql).ToString();
                int i = 1;
                if (obj != "0")
                {
                    return "rename";
                }
                sql = String.Format(@"insert into fg3_function_navigation (phid,userid,svgconfig,usertype,text) values ({0},{1},'{2}',0,'{3}')", phid, userid, svgConfig, text);
            }
            else
            {
                if (text == "默认功能导航图")
                {
                    //sql = "update fg3_function_navigation set svgconfig = '" + svgConfig + "',text = '默认功能导航图' where userid =" + userid + " and usertype = 0 and  and text is null";
                    sql = String.Format(@"update fg3_function_navigation set svgconfig = '{0}',text = '默认功能导航图' where userid ={1} and usertype = 0 and  and text is null", svgConfig, userid);
                }
                else
                {
                    //sql = "update fg3_function_navigation set svgconfig = '" + svgConfig + "' where userid =" + userid + " and usertype = 0 and text =" + text;
                    sql = String.Format(@"update fg3_function_navigation set svgconfig = '{0}' where userid ={1} and usertype = 0 and text ='{2}'", svgConfig, userid, text);
                }                
            }

            //string sql = "select count(*) from fg3_function_navigation where userid =" + userid + " and usertype = 0 ";
            //string obj = DbHelper.GetString(sql).ToString();
            //if (obj == "0")
            //{
            //    Int64 masterid = this.GetMaxID("fg3_function_navigation") + 1;
            //    sql = "insert into fg3_function_navigation (phid,userid,svgconfig,usertype) values ( " + masterid + "," + userid + ",'" + svgConfig + "',0)";
            //}
            //else
            //{
            //    sql = "update fg3_function_navigation set svgconfig = '" + svgConfig + "' where userid =" + userid + " and usertype = 0 ";
            //}
            try
            {
                DbHelper.ExecuteScalar(sql);
                string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UserConfigDac.UserConfigSave(userid, 0, dateflag);
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(string text)
        {
            long userid = AppInfoBase.UserID;
            StringBuilder strbuilder = new StringBuilder();
            if (text == "默认功能导航图")
            {
                strbuilder.Append("delete from fg3_function_navigation where userid =" + userid + " and usertype = 0 and text is null");
                //sql = "delete from fg3_function_navigation where userid =" + userid + " and usertype = 0 and  and text is null";                        
            }
            else
            {
                strbuilder.Append("delete from fg3_function_navigation where userid =" + userid + " and usertype = 0 and text ='" + text+"'");
                //sql = "delete from fg3_function_navigation where userid =" + userid + " and usertype = 0 and text =" + text;
            }                       
            try
            {
                DbHelper.ExecuteScalar(strbuilder.ToString());            
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

        //方案分配
        public bool UserConfigCopy(long fromUserId, int fromUserType, long toUserId, int toUserType, List<long> phid)
        {
            try
            {
                string sqlText = "select * from fg3_function_navigation where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
                DataTable dt = DbHelper.GetDataTable(sqlText);

                sqlText = "select count(*) from fg3_function_navigation where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                string obj = DbHelper.GetString(sqlText);
                if (obj != "0")
                {
                    sqlText = "delete from fg3_function_navigation where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                    DbHelper.ExecuteNonQuery(sqlText);
                }
                DataTable newTable = dt.Clone();
                //Int64 masterid = this.GetMaxID("fg3_function_navigation");
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    newRow.BeginEdit();
                    newRow["phid"] = phid[i++];
                    newRow["userid"] = toUserId;
                    newRow["usertype"] = toUserType;
                    newRow["svgconfig"] = dr["svgconfig"];
                    newRow["text"] = dr["text"];
                    newRow.EndEdit();
                    newTable.Rows.Add(newRow);
                }
                DbHelper.Update(newTable, "select * from fg3_function_navigation");
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
                string sqlText = "delete from fg3_function_navigation where userid = '" + userid + "' and usertype = '" + usertype + "'";
                DbHelper.ExecuteNonQuery(sqlText);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
