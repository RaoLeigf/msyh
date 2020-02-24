using Newtonsoft.Json.Linq;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
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
    public class ApplyCheckController : AFController
    {
        private IApplyCheckFacade proxy;
        /// <summary>
        /// 申请去审页面
        /// </summary>
        /// <returns></returns>
        public ApplyCheckController()
        {
            proxy = AopObjectProxy.GetObject<IApplyCheckFacade>(new ApplyCheckFacade());
        }

        public ActionResult ApplyCheck()
        {
            ViewBag.pa = System.Web.HttpContext.Current.Request.Params["params"];

            return View("ApplyCheck");
        }

        public bool ConfirmApplyCheck()
        {
            string _ucode = System.Web.HttpContext.Current.Request.Params["_ucode"];
            string _ocode = System.Web.HttpContext.Current.Request.Params["_ocode"];
            string _ccode = System.Web.HttpContext.Current.Request.Params["_ccode"];
            string _logid = System.Web.HttpContext.Current.Request.Params["_logid"];
            string paramvalue = System.Web.HttpContext.Current.Request.Params["paramvalue"];
            string msgdescription = System.Web.HttpContext.Current.Request.Params["msgdescription"];
            //string suite = System.Web.HttpContext.Current.Request.Params["suite"];
            //string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];
            string sortdate = System.Web.HttpContext.Current.Request.Params["sortdate"];
            string receiver = System.Web.HttpContext.Current.Request.Params["receiver"];
            string sender = System.Web.HttpContext.Current.Request.Params["sender"];
            string targetcboo = System.Web.HttpContext.Current.Request.Params["targetcboo"];

            return proxy.ConfirmApplyCheck(_ucode, _ocode, _logid, _ccode, paramvalue, msgdescription, Convert.ToDateTime(sortdate), receiver, sender, targetcboo);
        }

        public string ConfirmApplyCheckWithMsg()
        {
            string _ucode = System.Web.HttpContext.Current.Request.Params["_ucode"];
            string _ocode = System.Web.HttpContext.Current.Request.Params["_ocode"];
            string _ccode = System.Web.HttpContext.Current.Request.Params["_ccode"];
            string _logid = System.Web.HttpContext.Current.Request.Params["_logid"];
            string paramvalue = System.Web.HttpContext.Current.Request.Params["paramvalue"];
            string msgdescription = System.Web.HttpContext.Current.Request.Params["msgdescription"];
            //string suite = System.Web.HttpContext.Current.Request.Params["suite"];
            //string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];
            string sortdate = System.Web.HttpContext.Current.Request.Params["sortdate"];
            string receiver = System.Web.HttpContext.Current.Request.Params["receiver"];
            string sender = System.Web.HttpContext.Current.Request.Params["sender"];
            string targetcboo = System.Web.HttpContext.Current.Request.Params["targetcboo"];
            string msg = "";
            return proxy.ConfirmApplyCheck(_ucode, _ocode, _logid, _ccode, paramvalue, msgdescription, Convert.ToDateTime(sortdate), receiver, sender, targetcboo, out msg);
        }

        public bool DeleteApplyCheck()
        {
            string json = System.Web.HttpContext.Current.Request.Params["params"];
            JArray ja = JArray.Parse(json);
            string _ucode = string.Empty;
            string _ocode = string.Empty;
            string _logid = string.Empty;
            string paramvalue = string.Empty;
            string receiver = string.Empty;
            if (ja.Count > 0) {
                if (ja[0]["ucode"] != null && ja[0]["ucode"].ToString() != "")
                {
                    _ucode = ja[0]["ucode"].ToString();
                }
                if (ja[0]["ocode"] != null)
                {
                    _ocode = ja[0]["ocode"].ToString();
                }
                if (ja[0]["logid"] != null)
                {
                    _logid = ja[0]["logid"].ToString();
                }
                if (ja[0]["paramvalue"] != null)
                {
                    paramvalue = ja[0]["paramvalue"].ToString();
                }
                if (ja[0]["receiver"] != null)
                {
                    receiver = ja[0]["receiver"].ToString();
                }
            }
            

            //string _ucode = System.Web.HttpContext.Current.Request.Params["_ucode"];
            //string _ocode = System.Web.HttpContext.Current.Request.Params["_ocode"];
            //string _logid = System.Web.HttpContext.Current.Request.Params["_logid"];
            //string paramvalue = System.Web.HttpContext.Current.Request.Params["paramvalue"];
            //string receiver = System.Web.HttpContext.Current.Request.Params["receiver"];

            return proxy.DeleteApplyCheck(_ucode, _ocode, _logid, paramvalue, receiver);
        }
    }
}
