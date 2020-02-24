using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Mvc;
using Newtonsoft.Json;

using NG3.Web.Controller;
using NG3.Web.Mvc;
using NG3;
using NG3.Data.Builder;
using SUP.Common.Facade;
using SUP.Common.DataEntity;
using NG3.Aop.Transaction;
using SUP.Common.Base;

using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.SessionState;

namespace SUP.Common.Controller
{
    //public class CommonHelpController : System.Web.Mvc.Controller
    [SessionState(SessionStateBehavior.ReadOnly)] 
    public class CommonHelpController : AFController 
    {
        private CommonHelpFacade facade = null;
        private ICommonHelpFacade proxy;

        public CommonHelpController()
        {
            facade = new CommonHelpFacade();
            proxy = AopObjectProxy.GetObject<ICommonHelpFacade>(facade);
        }
  

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]        
        public ActionResult Index(string helpid)
        {
            this.InitialHelpObj(helpid);

            return View("CommonHelp");
        }

        public ActionResult ExtHelp(string helpid)
        {
            CommonHelpEntity item = proxy.GetCommonHelpItem(helpid);

            ViewBag.Flag = helpid;
            ViewBag.JsonTemplate = proxy.GetJsonTemplate(helpid);

            return View("ExtCommonHelp");
        }

        private void InitialHelpObj(string helpid)
        {
            CommonHelpEntity item = proxy.GetCommonHelpItem(helpid);

            ViewBag.Flag = helpid;
            ViewBag.HelpTitle = item.Title;
            ViewBag.Icon = string.Empty;
            ViewBag.CodeField = item.CodeField;
            ViewBag.NameField = item.NameField;
            ViewBag.SelectMode = 1;
            ViewBag.T0 = string.Empty;
            ViewBag.T1 = proxy.GetQueryTemplate(helpid);
            ViewBag.T2 = proxy.GetListTemplate(helpid);            
         
        }

        private void InitialExtHelpObj(string helpid)
        {
            CommonHelpEntity item = proxy.GetCommonHelpItem(helpid);

            ViewBag.Flag = helpid;
            ViewBag.JsonTemplate = proxy.GetJsonTemplate(helpid);
        }
        
        [ValidateInput(false)]
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetList()
        {
            string flag = Pub.Request("helpid");

            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件
            string leftLikeJsonQuery = System.Web.HttpContext.Current.Request.Params["leftLikefilter"];//查询条件，like value%
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];//客户端传入的sql查询条件，有些情况无法参数化

            string clientJsonQuery = Pub.Request("query");//客户端的查询条件,json格式的
            string clientQuery = string.Empty;
            if (!string.IsNullOrEmpty(clientJsonQuery))
            {
                clientQuery = clientJsonQuery; //DataConverterHelper.BuildQuery(clientJsonQuery);
            }

            int pageSize = 20;            
            int.TryParse(Pub.Request("rows"),out pageSize);
            int startRow;
            int.TryParse(Pub.Request("startRow"), out startRow);
            int pageIndex = startRow / pageSize;

            //var meta = this.RequestMeta.AsGridMeta();
            
            int totalRecord = 0;

            DataTable dt = proxy.GetList(flag, pageSize, pageIndex, ref totalRecord, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter,false,false);
            //DataPage d = new DataPage(dt, startRow, pageSize, totalRecord);
            //Ajax.WriteRaw(d.ToJSON());

            string str = DataConverterHelper.ToJson(dt,totalRecord);
            return str;
            //Ajax.WriteRaw(str);
            
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetHelpList()
        {
            bool ORMMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];

            bool isAutoComplete = false;
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//内部json查询条件, like %value%
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件, =value
            string leftLikeJsonQuery = System.Web.HttpContext.Current.Request.Params["leftLikefilter"];//查询条件，like value%
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];//客户端传入的sql查询条件，有些情况无法参数化

            string clientQuery = string.Empty;
            if (!string.IsNullOrEmpty(clientJsonQuery))
            {
                //clientQuery = DataConverterHelper.BuildQuery(clientJsonQuery);
                clientQuery = clientJsonQuery;
            }
            else
            {
                string queryValue = System.Web.HttpContext.Current.Request.Params["query"];//联想搜索过滤

                if (!string.IsNullOrEmpty(queryValue))
                {
                    isAutoComplete = true;
                    clientQuery = queryValue;
                   //queryValue = System.Web.HttpUtility.UrlDecode(queryValue,Encoding.GetEncoding("UTF-8"));
                   //clientQuery = this.BuildInputQuery(helpid,queryValue);
                }
            }

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);

            int totalRecord = 0;
            //DataTable dt = proxy.GetList(helpid,clientQuery, pageSize, (pageIndex-1), ref totalRecord);
            DataTable dt = proxy.GetList(helpid, pageSize, pageIndex, ref totalRecord, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter, isAutoComplete, ORMMode);
            string str = DataConverterHelper.ToJson(dt, totalRecord);

            return str;
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetListData()
        {
            return GetEasyList(false);//easyui的combobox不用格式化
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetEasyHelpList()
        {
            return GetEasyList(true);//easyui的grid需格式化
        }

        private string GetEasyList(bool format)
        {
            bool ORMMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            //string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string limit = System.Web.HttpContext.Current.Request.Params["rows"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];

            bool isAutoComplete = false;
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//内部json查询条件, like %value%
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件, =value
            string leftLikeJsonQuery = System.Web.HttpContext.Current.Request.Params["leftLikefilter"];//查询条件，like value%
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];//客户端传入的sql查询条件，有些情况无法参数化

            string clientQuery = string.Empty;
            if (!string.IsNullOrEmpty(clientJsonQuery))
            {
                //clientQuery = DataConverterHelper.BuildQuery(clientJsonQuery);
                clientQuery = clientJsonQuery;
            }
            else
            {
                string queryValue = System.Web.HttpContext.Current.Request.Params["query"];//联想搜索过滤

                if (!string.IsNullOrEmpty(queryValue))
                {
                    isAutoComplete = true;
                    clientQuery = queryValue;
                    //queryValue = System.Web.HttpUtility.UrlDecode(queryValue,Encoding.GetEncoding("UTF-8"));
                    //clientQuery = this.BuildInputQuery(helpid,queryValue);
                }
            }

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            if (pageSize == 0)
            {
                pageSize = 20;
            }
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);

            int totalRecord = 0;
            //DataTable dt = proxy.GetList(helpid,clientQuery, pageSize, (pageIndex-1), ref totalRecord);
            DataTable dt = proxy.GetList(helpid, pageSize, pageIndex, ref totalRecord, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter, isAutoComplete, ORMMode);
            string str = DataConverterHelper.ToJsonData(dt, totalRecord,format);

            System.Web.HttpContext.Current.Response.ContentType = "text/json";
            return str;
        }
        
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetHelpInfo(string helpid)
        {
            bool ORMMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;
            CommonHelpEntity item = proxy.GetCommonHelpItem(helpid,ORMMode);
            
            JObject jo = new JObject();
            jo.Add("Title", item.Title);
            jo.Add("HeadText", item.HeadText);
            if (ORMMode)
            {
                jo.Add("AllField", item.AllProperty);
                jo.Add("codeField", item.CodeProperty);
                jo.Add("nameField",item.NameProperty);
            }
            else
            {
                jo.Add("AllField", item.AllField);
                jo.Add("codeField", item.CodeField);
                jo.Add("nameField", item.NameField);
            }
            string str = JsonConvert.SerializeObject(jo);            
            
            return "{status : \"ok\", data:" + str + "}";        
        }

        public string GetHelpTemplate(string helpid)
        {
            CommonHelpEntity item = proxy.GetCommonHelpItem(helpid);

            //JObject jo = new JObject();
            //jo.Add("Title", item.Title);
            //jo.Add("Template", proxy.GetJsonTemplate(helpid));
            //string str = JsonConvert.SerializeObject(jo);

            string str = proxy.GetJsonTemplate(helpid);           
           
            //StreamReader sr = new StreamReader()
            str.Replace("\\\n", "");
            //str.Replace((char)13, ' ');

            return "{status : \"ok\", Title:\"" + item.Title + "\",template:" + str + "}";
        }
        
        private string BuildInputQuery(string helpid, string queryValue)
        {
            CommonHelpEntity item = proxy.GetCommonHelpItem(helpid);
            StringBuilder strb = new StringBuilder();

            //无奈
            NG3.Data.Service.DbHelper.Open();
            NG3.Data.DbVendor vender = NG3.Data.Service.DbHelper.Vendor; 
            NG3.Data.Service.DbHelper.Close();

            //获取汉字拼音首字母函数
            string functionName = "dbo.fun_getPY";
            if (vender == NG3.Data.DbVendor.Oracle)
            {
                functionName = "fun_getPY";
            }

            strb.Append(item.CodeField);
            strb.Append(" like '%");
            strb.Append(queryValue);
            strb.Append("%' or ");
            strb.Append(item.NameField);
            strb.Append(" like '%");
            strb.Append(queryValue);
            strb.Append("%' or ");
            strb.Append( functionName + "(");
            strb.Append(item.NameField);
            strb.Append(") like '%");
            strb.Append(queryValue);
            strb.Append("%'");


            return strb.ToString();

        }  
        
        /// <summary>
        /// 根据代码获取名称
        /// </summary>
        /// <param name="helpflag">帮助标记</param>
        /// <param name="code">代码值</param>
        /// <param name="clientQuery">查询条件</param>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetName(string helpid, string code,string helptype)
        {
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件

            if (string.IsNullOrEmpty(helptype))
            {
                helptype = SelectMode.Single.ToString();
            }

            string name = proxy.GetName(helpid, code, helptype,string.Empty, outJsonQuery);
            return "{status : \"ok\", name:\""+ name +"\"}";

        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetAllNames()
        {
            string valueobj = System.Web.HttpContext.Current.Request.Params["valueobj"];

            JArray arr = JsonConvert.DeserializeObject(valueobj) as JArray;

            IList<HelpValueNameEntity> list = new List<HelpValueNameEntity>();
            for (int i = 0; i < arr.Count; i++)
            {
                //HelpValueNameEntity temp = JsonConvert.DeserializeObject<HelpValueNameEntity>(arr[i].ToString());
                HelpValueNameEntity temp = new HelpValueNameEntity();
                temp.HelpID = arr[i]["HelpID"].ToString();
                temp.Code = arr[i]["Code"].ToString();
                temp.HelpType = arr[i]["HelpType"].ToString();
                temp.SelectMode = arr[i]["SelectMode"].ToString();
                temp.OutJsonQuery = arr[i]["OutJsonQuery"].ToString();

                list.Add(temp);
            }

            HelpValueNameEntity[] nameEntity = proxy.GetAllNames(list);

            string names = JsonConvert.SerializeObject(list);

            return "{status : \"ok\", name: " + names +" }";
        }

        public string GetSelectedData(string helpid, string codes,bool mode)
        {
            DataTable dt = proxy.GetSelectedData(helpid, codes,mode);
            string str = DataConverterHelper.ToJson(dt, dt.Rows.Count);

            return str;           
        }

        public string ValidateData(string helpid, string inputValue, string selectMode)
        {
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];
            string helptype = System.Web.HttpContext.Current.Request.Params["helptype"];
            bool ret = proxy.ValidateData(helpid, inputValue, clientSqlFilter,selectMode,helptype);

            ResponseResult result = new ResponseResult();
            result.Data = ret;           
            result.Status = ResponseStatus.Success;
          
            string response = JsonConvert.SerializeObject(result);
            return response;
        }
    }
}
