using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Collections.Specialized;
using NG3.Addin.Core.Cfg;
using NG3.Log.Log4Net;
using NG3.Log;

namespace NG3.Addin.Core
{
    public static class LogHelper<T>
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<string, ILogger> _loggerList = new System.Collections.Concurrent.ConcurrentDictionary<string, ILogger>();

        private static ILogger Logger
        {
            get
            {
                if (_loggerList.ContainsKey(typeof(T).ToString()))
                {
                    return _loggerList[typeof(T).ToString()];
                }
                else
                {                   
                    ILogger _logger = NGLogProvider.GetLogger(typeof(T), "NG3.Addin");
                    //ILogger _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(T),"NG3.Addin");
                    _loggerList.TryAdd(typeof(T).ToString(), _logger);
                    return _logger;
                }

            }
        }

        /// <summary>
        /// 输出异常
        /// </summary>
        public static void OutputException(Exception e)
        {
            Logger.Error(e.StackTrace, e);
            //if (e.InnerException != null)
            //    OutputException(e.InnerException);
        }

        public static void Error(string message)
        {
            //如果不是二次开发操作员则不显示日志
            if (!AddinOperator.CurrentUserIsOperator()) return;
            Logger.Error(message);
        }

        public static void Debug(string message)
        {
            //如果不是二次开发操作员则不显示日志
            if (!AddinOperator.CurrentUserIsOperator()) return;
            Logger.Debug(message);
        }

        public static void Info(string message)
        {
            //如果不是二次开发操作员则不显示日志
            if (!AddinOperator.CurrentUserIsOperator()) return;
            Logger.Info(message);
        }

        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        public static string StartMethodLog(string methodName)
        {
            string guid = Guid.NewGuid().ToString();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            CallContext.SetData(guid + "_watch", stopwatch);
            CallContext.SetData(guid + "_methodName", methodName);
            return guid;
        }

        public static string EndMethodLog(string guid, long warnTime = 200)
        {
            Stopwatch stopwatch = (Stopwatch)CallContext.GetData(guid + "_watch");
            if (stopwatch != null)
            {
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > warnTime)
                {
                    Info(string.Format("方法{0},执行时间{1}", CallContext.GetData(guid + "_methodName").ToString(), stopwatch.ElapsedMilliseconds));
                }
            }
            return guid;
        }


        public static void PrintEvn()
        {
            //如果不是二次开发操作员则不显示日志
            if (!AddinOperator.CurrentUserIsOperator()) return;

            var data = AddinEnvironment.RequestParams();
            foreach (var item in data.AllKeys)
            {               
                Logger.Info(item+" : "+ data[item]);

            }
        }
    }
}
