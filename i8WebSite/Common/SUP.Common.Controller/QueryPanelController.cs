using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Web.Controller;
using SUP.Common.Facade;
using NG3;
using NG3.Aop.Transaction;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SUP.Common.DataEntity.Individual;
using SUP.Common.Base;
using SUP.Common.DataEntity;

namespace SUP.Common.Controller
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class QueryPanelController : AFController
    {
        private IQueryPanelFacade proxy;
        public QueryPanelController()
        {
            QueryPanelFacade facade = new QueryPanelFacade();
            proxy = AopObjectProxy.GetObject<IQueryPanelFacade>(facade);
        }

        /// <summary>
        /// 保存用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="ClientJsonString"></param>
        /// <returns></returns>
        public void SetQueryPanelData(string PageId, string ClientJsonString)
        {
            proxy.SetQueryPanelData(PageId, ClientJsonString);
        }

        /// <summary>
        /// 获取用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <returns></returns>
        public string GetQueryPanelData(string PageId)
        {
            DataTable tmpDT = proxy.GetQueryPanelData(PageId);
            if (tmpDT.Rows.Count == 0)
            {
                return "{}";
            }
            JObject jo = tmpDT.Rows[0].ToJObject();
            return JsonConvert.SerializeObject(jo);
        }


        /// <summary>
        /// 获取内嵌查询面板控件
        /// </summary>
        public void GetIndividualQueryPanel()
        {
            string pageid = System.Web.HttpContext.Current.Request.Params["pageid"];
            IList<ExtControlInfoBase> list = proxy.GetIndividualQueryPanel(pageid, AppInfoBase.OCode, AppInfoBase.LoginID);


            DataTable tmpDT = proxy.GetQueryPanelData(pageid);
            string langInfo = proxy.GetLang();//获取多语言

            string sortfilter = proxy.GetSortFilterData(pageid, AppInfoBase.OCode, AppInfoBase.LoginID);

            string ischeck = proxy.GetCheckData(pageid, AppInfoBase.OCode, AppInfoBase.LoginID);

            Ajax.WriteStartObject();

            Ajax.WritePropertyName("list");
            Ajax.WriteValue(JsonConvert.SerializeObject(list));

            Ajax.WritePropertyName("rememberstr");
            if (tmpDT.Rows.Count == 0)
            {
                Ajax.WriteValue("{}");
            }
            else
            {
                Ajax.WriteValue(
                    JsonConvert.SerializeObject(
                        Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(
                            tmpDT.Rows[0]["remeberstr"].ToString())));
            }

            Ajax.WritePropertyName("langInfo");
            Ajax.WriteValue(langInfo);

            //返回排序数据
            Ajax.WritePropertyName("sortfilter");
            Ajax.WriteValue(sortfilter);

            //返回内嵌查询面板的记忆搜索的check状态值
            Ajax.WritePropertyName("ischeck");
            Ajax.WriteValue(ischeck);

            Ajax.WriteEndObject();
        }

        /// <summary>
        /// 获取用户查询面板自定义信息
        /// </summary>
        /// <returns></returns>
        public string GetIndividualQueryPanelInfo()
        {
            string pageid = System.Web.HttpContext.Current.Request.Params["pageid"];
            DataTable dt = proxy.GetIndividualQueryPanelInfo(pageid, AppInfoBase.OCode, AppInfoBase.LoginID);
            string str = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return str;
        }

        /// <summary>
        /// 保存查询条件设置信息
        /// </summary>
        /// <returns></returns>
        public string SaveQueryInfo()
        {
            string pageid = System.Web.HttpContext.Current.Request.Params["pageid"];
            string griddata = System.Web.HttpContext.Current.Request.Params["griddata"];

            string gridchange = System.Web.HttpContext.Current.Request.Params["gridchange"]; //修改行


            DataTable dt = DataConverterHelper.ToDataTable(griddata, "select ccode,'' as definetype, searchtable,searchfield,fname_chn,isplay,combflg,defaultdata,playindex AS displayindex,sortmode,sortorder from c_sys_search");
            DataTable dtchange = DataConverterHelper.ToDataTable(gridchange, "select ccode,pageid,'' as definetype, searchtable,searchfield,fname_chn,isplay,combflg,defaultdata,playindex AS displayindex,sortmode,sortorder from c_sys_search");


            int iret = proxy.SaveQueryInfo(dtchange, dt, pageid, AppInfoBase.OCode, AppInfoBase.LoginID);
            ResponseResult result = new ResponseResult();
            if (iret > 0)
            {
                result.Status = ResponseStatus.Success;
            }
            else
            {
                result.Status = ResponseStatus.Error;
            }
            string response = JsonConvert.SerializeObject(result);
            return response;
        }

        public string SaveCheckData()
        {
            string pageid = System.Web.HttpContext.Current.Request.Params["pageid"];
            string ischeck = System.Web.HttpContext.Current.Request.Params["ischeck"];
            try
            {
                int iret = proxy.SaveCheckData(pageid, AppInfoBase.OCode, AppInfoBase.LoginID,ischeck);
                
            }
            catch(Exception e)
            {
                
            }
            return "success";
        }

        //恢复默认设置
        public string RestoreDefault()
        {
            string pageid = System.Web.HttpContext.Current.Request.Params["pageid"];
            try
            {
                int iret = proxy.RestoreDefault(pageid, AppInfoBase.OCode, AppInfoBase.LoginID);

            }
            catch (Exception e)
            {

            }
            return "success";
        }


    }
}
