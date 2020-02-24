using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Util
{
    public static class FuncCalcTask 
    {
        public static string GetTaskId()
        {
            //与session相关
            var key = System.Web.HttpContext.Current.Session.SessionID;
            var taskId = FuncCache.GetObject(key);
            if(taskId==null)            
            {
                taskId = Guid.NewGuid().ToString();
                //必须根据当前的session是否如果有缓存则取缓存   
                FuncCache.AddObject(key,taskId);                
            }

            return taskId.ToString();
        }
    }
}
