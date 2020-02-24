using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.DataEntity;

namespace SUP.Common.Facade
{
    public interface ILayoutLogFacade
    {
        #region Grid
        /// <summary>
        /// 根据登录编号获取记忆数据
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        List<LayoutLogInfo> GetLayoutLogList(string logid);

        /// <summary>
        /// 根据logid和业务标识获取记忆数据
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        LayoutLogInfo GetLayoutLogInfo(string logid, string bustype);

        /// <summary>
        /// 新增记忆数据
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        int InsertLayoutlog(LayoutLogInfo layoutLogInfo);

        /// <summary>
        /// 更新记忆数据
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        int UpdateLayoutlog(LayoutLogInfo layoutLogInfo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        int DeleteLayoutLogInfo(string primaryKey);

        /// <summary>
        /// 存储pagesize
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="pagesize"></param>
        int savePagesize(string gid, string pagesize);
        #endregion

        #region ToolBar
        /// <summary>
        /// 保存ToolBar显示情况
        /// </summary>
        /// <param name="tbInfo"></param>
        /// <returns></returns>
        int SetToolBarData(ToolBarInfo tbInfo);

        /// <summary>
        /// 获取ToolBar显示情况
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="PageId"></param>
        /// <returns></returns>
        ToolBarInfo GetToolBarData(string logid, string PageId);
        #endregion
    }
}
