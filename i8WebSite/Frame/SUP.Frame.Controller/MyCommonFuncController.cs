using NG3.Aop.Transaction;
using NG3.Web.Controller;
using SUP.Common.Base;
using SUP.Frame.Facade;
using SUP.Frame.Facade.Interface;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web.SessionState;
using Newtonsoft.Json;
using i6.Biz;
using Enterprise.Common.UIP;
using NG3;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MyCommonFuncController : AFController
    {
        private IMyCommonFuncFacade proxy;
        private IMenuConfigFacade MenuConfigProxy;
        /// <summary>
        /// 自定义我的功能树页面
        /// </summary>
        /// <returns></returns>
        public MyCommonFuncController()
        {
            proxy = AopObjectProxy.GetObject<IMyCommonFuncFacade>(new MyCommonFuncFacade());
            MenuConfigProxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
        }

        public ActionResult FuncSelectMenu()
        {
            string logid = NG3.AppInfoBase.LoginID;
            string json = MenuConfigProxy.LoadEnFuncTreeRight();
            ViewBag.LoadEnFuncTreeRight = json;
            ViewBag.UserType = AppInfoBase.UserType;
            return View("FuncSelectMenu");
        }

        //public ActionResult FuncTypeManage()
        //{
        //    string logid = NG3.AppInfoBase.LoginID;
        //    DataTable dt = proxy.LoadMyMenuType();
        //    string json = DataConverterHelper.ToJson(dt, 0);
        //    ViewBag.MenuType = json;
        //    return View("FuncTypeManage");
        //}

        public string LoadMyMenu()
        {
            return JsonConvert.SerializeObject(proxy.LoadMyMenu());
        }
        public string LoadMyMenuByType()
        {
            string typecode = System.Web.HttpContext.Current.Request.Params["typecode"];
            return JsonConvert.SerializeObject(proxy.LoadMyMenuByType(typecode));
        }
        public int SaveMyMenu()
        {
            string gridData = System.Web.HttpContext.Current.Request.Params["gridData"];
            DataTable masterdt = DataConverterHelper.ToDataTable(gridData, "select * from fg_myfunction");
            int succCount = proxy.SaveMyMenu(masterdt);
            CommonUIP.RefurbishPorat(ReceiverType.Type_Ope, AppInfoBase.UserID.ToString(), PortalType.Portal_MyMenu);
            return succCount;
        }
        public string LoadMyMenuType()
        {
            return JsonConvert.SerializeObject(proxy.LoadMyMenuType());
        }
        public int SaveMyMenuType()
        {
            string rows = System.Web.HttpContext.Current.Request.Params["rows"];
            //JObject jo = JObject.Parse(rows);
            //Object obj = JsonConvert.DeserializeObject(rows);
            //JArray ja = (JArray)JsonConvert.DeserializeObject(rows);
            //DataTable masterdt = DataConverterHelper.ToDataTable(rows, "select * from my_menu_type");
            //DataTable masterdt = new DataTable();
            if (proxy.SaveMyMenuType(rows) > 0)
            {
                CommonUIP.RefurbishPorat(ReceiverType.Type_Ope, AppInfoBase.UserID.ToString(), PortalType.Portal_MyMenu);
                return 1;
            }
            else
            {
                CommonUIP.RefurbishPorat(ReceiverType.Type_Ope, AppInfoBase.UserID.ToString(), PortalType.Portal_MyMenu);
                return 0;
            }
        }
        
           
    }
}
