using Common.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Quartz.Impl.Triggers;
using NG3.Data.Service;


namespace RefreshJobSchedule
{
    public partial class WinRefreshDataService : ServiceBase
    {
        private readonly ILog logger;
        private IScheduler scheduler;   //调度器

        private string connectString = ConfigurationManager.ConnectionStrings["DMPDbContext"].ConnectionString;
        private string userConnectString = ""; //ConfigurationManager.ConnectionStrings["G6DbContext"].ConnectionString;
        private static string cornAddWarningRules = ConfigurationManager.AppSettings["cronAddRefreshJob"];  //配置文件设置
        private string userDbTran = "";

        private string CronStr = ConfigurationManager.AppSettings["cronAddRefreshJob"];

        private string Server = ConfigurationManager.AppSettings["Server"];
        private string Port = ConfigurationManager.AppSettings["Port"];
        private string Dir = ConfigurationManager.AppSettings["Dir"];

        private string DBName = ConfigurationManager.AppSettings["DBName"];
        private string UserId = ConfigurationManager.AppSettings["UserId"];
        private string OrgId = ConfigurationManager.AppSettings["OrgId"];

        private string AppKey = ConfigurationManager.AppSettings["AppKey"];
        private string AppSecret = ConfigurationManager.AppSettings["AppSecret"];

        /// <summary>
        /// 构造函数
        /// </summary>
        public WinRefreshDataService()
        {
            InitializeComponent();

            //初始化
            logger = LogManager.GetLogger(this.GetType());
            //新建一个调度器工工厂
            ISchedulerFactory factory = new StdSchedulerFactory();
            //使用工厂生成一个调度器
            scheduler = factory.GetScheduler();

            /*
            DbHelper.Open(connectString);
            DataSet ds = DbHelper.GetDataSet(connectString, "select f_type,f_database,f_userid,f_connection,f_account,f_year,sysno,f_timingrules from sys_setting");

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["f_type"].ToString() == "oracle")
                {
                    userDbTran = ds.Tables[0].Rows[0]["f_userid"].ToString();
                }
                else
                {
                    userDbTran = ds.Tables[0].Rows[0]["f_database"].ToString();
                }
                userConnectString = ds.Tables[0].Rows[0]["f_connection"].ToString();
            }
            else
            {
                return;
            }
            */

            //Test();
            //StartRefreshService4All();
        }

        /// <summary>
        /// 重写 启动
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            StartRefreshService4All();
        }

        /// <summary>
        /// 重写 停止
        /// </summary>
        protected override void OnStop()
        {
            if (!scheduler.IsShutdown)
            {
                scheduler.Shutdown();
            }
            logger.Info("刷新数据服务成功终止");
        }

        /// <summary>
        /// 重写 暂停
        /// </summary>
        protected override void OnPause()
        {
            scheduler.PauseAll();
            base.OnPause();
            logger.Info("刷新服务成功暂停");
        }
        /// <summary>
        /// 重写继续
        /// </summary>
        protected override void OnContinue()
        {
            scheduler.ResumeAll();
            base.OnContinue();
            logger.Info("刷新服务成功继续");
        }

        private void Test()
        {
            //启动调度器
            scheduler.Start();

            // cornStr = "0 15 10 * * ?"  //每天上午10:15:00触发

            /*
            IRulesMstRepository serviceRulesMst = new RulesMstRepository();
            List<RulesMstEntity> rulesMstList = serviceRulesMst.FindList("select * from dmp_rules_mst where use_flag=1 order by seqno;");
            DateTime dtNow = DateTime.Now;
            DateTimeOffset utcTime = dtNow;
            DateTime dt = dtNow;
            string cornStr = "{0} {1} {2} * * ?"; //{0}秒 {1}分 {2}时
            string cornTemp = "";

            if (rulesMstList.Count > 0)
            {
                int i = 0;
                foreach (RulesMstEntity mst in rulesMstList)
                {
                    //dt = mst.running_time.ToDate();
                    cornTemp = string.Format(cornStr,dt.Second.ToString(), dt.Minute.ToString(), dt.Hour.ToString());

                    i++;
                    IJobDetail job1 = JobBuilder.Create<GetSuspectDataJob>()
                        .WithIdentity("Job" + i.ToString(), "Group" + i.ToString())
                        .UsingJobData("rulesmst_phid", mst.phid.ToString())
                        .UsingJobData("ruletypes_phid", mst.phid_ruletypes.ToString())
                        .UsingJobData("rule_code", mst.rule_code)
                        .UsingJobData("rule_name", mst.rule_name)
                        .UsingJobData("userConnectString", userConnectString)
                        .UsingJobData("userDbTran", userDbTran)
                        .Build();
                    ITrigger trigger1 = TriggerBuilder.Create()
                             .WithIdentity("Trigger" + i.ToString(), "Group" + i.ToString())
                             .StartAt(utcTime)
                             .WithCronSchedule(cornTemp)
                             .Build();
                    scheduler.ScheduleJob(job1, trigger1);
                    logger.Info(string.Format("预警规则[{0} {1}]将在{2}触发", mst.rule_code, mst.rule_name, dt.Hour.ToString()+ ":" +dt.Minute.ToString()));
                }
            }
            */
        }
                      

        /// <summary>
        /// 所有支付数据按统一时间调度监管服务
        /// </summary>
        private void StartRefreshService4All()
        {
            if (!scheduler.IsStarted)
            {
                //启动调度器
                scheduler.Start();
                logger.Info("刷新数据服务开启");
                
                #region 增加扫描任务
                try
                {
                    string cornStr = "";
                    /*
                    DbHelper.Open(connectString);
                    DataSet ds = DbHelper.GetDataSet(connectString, "select F_TimingRules from sys_setting");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cornStr = ds.Tables[0].Rows[0]["F_TimingRules"].ToString();
                    }
                    */
                    cornStr = CronStr;

                    if (!string.IsNullOrEmpty(cornStr))
                    {
                        //测试corn是否有效
                        if (cronTest(cornStr))
                        {               
                            DateTime dtNow = DateTime.Now;
                            DateTimeOffset utcTime = dtNow;

                            //2、创建一个任务
                            IJobDetail job1 = JobBuilder.Create<RefreshGKPaymentDataAllJob>()
                                                        .WithIdentity("JobRefresh4All", "Group1")
                                                        .Build();
                            //3、创建一个触发器
                            ITrigger trigger1 = TriggerBuilder.Create()
                                                        .WithIdentity("TriggerRefresh4All", "Group1")
                                                        .StartAt(utcTime)
                                                        .WithCronSchedule(cornStr)
                                                        .Build();
                            //4、将任务与触发器添加到调度器中
                            job1.JobDataMap.Put("scheduler", scheduler);
                            job1.JobDataMap.Put("corn", cornStr);

                            job1.JobDataMap.Put("AppKey", AppKey);
                            job1.JobDataMap.Put("AppSecret", AppSecret);

                            job1.JobDataMap.Put("Server", Server);
                            job1.JobDataMap.Put("Port", Port);
                            job1.JobDataMap.Put("Dir", Dir);

                            job1.JobDataMap.Put("DBName", DBName);
                            job1.JobDataMap.Put("UserId", UserId);
                            job1.JobDataMap.Put("OrgId", OrgId);


                            scheduler.ScheduleJob(job1, trigger1); //增加计划任务
                            logger.Info(string.Format("扫描任务已增加,定时规则为:'{0}'", cornStr));
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Error("扫描任务增加出错", e);
                }
                #endregion


                #region 动态增加计划任务工作 (修改了扫描任务的时间规则,得重新启动触发器) 
                /*
                //任务
                IJobDetail job2 = JobBuilder.Create<AddRefreshAllJob>()
                    .WithIdentity("AddRefreshJob", "Group2")
                    .Build();

                //传参数
                job2.JobDataMap.Put("scheduler", scheduler);
                job2.JobDataMap.Put("jobkey", "JobRefresh4All");
                job2.JobDataMap.Put("triggerkey", "TriggerRefresh4All");
                job2.JobDataMap.Put("userConnectString", userConnectString);
                job2.JobDataMap.Put("userDbTran", userDbTran);
                job2.JobDataMap.Put("corn", cornStr);
                job2.JobDataMap.Put("dmpConnectString", connectString);

                //触发器
                ITrigger trigger2 = TriggerBuilder.Create()
                    .WithIdentity("AddRefreshTrigger", "Group2")
                    .StartAt(DateTime.Now)
                    .WithCronSchedule(cornAddWarningRules) //每30秒执行 "0/30 * * * * ?"
                    .Build();

                scheduler.ScheduleJob(job2, trigger2); //增加计划任务
                */
                #endregion
            }
        }

        private bool cronTest(string cronStr) {
            bool ret = true;
            try
            {
                CronExpression exp = new CronExpression(cronStr);
                DateTime dt = DateTime.Now;
                DateTimeOffset dtof = new DateTimeOffset(dt);
                int i = 0;
                while (i < 5)
                {
                    dtof = (DateTimeOffset)exp.GetNextValidTimeAfter(dtof);
                    i++;
                }
            }
            catch (Exception e)
            {
                ret = false;
                logger.Error( string.Format("Corn表达式:'{0}'错误", cronStr), e);
            }
                        
            return ret;
        } 



    }
}
