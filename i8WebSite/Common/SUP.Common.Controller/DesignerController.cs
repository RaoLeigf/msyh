using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using NG3.Web.Controller;
using NG3.Web.Mvc;

using NG3.Aop.Transaction;
using SUP.Common.Facade;
using SUP.Common.DataEntity;
using System.Web;
using SUP.Common.Base;
using System.Data;
using NG3;

namespace SUP.Common.Controller
{
    public class DesignerController : AFController
    {
        private IIndividualUIFacade proxy;


        public DesignerController()
        {
            proxy = AopObjectProxy.GetObject<IIndividualUIFacade>(new IndividualUIFacade());
        }

        //[LogExceptionFilter]
        //[HandleError]
        //[ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index()
        {
            //return RedirectToAction("Login", "Home", new { area = ""});
            //return Redirect(HttpContext.Request.ApplicationPath + "/Home/Login");

           

            string app = System.Web.HttpContext.Current.Request.ApplicationPath;

            string id = System.Web.HttpContext.Current.Request.Params["phid"];
            string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];
            string busName = System.Web.HttpContext.Current.Request.Params["busname"];

            ViewBag.Title = "界面设计-" + HttpUtility.UrlDecode(busName);

            try
            {
                // NG3Config目录下的config.xml文件
                string jsVersion = Config.Get("NG3JsVersion");
                string fileurl = proxy.GetIndividualRegUrl(bustype);
                fileurl += string.Format("?_v={0}", jsVersion);

                string url = string.Empty;
                if (app == "/")
                {
                    url = "/NG3Resource/IndividualInfo/" + fileurl;
                }
                else
                {
                    url = app + "/NG3Resource/IndividualInfo/" + fileurl;
                }
                ViewBag.srciptUrl = url;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //psoft组的GcHelpIn.js会引起i6s设计器有问题
            string product = NG3.AppInfoBase.UP.Product + NG3.AppInfoBase.UP.Series;
            if (product.ToUpper() == "I6P" ||  product.ToUpper() == "I8")
            {
                string url = string.Empty;
                if (app == "/")
                {
                    url = "/NG3Resource/js/GcCommon/GcHelpIn.js";
                }
                else
                {
                    url = app + "/NG3Resource/js/GcCommon/GcHelpIn.js";
                }
                ViewBag.GcHelpInUrl = url;
            }

            //throw new Exception();

            //LayoutLogInfo info = proxy.GetLayoutLogInfo("*","HREmpInfoEdit");
            string individaulInfo = proxy.GetIndividualUIbyCode(id); //(info == null) ? string.Empty : info.Value;

            ViewBag.id = id;
            ViewBag.individualInfo = individaulInfo;
            ViewBag.busType = bustype;

            return View("Designer");
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult IndividualInfoList()
        {
            this.GridStateIDs = new string[] { "SupIndividualInfoListgrid" };
            return View("IndividualInfoList");
        }


        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult IndividualPropertyList()
        {
            //TestOracle();
            this.GridStateIDs = new string[] { "IndividualPropertyList" };
            return View("IndividualPropertyList");
        }

        private static void TestOracle()
        {
            string result;
            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            string pubConn = string.Empty;
            string userConn = string.Empty;
            pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
            userConn = dbbuilder.GetDefaultConnString();//取默认连接串

            userConn = dbbuilder.GetAccConnstringElement("10.0.16.168:1521/orclup.rd.ngsoft.com", "NG0001", pubConn, out result);

            userConn = dbbuilder.GetAccConnstringElement(0, "NG0001", pubConn, out result);

           DataTable tb = dbbuilder.GetDBserverList();
           DataTable dt =  dbbuilder.GetAccountList("10.0.0.233:1521/orcl.rd.ngsoft.com");
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult IndividualPropertyEdit()
        {
            this.GridStateIDs = new string[] { "Sup_IndividualPropertyEdit" }; 
            ViewBag.Code = System.Web.HttpContext.Current.Request.Params["code"];
            ViewBag.Tname = System.Web.HttpContext.Current.Request.Params["tname"];
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];
            ViewBag.BusID = System.Web.HttpContext.Current.Request.Params["busid"];            
            ViewBag.BusType = System.Web.HttpContext.Current.Request.Params["bustype"];
            ViewBag.BusName = System.Web.HttpContext.Current.Request.Params["busname"];

            return View("IndividualPropertyEdit");
        }

        public ActionResult IndividualPropertyUIList()
        {
            this.GridStateIDs = new string[] { "Sup_IndividaulPropertyUIList" };           
            return View("IndividualPropertyUIList");
        }

        public ActionResult IndividualPropertyUIEdit()
        {
            this.GridStateIDs = new string[] { "Sup_IndividualPropertyUIEdit" };
            ViewBag.tname = System.Web.HttpContext.Current.Request.Params["tname"];
            ViewBag.BusType = System.Web.HttpContext.Current.Request.Params["bustype"];
            ViewBag.BusName = System.Web.HttpContext.Current.Request.Params["busname"];
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];
            return View("IndividualPropertyUIEdit");
        }



        /// <summary>
        /// 脚本代码
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult IndividualScript()
        {
            ViewBag.BusType = System.Web.HttpContext.Current.Request.Params["bustype"];
            string id = System.Web.HttpContext.Current.Request.Params["id"];
            ViewBag.ID = id;
            string schemaName = proxy.GetSchemaName(Convert.ToInt64(id));
            ViewBag.Title = "脚本设计-" + HttpUtility.UrlDecode(schemaName);
            return View("IndividualScript");
        }


        public string GetScriptCode()
        {            
            string id = System.Web.HttpContext.Current.Request.Params["id"];

            string script = proxy.GetScriptCode(Convert.ToInt64(id));

            return script;
          
        }

        /// <summary>
        /// 保存脚本代码
        /// </summary>
        /// <returns></returns>
        public string SaveScript()
        {
            string codeScript = System.Web.HttpContext.Current.Request.Params["codeScript"];
            string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];
            string id = System.Web.HttpContext.Current.Request.Params["id"];

            int iret = proxy.SaveScript(bustype, Convert.ToInt64(id),codeScript);
            if (iret > 0)
            {
                return "{status : \"ok\"}";
            }
            else
            {
                return "{status : \"error\"}";
            }
        }

        /// <summary>
        /// 发布脚本
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public string PublishScript(string busType, string scriptCode)
        {
            string codeScript = System.Web.HttpContext.Current.Request.Params["codeScript"];
            string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];
            string id = System.Web.HttpContext.Current.Request.Params["id"];

            int iret = proxy.PublishScript(bustype, Convert.ToInt64(id), codeScript);
            if (iret > 0)
            {
                return "{status : \"ok\"}";
            }
            else
            {
                return "{status : \"error\"}";
            }
        }


    }
}
