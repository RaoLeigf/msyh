using System;
using System.Collections.Generic;

using Quartz;
using System.Data;
using NG3.Data.Service;

namespace RefreshJobSchedule
{
    /// <summary>
    /// 动态增加或修改获预警规则调度任务
    /// </summary>
    public class AddRefreshAllJob:IJob
    {
        //使用Common.Logging.dll日志接口实现日志记录
        private static readonly Common.Logging.ILog logger = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static IList<string> rule_list = new List<string>();
       
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //logger.Info("开始动态增加预警规则任务");

                IScheduler scheduler = (IScheduler)context.JobDetail.JobDataMap.Get("scheduler");

                //同步保存已经增加计划任务的规则
                string jobKey = context.JobDetail.JobDataMap.Get("jobkey").ToString();
                string triggerKey = context.JobDetail.JobDataMap.Get("triggerkey").ToString();
                string dmpConnectString = context.JobDetail.JobDataMap.Get("dmpConnectString").ToString();

                JobKey k = new JobKey(jobKey , "Group1");

                string cornOld = scheduler.Context.Get("corn").ToString(); //旧的触发时间

                string cornStr = "";

                DbHelper.Open(dmpConnectString);
                DataSet ds = DbHelper.GetDataSet(dmpConnectString, "select F_TimingRules from sys_setting");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cornStr = ds.Tables[0].Rows[0]["F_TimingRules"].ToString();
                }

                #region 动态增加预警任务
                if (scheduler.CheckExists(k))
                { 
                    //已经有增加了任务,修改triger,再重启任务

                    if (!string.IsNullOrEmpty(cornStr) && cornStr != cornOld)
                    {
                        if (cronTest(cornStr))
                        {
                            TriggerKey tk = new TriggerKey(triggerKey, "Group1");

                            DateTime dtNow = DateTime.Now;
                            DateTimeOffset utcTime = dtNow;
                            ITrigger trigger1 = TriggerBuilder.Create()
                                                    .WithIdentity(triggerKey, "Group1")
                                                    .StartAt(utcTime)
                                                    .WithCronSchedule(cornStr)
                                                    .Build();
                            scheduler.RescheduleJob(tk, trigger1);
                            scheduler.ResumeTrigger(tk);
                            logger.Info(string.Format("扫描任务已更新,定时规则为:'{0}'", cornStr));

                            scheduler.Context.Put("corn", cornStr);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(cornStr))
                    {
                        if (cronTest(cornStr))
                        {
                            DateTime dtNow = DateTime.Now;
                            DateTimeOffset utcTime = dtNow;

                            IJobDetail job1 = JobBuilder.Create<RefreshGKPaymentDataAllJob>()
                                                        .WithIdentity(jobKey, "Group1")
                                                        .Build();

                            ITrigger trigger1 = TriggerBuilder.Create()
                                                        .WithIdentity(triggerKey, "Group1")
                                                        .StartAt(utcTime)
                                                        .WithCronSchedule(cornStr)
                                                        .Build();

                            scheduler.ScheduleJob(job1, trigger1); //增加计划任务
                            logger.Info(string.Format("扫描任务已增加,定时规则为:{0}", cornStr));
                        }
                    }
                }
                #endregion

                //logger.Info("动态增加任务结束!");
            }
            catch (Exception ex)
            {
                logger.Error("AddRefreshAllJob 运行异常", ex);
            }
        }

        private bool cronTest(string cronStr)
        {
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
                logger.Error(string.Format("Corn表达式:'{0}'错误", cronStr), e);
            }

            return ret;
        }
    }
}
