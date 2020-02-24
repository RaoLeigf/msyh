using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Web.Mvc;
using SUP.Common.DataEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using SUP.Common.Base;
using SUP.Common.Facade;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using System.Web.Mvc;
using NG3;

namespace SUP.Common.Controller
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class RichHelpController : AFController
    {

        private RichHelpFacade facade = null;
        private IRichHelpFacade proxy;

        public RichHelpController()
        {
            facade = new RichHelpFacade();
            proxy = AopObjectProxy.GetObject<IRichHelpFacade>(facade);
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetHelpList()
        {
            bool ormMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string queryPropertyID = System.Web.HttpContext.Current.Request.Params["propertyID"];
            string queryPropertyCode = System.Web.HttpContext.Current.Request.Params["propertyCode"];
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];
            string infoRightUIContainerID = System.Web.HttpContext.Current.Request.Params["UIContainerID"];//信息权限注册的容器ID
            string busCode = System.Web.HttpContext.Current.Request.Params["BusCode"];//信息权限，业务类型phid

            bool isAutoComplete = false;
            string searchkey = System.Web.HttpContext.Current.Request.Params["searchkey"];//内部json查询条件, like %value%          
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件, =value
            string leftLikeJsonQuery = System.Web.HttpContext.Current.Request.Params["leftLikefilter"];//查询条件，like value%
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];//客户端传入的sql查询条件，有些情况无法参数化

            string treesearchkey = System.Web.HttpContext.Current.Request.Params["treesearchkey"];//树上的关键字的值
            string treerefkey = System.Web.HttpContext.Current.Request.Params["treerefkey"];//列表与树的关联字段

            string[] keys = new string[] { };
            if (!string.IsNullOrWhiteSpace(searchkey))
            {
                keys =  Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(searchkey);
            }

            string clientQuery = string.Empty;           
            string queryValue = System.Web.HttpContext.Current.Request.Params["query"];//联想搜索过滤
            if (!string.IsNullOrEmpty(queryValue))
            {
                isAutoComplete = true;
                clientQuery = queryValue;

                if (queryValue.IndexOf("'") > -1)//单引号sql注入
                {
                    return string.Empty;//
                }                  
            }           

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);

            int totalRecord = 0;
            RichHelpListArgEntity entity = new RichHelpListArgEntity();
            entity.InfoRightUIContainerID = infoRightUIContainerID;
            entity.BusCode= busCode;
            entity.Helpid = helpid;
            entity.PageSize = pageSize;
            entity.PageIndex = pageIndex;
            entity.Keys = keys;
            entity.Treerefkey = treerefkey;
            entity.Treesearchkey = treesearchkey;
            entity.OutJsonQuery = outJsonQuery;
            entity.ClientQuery = clientQuery;
            entity.ClientSqlFilter = clientSqlFilter;
            entity.QueryPropertyID = queryPropertyID;
            entity.QueryPropertyCode = queryPropertyCode;
            entity.IsAutoComplete = isAutoComplete;

            object obj = proxy.GetList(entity, ref totalRecord, ormMode);
            if (obj is DataTable)
            {
                DataTable dt = obj as DataTable;
                string str = DataConverterHelper.ToJson(dt, totalRecord);
                return str;               
            }
            else
            {
                System.Collections.IList list = obj as System.Collections.IList;
                string str = DataConverterHelper.EntityListToJson(list,totalRecord);
                return str;
            }
          
        }


        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetDetailList()
        {
            bool ormMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string masterCode = System.Web.HttpContext.Current.Request.Params["masterCode"];

            DataTable dt = proxy.GetDetailList(helpid, masterCode, ormMode);

            string str = DataConverterHelper.ToJson(dt, dt.Rows.Count);

            return str;
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetHelpInfo(string helpid)
        {
            bool ORMMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;

            CommonHelpEntity item = proxy.GetCommonHelpItem(helpid);
            JArray arr = proxy.GetRichQueryItems(helpid);
            JObject queryObj = proxy.GetQueryFilter(helpid, AppInfoBase.OCode, AppInfoBase.LoginID);

            JObject jo = new JObject();
            jo.Add("Title", item.Title);
            jo.Add("HeadText", item.HeadText);
            jo.Add("detailHeadText", item.DetailTableHeaders);
            if (ORMMode)
            {
                jo.Add("AllField", item.AllProperty);
                jo.Add("codeField", item.CodeProperty);
                jo.Add("nameField", item.NameProperty);

                jo.Add("detailTableFields", item.DetailTablePropertys);
                jo.Add("masterTableKey", item.MasterTableKeyProperty);
            }
            else
            {
                jo.Add("AllField", item.AllField);
                jo.Add("codeField", item.CodeField);
                jo.Add("nameField", item.NameField);

                jo.Add("detailTableFields", item.DetailTableFields);
                jo.Add("masterTableKey", item.MasterTableKey);
            }

            jo.Add("queryProperty", JsonConvert.SerializeObject(item.QueryPropertyItem));
            jo.Add("existQueryProp", item.ExistQueryProperty);
            jo.Add("showTree", item.ShowTree);
            jo.Add("richQueryItem", JsonConvert.SerializeObject(arr));
            jo.Add("queryFilter", JsonConvert.SerializeObject(queryObj));

            string str = JsonConvert.SerializeObject(jo);

            return "{status : \"ok\", data:" + str + "}";
        }

        /// <summary>
        /// 根据代码获取名称
        /// </summary>
        /// <param name="helpflag">帮助标记</param>
        /// <param name="code">代码值</param>
        /// <param name="clientQuery">查询条件</param>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetName(string helpid, string code, string helptype)
        {
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件

            if (string.IsNullOrEmpty(helptype))
            {
                helptype = SelectMode.Single.ToString();
            }

            string name = proxy.GetName(helpid, code, helptype, string.Empty, outJsonQuery);
            return "{status : \"ok\", name:\"" + name + "\"}";

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
                //temp.SelectMode = arr[i]["SelectMode"].ToString();
                temp.SelectMode = "1";
                temp.OutJsonQuery = arr[i]["OutJsonQuery"].ToString();

                list.Add(temp);
            }

            HelpValueNameEntity[] nameEntity = proxy.GetAllNames(list);

            string names = JsonConvert.SerializeObject(list);

            return "{status : \"ok\", name: " + names + " }";
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public JsonResult GetTreeList()
        {
            bool ormMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string clientQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//内部json查询条件, like %value%
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件, =value
            string leftLikeJsonQuery = System.Web.HttpContext.Current.Request.Params["leftLikefilter"];//查询条件，like value%
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];//客户端传入的sql查询条件，有些情况无法参数化
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];


            IList<TreeJSONBase> list = proxy.GetTreeList(helpid, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter, nodeid,ormMode);
            return this.Json(list, JsonRequestBehavior.AllowGet);

        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public JsonResult GetQueryProTree(string code, string node)
        {
            IList<TreeJSONBase> list = proxy.GetQueryProTree(code, node);
            return this.Json(list, JsonRequestBehavior.AllowGet);

        }

         [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string GetCommonUseList(string helpid)
        {
            bool ormMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;

            DataTable dt = proxy.GetCommonUseList(helpid, AppInfoBase.LoginID, AppInfoBase.OCode, ormMode);
            string str = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return str;
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string SaveCommonUseData()
        {
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string codeValue = System.Web.HttpContext.Current.Request.Params["codeValue"];
            int iret = proxy.SaveCommonUseData(helpid, codeValue, AppInfoBase.LoginID, AppInfoBase.OCode);
            ResponseResult result = new ResponseResult();
            if (iret > 0)
            {
                result.Status = ResponseStatus.Success;
            }
            else
            {
                if (iret == -2)
                {
                    result.Msg = "该数据项在[常用数据]中已经存在！";
                }
                result.Status = ResponseStatus.Error;
            }
            string response = JsonConvert.SerializeObject(result);
            return response;
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string DeleteCommonUseData()
        {
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string codeValue = System.Web.HttpContext.Current.Request.Params["codeValue"];

            int iret = proxy.DeleteCommonUseData(helpid, codeValue, AppInfoBase.LoginID, AppInfoBase.OCode);

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

        #region 树节点记忆功能处理
        /// <summary>
        /// 获取树节点的记忆状态
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetTreeMemoryInfo()
        {
            string type = System.Web.HttpContext.Current.Request.Params["type"];
            string busstype = System.Web.HttpContext.Current.Request.Params["busstype"];
            TreeMemoryEntity treeMemoryEntity = proxy.GetTreeMemory(type, string.IsNullOrEmpty(busstype) ? "all" : busstype);
            return this.Json(treeMemoryEntity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新树节点的记忆状态
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public void UpdataTreeMemory()
        {
            string type = System.Web.HttpContext.Current.Request.Params["type"];
            string busstype = System.Web.HttpContext.Current.Request.Params["busstype"];
            string FoucedNodeValue = System.Web.HttpContext.Current.Request.Params["foucednodevalue"];
            string IsMemo = System.Web.HttpContext.Current.Request.Params["ismemo"];
            TreeMemoryEntity treeMemoryEntity = new TreeMemoryEntity(NG3.AppInfoBase.LoginID, NG3.AppInfoBase.OCode, type, string.IsNullOrEmpty(busstype) ? "all" : busstype);
            treeMemoryEntity.FoucedNodeValue = FoucedNodeValue;
            treeMemoryEntity.IsMemo = IsMemo == "true";
            proxy.UpdataTreeMemory(treeMemoryEntity);
        }

        
        #endregion

        public string GetRichQueryList()
        {
            DataStoreParam storeparam = this.GetDataStoreParam();

            bool ormMode = System.Web.HttpContext.Current.Request.Params["ORMMode"] == "true" ? true : false;
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string outJsonQuery = System.Web.HttpContext.Current.Request.Params["outqueryfilter"];//外部json查询条件
            string leftLikeJsonQuery = System.Web.HttpContext.Current.Request.Params["leftLikefilter"];//查询条件，like value%
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];//客户端传入的sql查询条件，有些情况无法参数化
            string infoRightUIContainerID = System.Web.HttpContext.Current.Request.Params["UIContainerID"];//信息权限注册的容器ID

            string clientJsonQuery = Pub.Request("query");//客户端的查询条件,json格式的
            string clientQuery = string.Empty;
            if (!string.IsNullOrEmpty(clientJsonQuery))
            {
                clientQuery = clientJsonQuery; //DataConverterHelper.BuildQuery(clientJsonQuery);
            }

             int totalRecord = 0;         
            RichHelpListArgEntity entity = new RichHelpListArgEntity();
            entity.InfoRightUIContainerID = infoRightUIContainerID;
            entity.Helpid = helpid;
            entity.PageSize = storeparam.PageSize;
            entity.PageIndex = storeparam.PageIndex;
            //entity.Keys = keys;
            //entity.Treerefkey = treerefkey;
            //entity.Treesearchkey = treesearchkey;
            entity.OutJsonQuery = outJsonQuery;
            entity.ClientQuery = clientQuery;
            entity.ClientSqlFilter = clientSqlFilter;
            //entity.QueryPropertyID = queryPropertyID;
            //entity.QueryPropertyCode = queryPropertyCode;
            //entity.IsAutoComplete = isAutoComplete;

            //object obj = proxy.GetRichQueryList(helpid, storeparam.PageSize, storeparam.PageIndex, ref totalRecord, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter, ormMode);
            object obj = proxy.GetRichQueryList(entity, ref totalRecord, ormMode);
            if (obj is DataTable)
            {
                DataTable dt = obj as DataTable;
                string str = DataConverterHelper.ToJson(dt, totalRecord);

                return str;
            }
            else
            {
                System.Collections.IList list = obj as System.Collections.IList;
                string str = DataConverterHelper.EntityListToJson(list, totalRecord);
                return str;
            }
        }

        public string GetRichQueryUIInfo()
        {
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            DataTable dt = proxy.GetRichQueryUIInfo(helpid, AppInfoBase.OCode, AppInfoBase.LoginID);
            string str = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return str;
        }

        public string SaveQueryInfo()
        {
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string griddata = System.Web.HttpContext.Current.Request.Params["griddata"];

            DataTable dt = DataConverterHelper.ToDataTable(griddata, "select code,definetype,tablename,field,fname_chn,operator,defaultdata,displayindex from fg_helpinfo_richquery");

            int iret = proxy.SaveQueryInfo(dt, helpid, AppInfoBase.OCode, AppInfoBase.LoginID);
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

        /// <summary>
        /// 查询记忆
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string SaveQueryFilter()
        {
            string helpid = System.Web.HttpContext.Current.Request.Params["helpid"];
            string json = System.Web.HttpContext.Current.Request.Params["data"];

            int iret = proxy.SaveQueryFilter(helpid, json);
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

        public string GetListExtendInfo(string code)
        {
            JObject jo = proxy.GetListExtendInfo(code);

            string response = JsonConvert.SerializeObject(jo);
            return response;
        }

        public string GetSelectedData(string helpid, string codes, bool mode)
        {
            DataTable dt = proxy.GetSelectedData(helpid, codes, mode);
            string str = DataConverterHelper.ToJson(dt, dt.Rows.Count);

            return str;
        }

        public string ValidateData(string helpid, string inputValue, string selectMode)
        {
            string clientSqlFilter = System.Web.HttpContext.Current.Request.Params["clientSqlFilter"];
            string helptype = System.Web.HttpContext.Current.Request.Params["helptype"];
            bool ret = proxy.ValidateData(helpid, inputValue,clientSqlFilter,selectMode,helptype);

            ResponseResult result = new ResponseResult();
            result.Data = ret;
            result.Status = ResponseStatus.Success;

            string response = JsonConvert.SerializeObject(result);
            return response;
        }

    }
}
