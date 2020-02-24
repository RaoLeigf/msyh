using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using SUP.Frame.DataAccess;

namespace SUP.Frame.DataAccess 
{
    
    
    
    public class MyFuncTreeDac
   {
    
        //    #region 日志相关
        //    private static ILogger _logger = null;
        //    internal static ILogger Logger
        //    {
        //        get
        //        {
        //            if (_logger == null)
        //            {
        //                _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(MyFuncTreeDac), LogType.logdac);
        //            }
        //            return _logger;
        //        }
        //    }
        //    #endregion
        //   使用：
        //_logger.Info(logInfo);

    /// <summary>
    /// 加载我的功能树
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    public DataTable LoadMenuData(long userid)
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();
            strbuilder.Append("select * from ");
            strbuilder.Append("(select fg3_myfunctree.phid,fg3_myfunctree.userid,fg3_myfunctree.id,fg3_myfunctree.pid,fg3_myfunctree.originalcode,fg3_myfunctree.seq,fg3_myfunctree.name,fg3_menu.url,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,fg3_menu.functionname,fg3_myfunctree.urlparm,fg3_menu.norightcontrol,fg3_menu.busphid");
            strbuilder.Append(" from fg3_myfunctree,fg3_menu");
            strbuilder.Append(" where fg3_menu.code = fg3_myfunctree.originalcode and (originalcode <> '' or originalcode is not null) and fg3_myfunctree.usertype = 0 and userid = " + userid + "");
            strbuilder.Append(" union ");
            strbuilder.Append(" select  phid,userid,id,pid,originalcode,seq,name,url,null,null,null,null,null,null,urlparm,null,null");
            strbuilder.Append(" from fg3_myfunctree");
            strbuilder.Append(" where (originalcode = '' or originalcode is null) and userid = " + userid + " and usertype = 0 ");
            strbuilder.Append(" union ");
            strbuilder.Append(" select fg3_myfunctree.phid,fg3_myfunctree.userid,fg3_myfunctree.id,fg3_myfunctree.pid,fg3_myfunctree.originalcode,fg3_myfunctree.seq,fg3_myfunctree.name,fg3_myfunctree.url,null,null,null,null,null,null,fg3_myfunctree.urlparm,null,null");
            strbuilder.Append(" from rw_report_main,fg3_myfunctree");
            strbuilder.Append(" where (rw_report_main.rep_code = fg3_myfunctree.originalcode and fg3_myfunctree.usertype = 0 and userid = " + userid + ") or (fg3_myfunctree.originalcode = 'ReportListRoot' and fg3_myfunctree.usertype = 0 and userid = " + userid + ")) t1");
            strbuilder.Append(" order by seq");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            if(menudt.Rows.Count <1)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select * from ");
                strbuilder.Append("(select fg3_myfunctree.phid,fg3_myfunctree.userid,fg3_myfunctree.id,fg3_myfunctree.pid,fg3_myfunctree.originalcode,fg3_myfunctree.seq,fg3_myfunctree.name,fg3_menu.url,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,fg3_menu.functionname,fg3_myfunctree.urlparm,fg3_menu.norightcontrol,fg3_menu.busphid");
                strbuilder.Append(" from fg3_myfunctree,fg3_menu");
                strbuilder.Append(" where fg3_menu.code = fg3_myfunctree.originalcode and (originalcode <> '' or originalcode is not null) and fg3_myfunctree.usertype = 1 and userid = " + userid);
                strbuilder.Append(" union ");
                strbuilder.Append(" select  phid,userid,id,pid,originalcode,seq,name,url,null,null,null,null,null,null,urlparm,null,null");
                strbuilder.Append(" from fg3_myfunctree");
                strbuilder.Append(" where (originalcode = '' or originalcode is null) and userid = " + userid + " and usertype = 1 ");
                strbuilder.Append(" union ");
                strbuilder.Append(" select fg3_myfunctree.phid,fg3_myfunctree.userid,fg3_myfunctree.id,fg3_myfunctree.pid,fg3_myfunctree.originalcode,fg3_myfunctree.seq,fg3_myfunctree.name,fg3_myfunctree.url,null,null,null,null,null,null,fg3_myfunctree.urlparm,null,null");
                strbuilder.Append(" from rw_report_main,fg3_myfunctree");
                strbuilder.Append(" where (rw_report_main.rep_code = fg3_myfunctree.originalcode and fg3_myfunctree.usertype = 1 and userid = " + userid + ") or (fg3_myfunctree.originalcode = 'ReportListRoot' and fg3_myfunctree.usertype = 1 and userid = " + userid + ")) t1");
                strbuilder.Append(" order by seq");

                menudt = DbHelper.GetDataTable(strbuilder.ToString());
            }
            return menudt;

        }

        public int Save(DataTable masterdt, long userid, List<long> phid)
        {     
            string sql = string.Format("delete from fg3_myfunctree where userid={0} and usertype = 0", userid);
            //string sql = "delete from fg3_myfunctree where userid=" + userid + "";
            int iret = DbHelper.ExecuteNonQuery(sql);

            if (masterdt.Rows.Count == 0)//删除用户我的功能树所有节点
            {
                return 1;
            }
            //Int64 masterid = this.GetMaxID("fg3_myfunctree"); 
            int i = 0;
            //处理主表的主键
            foreach (DataRow dr in masterdt.Rows)
            {
               
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    //Guid.NewGuid().ToString();//主表的主键
                    //dr["phid"] = ++masterid;
                    dr["phid"] = phid[i++];
                    dr["userid"] = userid;
                    dr["usertype"] = 0;
                }

            }
            string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            UserConfigDac.UserConfigSave(userid, 0,dateflag);
            int m = DbHelper.Update(masterdt, "select * from fg3_myfunctree");
            return m;
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
        public bool UserConfigCopy(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            try
            {
                string sqlText = "select * from fg3_myfunctree where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
                DataTable dt = DbHelper.GetDataTable(sqlText);                

                sqlText = "select count(*) from fg3_myfunctree where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                string obj = DbHelper.GetString(sqlText);
                if (obj != "0")
                {
                    sqlText = "delete from fg3_myfunctree where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";                 
                    DbHelper.ExecuteNonQuery(sqlText);
                }
                DataTable newTable = dt.Clone();
                Int64 masterid = this.GetMaxID("fg3_myfunctree");
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    newRow.BeginEdit();
                    newRow["phid"] = ++masterid;
                    newRow["userid"] = toUserId;
                    newRow["usertype"] = toUserType;
                    newRow["id"] = dr["id"];
                    newRow["pid"] = dr["pid"];
                    newRow["originalcode"] = dr["originalcode"];
                    newRow["seq"] = dr["seq"];
                    newRow["name"] = dr["name"];
                    newRow["url"] = dr["url"];
                    newRow["urlparm"] = dr["urlparm"];
                    newRow.EndEdit();
                    newTable.Rows.Add(newRow);
                }
                DbHelper.Update(newTable, "select * from fg3_myfunctree");
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
                string sqlText = "delete from fg3_myfunctree where userid = '" + userid + "' and usertype = '" + usertype + "'";
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
