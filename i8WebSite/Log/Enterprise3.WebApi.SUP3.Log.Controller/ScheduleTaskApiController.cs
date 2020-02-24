using NG3.Log.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.SUP3.Log.Controller
{
    /// <summary>
    /// 定时任务 webApi主要是用于处理后台日志
    /// </summary>
    public class ScheduleTaskController:ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Get()
        {
            return NG3LoggerManager.LogSorting();            
        }
    }
}
