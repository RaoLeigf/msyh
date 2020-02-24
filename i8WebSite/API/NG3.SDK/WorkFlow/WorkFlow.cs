using System;
using System.Collections.Generic;
using System.Web;

namespace NG3.SDK
{
    public class WorkFlow
    {
        /// <summary>
        /// 获取流程定义列表
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">业务参数:userId={$}&start={$}&size={$}&sort={$}&order={$}</param>
        /// <returns></returns>
        public static string ProcessDefinitions(Builder builder, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + "process-definitions?" + values);
        }

        /// <summary>
        /// 获取某执行人,候选人或候选组的任务列表
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="values">业务参数:[assignee={$}|candidate={$}|candidate-group={$}]&start={$}&size={$}&sort={$}&order={$}&order={$}&userId={$}</param>
        /// <returns></returns>
        public static string Tasks(Builder builder, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + "tasks?" + values);
        }
         
        /// <summary>
        /// 执行任务操作（包括申领，取消申领，和完成任务操作）
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="taskid">任务id</param>
        /// <param name="operation">操作类型:claim,unclaim,complete</param>
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string Task(Builder builder,string userid, string taskid,string operation,string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPut(builder, new APIConfig().WorkFlowServer + "task/" + taskid + "/" + operation + "?userId=" + userid, values);
        }

        /// <summary>
        /// 执行任务委托或转签（operation=delegation（委托），operation=reassign（转签））
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="taskid">任务id</param>
        /// <param name="operation">operation=delegation（委托），operation=reassign（转签）</param>
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string TaskTrust(Builder builder, string userid, string taskid, string operation, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPut(builder, new APIConfig().WorkFlowServer + "senior/" + taskid + "/" + operation + "?userId=" + userid, values);
        }

 /// <summary>
        /// 对某个任务实现自由流跳转（包括后退）
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="taskid">任务id</param>
        /// <param name="operation">"before" 或 "after"</param>
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string TaskTransfer(Builder builder, string userid, string taskid, string destTaskId, string operation, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPut(builder, new APIConfig().WorkFlowServer + "senior/" + taskid + "/" + destTaskId + "/" + operation + "?userId=" + userid, values);
        }

        /// <summary>
        /// 流程实例控制（删除，挂起与激活）
        /// </summary>
        /// <param name="builder">系统参数</param>
        /// <param name="processInstanceId">流程实例ID</param>
        /// <param name="operation">delete suspend active三选一</param>
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string FlowInstanceControl(Builder builder, string processInstanceId, string operation, string userid, string values, string cascade)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPut(builder, new APIConfig().WorkFlowServer + "senior/" + processInstanceId + "/processInstance/" + operation + "?userId=" + userid + "&cascade=" + cascade, values);
        }
/// <summary>
        /// 流程定义控制（挂起与激活）
        /// </summary>变量{"key":"$"}</param>
        /// <returns></return
        /// <param name="builder">系统参数</param>
        /// <param name="processDefinitionId">流程实例ID</param>
        /// <param name="operation">delete suspend active三选一</param>
        /// <param name="values">业务参数:流程s>
        public static string FlowDefinitionControl(Builder builder, string processDefinitionId, string operation, string userid, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPut(builder, new APIConfig().WorkFlowServer + "senior/" + processDefinitionId + "/processDefinition/" + operation + "?userId=" + userid, values);
        }
        /// <summary>
        /// 开始一个流程
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string ProcessInstance(Builder builder,string values,string userid)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoPost(builder, new APIConfig().WorkFlowServer + "process-instance?userId=" + userid, values);
        }

        /// <summary>
        /// 获取流程实例详情,流程变量,任务信息等
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="processInstanceId">流程定义Id</param>
        /// <returns></returns>
        public static string GetProcessInstance(Builder builder, string processInstanceId, string userid)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + "process-instance/" + processInstanceId + "?userId=" + userid);
        }
        /// <summary>
        /// 获取流程变量
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="processInstanceId">流程定义Id</param>
        /// <returns></returns>
        public static string GetVariableValue(Builder builder, string processInstanceId, string taskId, string varNames, string userId)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("senior/getVariableValue?processInstanceId={0}&taskId={1}&varNames={2}&userId={3}", processInstanceId, taskId, varNames, userId);
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }

        /// <summary>
        /// 获取某个流程任务的前驱节点列表或后驱节点列表
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="taskId">任务Id</param>
        /// <param name="direction">"before" 或 "after "</param>
        /// <returns></returns>
        public static string GetDrivePoint(Builder builder,  string taskId, string direction, string userId)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("senior/{0}/{2}?userId={1}", taskId, userId, direction);
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }
        /// <summary>
        /// 任务评论获取
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="taskId">任务Id</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public static string GetComment(Builder builder, string taskId, string userId)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("comment/{0}?userId={1}", taskId, userId);
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }

        /// <summary>
        /// 任务评论新增
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="taskId">任务Id</param>
        /// <param name="userId">userId</param>
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string AddComment(Builder builder, string taskId, string userId, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0"; 

            string url = string.Format("comment/{0}?userId={1}", taskId, userId);
            return HttpClientHelp.DoPost(builder, new APIConfig().WorkFlowServer + url, values);
        }

        /// <summary>
        /// 获取活动任务id和任务名
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="processInstanceId">流程实例ID</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public static string GetActiveInfo(Builder builder, string processInstanceId, string userId)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("senior/getActiveActivities/{0}?userId={1}",processInstanceId, userId);
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }

        /// <summary>
        /// 流程任务历史获取
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="processInstanceId">流程实例ID</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public static string GetTaskHistory(Builder builder, string processInstanceId, string userId)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("process-instance/{0}/historyTasks?userId={1}", processInstanceId, userId);
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }
        
        /// <summary>
        /// 获取下一个人工任务的信息
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="taskId">任务ID</param>
        /// <param name="processDefinitionId">流程定义ID</param>
        /// <param name="nextActivityId">待跳转节点的ID</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public static string GetNextTaskInfo(Builder builder, string taskId, string processDefinitionId, string nextActivityId, string userId, string isExcludeProcessVar)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("senior/taskInfor?taskId={0}&processDefinitionId={1}&nextActivityId={2}&userId={3}&isExcludeProcessVar={4}", taskId, processDefinitionId, nextActivityId, userId, isExcludeProcessVar);
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }

        /// <summary>
        /// 获取后续节点的信息
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="taskId">任务ID</param>
        /// <param name="processDefinitionId">流程定义ID</param>
        /// <param name="nextActivityId">待跳转节点的ID</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public static string GetNextTaskInfos(Builder builder, string taskId, string processDefinitionId, string userId,string isExcludeProcessVar,string prms)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss"); 
            builder.SecurityEntity.Version = "1.0";

            if (prms != null && prms.Length > 0) prms = "&" + prms;
            string url = string.Format("senior/taskInfors?taskId={0}&processDefinitionId={1}&userId={2}&isExcludeProcessVar={3}{4}", taskId, processDefinitionId, userId, isExcludeProcessVar, prms);
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }

        /// <summary>
        /// 任务动态选人人员获取
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="definitionId">流程定义Id</param>
        /// <param name="任务Id">任务Id</param>
        /// <param name="userId">userId</param>
        /// <param name="activityId">活动Id</param>
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string GetTaskEmpDynamicChoose(Builder builder, string definitionId, string taskId, string activityId, string userId, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("dynamic/choose/{0}/{1}/{2}?userId={3}", taskId,definitionId,activityId, userId);
            return HttpClientHelp.DoPost(builder, new APIConfig().WorkFlowServer + url, values);
        }

        /// <summary>
        /// 任务动态选人
        /// </summary>
        /// <param name="builder">系统参数</param>       
        /// <param name="definitionId">流程定义Id</param>
        /// <param name="任务Id">任务Id</param>
        /// <param name="userId">userId</param>
        /// <param name="activityId">活动Id</param>
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string TaskEmpDynamicChoose(Builder builder, string definitionId, string taskId, string activityId, string userId, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("dynamic/select/{0}/{1}/{2}?userId={3}", taskId, definitionId, activityId, userId);
            return HttpClientHelp.DoPut(builder, new APIConfig().WorkFlowServer + url, values); 
        }

        /// <summary>
        /// 获取设置好的数据库链接
        /// </summary>
        /// <param name="builder">系统参数</param> 
        /// <returns></returns>
        public static string GetWrokFlowDB(Builder builder)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("activiti-explorer2/service/config/db");
            return HttpClientHelp.DoGet(builder, new APIConfig().WorkFlowServer + url);
        }

        /// <summary>
        /// 设置数据库链接
        /// </summary>
        /// <param name="builder">系统参数</param>  
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string SetWrokFlowDB(Builder builder, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0";

            string url = string.Format("activiti-explorer2/service/config/db");
            return HttpClientHelp.DoPost(builder, new APIConfig().WorkFlowServer + url, values);
        }
        
        /// <summary>
        /// 传阅任务增加
        /// </summary>
        /// <param name="builder">系统参数</param>  
        /// <param name="values">业务参数:流程变量{"key":"$"}</param>
        /// <returns></returns>
        public static string ReadPass(Builder builder, string userId, string values)
        {
            builder.SecurityEntity.Join = false;
            builder.SecurityEntity.Format = "json";
            builder.SecurityEntity.TimeStamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            builder.SecurityEntity.Version = "1.0"; 

            string url = string.Format("senior/readTask?userId={0}",  userId);
            return HttpClientHelp.DoPost(builder, new APIConfig().WorkFlowServer + url, values);
        }
    }
}
