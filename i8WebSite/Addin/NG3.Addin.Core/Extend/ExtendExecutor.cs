using NG3.Addin.Core.Cfg;
using NHibernate;
using Spring.Data.NHibernate.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Extend
{
    public class ExtendExecutor
    {
        //注入
        private  HibernateTemplate hibernateTemplate;
        public string Executor(string extendName)
        {

            LogHelper<ExtendExecutor>.Info("当前扩展程序的方法名是:"+extendName);
            ISession session = null;
            string rtn = string.Empty;
            try
            {
                session = hibernateTemplate.SessionFactory.OpenSession();
                if (session == null) throw new AddinException("NHibernate无法获取Session");

                //初始化配置
                ExtendConfigure.InitConfigure(session);

                //打出环境参数
                LogHelper<ExtendExecutor>.PrintEvn();

                IExtendAction action = ExtendActionFactory.GetExtendAction(session, extendName);

                

                if(action!=null)
                {
                    rtn = action.Execute();
                }
                                 
                session.Close();
                LogHelper<ExtendExecutor>.Info("当前扩展程序的执行结果集是:"+(rtn==null?"null":rtn));
                return rtn;
            }
            catch (Exception e)
            {
                LogHelper<ExtendExecutor>.Error("执行扩展方法出错:" + e.Message);
                throw;
            }
            finally
            {
                try
                {
                    if (session.IsOpen)
                    {
                        session.Close();
                    }
                        
                }
                catch (Exception)
                {

                    throw;
                }
                
            }

        }

        /// <summary>
        /// 重新刷新缓存配置
        /// </summary>
        public void RefreshConfigure()
        {
            ISession session = hibernateTemplate.SessionFactory.GetCurrentSession();
            if (session == null) throw new AddinException("无法通过GetCurrentSession获得session");
            ExtendConfigure.Refresh(session);
        }
    }
}
