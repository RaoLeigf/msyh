using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3;
using SUP.Common.Rule;

namespace SUP.Common.Facade
{
    public class LayoutLogFacade : ILayoutLogFacade
    {
        /// <summary>
        /// 
        /// </summary>
        private LayoutLogRule layoutLogRule;
        /// <summary>
        /// 
        /// </summary>
        public LayoutLogFacade()
        {
            layoutLogRule = new LayoutLogRule();
        }

        #region Grid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public List<DataEntity.LayoutLogInfo> GetLayoutLogList(string logid)
        {
            return layoutLogRule.GetLayoutLogList(logid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        [DBControl]
        public DataEntity.LayoutLogInfo GetLayoutLogInfo(string logid, string bustype)
        {
            return layoutLogRule.GetLayoutLogInfo(logid, bustype);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        [DBControl]
        public int InsertLayoutlog(DataEntity.LayoutLogInfo layoutLogInfo)
        {
            return layoutLogRule.InsertLayoutlog(layoutLogInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layoutLogInfo"></param>
        /// <returns></returns>
        [DBControl]
        public int UpdateLayoutlog(DataEntity.LayoutLogInfo layoutLogInfo)
        {
            return layoutLogRule.UpdateLayoutlog(layoutLogInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        [DBControl]
        public int DeleteLayoutLogInfo(string primaryKey)
        {
            return layoutLogRule.DeleteLayoutLogInfo(primaryKey);
        }

        [DBControl]
        public int savePagesize(string gid, string pagesize)
        {
            return layoutLogRule.savePagesize(gid, pagesize);
        }
        #endregion

        #region ToolBar
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbInfo"></param>
        /// <returns></returns>
        [DBControl]
        public int SetToolBarData(DataEntity.ToolBarInfo tbInfo)
        {
            return layoutLogRule.SetToolBarData(tbInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="PageId"></param>
        /// <returns></returns>
        [DBControl]
        public DataEntity.ToolBarInfo GetToolBarData(string logid, string PageId)
        {
            return layoutLogRule.GetToolBarData(logid, PageId);
        }
        #endregion
    }
}
