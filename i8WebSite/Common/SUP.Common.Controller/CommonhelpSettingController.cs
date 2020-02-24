using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Facade;
using NG3.Aop.Transaction;
using System.Data;
using NG3.Web.Controller;
using SUP.Common.DataEntity;
using SUP.Common.Base;
using NG3.Web.Mvc;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NG3;
using System.Web.SessionState;

namespace SUP.Common.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CommonhelpSettingController :AFController
    {
        private ICommonhelpSettingFacade proxy;

        public CommonhelpSettingController()
        {
            proxy = AopObjectProxy.GetObject<ICommonhelpSettingFacade>(new CommonhelpSettingFacade());
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult CommonHelpList()
        {
            return View("CommonHelpList");
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult CommonHelpSetting()
        {
            string code = System.Web.HttpContext.Current.Request.Params["id"];
            string otype = System.Web.HttpContext.Current.Request.Params["otype"];
            ViewBag.ID = code;
            ViewBag.OType = otype;
            return View("CommonHelpSetting");
        }

        public string GetList()
        {
            DataStoreParam storeparam =   this.GetDataStoreParam();
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //string clientQuery = string.Empty;
            //if (!string.IsNullOrEmpty(clientJsonQuery))
            //{
            //    clientQuery = DataConverterHelper.BuildQuery(clientJsonQuery);
            //}
            int totalRecord = 0;

            DataTable dt = proxy.GetList(clientJsonQuery, storeparam.PageSize, storeparam.PageIndex, ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        public string GetMaster(string id)
        {
            ResponseResult ret = proxy.GetMaster(id);
            DataTable dt = ret.Data as DataTable;

            if (dt != null || dt.Rows.Count > 0)
            {
                JObject jo = dt.Rows[0].ToJObject();               
                ret.Data = jo;      
            }
            else
            {
                ret.Status = ResponseStatus.Error.ToString();
                ret.Data = "";
            }
            string response = JsonConvert.SerializeObject(ret);
            return response;
        }

        public string GetSystemField(string masterId)
        {
            DataTable dt = proxy.GetSystemField(masterId);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }

        public string GetUserField(string masterId)
        {
            DataTable dt = proxy.GetUserField(masterId);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }

        public string GetDetailTableField(string masterId)
        {
            DataTable dt = proxy.GetDetailTableField(masterId);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }
        public string GetQueryProperty(string masterId)
        {
            DataTable dt = proxy.GetQueryProperty(masterId);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }

        public string GetRichQuery(string masterId)
        {
            DataTable dt = proxy.GetRichQuery(masterId);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }

        public string Save() 
        {
            string masterId = System.Web.HttpContext.Current.Request.Form["masterid"];
            string masterdata = System.Web.HttpContext.Current.Request.Form["masterData"];
            string sysdata = System.Web.HttpContext.Current.Request.Form["sysdata"];
            string userdata = System.Web.HttpContext.Current.Request.Form["userdata"];
            string detailtabledata = System.Web.HttpContext.Current.Request.Form["detailtabledata"];

            string queryPropData = System.Web.HttpContext.Current.Request.Form["queryPropData"];
            string richQueryData = System.Web.HttpContext.Current.Request.Form["richQueryData"];

            DataTable masterdt = new DataTable();
            DataTable sysdt = new DataTable();
            DataTable userdt = new DataTable();
            DataTable detaildt = new DataTable();
            DataTable queryPropDt = new DataTable();
            DataTable richQueryDt = new DataTable();


            if (!string.IsNullOrWhiteSpace(masterdata))
            {
                masterdt = DataConverterHelper.ToDataTable(masterdata,"select * from fg_helpinfo_master");
            }
            if (!string.IsNullOrWhiteSpace(sysdata))
            {
                sysdt = DataConverterHelper.ToDataTable(sysdata, "select * from fg_helpinfo_sys");
            }
            if (!string.IsNullOrWhiteSpace(userdata))
            {
                userdt = DataConverterHelper.ToDataTable(userdata, "select * from fg_helpinfo_user");
            }
            if (!string.IsNullOrWhiteSpace(queryPropData))
            {
                queryPropDt = DataConverterHelper.ToDataTable(queryPropData, "select * from fg_helpinfo_queryprop");
            }
            if (!string.IsNullOrWhiteSpace(richQueryData))
            {
                richQueryDt = DataConverterHelper.ToDataTable(richQueryData, "select * from fg_helpinfo_richquery");
            }
            if (!string.IsNullOrWhiteSpace(detailtabledata))
            {
                detaildt = DataConverterHelper.ToDataTable(detailtabledata, "select * from fg_helpinfo_detailtable");
            }

            CommonHelpSettingEntity entity = new CommonHelpSettingEntity();
            entity.MasterDt = masterdt;
            entity.SystemFieldDt = sysdt;
            entity.UserFieldDt = userdt;
            entity.QueryProperty = queryPropDt;
            entity.RichQuery = richQueryDt;
            entity.DetailDt = detaildt;

            ResponseResult result = proxy.Save(masterId, entity);

            string response = JsonConvert.SerializeObject(result);
            return response;
        }
    }
}
