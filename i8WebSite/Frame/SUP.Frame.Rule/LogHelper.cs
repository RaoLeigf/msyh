using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Collections.Specialized;
using NG3.Log.Log4Net;
using NG3.Log;

namespace SUP.Frame.Rule
{
    public static class LogHelper<T>
    {
        private static ILogger Logger
        {
            get
            {
                ILogger _logger = NGLogProvider.GetLogger(typeof(T), "SUP.Frame");
                return _logger;
            }
        }

        /// <summary>
        /// 输出异常
        /// </summary>
        public static void OutputException(Exception e)
        {
            Logger.Error(e.StackTrace, e);
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        public static void Info(string message)
        {
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
        
    }
}
