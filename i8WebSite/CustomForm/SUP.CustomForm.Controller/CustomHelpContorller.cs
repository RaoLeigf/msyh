using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using SUP.CustomForm.Facade;
using SUP.CustomForm.Facade.Interface;
using SUP.CustomForm.DataEntity;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SUP.CustomForm.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomHelpController : AFController
    {
        private IHelpFacade Fac;

        public CustomHelpController()
        {
            Fac = AopObjectProxy.GetObject<IHelpFacade>(new HelpFacade());
        }

        /// <summary>
        /// 获取帮助列表;
        /// </summary>
        /// <returns></returns>
        public string GetHelpList()
        {
            string helpId = System.Web.HttpContext.Current.Request.Params["helpid"];
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];

            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//客户端的查询条件,json格式的;
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件, =value
            bool isAutoComplete = false;
            string clientQuery = System.Web.HttpContext.Current.Request.Params["query"];//谷歌查询条件

            if (!string.IsNullOrEmpty(clientJsonQuery)) //非谷歌,普通查询
            {
                clientQuery = clientJsonQuery; //DataConverterHelper.BuildQuery(clientJsonQuery);
            }
            else
            {
                isAutoComplete = true; //谷歌查询
            }

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);
            int totalRecord = 0;

            DataTable dt = Fac.GetHelpList(helpId, pageSize, pageIndex, ref totalRecord, clientQuery, isAutoComplete, outJsonQuery);
            string json = DataConverterHelper.ToJson(dt, totalRecord);

            return json;
        }

        /// <summary>
        /// 获取帮助信息;
        /// </summary>
        /// <param name="helpId"></param>
        /// <returns></returns>
        public string GetHelpInfo(string helpId)
        {
            HelpEntity item = Fac.GetHelpItem(helpId);

            JObject jo = new JObject();
            jo.Add("Title", item.Title);
            jo.Add("HeadText", item.HeadText);
            jo.Add("AllField", item.AllField);
            jo.Add("codeField", item.CodeField);
            jo.Add("nameField", item.NameField);
            string str = JsonConvert.SerializeObject(jo);

            return "{status : \"ok\", data:" + str + "}";
        }

        /// <summary>
        /// 代码转名称;
        /// </summary>
        /// <param name="helpId"></param>
        /// <param name="codeValue"></param>
        /// <returns></returns>
        public string GetName(string helpId, string codeValue, string selectMode)
        {
            string nameValue = Fac.GetName(helpId, codeValue, selectMode);

            if (nameValue != "")
            {
                return "{status : \"ok\", name:\"" + nameValue + "\"}";
            }
            else
            {
                return "{status : \"false\", name:\"" + nameValue + "\"}";
            }
        }

        /// <summary>
        /// 代码转名称批量方法;
        /// </summary>
        /// <returns></returns>
        public string GetAllNames()
        {
            string valueobj = System.Web.HttpContext.Current.Request.Params["valueobj"];
            var arr = JsonConvert.DeserializeObject(valueobj) as JArray;

            IList<HelpValueNameEntity> list = new List<HelpValueNameEntity>();
            for (int i = 0; i < arr.Count; i++)
            {
                var temp = new HelpValueNameEntity();
                temp.HelpID = arr[i]["HelpID"].ToString();
                temp.Code = arr[i]["Code"].ToString();
                temp.HelpType = arr[i]["HelpType"].ToString();
                temp.SelectMode = arr[i]["SelectMode"].ToString();
                temp.OutJsonQuery = arr[i]["OutJsonQuery"].ToString();

                list.Add(temp);
            }

            HelpValueNameEntity[] nameEntity = Fac.GetAllNames(list);
            string names = JsonConvert.SerializeObject(list);
            return "{status : \"ok\", name:" + names + "}";
        }

        /// <summary>
        /// 数据有效性;
        /// </summary>
        /// <param name="helpId"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public string ValidateData(string helpId, string inputValue)
        {
            //modify by ljy 2017.04.13 焦点一开验证grid中有问题先注释掉
            //bool ret = Fac.ValidateData(helpId, inputValue);

            ResponseResult result = new ResponseResult();
            result.Data = true;// ret;
            result.Status = ResponseStatus.Success;

            string response = JsonConvert.SerializeObject(result);
            return response;
        }
    }
}
