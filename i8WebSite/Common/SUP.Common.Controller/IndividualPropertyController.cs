using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.DataEntity.Individual;
using SUP.Common.Facade;
using NG3.Aop.Transaction;
using SUP.Common.Base;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NG3;
using NG3.Web.Controller;
using System.Web.Mvc;
using SUP.Common.DataEntity;
using System.Web.SessionState;

namespace SUP.Common.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public  class IndividualPropertyController : AFController
    {
        private IIndividualPropertyFacade proxy;
        private IQueryPanelFacade queryPanelproxy;
 
        public IndividualPropertyController()
        {
            proxy = AopObjectProxy.GetObject<IIndividualPropertyFacade>(new IndividualPropertyFacade());
            QueryPanelFacade facade = new QueryPanelFacade();
            queryPanelproxy = AopObjectProxy.GetObject<IQueryPanelFacade>(facade);
        }

        public string GetTableRegList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];
            

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);

            int totalRecord = 0;

            DataTable dt = proxy.GetTableRegList(clientJsonQuery, pageSize, pageIndex, ref totalRecord);

            string json = DataConverterHelper.ToJson(dt, totalRecord);

            return json;
        }

        public string GetTableRegInfo(string tname,string busid)
        {            

             try
            {
                Int64 busPhid = Convert.ToInt64(busid);
                DataTable dt = proxy.GetTableRegInfo(tname,busPhid);
                if (dt.Rows.Count > 0)
                {
                    JObject jo = dt.Rows[0].ToJObject();
                    string json = JsonConvert.SerializeObject(jo);
                    return "{status : \"ok\", data:" + json + "}";
                }
                else
                {
                    return "{status : \"error\"}";
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public string GetColumnsInfo(string tname)
        {
            DataTable dt = proxy.GetColumnsInfo(tname);

            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);

            return json;
        }

        public string Save(DataTable columnregdt)
        {
           
            string griddata = System.Web.HttpContext.Current.Request.Form["griddata"];

            try
            {
                DataTable detaildt = DataConverterHelper.ToDataTable(griddata, "select * from fg_columns");

                ResponseResult result = proxy.Save(detaildt);

                string response = JsonConvert.SerializeObject(result);
                return response;             
            }
            catch (Exception ex)
            {
                ResponseResult result = new ResponseResult();
                result.Status = ResponseStatus.Error;
                result.Msg = ex.Message;
                string response = JsonConvert.SerializeObject(result);
                return response;              
            }
          
        }

       public JsonResult  GetIndividualFieldTree(string bustype)
       {
           IList<TreeJSONBase> list = proxy.GetIndividualFieldTree(bustype);
           return this.Json(list, JsonRequestBehavior.AllowGet);
       }

       public string GetColomnInfo(string tname)
       {
           DataTable dt = proxy.GetColumnInfo(tname);

           string json = JsonConvert.SerializeObject(dt);

           return "{status : \"ok\", data:" + json + "}";
       }

       public string GetBusTypeList()
       {
           DataStoreParam storeparam = this.GetDataStoreParam();
           string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件         
           int totalRecord = 0;

           DataTable dt = proxy.GetBusTypeList(clientJsonQuery, storeparam.PageSize, storeparam.PageIndex, ref totalRecord);
           string json = DataConverterHelper.ToJson(dt, totalRecord);
           return json;
       }

       public string GetPropertyUIInfo(string tablename,string bustype)
       {
           DataTable dt = proxy.GetPropertyUIInfo(tablename,bustype);
           string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
           return json;
       }

       public string SavePropertyUIInfo()
       {
           string griddata = System.Web.HttpContext.Current.Request.Form["griddata"];

            try
            {
                DataTable dt = DataConverterHelper.ToDataTable(griddata, "select * from fg_col_uireg");
                ResponseResult result = proxy.SavePropertyUIInfo(dt);

                string response = JsonConvert.SerializeObject(result);
                return response;
            }
            catch (Exception ex)
            {
                ResponseResult result = new ResponseResult();
                result.Status = ResponseStatus.Error;
                result.Msg = ex.Message;
                string response = JsonConvert.SerializeObject(result);
                return response;              
            }
        
       }
       

        public JsonResult LoadBusTree() {

            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            string tablename = System.Web.HttpContext.Current.Request.Params["tablename"];
            //string tablename = Pub.Request("tablename");
            IList<TreeJSONBase> list = proxy.LoadBusTree(nodeid,tablename);
            return this.Json(list, JsonRequestBehavior.AllowGet);

        }

        public string GetBusTables()
        {
            string busID = System.Web.HttpContext.Current.Request.Params["busid"];
            DataTable dt = proxy.GetBusTables(busID);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }

        public string GetInUseFiedlUIInfo()
        {
            string fieldUIId = System.Web.HttpContext.Current.Request.Form["fieldUIId"];
            try
            {
                ResponseResult result = proxy.GetInUseFiedlUIInfo(fieldUIId);
                string response = JsonConvert.SerializeObject(result);
                return response;
            }
            catch (Exception ex)
            {
                ResponseResult result = new ResponseResult();
                result.Status = ResponseStatus.Error;
                result.Msg = ex.Message;
                string response = JsonConvert.SerializeObject(result);
                return response;
            }
        }

    }
}
