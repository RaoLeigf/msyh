using System.Data;
using System.Text;
using NG3.Data;
using NG3.Data.Service;
using SUP.Common.DataEntity;
using System;

namespace SUP.Common.DataAccess
{
    /// <summary>
    /// 页面记忆功能 数据库访问层
    /// </summary>
    public class LayoutLogDac
    {
        #region Grid

        /// <summary>
        /// 根据登录编号获取记忆数据
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        public DataTable GetLayoutLogdt(string logid)
        {
            //string sqlString = " select * from fg_layoutlog where logid=" + DbConvert.ToSqlString(logid);
            string sqlString = " select gid,bustype,logid,pagesize from fg_layoutlog where logid=" + DbConvert.ToSqlString(logid);
            return DbHelper.GetDataTable(sqlString);
        }

        //clob字段单独获取
        public string GetLayoutValue(string logid)
        {
            //string sqlString = " select * from fg_layoutlog where logid=" + DbConvert.ToSqlString(logid);
            string sqlString = " select value from fg_layoutlog where logid=" + DbConvert.ToSqlString(logid);
            return DbHelper.GetString(sqlString);
        }


        public DataTable GetLayoutLogdt(string logid, string[] bustypes)
        {
            string inString = string.Join("','", bustypes);
            inString = "('" + inString + "')";
            string sqlString = string.Format(" select * from fg_layoutlog where logid={0} and bustype in {1}", DbConvert.ToSqlString(logid),inString);
            return DbHelper.GetDataTable(sqlString);
        }

        /// <summary>
        /// 根据logid和业务标识获取记忆数据
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public DataRow GetLayoutLogDr(string logid, string bustype)
        {
            //string sqlString = " select * from fg_layoutlog where logid=" + DbConvert.ToSqlString(logid) + " and bustype=" + DbConvert.ToSqlString(bustype);
            string sqlString = " select gid,bustype,logid,pagesize from fg_layoutlog where logid=" + DbConvert.ToSqlString(logid) + " and bustype=" + DbConvert.ToSqlString(bustype);
            DataTable logdt = DbHelper.GetDataTable(sqlString);
            return logdt.Rows.Count > 0 ? logdt.Rows[0] : null;
        }

        //clob字段单独获取
        public string GetLayoutValue(string logid, string bustype)
        {
            string sqlString = " select value from fg_layoutlog where logid=" + DbConvert.ToSqlString(logid) + " and bustype=" + DbConvert.ToSqlString(bustype);
            return DbHelper.GetString(sqlString);
        }

        /// <summary>
        /// 新增记忆数据
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        public int InsertLayoutlog(LayoutLogInfo layoutLogInfo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into fg_layoutlog(gid,bustype,logid,value) values ");
            sql.Append(" ({0},{1},{2},{3})");

            NGDataParameter[] paramList = new NGDataParameter[4];

            paramList[0] = new NGDataParameter("gid", layoutLogInfo.Gid);
            paramList[1] = new NGDataParameter("bustype", layoutLogInfo.Bustype);
            paramList[2] = new NGDataParameter("logid", layoutLogInfo.Logid);
            paramList[3] = new NGDataParameter("value", NGDbType.Text, layoutLogInfo.Value);

            return DbHelper.ExecuteNonQuery(sql.ToString(), paramList);
        }
        /// <summary>
        /// 更新记忆数据
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        public int UpdateLayoutlog(LayoutLogInfo layoutLogInfo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update fg_layoutlog set bustype={0},logid={1},value={2} ");
            sql.Append(" where gid={3}");

            NGDataParameter[] paramList = new NGDataParameter[4];

            paramList[0] = new NGDataParameter("bustype", layoutLogInfo.Bustype);
            paramList[1] = new NGDataParameter("logid", layoutLogInfo.Logid);
            paramList[2] = new NGDataParameter("value", NGDbType.Text,layoutLogInfo.Value);
            paramList[3] = new NGDataParameter("gid", layoutLogInfo.Gid);

            return DbHelper.ExecuteNonQuery(sql.ToString(), paramList);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public int DeleteLayoutLogInfo(string primaryKey)
        {
            //需要修改 parmaryKey 为自己的主键
            string Sql = string.Format("delete from fg_layoutlog where gid ={0}", DbConvert.ToSqlString(primaryKey));
            return DbHelper.ExecuteNonQuery(Sql);
        }

        public int savePagesize(string gid, string pagesize)
        {
            string sql = "update fg_layoutlog set pagesize ='{0}' where gid='{1}'";

            sql = string.Format(sql, pagesize, gid);
            return DbHelper.ExecuteNonQuery(sql);
        }
        #endregion

        #region ToolBar
        /// <summary>
        /// 保存ToolBar显示情况
        /// </summary>
        /// <param name="tbInfo"></param>
        public int SetToolBarData(ToolBarInfo tbInfo)
        {
            string sqlWhere = " where c_type='NG3_ToolBarInfo' and logid='" + tbInfo.LogID + "' and c_name='" + tbInfo.PageID + "'";
            if (string.IsNullOrEmpty(tbInfo.Value) || tbInfo.Value == "{}")
            {
                return DbHelper.ExecuteNonQuery("delete from c_sys_userparm" + sqlWhere);
            }
            string sqlstr = "select * from c_sys_userparm" + sqlWhere;
            DataTable tmpDT = DbHelper.GetDataTable(sqlstr);
            if (tmpDT.Rows.Count == 0)
            {
                sqlstr = "select c_code from c_sys_userparm order by c_code desc";
                DataTable dt = DbHelper.GetDataTable(sqlstr);
                int c_code = 1;
                if (dt.Rows.Count > 0)
                {
                    c_code = Convert.ToInt32(dt.Rows[0]["c_code"]) + 1;
                }
                DataRow dr = tmpDT.NewRow();
                dr["c_code"] = c_code.ToString("0000000000");
                dr["logid"] = tbInfo.LogID;
                dr["c_name"] = tbInfo.PageID;
                dr["c_value"] = tbInfo.Value;
                dr["c_type"] = "NG3_ToolBarInfo";
                tmpDT.Rows.Add(dr);
            }
            else
            {
                tmpDT.Rows[0]["c_value"] = tbInfo.Value;
            }
            return DbHelper.Update(tmpDT, "select * from c_sys_userparm");
        }

        /// <summary>
        /// 获取ToolBar显示情况
        /// </summary>
        ///<param name="LoginID"></param>
        ///<param name="PageId"></param>
        public ToolBarInfo GetToolBarData(string LoginID, string PageId)
        {
            ToolBarInfo tbInfo = new ToolBarInfo();
            tbInfo.LogID = LoginID;
            tbInfo.PageID = PageId;
            string sqlWhere = " where c_type='NG3_ToolBarInfo' and logid='" + LoginID + "' and c_name='" + PageId + "'";
            string sqlstr = "select c_value from c_sys_userparm" + sqlWhere;
            DataTable tmpDT = DbHelper.GetDataTable(sqlstr);
            if (tmpDT.Rows.Count == 0)
            {
                tbInfo.Value = "{}";
            }
            else
            {
                tbInfo.Value = tmpDT.Rows[0]["c_value"].ToString();
            }
            return tbInfo;
        }
        #endregion
    }
}
