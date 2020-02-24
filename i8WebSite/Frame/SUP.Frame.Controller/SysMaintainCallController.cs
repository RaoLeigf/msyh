using Newtonsoft.Json;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using SUP.Common.Base;
using SUP.Frame.Facade;
using System;
using System.Data;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SysMaintainCallController : AFController
    {

        private ISysMaintainCallFacade proxy;

        public SysMaintainCallController()
        {
            proxy = AopObjectProxy.GetObject<ISysMaintainCallFacade>(new SysMaintainCallFacade());
        }

        public ActionResult List()
        {
            if (NG3.AppInfoBase.UserType != UserType.System)
            {
                HttpContext.Response.Redirect("~/SUP/ErrorPage?msg=系统管理员才能设置！"); //重定向到错误页面
            }

            return View("SysMaintainCallList");
        }

        public ActionResult Edit()
        {
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];//操作类型
            ViewBag.Phid = System.Web.HttpContext.Current.Request.Params["phid"];
            ViewBag.OverTime = System.Web.HttpContext.Current.Request.Params["overtime"];
            return View("SysMaintainCallEdit");
        }

        public string GetSysMaintainCall()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            DataTable dt = proxy.GetSysMaintainCall(clientJsonQuery);
            int totalRecord = dt.Rows.Count;
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        public string GetSysMaintainCallByPhid(string phid)
        {
            DataTable dt = proxy.GetSysMaintainCallByPhid(phid);
            return JsonConvert.SerializeObject(dt);
        }

        public string CheckSysMaintainCallState()
        {
            return proxy.CheckSysMaintainCallState();
        }

        public bool DelSysMaintainCall(string phid)
        {
            return proxy.DelSysMaintainCall(phid);
        }

        public string StartSysMaintainCall(string phid)
        {
            return proxy.StartSysMaintainCall(phid);
        }

        public bool CloseSysMaintainCall(string phid)
        {
            return proxy.CloseSysMaintainCall(phid);
        }

        public string GetUcodeList()
        {
            DataTable dt = proxy.GetUcodeList();
            string json = "[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                json += "{\"code\":\"" + dt.Rows[i]["ucode"] + "\",\"name\":\"" + dt.Rows[i]["ucode"] + "\"}";
                if (i != dt.Rows.Count - 1)
                {
                    json += ",";
                }
            }
            json += "]";
            return "{items: " + json + "}"; ;
        }

        public string GetUserList()
        {
            string ucode = System.Web.HttpContext.Current.Request.Params["ucode"];
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];
            string searchkey = System.Web.HttpContext.Current.Request.Params["searchkey"];//内部json查询条件, like %value%
            string connectString = proxy.GetUserConnectString(ucode);
            int totalRecord = 0;
            DataTable dt = proxy.GetUserList(connectString, int.Parse(limit), int.Parse(page), searchkey, ref totalRecord);
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        public string SaveSysMaintainCall(string phid, string title, string starttime, string preenddate, string endtype, string enddate, string runinfo, string endinfo, string netfreecall, string netfreecallucode, string allowlogin, string otype)
        {
            return proxy.SaveSysMaintainCall(phid, title, starttime, preenddate, endtype, enddate, runinfo, endinfo, netfreecall, netfreecallucode, allowlogin, otype);
        }

    }
}
