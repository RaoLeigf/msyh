using Newtonsoft.Json;
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
    public class AppAutoAuthorizeController: AFController
    {
        private IAppAutoAuthorizeFacade proxy;
        /// <summary>
        /// app自动分配页面
        /// </summary>
        /// <returns></returns>
        public AppAutoAuthorizeController()
        {
            proxy = AopObjectProxy.GetObject<IAppAutoAuthorizeFacade>(new AppAutoAuthorizeFacade());
        }

        public ActionResult AppAutoAuthorize()
        {
            return View("AppAutoAuthorize");
        }


        /// <summary>
        /// 获取app自动授权设置grid列表
        /// </summary>
        public string GetAppAutoAuthorizeList()
        {
            string rolename = System.Web.HttpContext.Current.Request.Params["rolename"];
            return JsonConvert.SerializeObject(proxy.GetAppAutoAuthorizeList(rolename));
        }

        public int Save()
        {
            string gridData = System.Web.HttpContext.Current.Request.Params["gridData"];

            DataTable dt = DataConverterHelper.ToDataTable(gridData, "select * from fg3_app_autoauthorize");
            return proxy.Save(dt);
        }
    }
}
