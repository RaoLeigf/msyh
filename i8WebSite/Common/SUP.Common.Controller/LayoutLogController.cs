using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NG3.Web.Controller;
using NG3.Web.Mvc;
using NG3;
using NG3.Data.Builder;
using SUP.Common.Facade;
using SUP.Common.DataEntity;
using NG3.Aop.Transaction;
using SUP.Common.Base;

using System.IO;

namespace SUP.Common.Controller
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class LayoutLogController : AFController
    {

        private LayoutLogFacade facade = null;

        private ILayoutLogFacade proxy;

        public LayoutLogController()
        {
            facade = new LayoutLogFacade();
            proxy = AopObjectProxy.GetObject<ILayoutLogFacade>(facade);
        }

        #region  Grid 布局
        /// <summary>
        /// 获取指定业务布局记忆数据
        /// </summary>
        /// <returns></returns>        
        public JsonResult ReadLayout()
        {
            List<LayoutLogInfo> layoutLogInfo = new List<LayoutLogInfo>();

            if (NG3.AppInfoBase.IsUserLogined)
            {
                layoutLogInfo = proxy.GetLayoutLogList(NG3.AppInfoBase.LoginID);
            }

            return Json(layoutLogInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存布局
        /// </summary>
        /// <param name="bustype"></param>
        /// <param name="layoutValue"></param>
        /// <returns></returns>       
        public void SaveLayout(string bustype, string layoutValue)
        {
            if (NG3.AppInfoBase.IsUserLogined)
            {
                LayoutLogInfo layoutLogInfo = proxy.GetLayoutLogInfo(NG3.AppInfoBase.LoginID, bustype);

                if (layoutLogInfo != null)
                {
                    layoutLogInfo.Value = layoutValue;
                    proxy.UpdateLayoutlog(layoutLogInfo);
                }
                else
                {
                    layoutLogInfo = new LayoutLogInfo();
                    layoutLogInfo.Bustype = bustype;
                    layoutLogInfo.Logid = NG3.AppInfoBase.LoginID;
                    layoutLogInfo.Value = layoutValue;
                    proxy.InsertLayoutlog(layoutLogInfo);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>        
        public void ClearLayout(string bustype)
        {
            if (NG3.AppInfoBase.IsUserLogined)
            {
                LayoutLogInfo layoutLogInfo = proxy.GetLayoutLogInfo(NG3.AppInfoBase.LoginID, bustype);

                if (layoutLogInfo != null)
                {
                    proxy.DeleteLayoutLogInfo(layoutLogInfo.Gid);
                }
            }

        }


        //存pagesize
        public int savePagesize(string gid,string pagesize)
        {
            try
            {
                int m = proxy.savePagesize(gid, pagesize);
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveIndividualLayoutInfo(string bustype, string layoutValue)
        {
            try
            {
                LayoutLogInfo layoutLogInfo = proxy.GetLayoutLogInfo("*", bustype);

                if (layoutLogInfo != null)
                {
                    layoutLogInfo.Value = layoutValue;
                    proxy.UpdateLayoutlog(layoutLogInfo);
                }
                else
                {
                    layoutLogInfo = new LayoutLogInfo();
                    layoutLogInfo.Bustype = bustype;
                    layoutLogInfo.Logid = "*";//NG3.AppInfoBase.LoginID;
                    layoutLogInfo.Value = layoutValue;
                    proxy.InsertLayoutlog(layoutLogInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return "{status : \"ok\"}";
        }
        #endregion

        #region ToolBar 显示
        /// <summary>
        /// 保存ToolBar显示情况
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="HideBtns"></param>
        public void SetToolBarData(string PageId, string HideBtns)
        {
            ToolBarInfo tbInfo = new ToolBarInfo();
            tbInfo.PageID = PageId;
            tbInfo.LogID = NG3.AppInfoBase.LoginID;
            tbInfo.Value = HideBtns;
            proxy.SetToolBarData(tbInfo);
        }

        /// <summary>
        /// 获取ToolBar显示情况
        /// </summary>
        /// <param name="PageId"></param>
        public JsonResult GetToolBarData(string PageId)
        {
            ToolBarInfo tbInfo = proxy.GetToolBarData(NG3.AppInfoBase.LoginID, PageId);
            return Json(tbInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
