using System;
using System.Collections.Generic;
using System.Data;
using SUP.Common.DataAccess;
using SUP.Common.DataEntity;

namespace SUP.Common.Rule
{
    public class LayoutLogRule
    {
        /// <summary>
        /// 
        /// </summary>
        private LayoutLogDac layoutLogDac;
        /// <summary>
        /// 构造函数
        /// </summary>
        public LayoutLogRule()
        {
            layoutLogDac = new LayoutLogDac();
        }
        #region Grid
        /// <summary>
        /// 根据登录编号获取记忆数据
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        public List<LayoutLogInfo> GetLayoutLogList(string logid)
        {
            DataTable dt = layoutLogDac.GetLayoutLogdt(logid);
            List<LayoutLogInfo> layoutLogList = new List<LayoutLogInfo>();
            LayoutLogInfo layoutLogInfo;

            foreach (DataRow dr in dt.Rows)
            {
                layoutLogInfo = new LayoutLogInfo();
                layoutLogInfo.Gid = dr["gid"].ToString();
                layoutLogInfo.Bustype = dr["bustype"].ToString();
                layoutLogInfo.Logid = dr["logid"].ToString();
                //layoutLogInfo.Value = dr["value"] == DBNull.Value ? "" : dr["value"].ToString();
                layoutLogInfo.Value = layoutLogDac.GetLayoutValue(logid, dr["bustype"].ToString());//oracle循环读取数据库
                layoutLogList.Add(layoutLogInfo);
            }

            return layoutLogList;
        }
        /// <summary>
        /// 根据logid和业务标识获取记忆数据
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public LayoutLogInfo GetLayoutLogInfo(string logid, string bustype)
        {
            DataRow dr = layoutLogDac.GetLayoutLogDr(logid, bustype);
            LayoutLogInfo layoutLogInfo;
            if (dr != null)
            {
                layoutLogInfo = new LayoutLogInfo();
                layoutLogInfo.Gid = dr["gid"].ToString();
                layoutLogInfo.Bustype = dr["bustype"].ToString();
                layoutLogInfo.Logid = dr["logid"].ToString();
                //layoutLogInfo.Value = dr["value"] == DBNull.Value ? "" : dr["value"].ToString();
                layoutLogInfo.Value = layoutLogDac.GetLayoutValue(logid, bustype);//oracle循环读取数据库
                layoutLogInfo.Pagesize = dr["pagesize"] == DBNull.Value ? "" : dr["pagesize"].ToString();
            }
            else
            {
                layoutLogInfo = null;
            }

            return layoutLogInfo;
        }

        public List<LayoutLogInfo> GetLayoutLogInfo(string logid, string [] bustypes)
        {
            DataTable dt = layoutLogDac.GetLayoutLogdt(logid,bustypes);
            List<LayoutLogInfo> layoutLogList = new List<LayoutLogInfo>();
            LayoutLogInfo layoutLogInfo;

            foreach (DataRow dr in dt.Rows)
            {
                layoutLogInfo = new LayoutLogInfo();
                layoutLogInfo.Gid = dr["gid"].ToString();
                layoutLogInfo.Bustype = dr["bustype"].ToString();
                layoutLogInfo.Logid = dr["logid"].ToString();
                layoutLogInfo.Value = dr["value"] == DBNull.Value ? "" : dr["value"].ToString();
                layoutLogInfo.Pagesize = dr["pagesize"] == DBNull.Value ? "" : dr["pagesize"].ToString();
                layoutLogList.Add(layoutLogInfo);
            }

            return layoutLogList;
        }

        /// <summary>
        /// 新增记忆数据
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        public int InsertLayoutlog(LayoutLogInfo layoutLogInfo)
        {
            layoutLogInfo.Gid = Guid.NewGuid().ToString();
            layoutLogInfo.Value = string.IsNullOrEmpty(layoutLogInfo.Value) ? "" : layoutLogInfo.Value;

            return layoutLogDac.InsertLayoutlog(layoutLogInfo);
        }
        /// <summary>
        /// 更新记忆数据
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        public int UpdateLayoutlog(LayoutLogInfo layoutLogInfo)
        {
            layoutLogInfo.Value = string.IsNullOrEmpty(layoutLogInfo.Value) ? "" : layoutLogInfo.Value;

            return layoutLogDac.UpdateLayoutlog(layoutLogInfo);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public int DeleteLayoutLogInfo(string primaryKey)
        {
            return layoutLogDac.DeleteLayoutLogInfo(primaryKey);
        }

        /// <summary>
        /// 更新pagesize
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public int savePagesize(string gid, string pagesize)
        {
            return layoutLogDac.savePagesize(gid,pagesize);
        }
        #endregion

        #region ToolBar
        /// <summary>
        /// 保存ToolBar显示情况
        /// </summary>
        /// <param name="tbInfo"></param>
        public int SetToolBarData(ToolBarInfo tbInfo)
        {
            return layoutLogDac.SetToolBarData(tbInfo);
        }

        /// <summary>
        /// 获取ToolBar显示情况
        /// </summary>
        ///<param name="LoginID"></param>
        ///<param name="PageId"></param>
        public ToolBarInfo GetToolBarData(string LoginID, string PageId)
        {
            return layoutLogDac.GetToolBarData(LoginID, PageId);
        }
        #endregion
    }
}
