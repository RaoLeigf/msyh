using System;
using Quartz;

using Enterprise3.WebApi.Client.Models;
using Enterprise3.WebApi.Client;
using Enterprise3.WebApi.Client.Enums;

namespace RefreshJobSchedule
{
    public class RefreshGKPaymentDataAllJob:IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录
        Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {

            //string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            //string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            //string configFilePath = assemblyDirPath + "\\Configs\\log4net.config";
            //XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));
            

            try
            {
                AppInfoBase appInfo;

                string url = string.Empty;
                string server = context.JobDetail.JobDataMap.Get("Server").ToString();
                string port = context.JobDetail.JobDataMap.Get("Port").ToString();
                string dir = context.JobDetail.JobDataMap.Get("Dir").ToString();

                if (string.IsNullOrEmpty(server)) {
                    server = "127.0.0.1";
                }

                if (string.IsNullOrEmpty(port)) {
                    port = "80";
                }

                if (!string.IsNullOrEmpty(dir))
                {
                    url = "http://" + server + ":" + port + "/" + dir + "/";
                }
                else
                {
                    url = "http://" + server + ":" + port + "/";
                }

                string appKey = context.JobDetail.JobDataMap.Get("AppKey").ToString();
                string appSecret = context.JobDetail.JobDataMap.Get("AppSecret").ToString();

                string dbname = context.JobDetail.JobDataMap.Get("DBName").ToString();
                string userId = context.JobDetail.JobDataMap.Get("UserId").ToString();
                string orgId = context.JobDetail.JobDataMap.Get("OrgId").ToString();

                long UserID = 0;
                if (!string.IsNullOrEmpty(userId)) {
                    UserID = long.Parse(userId);
                }

                long OrgID = 0;
                if (!string.IsNullOrEmpty(orgId))
                {
                    OrgID = long.Parse(orgId);
                }

                appInfo = new AppInfoBase
                {
                    AppKey = appKey, //必须
                    AppSecret = appSecret, //必须
                    DbName = dbname, //可不传，默认为默认账套
                    UserId = UserID,
                    OrgId = OrgID

                    //DbServerName = "10.0.15.106",
                    //OCode = "100",
                    //OrgName = "广东省总工会",
                    //SessionKey = "会话标识",
                    //TokenKey = string.Empty,
                    //UName = "帐套名称",
                    //UserKey = "003",
                    //UserName = "省总操作员"
                };

                IScheduler scheduler = (IScheduler)context.JobDetail.JobDataMap.Get("scheduler");
                logger.Info("刷新支付数据数据......开始");

                WebApiClient client = new WebApiClient(url, appInfo, EnumDataFormat.Json);
                var res = client.Post("api/GGK/GKPaymentMstApi/PostRefreshAllPaymentsState", "");

                logger.Info("刷新支付数据数据......完毕!");
            }
            catch (Exception ex)
            {
                logger.Error("RefreshGKPaymentDataJob 运行异常", ex);
            }
        }      
        
    }
}
