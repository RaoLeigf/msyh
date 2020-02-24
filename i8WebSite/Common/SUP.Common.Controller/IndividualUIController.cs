using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SUP.Common.Facade;
using NG3.Aop.Transaction;
using System.Data;
using SUP.Common.Base;
using NG3.Web.Controller;
using System.Web.SessionState;
using System.Web.Mvc;
using SUP.Common.DataEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SUP.Common.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IndividualUIController : AFController
    {
        
        private IIndividualUIFacade proxy;

        public IndividualUIController()
        {
            proxy = AopObjectProxy.GetObject<IIndividualUIFacade>(new IndividualUIFacade());
        }

        public string Save(string data)
        {

            try
            {
                DataTable dt = DataConverterHelper.ToDataTable(data, "select * from fg_individualinfo");
                var dellist = new List<string>();

                var o = JObject.Parse(data)["table"]["deletedRow"];
                if (o != null)
                {
                    foreach (JToken child in o.Children())
                    {
                        dellist.Add(child["row"]["key"].ToString());
                    }
                }


                int iret = proxy.Save(dt, dellist);

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
            catch (Exception ex)
            {
                ResponseResult result = new ResponseResult();
                result.Status = ResponseStatus.Error;
                result.Msg = ex.Message;
                string response = JsonConvert.SerializeObject(result);
                return response;
                //return "{Status : \"error\",Msg:'" + ex.Message + "'}";
                //throw ex;
            }

        }

        public string GetIndividualInfoList(string bustype)
        {
            DataTable dt = proxy.GetIndividualInfoList(bustype);

            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);


            return json;
        }

        public string Delete(string code)
        {
            int iret = proxy.Delete(code);

            if (iret > 0)
            {
                return "{status : \"ok\"}";
            }
            else
            {
                return "{status : \"error\"}";
            }
        }

        public string GetIndividualRegList()
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];
            

            int pageSize = 20;
            int.TryParse(limit, out pageSize);
            int pageIndex = 0;
            int.TryParse(page, out pageIndex);

            int totalRecord = 0;

            DataTable dt = proxy.GetIndividualRegList(clientJsonQuery, pageSize, pageIndex, ref totalRecord);

            string json = DataConverterHelper.ToJson(dt, totalRecord);

            return json;
        }

        /// <summary>
        /// 保存自定义界面信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string SaveIndividualUI(string id,string uiinfo)
        {
            try
            {
                int iret = proxy.SaveIndividualUI(id, uiinfo);
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
            catch (Exception ex)
            {
                ResponseResult result = new ResponseResult();
                result.Status = ResponseStatus.Error;
                result.Msg = ex.Message;
                string response = JsonConvert.SerializeObject(result);
                return response;               
            }
         
          


        }

        public string Copy()
        {
          
            try
            {
                string id = System.Web.HttpContext.Current.Request.Params["id"];// 
                Int64 phid = Convert.ToInt64(id);
                int iret = proxy.Copy(phid);

                JObject jo = null;
                if (iret > 0)
                {
                    jo = new JObject
                    {
                        {"Status","success" },
                        {"Data",iret}
                    };
                }
                else {
                    jo = new JObject
                    {
                        {"Status","error" },
                        {"Msg","更新0行"}
                    };
                }

                return JsonConvert.SerializeObject(jo);
                //return "{Status : \"success\",Data:'" + ex.Message + "'}";
            }
            catch (Exception ex)
            {
                return "{Status : \"error\",Msg:'" + ex.Message + "'}";
            }
        }

        #region 自定义界面组织树相关
        
        public JsonResult GetOrgNumberByPhid(string phid,string bustypephid)
        {
            DataTable dt = proxy.GetOrgNumberByPhid(bustypephid);

            
            //处理list1（这个业务类型（bustypephid）包含的所有，数量多）
            var list1 = new List<Int64> ();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr2 = dt.Rows[i];
                list1.Add(Convert.ToInt64(dr2["object_id"]));
            }

            //处理list2（这个业务（phid）点本身包含几个，数量少）
            var list2 = new List<Int64>();
            if (!String.IsNullOrEmpty(phid))
            {
                DataRow[] rows = dt.Select(string.Format("individualinfo_phid ='{0}'", phid));
                foreach (DataRow row in rows)
                {
                    list2.Add(Convert.ToInt64(row["object_id"]));
                }
            }

            //加起来返回
            var listFinish = new List<object>();

            listFinish.Add(new { list1 = list1 });
            listFinish.Add(new { list2 = list2 });

            var jsons = Json(listFinish, JsonRequestBehavior.AllowGet);
            return jsons;

        }
        public string SaveOrg()
        {

            string phid = System.Web.HttpContext.Current.Request.Params["phid"];//phid
            string addOrg = System.Web.HttpContext.Current.Request.Params["addOrg"];//增加
            string delOrg = System.Web.HttpContext.Current.Request.Params["delOrg"];//删除

            //处理新增
            JArray ja = JArray.Parse(addOrg);
            DataTable dtAddOrg = new DataTable();
            dtAddOrg.Columns.Add("phid");
            dtAddOrg.Columns.Add("individualinfo_phid");
            dtAddOrg.Columns.Add("object_id");
            dtAddOrg.Columns.Add("Object_type");
            dtAddOrg.Columns.Add("busphid");

            if (ja.Count > 0)
            {
                for (int i = 0; i < ja.Count; i++)
                {
                    DataRow dr = dtAddOrg.NewRow();                    
                    dr["phid"] = "";
                    dr["individualinfo_phid"] = phid;
                    dr["object_id"] = ja[i]["object_id"].ToString();
                    dr["Object_type"] = 0;
                    dr["busphid"] = ja[i]["busphid"].ToString();
                    dtAddOrg.Rows.Add(dr);
                }
            }


            //处理删除
            JArray ja1 = JArray.Parse(delOrg);
            var listDelOrg = new List<string>();
            if (ja1.Count > 0)
            {
                for (int i = 0; i < ja1.Count; i++)
                {
                    listDelOrg.Add(Convert.ToString(ja1[i]["object_id"]));
                }
            }

            try
            {

                int iret = proxy.SaveOrg(dtAddOrg, listDelOrg, phid);

                ResponseResult result = new ResponseResult();
                if (iret >= 0)
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
            catch (Exception ex)
            {
                ResponseResult result = new ResponseResult();
                result.Status = ResponseStatus.Error;
                result.Msg = ex.Message;
                string response = JsonConvert.SerializeObject(result);
                return response;
                //return "{Status : \"error\",Msg:'" + ex.Message + "'}";
                //throw ex;
            }

        }
        
        #endregion

        #region 升级

        /// <summary>
        /// 自定义界面升级
        /// </summary>
        public ActionResult IndividualUIUpdate()
        {
            this.GridStateIDs = new string[] { "6e1730a4-99e9-40e5-9b14-12701bdb5cc0", "6da9cbb6-0c4c-4b69-9665-8869c7d4148b" };
            string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];
            ViewBag.bustype = bustype;
            return View("IndividualUIUpdate");
        }

        public string GetToCheckList()
        {
            string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];

            DataTable dt = proxy.GetToCheckList(bustype);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }


        /// <summary>
        /// 更新检测
        /// </summary>
        /// <returns></returns>
        public string CheckUIInfo()
        {
            try
            {
                string bustype = System.Web.HttpContext.Current.Request.Params["bustype"];
                string ids = System.Web.HttpContext.Current.Request.Params["ids"];//客户端勾选的数据项

                List<Int64> idList = null;
                string str = proxy.CheckUIInfo(ref idList,bustype,ids);

                JObject data = new JObject { { "idlist",JsonConvert.SerializeObject(idList) }, { "msg", str } };
                JObject jo = new JObject
                {
                  {"Status","success" },
                  {"Data",data}
               };

                return JsonConvert.SerializeObject(jo);
                //return "{Status : \"success\",Data:'" + ex.Message + "'}";
            }
            catch (Exception ex)
            {
                return "{Status : \"error\",Msg:'" + ex.Message + "'}";
            }

        }
        /// <summary>
        /// 升级
        /// </summary>
        /// <returns></returns>
        public string UpdateUIInfo()
        {
            try
            {
                string ids = System.Web.HttpContext.Current.Request.Params["ids"];//               
                string str = proxy.UpdateUIInfo(ids);
           
                JObject jo = new JObject
                {
                  {"Status","success" },
                  {"Data",str}
               };

                return JsonConvert.SerializeObject(jo);
                //return "{Status : \"success\",Data:'" + ex.Message + "'}";
            }
            catch (Exception ex)
            {
                return "{Status : \"error\",Msg:'" + ex.Message + "'}";
            }
        }


        public string CheckSysTemplate()
        {
            try
            {                                
                string str = proxy.CheckSysTemplate();

                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CheckUserTemplate()
        {
            try
            {
                string str = proxy.CheckAllUserTemplate();

                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //脚本同步
        public string SyncScript()
        {
            try
            {
                //string ids = System.Web.HttpContext.Current.Request.Params["ids"];//               
                int count = proxy.SyncScript();

                JObject jo = new JObject
                {
                  {"Status","success" },
                  {"count",count}
               };

                return JsonConvert.SerializeObject(jo);
            }
            catch (Exception ex)
            {
                return "{Status : \"error\",Msg:'" + ex.Message + "'}";
            }
        }


        #endregion
    }
}
