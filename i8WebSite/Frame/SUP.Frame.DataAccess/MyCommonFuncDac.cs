using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using SUP.Frame.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3;
using SUP.Common.Base;

namespace SUP.Frame.DataAccess
{
    public class MyCommonFuncDac
    {   
        /// <summary>
        /// 加载我的常用功能
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable LoadMyMenu()
        {
            string logid = NG3.AppInfoBase.LoginID;
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            //myEntity.GetFields = "id,alreadyitem,customtext,runtype,runadr,menutypecode,imgcode,'' purl,param,logid,menuid,lineid,suite,moduleno";
            //myEntity.WhereClause = string.Format("fg_myfunction.logid='{0}' and menutypecode='{1}'", i6.Biz.i6SessionInfo.LoginID, typeCode);
            //myEntity.OrderClause = "lineid asc";

            strbuilder.Append("select * from ");
            strbuilder.Append("(select fg_myfunction.id,fg_myfunction.alreadyitem,fg_myfunction.customtext,fg3_menu.url as runadr,fg_myfunction.menutypecode,fg_myfunction.menuid,fg_myfunction.lineid,fg_myfunction.param,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,my_menu_type.name");
            strbuilder.Append(" from fg_myfunction,fg3_menu,my_menu_type");
            strbuilder.Append(" where fg3_menu.id = fg_myfunction.menuid and my_menu_type.code = fg_myfunction.menutypecode and (fg_myfunction.suite <> '' or fg_myfunction.suite is not null) and fg_myfunction.logid = '" + logid + "'");
            strbuilder.Append(" union ");
            strbuilder.Append(" select  id,alreadyitem,customtext,runadr,menutypecode,menuid,fg_myfunction.lineid,param,null,null,null,null,null,my_menu_type.name");
            strbuilder.Append(" from fg_myfunction,my_menu_type");
            strbuilder.Append(" where (fg_myfunction.suite = '' or fg_myfunction.suite is null) and my_menu_type.code = fg_myfunction.menutypecode and fg_myfunction.logid = '" + logid + "')a order by lineid");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());
          
            return menudt;
        }

        public DataTable LoadMyMenuByType(string typecode)
        {
           //string logid = NG3.AppInfoBase.LoginID;
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();
            //long userid = NG3.AppInfoBase.UserID;
            //myEntity.GetFields = "id,alreadyitem,customtext,runtype,runadr,menutypecode,imgcode,'' purl,param,logid,menuid,lineid,suite,moduleno";
            //myEntity.WhereClause = string.Format("fg_myfunction.logid='{0}' and menutypecode='{1}'", i6.Biz.i6SessionInfo.LoginID, typeCode);
            //myEntity.OrderClause = "lineid asc";

            //不应该加logid，直接按类型code对应
            //strbuilder.Append("select * from ");
            //strbuilder.Append("(select fg3_menu.code,fg_myfunction.id,fg_myfunction.alreadyitem,fg_myfunction.customtext,fg3_menu.url as runadr,fg_myfunction.menutypecode,fg_myfunction.menuid,fg_myfunction.lineid,fg_myfunction.param,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,fg3_menu.functionname,my_menu_type.name");
            //strbuilder.Append(" from fg_myfunction,fg3_menu,my_menu_type");
            //strbuilder.Append(" where fg_myfunction.menutypecode = '"+ typecode + "' and fg3_menu.id = fg_myfunction.menuid and my_menu_type.code = fg_myfunction.menutypecode and (fg_myfunction.suite <> '' or fg_myfunction.suite is not null) and fg_myfunction.logid = '" + logid + "'");
            //strbuilder.Append(" union ");
            //strbuilder.Append(" select null,id,alreadyitem,customtext,runadr,menutypecode,menuid,fg_myfunction.lineid,param,null,null,null,null,null,null,my_menu_type.name");
            //strbuilder.Append(" from fg_myfunction,my_menu_type");
            //strbuilder.Append(" where fg_myfunction.menutypecode = '" + typecode + "' and (fg_myfunction.suite = '' or fg_myfunction.suite is null) and my_menu_type.code = fg_myfunction.menutypecode and fg_myfunction.logid = '" + logid + "')a order by lineid");

            strbuilder.Append("select * from ");
            strbuilder.Append("(select fg3_menu.code,fg_myfunction.id,fg_myfunction.alreadyitem,fg_myfunction.customtext,fg3_menu.url as runadr,fg_myfunction.menutypecode,fg_myfunction.menuid,fg_myfunction.lineid,fg_myfunction.param,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,fg3_menu.functionname,my_menu_type.name,fg3_menu.busphid");
            strbuilder.Append(" from fg_myfunction,fg3_menu,my_menu_type");
            strbuilder.Append(" where fg_myfunction.menutypecode = '" + typecode + "' and fg3_menu.id = fg_myfunction.menuid and my_menu_type.code = fg_myfunction.menutypecode and (fg_myfunction.suite <> '' or fg_myfunction.suite is not null)");
            strbuilder.Append(" union ");
            strbuilder.Append(" select null,id,alreadyitem,customtext,runadr,menutypecode,menuid,fg_myfunction.lineid,param,null,null,null,null,null,null,my_menu_type.name,null");
            strbuilder.Append(" from fg_myfunction,my_menu_type");
            strbuilder.Append(" where fg_myfunction.menutypecode = '" + typecode + "' and (fg_myfunction.suite = '' or fg_myfunction.suite is null) and my_menu_type.code = fg_myfunction.menutypecode)a order by lineid");

            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            //if (menudt.Rows.Count < 1)
            //{
            //    strbuilder.Remove(0, strbuilder.Length);
            //    userid = UserConfigDac.ActorGet(userid);
            //    strbuilder.Append("select * from ");
            //    strbuilder.Append("(select fg3_menu.code,fg_myfunction.id,fg_myfunction.alreadyitem,fg_myfunction.customtext,fg3_menu.url as runadr,fg_myfunction.menutypecode,fg_myfunction.menuid,fg_myfunction.lineid,fg_myfunction.param,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,fg3_menu.functionname,my_menu_type.name");
            //    strbuilder.Append(" from fg_myfunction,fg3_menu,my_menu_type");
            //    strbuilder.Append(" where fg_myfunction.menutypecode = '" + typecode + "'and fg_myfunction.usertype = 1 and fg3_menu.id = fg_myfunction.menuid and my_menu_type.code = fg_myfunction.menutypecode and (fg_myfunction.suite <> '' or fg_myfunction.suite is not null) and fg_myfunction.userid = '" + userid + "'");
            //    strbuilder.Append(" union ");
            //    strbuilder.Append(" select null,id,alreadyitem,customtext,runadr,menutypecode,menuid,fg_myfunction.lineid,param,null,null,null,null,null,null,my_menu_type.name");
            //    strbuilder.Append(" from fg_myfunction,my_menu_type");
            //    strbuilder.Append(" where fg_myfunction.menutypecode = '" + typecode + "'and fg_myfunction.usertype = 1 and (fg_myfunction.suite = '' or fg_myfunction.suite is null) and my_menu_type.code = fg_myfunction.menutypecode and fg_myfunction.userid = '" + userid + "')a order by lineid");
            //    menudt = DbHelper.GetDataTable(strbuilder.ToString());
            //}

            return menudt;
        }
        
        public int SaveMyMenu(DataTable masterdt)
        {
            string logid = NG3.AppInfoBase.LoginID;
            //long userid = NG3.AppInfoBase.UserID;
            string sql = string.Format("delete from fg_myfunction where logid='{0}'", logid);
            int iret = DbHelper.ExecuteNonQuery(sql);

            if (masterdt.Rows.Count == 0)//删除用户我的功能树所有节点
            {
                return 1;
            }
            int i = 0;
            //处理主表的主键
            foreach (DataRow dr in masterdt.Rows)
            {

                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    //Guid.NewGuid().ToString();//主表的主键
                    dr["id"] = Guid.NewGuid().ToString();
                    dr["logid"] = logid;
                    //dr["userid"] = userid;
                    dr["lineid"] = ++i;
                    //dr["usertype"] = 0;
                }
            }           
            int m = DbHelper.Update(masterdt, "select * from fg_myfunction");
            return m;
        }


        /// <summary>
        /// 我的功能分类
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable LoadMyMenuType()
        {
            long userid = NG3.AppInfoBase.UserID;
            string logid = NG3.AppInfoBase.LoginID;
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();
            if (String.Compare(AppInfoBase.UserType, UserType.System, true) == 0)
            {
                strbuilder.Append("select code,lineid,name from");
                strbuilder.Append(" my_menu_type");
                strbuilder.Append(" where logid = '" + logid + "'");
                menudt = DbHelper.GetDataTable(strbuilder.ToString());
                return menudt;
            }

            strbuilder.Append("select code,lineid,name from");
            strbuilder.Append(" my_menu_type");
            strbuilder.Append(" where userid = '" + userid + "' and usertype = 0 order by lineid");
            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            if (menudt.Rows.Count < 1)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select code,lineid,name from");
                strbuilder.Append(" my_menu_type");
                strbuilder.Append(" where userid = '" + userid + "' and usertype = 1 order by lineid");
                menudt = DbHelper.GetDataTable(strbuilder.ToString());
            }

            return menudt;
        }

        /// <summary>
        /// 保存功能分类
        /// </summary>
        /// <param name="rows"></param>
        public int SaveMyMenuType(string rows)
        {
            JArray ja = (JArray)JsonConvert.DeserializeObject(rows);
            long userid = NG3.AppInfoBase.UserID;
            string logid = NG3.AppInfoBase.LoginID;
            string sql = string.Format("delete from my_menu_type where (userid={0} and usertype = 0) or logid = '{1}'", userid,logid);
            int iret = DbHelper.ExecuteNonQuery(sql);

            if (ja.Count < 1)//删除用户我的功能树所有节点
            {
                return 1;
            }

            string sqlText = string.Format("select * from my_menu_type where (userid={0} and usertype = 0)", userid);
            DataTable dt = DbHelper.GetDataTable(sqlText);
            DataTable newTable = dt.Clone();
            for (int i = 0; i < ja.Count; i++)
            {
                DataRow newRow = newTable.NewRow();
                newRow.BeginEdit();
                newRow["code"] = string.IsNullOrEmpty(ja[i]["code"].ToString())?Guid.NewGuid().ToString(): ja[i]["code"].ToString();
                newRow["userid"] = userid;
                newRow["name"] = ja[i]["name"].ToString();
                newRow["usertype"] = 0;
                //newRow["lineid"] = ja[i]["lineid"].ToString();
                newRow["lineid"] = (i+1).ToString();
                newRow["logid"] = logid;
                newRow["def1"] = null;
                newRow.EndEdit();
                newTable.Rows.Add(newRow);
            } 

            string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            UserConfigDac.UserConfigSave(userid, 0, dateflag);
            return DbHelper.Update(newTable, "select * from my_menu_type");             
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

        public string GetLogid(long userid)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("select userno from fg3_user where phid = '" + userid + "'");
            return DbHelper.GetString(strbuilder.ToString());           
        }

        //方案分配
        public bool UserConfigCopy(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            try
            {
                //取出被复制的分类表，及各分类对应节点表
                string sqlText = "select * from my_menu_type where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
                DataTable dt = DbHelper.GetDataTable(sqlText);
                StringBuilder strbuilder = new StringBuilder();
                DataTable fromDt = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    strbuilder.Append("select * from fg_myfunction where menutypecode = '" + dt.Rows[0]["code"] + "'");
                    if (dt.Rows.Count > 1)
                    {
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            strbuilder.Append(" or menutypecode = '" + dt.Rows[i]["code"] + "'");
                        }
                    }
                    fromDt = DbHelper.GetDataTable(strbuilder.ToString());
                }
                

                //删除被授予用户的分类，及分类对应的节点
                sqlText = "select * from my_menu_type where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                DataTable delDt = DbHelper.GetDataTable(sqlText);
                                
                if (delDt.Rows.Count > 0)
                {
                    strbuilder.Remove(0, strbuilder.Length);
                    strbuilder.Append("delete from fg_myfunction where menutypecode = '" + delDt.Rows[0]["code"] + "'");
                    if(delDt.Rows.Count > 1)
                    {
                        for (int i = 1; i < delDt.Rows.Count; i++)
                        {
                            strbuilder.Append("or menutypecode = '" + delDt.Rows[i]["code"] + "'");
                        }
                    }
                    DbHelper.ExecuteNonQuery(strbuilder.ToString());
                    sqlText = "delete from my_menu_type where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                    DbHelper.ExecuteNonQuery(sqlText);
                }

                //循环被复制者的分类，给被授权对象新建数据
                string toLogid = null;
                if(toUserType == 0)
                {
                    sqlText = "select userno from fg3_user where phid = '" + toUserId + "'";
                    toLogid = DbHelper.GetString(sqlText);
                }

                DataTable newTable = dt.Clone();
                string code;

                DataTable toDt = fromDt.Clone();

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    newRow.BeginEdit();
                    code = Guid.NewGuid().ToString();
                    newRow["code"] = code;
                    newRow["userid"] = toUserId;
                    newRow["usertype"] = toUserType;
                    newRow["lineid"] = dr["lineid"];
                    newRow["name"] = dr["name"];
                    newRow["logid"] = toLogid;
                    newRow["def1"] = null;
                    newRow.EndEdit();
                    newTable.Rows.Add(newRow);
                    foreach (DataRow todr in fromDt.Rows)
                    {
                        if(todr["menutypecode"].ToString() == dr["code"].ToString())
                        {
                            DataRow toRow = toDt.NewRow();
                            toRow.BeginEdit();
                            toRow["id"] = Guid.NewGuid().ToString();
                            toRow["menutypecode"] = code;
                            toRow["logid"] = toLogid;
                            toRow["alreadyitem"] = todr["alreadyitem"];
                            toRow["customtext"] = todr["customtext"];
                            toRow["runtype"] = todr["runtype"];
                            toRow["runadr"] = todr["runadr"];
                            toRow["imgcode"] = todr["imgcode"];
                            toRow["param"] = todr["param"];
                            toRow["lineid"] = todr["lineid"];
                            toRow["menuid"] = todr["menuid"];
                            toRow["moduleno"] = todr["moduleno"];
                            toRow["suite"] = todr["suite"];
                            toRow["isused"] = todr["isused"];
                            toRow.EndEdit();
                            toDt.Rows.Add(toRow);
                        }
                    }
                }
                DbHelper.Update(newTable, "select * from my_menu_type");
                DbHelper.Update(toDt, "select * from fg_myfunction");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UserConfigDel(long toUserId, int toUserType)
        {
            try
            {
                string sqlText = "select * from my_menu_type where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                DataTable delDt = DbHelper.GetDataTable(sqlText);

                if (delDt.Rows.Count > 0)
                {
                    StringBuilder strbuilder = new StringBuilder();
                    strbuilder.Append("delete from fg_myfunction where menutypecode = '" + delDt.Rows[0]["code"] + "'");
                    if (delDt.Rows.Count > 1)
                    {
                        for (int i = 1; i < delDt.Rows.Count; i++)
                        {
                            strbuilder.Append("or menutypecode = '" + delDt.Rows[i]["code"] + "'");
                        }
                    }
                    DbHelper.ExecuteNonQuery(strbuilder.ToString());
                    sqlText = "delete from my_menu_type where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                    DbHelper.ExecuteNonQuery(sqlText);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
