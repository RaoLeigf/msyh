using NG3.Aop.Transaction;
using NG3.Web.Controller;
using SUP.Common.Base;
using SUP.Frame.Facade;
using SUP.Frame.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ShortcutMenuController : AFController
    {
        private IShortcutMenuFacade proxy;
        private IMenuConfigFacade MenuConfigProxy;
        /// <summary>
        /// 快捷功能
        /// </summary>
        /// <returns></returns>
        public ShortcutMenuController()
        {
            proxy = AopObjectProxy.GetObject<IShortcutMenuFacade>(new ShortcutMenuFacade());
            MenuConfigProxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
        }

        public ActionResult ShortcutMenu()
        {
            string isweb = System.Web.HttpContext.Current.Request.Params["isweb"];
            string logid = NG3.AppInfoBase.LoginID;

            string json = MenuConfigProxy.LoadEnFuncTreeRight();
            ViewBag.LoadEnFuncTreeRight = json;
            ViewBag.UserType = NG3.AppInfoBase.UserType;
            ViewBag.isweb = isweb;//是否为web版请求
            return View("ShortcutMenu");
        }

        public string AddShortcutMenu()
        {
            string originalcode = System.Web.HttpContext.Current.Request.Params["originalcode"];
            string name = System.Web.HttpContext.Current.Request.Params["name"];
            string url = System.Web.HttpContext.Current.Request.Params["itemurl"];
            string busphid = System.Web.HttpContext.Current.Request.Params["busphid"];
            return proxy.AddShortcutMenu(originalcode, name, url,busphid);
        }

        /// <summary>
        /// 获取快捷功能grid列表
        /// </summary>
        public string GetShortcutMenuList()
        {
            string json = string.Empty;
            string phid = System.Web.HttpContext.Current.Request.Params["phid"];
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);
            int totalRecord = 0;

            DataTable dt = proxy.GetShortcutMenuList(Convert.ToInt64(phid), pageSize, pageIndex, ref totalRecord);
            json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }


        /// <summary>
        /// 获取快捷功能grid列表
        /// </summary>
        public string GetShortcutMenuForWeb()
        {
            string json = string.Empty;
            string userid = System.Web.HttpContext.Current.Request.Params["userid"];
            int totalRecord = 0;

            DataTable dt = proxy.GetShortcutMenuForWeb(Convert.ToInt64(userid));
            json = DataConverterHelper.SerializeObject(dt);
            return json;
        }

        public string GetShortcutKey()
        {
            DataTable dt= proxy.GetShortcutKey();
            int totalRecord = dt.Rows.Count;
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        public string SaveShortcutMenu()
        {
            string gridData = System.Web.HttpContext.Current.Request.Params["gridData"];
            DataTable dt = DataConverterHelper.ToDataTable(gridData, "select * from fg3_shortcutmenu");
            DataTable dtShortcut = proxy.SaveShortcutMenu(dt);
            int totalRecord = dtShortcut.Rows.Count;
            string json = DataConverterHelper.ToJson(dtShortcut, totalRecord);
            return json;
        }


    }
}
