#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Controller
    * 类 名 称：			PaymentMstController
    * 文 件 名：			PaymentMstController.cs
    * 创建时间：			2019/5/15 
    * 作    者：			吾丰明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using Enterprise3.WebApi.ApiControllerBase;
using System.Web.Http;
using System.Linq;
using Enterprise3.Common.Base.Criterion;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Enterprise3.WebApi.ApiControllerBase.Models;
using System.IO;
using Spring.Data.Common;
using GData3.Common.Utils.Filters;
using Newtonsoft.Json.Linq;
using NG3.WorkFlow.Rule;
using NG3.Data.Service;
using System.Data;
using NG3.WorkFlow.Engine.DB;
using NG3.WorkFlow.Engine;
using Enterprise3.WebApi.GWF3.WF.Model.Request;
using Enterprise3.WebApi.GWF3.WF.Model;
using Enterprise3.WebApi.GWF3.WF.Model.common;
using NG3;
using NG3.WorkFlow.Engine.Plugin;
using NG3.WorkFlow.Interfaces;

namespace Enterprise3.WebApi.GWF3.WF.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class WorkFlowApiController : ApiBase
    {
        private IWorkFlowService service;

        /// <summary>
        /// 构造函数
        /// </summary>

        public WorkFlowApiController()
        {
            //获取AppInfo值 头部信息记录
            #region 设置当前线程数据库
            var base64EncodedBytes = Convert.FromBase64String(HttpContext.Current.Request.Headers.GetValues("AppInfo").First());
            var jsonText = Encoding.UTF8.GetString(base64EncodedBytes);
            var AppInfo = JsonConvert.DeserializeObject<ApiControllerBase.Models.AppInfoBase>(jsonText);

            if (AppInfo != null)
            {

                DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
                string result, userConn;
                var pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);
                string defaultConn = dbbuilder.GetDefaultConnString();

                if (AppInfo.DbName.ToLower() == "ngsoft")
                {
                    userConn = pubConn;
                }
                else
                {
                    userConn = string.IsNullOrWhiteSpace(AppInfo.DbName)
                        ? defaultConn
                        : dbbuilder.GetAccConnstringElement(0, AppInfo.DbName, pubConn,
                            out result);
                }

                //设置当前数据库连接信息
                ConnectionInfoService.SetCallContextConnectString(userConn);
                MultiDelegatingDbProvider.CurrentDbProviderName = AppInfo.DbName;

            }
            #endregion

            service = new WorkFlowServiceProxy();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetValue(int id)
        {
            return "value" + id;
        }

        /// <summary>
        /// 审批列表 -- 待办任务
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPendingTaskInstances([FromUri]BaseListModel paramters)
        {

            try
            {
                DbHelper.Open();

                int index = paramters.pageindex;

                int psize = 15;
                if (paramters.pagesize != 0)
                    psize = paramters.pagesize;

                string filter = "";
                if (!string.IsNullOrWhiteSpace(paramters.filter))
                    filter = paramters.filter;

                string sortstr = "";
                if (!string.IsNullOrWhiteSpace(paramters.sortstr))
                    sortstr = paramters.sortstr;

                int rowCount = 0;
                DataTable dt = WorkFlowDac.Instance.GetPendingTaskInstances4MobileApp(paramters.userid, psize, index, ref rowCount, filter, sortstr);


                return DCHelper.ModelListToJson(dt, rowCount, paramters.pageindex, paramters.pagesize);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }

        }


        /// <summary>
        /// 审批列表 -- 【我发起的流程-1 和 已办任务-2】
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMyAppFlowInstances([FromUri]FlowInstanModel paramters)
        {
            try
            {
                DbHelper.Open();

                //我发起的流程-1,已办任务-2
                int myflowtype = 1;
                if (paramters.myflowtype != 0)
                    myflowtype = paramters.myflowtype;

                int index = paramters.pageindex;

                int psize = 15;
                if (paramters.pagesize != 0)
                    psize = paramters.pagesize;

                string filter = "";
                if (!string.IsNullOrWhiteSpace(paramters.filter))
                    filter = paramters.filter;

                string sortstr = string.Empty;
                if (string.IsNullOrWhiteSpace(paramters.sortstr))
                    sortstr = "actdt desc";

                int rowCount = 0;
                DataTable dt = WorkFlowDac.Instance.GetMyAppFlowInstance4MobileApp(paramters.userid, myflowtype, psize, index, ref rowCount, filter, sortstr);

                return DCHelper.ModelListToJson(dt, rowCount, paramters.pageindex, paramters.pagesize);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);

            }
            finally
            {
                DbHelper.Close();
            }
        }

        /// <summary>
        /// 审批流详细请求--待办任务
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="piid">当前流程号</param>
        /// <param name="taskinstid">任务号</param>
        /// <returns></returns>
        [HttpGet]
        public object GetTaskInstanceInfo([FromUri]BaseInfoModel paramters)
        {
            string jsonStr = string.Empty;

            try
            {
                var data = service.GetTaskExecInfo4App(paramters.piid, paramters.taskid, paramters.userid);
                data.Add("Status", "success");

                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowType"></param>
        /// <param name="logid">用户id</param>
        /// <param name="piid">当前流程号</param>
        /// <returns></returns>
        [HttpGet]
        public object GetFlowAllInfo([FromUri]FlowInfoModel paramters)
        {

            try
            {

                string jsonStr = string.Empty;
                object obj = null;

                if (paramters.flowType == "af")
                {
                    DllClassService dcs = new DllClassService("NG.AppFlow.Facade.dll");
                    obj = dcs.Invoke("NG.AppFlow.Facade.AppFlowFacade", "GetFlowAllInfo4MobileApp", new object[] { paramters.userid, paramters.piid });
                }
                else if (paramters.flowType == "oawf")
                {
                    DllClassService dcs = new DllClassService("NG.WorkFlow.Facade.dll");
                    obj = dcs.Invoke("NG.WorkFlow.Facade.WorkflowFacade", "GetFlowAllInfo4MobileApp", new object[] { paramters.userid, paramters.piid });
                }
                else if (paramters.flowType == "psoftwf")
                {
                    DllClassService dcs = new DllClassService("NG.AppFlow.Facade.dll");
                    obj = dcs.Invoke("NG.AppFlow.Facade.PSoftWorkFlowFacade", "GetFlowAllInfo4MobileApp", new object[] { paramters.userid, paramters.piid });
                }
                else if (paramters.flowType == "wf")
                {
                    var data = service.GetFlowInfo4App(paramters.piid, paramters.userid);
                    data.Add("Status", "success");
                    return data;
                }

                if (obj == null || !(obj is List<object>))
                {
                    Exception errorEx = new Exception("取数出错！");
                    jsonStr = AppJsonHelper.ConvertErrorInfoToJsonStr(errorEx);
                }
                else
                {
                    List<object> dts = obj as List<object>;
                    jsonStr = AppJsonHelper.ConvertObjectListToJsonStr(dts);
                }

                return jsonStr;
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetTaskInfoExtend([FromUri]FlowInfoModel paramters)
        {
            string piid = paramters.piid;
            string taskid = paramters.taskid;

            try
            {
                DbHelper.Open();
                var pi = WorkFlowDac.Instance.GetProcInstTraceEntity(piid);
                var tn = WorkFlowDac.Instance.GetRunintTask(taskid);
                JObject o = new JObject();
                o.Add("flowtype", "wf");
                o.Add("biztype", pi.BillInfo.BizID);
                string bizName = BizCfgProviderFactory.GetBizCfgProvider().GetBizInfoNameWithLang(pi.BillInfo.BizID);
                o.Add("bizname", bizName);
                o.Add("piid", piid);
                o.Add("initiator", pi.StartUserId);
                if (tn != null)
                {
                    o.Add("taskinstid", taskid);
                    o.Add("nodeid", tn.TaskDefKey);
                    o.Add("project_name", "");
                    o.Add("taskdesc", pi.FlowDesc);
                }
                else
                {
                    o.Add("keyword", pi.FlowDesc);
                    var dt = WorkFlowDac.Instance.GetTaskInst(taskid);
                    DateTime time = default(DateTime);
                    if (dt.Rows.Count > 0 && dt.Rows[0]["end_time_"] != DBNull.Value)
                    {
                        time = Convert.ToDateTime(dt.Rows[0]["end_time_"]);
                    }
                    o.Add("actdt", time.ToString("yyyy-MM-dd HH:mm"));
                }
                return o;
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }

        }

        #region 办理操作
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramJo"></param>
        /// <returns></returns>
        [HttpPost]
        public object Transmit(JObject paramJo)
        {

            try
            {
                string newusercode = !paramJo["transmituser"].IsNullOrEmpty() ? (string)paramJo["transmituser"] : string.Empty;
                if (string.IsNullOrEmpty(newusercode))
                    throw new Exception("未设置转签人员");

                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                if (string.IsNullOrEmpty(ec.Remark))
                {
                    ec.Remark = "转签";
                }
                ec.IsMobile = true;
                service.ExecuteTaskReassign(ec, newusercode);
                return AppJsonHelper.ConvertResultToJson(true, string.Empty);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public object Approve(JObject paramJo)
        {

            string jsonStr = string.Empty;
            try
            {
                //var bizdata = paramJo["bizdata"].IsNullOrEmpty() == false ? (string)paramJo["bizdata"] : string.Empty;

                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                ec.IsMobile = true;
                if (string.IsNullOrEmpty(ec.Remark))
                {
                    ec.Remark = "同意";
                }
                //service.SaveMobileBizData(ec, bizdata);

                service.ExecuteTaskComplete(ec);
                return AppJsonHelper.ConvertResultToJson(true, string.Empty);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public object RollBack(JObject paramJo)
        {

            try
            {
                //var bizData = paramJo["bizdata"].IsNullOrEmpty() == false ? (string)paramJo["bizdata"] : string.Empty;

                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                if (string.IsNullOrEmpty(ec.RollBackNodeID))
                    throw new Exception("未指定回退节点");
                ec.IsMobile = true;
                if (string.IsNullOrEmpty(ec.Remark))
                {
                    ec.Remark = "不同意";
                }

                //service.SaveMobileBizData(ec, bizData);
                service.ExecuteRollBack(ec);
                return AppJsonHelper.ConvertResultToJson(true, string.Empty);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public object Terminate(JObject paramJo)
        {
            try
            {
                //var bizData = paramJo["bizdata"].IsNullOrEmpty() == false ? (string)paramJo["bizdata"] : string.Empty;
                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                ec.IsMobile = true;
                if (string.IsNullOrEmpty(ec.Remark))
                {
                    ec.Remark = "不同意";
                }

                //service.SaveMobileBizData(ec, bizData);
                service.ExecuteFlowTerminate(ec);
                return AppJsonHelper.ConvertResultToJson(true, string.Empty);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public object AddTis(JObject paramJo)
        {

            try
            {
                List<string> userList = new List<string>();
                //var bizData = paramJo["bizdata"].IsNullOrEmpty() == false ? (string)paramJo["bizdata"] : string.Empty;
                var users = paramJo["users"].IsNullOrEmpty() == false ? (string)paramJo["users"] : string.Empty;
                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                ec.IsMobile = true;
                if (string.IsNullOrEmpty(ec.Remark))
                {
                    ec.Remark = "加签";
                }
                string[] str = users.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                userList.AddRange(str);

                //service.SaveMobileBizData(ec, bizData);
                service.ExecuteAddSubTasks(ec, userList);
                return AppJsonHelper.ConvertResultToJson(true, string.Empty);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        public object CheckApproveValid()
        {
            JObject rst = new JObject();
            string taskid = System.Web.HttpContext.Current.Request.Params["taskid"] ?? string.Empty;
            string piid = System.Web.HttpContext.Current.Request.Params["piid"] ?? string.Empty;
            try
            {
                NG3.WorkFlow.Engine.Models.ProcInstTraceEntity piEntity;
                try
                {
                    DbHelper.Open();
                    piEntity = WorkFlowDac.Instance.GetProcInstTraceEntity(piid);
                }
                finally
                {
                    DbHelper.Close();
                }
                WorkFlowExecutionContext ec = new WorkFlowExecutionContext(piEntity.BillInfo);
                ec.PiId = piid;
                ec.TaskId = taskid;
                ec.IsMobile = true;
                ApproveValidResult r = service.CheckApproveValid(ec);
                JObject data = new JObject();
                data.Add("result", (int)r.ValidResult);
                data.Add("msg", r.Msg);

                rst.Add("data", data);
                rst.Add("success", true);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            return rst;
        }

        public object CheckCancelApproveValid()
        {
            JObject rst = new JObject();
            string taskid = System.Web.HttpContext.Current.Request.Params["taskid"] ?? string.Empty;
            string piid = System.Web.HttpContext.Current.Request.Params["piid"] ?? string.Empty;
            try
            {
                NG3.WorkFlow.Engine.Models.ProcInstTraceEntity piEntity;
                try
                {
                    DbHelper.Open();
                    piEntity = WorkFlowDac.Instance.GetProcInstTraceEntity(piid);
                }
                finally
                {
                    DbHelper.Close();
                }

                WorkFlowExecutionContext ec = new WorkFlowExecutionContext(piEntity.BillInfo);
                ec.TaskId = taskid;
                ec.PiId = piid;
                ec.IsMobile = true;
                ApproveValidResult r = service.CheckCancelApproveValid(ec);
                rst.Add("result", (int)r.ValidResult);
                rst.Add("msg", r.Msg);
                rst.Add("success", true);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            return rst;
        }

        [HttpPost]
        public object CheckBizSaveByMobileApp(JObject paramJo)
        {

            string jsonStr = string.Empty;
            try
            {
                var bizdata = paramJo["bizdata"].IsNullOrEmpty() == false ? (string)paramJo["bizdata"] : string.Empty;
                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                ec.IsMobile = true;
                var resp = service.CheckBizSaveByMobileApp(ec, bizdata);
                string status = "ok";
                if (resp.ValidResult == ApproveValidType.No)
                {
                    status = "no";
                }
                else if (resp.ValidResult == ApproveValidType.Clew)
                {
                    status = "clew";
                }
                JObject data = new JObject();
                data.Add("status", status);
                data.Add("msg", resp.Msg);
                return data;
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public object SaveBizDataByMobillApp(JObject paramJo)
        {

            string jsonStr = string.Empty;
            try
            {
                var bizdata = paramJo["bizdata"].IsNullOrEmpty() == false ? (string)paramJo["bizdata"] : string.Empty;

                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                ec.IsMobile = true;
                service.SaveMobileBizData(ec, bizdata);
                return AppJsonHelper.ConvertResultToJson(true, string.Empty);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetProcessDefinitions([FromUri]ProcessDefModel paramters)
        {
            try
            {
                string billtype = paramters.billtype ?? string.Empty;
                string billcode = paramters.billcode ?? string.Empty;
                string orgid = paramters.orgid ?? string.Empty;


                WorkFlowBizInfo biz = new WorkFlowBizInfo(billtype, billcode, orgid, string.Empty);
                return service.GetProcessDefinitionByBizInfo(biz, paramters.userid);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetDesignateInfo([FromUri]ProcessDefModel paramters)
        {
            try
            {
                string billtype = paramters.billtype ?? string.Empty;
                string billcode = paramters.billcode ?? string.Empty;
                string pdid = paramters.pdid ?? string.Empty;

                WorkFlowBizInfo biz = new WorkFlowBizInfo(billtype, billcode);
                WorkFlowExecutionContext ec = new WorkFlowExecutionContext(biz);
                ec.PdId = pdid;
                ec.UserId = paramters.userid;
                ec.IsMobile = true;
                return service.GetDynamicInfo(ec, true);
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramJo"></param>
        /// <returns></returns>
        [HttpPost]
        public object CreateProcessInstance(JObject paramJo)
        {
            JObject rst = new JObject();
            try
            {
                string flowdesc = paramJo["flowdesc"].IsNullOrEmpty() ? string.Empty : (string)paramJo["flowdesc"];
                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.LoadFormJson(paramJo);
                ec.IsMobile = true;
                string orgId = paramJo["orgId"].IsNullOrEmpty() ? string.Empty : (string)paramJo["orgId"];
                if (!string.IsNullOrEmpty(orgId))
                {
                    ec.OrgId = orgId;
                }
                string piid = service.CreateProcessInstance(ec, 50, flowdesc);
                rst.Add("success", true);
                rst.Add("piid", piid);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            return rst;
        }


        [HttpGet]
        public object GetAllSignature([FromUri]BaseInfoModel paramters)
        {
            try
            {
                string jsonStr = string.Empty;
                DbHelper.Open();
                DataTable dt = SignatureDac.Instance.GetSignatureInsureFile(paramters.userid);
                return AppJsonHelper.ConvertDtToJson(dt);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }
        }

        [HttpGet]
        public object GetAllUser([FromUri]BaseListModel paramters)
        {
            try
            {
                string jsonStr = string.Empty;
                DbHelper.Open();
                int rowCount = 0;

                DataTable dt = UserOrgIntegrationDac.Instance.SearchUsersByPageIndex(string.Empty, paramters.filter, paramters.pageindex.ToString(), paramters.pagesize.ToString(), ref rowCount);
                dt.Columns["phid"].ColumnName = "userId";
                dt.Columns["userno"].ColumnName = "userNo";
                dt.Columns["username"].ColumnName = "userName";
                dt.Columns["deptname"].ColumnName = "deptName";
                return AppJsonHelper.ConvertDtToJson(dt, "data", rowCount);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }
        }

        [HttpGet]
        public object GetRollBackInfo([FromUri]BaseInfoModel paramters)
        {
            string jsonStr = string.Empty;
            //List<DataTable> rollBackInfoDtList = new List<DataTable>();
            try
            {

                DataTable nextNodes = new DataTable("rollBackNodes");
                nextNodes.Columns.Add("nodeid");
                nextNodes.Columns.Add("nodetext");
                nextNodes.Columns.Add("designate_actor", typeof(int));
                nextNodes.Columns.Add("designate_anyactor", typeof(int));

                JArray ja = service.GetRollBackNode(paramters.taskid, paramters.userid, string.Empty);
                foreach (JObject o in ja)
                {
                    DataRow dr = nextNodes.NewRow();
                    dr["nodeid"] = o["id"].Value<string>();
                    dr["nodetext"] = o["name"].Value<string>();
                    dr["designate_actor"] = 0;
                    dr["designate_anyactor"] = 0;
                    nextNodes.Rows.Add(dr);
                }
                return AppJsonHelper.ConvertDtToJson(nextNodes, "rollBackNodes");
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpGet]
        public object GetTaskBizContent([FromUri]BaseInfoModel paramters)
        {
            object obj = null;
            try
            {
                if (paramters.flowType == "af")
                {
                    DllClassService dcs = new DllClassService("NG.AppFlow.Facade");
                    obj = dcs.Invoke("NG.AppFlow.Facade.AppFlowFacade", "GetBizContent4MobileApp", new object[] { paramters.piid });

                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append("{");
                    //sb.Append(string.Format("\"data\":\"{0}\"", obj.ToString())).Append(",");
                    //sb.Append("\"name\":\"\",");
                    //sb.Append("\"type\":\"bytes\",");
                    //sb.Append("\"status\":\"succeed\",");
                    //sb.Append("\"errmsg\":\"\"}");

                    JObject data = new JObject();
                    data.Add("data", obj.ToString());
                    data.Add("name", "");
                    data.Add("type", "bytes");
                    data.Add("status", "succeed");
                    return data;
                }
                else if (paramters.flowType == "oawf")
                {
                    DllClassService dcs = new DllClassService("NG.WorkFlow.Facade.dll");
                    obj = dcs.Invoke("NG.WorkFlow.Facade.WorkflowFacade", "GetBizContentUrl4MobileApp", new object[] { paramters.piid });

                    string[] ss = (obj.ToString()).Split(new string[] { "@@**" }, StringSplitOptions.None);
                    string url = ss.Length > 0 ? ss[0] : string.Empty;
                    string name = ss.Length > 1 ? ss[1] : string.Empty;

                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append("{");
                    //sb.Append(string.Format("\"data\":\"{0}\"", url)).Append(",");
                    //sb.Append(string.Format("\"name\":\"{0}\"", name)).Append(",");
                    //sb.Append("\"type\":\"URL\",");
                    //sb.Append("\"status\":\"succeed\",");
                    //sb.Append("\"errmsg\":\"\"}");
                    //jsonStr = sb.ToString();

                    JObject data = new JObject();
                    data.Add("data", url);
                    data.Add("name", name);
                    data.Add("type", "URL");
                    data.Add("status", "succeed");
                    return data;
                }
                else if (paramters.flowType == "psoftwf")
                {
                    DllClassService dcs = new DllClassService("NG.AppFlow.Facade");
                    obj = dcs.Invoke("NG.AppFlow.Facade.PSoftWorkFlowFacade", "GetBizContent4MobileApp", new object[] { paramters.piid, string.Empty });

                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append("{");
                    //sb.Append(string.Format("\"data\":\"{0}\"", obj.ToString())).Append(",");
                    //sb.Append("\"name\":\"\",");
                    //sb.Append("\"type\":\"bytes\",");
                    //sb.Append("\"status\":\"succeed\",");
                    //sb.Append("\"errmsg\":\"\"}");
                    //jsonStr = sb.ToString();

                    JObject data = new JObject();
                    data.Add("data", obj.ToString());
                    data.Add("name", "");
                    data.Add("type", "bytes");
                    data.Add("status", "succeed");
                    return data;
                }
                else if (paramters.flowType == "wf")
                {
                    try
                    {
                        DbHelper.Open();
                        string fileName = string.Empty;
                        string url = WorkFlowEngine.Instance.GetBizPdf(paramters.piid, out fileName);
                        JObject data = new JObject();
                        data.Add("data", url);
                        data.Add("name", fileName);
                        data.Add("type", "URL");
                        data.Add("status", "succeed");
                        return data;
                    }
                    finally
                    {
                        DbHelper.Close();
                    }
                }
                else
                {
                    throw new Exception("获取正文出错");
                }
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpGet]
        public object GetBizFiledHelpData([FromUri]BaseListModel paramters)
        {

            try
            {
                DbHelper.Open();


                int rowCount = 0;
                DataTable dt = WorkFlowDac.Instance.GetMobileBizFiledHelpData(paramters.sqlstr, paramters.pagesize, paramters.pageindex, ref rowCount, paramters.filter);

                return AppJsonHelper.ConvertDtToJson(dt, "data", rowCount);

            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }
        }

        [HttpPost]
        public object SaveAudioRemark()
        {

            var piid = Pub.Request("piid");
            var taskinstid = Pub.Request("taskinstid");

            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            string gudi = string.Empty;
            try
            {
                if (files.Count > 0)
                {
                    using (Stream sr = files[0].InputStream)
                    {
                        byte[] bb = new byte[sr.Length];
                        sr.Read(bb, 0, bb.Length);
                        sr.Close();

                        gudi = WorkFlowEngine.Instance.SaveTaskAudioRemark(taskinstid, bb);
                    }
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

            JObject data = new JObject();
            data.Add("guid", gudi);
            data.Add("status", string.IsNullOrEmpty(gudi) ? "error" : "succeed");
            return data;
        }

        [HttpPost]
        public object AddTaskHisAttachment()
        {
            //LogHelper<WorkFlowAppController>.Error("AddTaskHisAttachment");
            JObject data = new JObject();
            //var taskinstid = paramJo["taskinstid"].IsNullOrEmpty() == false ? (string)paramJo["taskinstid"] : string.Empty;
            //var filename = paramJo["filename"].IsNullOrEmpty() == false ? (string)paramJo["filename"] : string.Empty;
            var taskinstid = Pub.Request("taskinstid");
            var filename = Pub.Request("filename");
            var asr_fid = Pub.Request("asr_fid");
            var guid = Pub.Request("asr_guid");
            var mediaid = Pub.Request("mediaid");
            //LogHelper<WorkFlowAppController>.Error("AddTaskHisAttachment:"+taskinstid+","+filename+","+guid); 
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

            string errmsg = string.Empty;
            try
            {
                DbHelper.Open();
                byte[] bb = null;
                if (!string.IsNullOrWhiteSpace(asr_fid))
                {
                    bb = NG3.WorkFlow.Engine.WorkFlowEngine.Instance.GetAttachmentByAsrFid(asr_fid);
                }
                else if (!string.IsNullOrWhiteSpace(mediaid))
                {
                    var url = string.Format("API/AppServer/Get?action=getattach&mediaid={0}&moduleno=MEA&ucode={1}",
                        mediaid, EnvironmentHelp.GetDatabase(string.Empty).Replace("NG", ""));
                    HttpClient client = EnvironmentHelp.GetHttpClientInst();
                    var resp = client.GetAsync(url).Result;
                    resp.EnsureSuccessStatusCode();
                    var respContent = resp.Content.ReadAsStringAsync().Result;
                    //LogHelper<WorkFlowAppController>.Error(string.Format("获取微信附件正文{0},url：{1}", respContent, url));
                    JObject o = JObject.Parse(respContent);
                    if (o["success"].GetStringValue() == "true" && !string.IsNullOrWhiteSpace(o["file"].GetStringValue()))
                    {
                        bb = System.Convert.FromBase64String(o["file"].GetStringValue());
                    }
                    if (string.IsNullOrWhiteSpace(filename))
                    {
                        filename = Guid.NewGuid().ToString() + o["filetype"].GetStringValue();
                    }
                    else if (!filename.Contains("."))
                    {
                        filename += o["filetype"].GetStringValue();
                    }
                }
                else if (files != null && files.Count > 0)
                {
                    if (string.IsNullOrEmpty(filename))
                    {
                        filename = files[0].FileName;
                    }
                    using (Stream sr = files[0].InputStream)
                    {
                        bb = new byte[sr.Length];
                        sr.Read(bb, 0, bb.Length);
                        sr.Close();
                    }
                }

                if (string.IsNullOrWhiteSpace(filename) || bb.Length < 1)
                {
                    errmsg = "文件名为空或未上传正文！";
                }
                guid = NG3.WorkFlow.Engine.WorkFlowEngine.Instance.AddTaskHisAttachment(guid, taskinstid, filename, bb);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }

            data.Add("guid", guid);
            data.Add("errmsg", errmsg);
            data.Add("status", string.IsNullOrEmpty(guid) ? "error" : "succeed");
            return data;
        }

        [HttpPost]
        public object DelTaskHisAttachment(JObject paramJo)
        {
            JObject data = new JObject();
            //var piid = paramJo["piid"].IsNullOrEmpty() == false ? (string)paramJo["piid"] : string.Empty;
            var taskinstid = paramJo["taskinstid"].IsNullOrEmpty() == false ? (string)paramJo["taskinstid"] : string.Empty;
            var filename = paramJo["filename"].IsNullOrEmpty() == false ? (string)paramJo["filename"] : string.Empty;
            var guid = paramJo["asr_guid"].IsNullOrEmpty() == false ? (string)paramJo["asr_guid"] : string.Empty;
            try
            {
                DbHelper.Open();
                if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(filename))
                {
                    throw new Exception("参数错误");
                }
                WorkFlowEngine.Instance.DelTaskHisAttachment(guid, filename);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }
            data.Add("errmsg", string.Empty);
            data.Add("status", "succeed");
            return data;
        }

        [HttpGet]
        public object GetTaskHisAttachment()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                DbHelper.Open();
                string piid = HttpContext.Current.Request.Params["piid"] ?? string.Empty;
                string taskid = HttpContext.Current.Request.Params["taskid"] ?? string.Empty;
                var attList = NG3.WorkFlow.Engine.WorkFlowEngine.Instance.GetTaskHisAttachment(taskid);

                data.Add("data", attList);
                data.Add("status", "succeed");
                return data;
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
            finally
            {
                DbHelper.Close();
            }
        }

        [HttpGet]
        public object GetAudioRemark()
        {
            string phid = Pub.Request("audioremark");
            string errmsg = string.Empty;
            string url = string.Empty;
            try
            {
                try
                {
                    DbHelper.Open();
                    url = NG3.WorkFlow.Engine.WorkFlowEngine.Instance.GetTaskAudioRemark(phid);
                }
                finally
                {
                    DbHelper.Close();
                }
                if (string.IsNullOrEmpty(url))
                {
                    DllClassService dcs = new DllClassService("NG.AppFlow.Facade");
                    object obj = dcs.Invoke("NG.AppFlow.Facade.AppFlowFacade", "GetAudioRemark", new object[] { phid });

                    if (obj != null)
                        url = obj.ToString();
                }
                JObject data = new JObject();
                data.Add("url", url);
                data.Add("status", string.IsNullOrEmpty(url) ? "error" : "succeed");
                return data;
            }
            catch (System.Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpGet]
        public object CanDrawback()
        {
            try
            {
                string piid = System.Web.HttpContext.Current.Request.Params["piid"] ?? string.Empty;
                string errorMsg = string.Empty;
                bool temp = service.CanTaskRetrieve(piid, NG3.AppInfoBase.UserID.ToString(), out errorMsg);
                return new JObject() { { "candrawback", temp }, { "errorMsg", errorMsg } };
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public object Drawback()
        {
            try
            {
                bool temp = false;
                string piid = System.Web.HttpContext.Current.Request.Params["piid"] ?? string.Empty;
                WorkFlowExecutionContext ec = new WorkFlowExecutionContext();
                ec.PiId = piid;
                ec.UserId = NG3.AppInfoBase.UserID.ToString();
                service.ExecuteTaskRetrieve(ec, ec.UserId);
                temp = true;
                return new JObject() { { "result", temp } };
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        public object SaveAttachment()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            string AsrAttachTable = Pub.Request("asratttable");
            var AsrTable = Pub.Request("asrtable");
            var asrCode = Pub.Request("asrcode");
            var AttachName = Pub.Request("attachname");
            var logid = Pub.Request("logid");

            string errmsg = string.Empty;
            try
            {
                if (files.Count > 0)
                {
                    Stream sr = files[0].InputStream;
                    byte[] bb = new byte[sr.Length];
                    sr.Read(bb, 0, bb.Length);
                    sr.Close();

                    DllClassService dcs = new DllClassService("NG.AppFlow.Facade");
                    object obj = dcs.Invoke("NG.AppFlow.Facade.AppFlowFacade", "SaveAttachment", new object[] { AsrAttachTable, AsrTable, asrCode, AttachName, logid, bb });

                    if (obj != null)
                        errmsg = obj.ToString();
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

            JObject data = new JObject();
            data.Add("errmsg", errmsg);
            data.Add("status", errmsg != "1" ? "error" : "succeed");
            return data;
        }

        [HttpGet]
        public object GetProcessInstanceDiagramLayout()
        {
            string piid = Pub.Request("piid");
            var data = NG3.WorkFlow.Engine.WebApi.WorkFlowRestApiHelp.GetProcessInstanceDiagramLayout(piid);
            return JObject.Parse(data); //Newtonsoft.Json.JsonConvert(data);
        }

        [HttpGet]
        public object GetProcessInstanceDiagramhighlights()
        {
            string piid = Pub.Request("piid");
            var data = NG3.WorkFlow.Engine.WebApi.WorkFlowRestApiHelp.GetProcessInstanceDiagramhighlights(piid);
            return JObject.Parse(data);
        }


    }
}
