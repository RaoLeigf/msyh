using AopAlliance.Intercept;
using Enterprise3.Common.Model.Results;
using NG3.Addin.Core.Cfg;
using NG3.Addin.Model.Domain.BusinessModel;
using NG3.Addin.Model.Enums;
using NHibernate;
using Spring.Data.NHibernate.Generic;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Interceptor
{
    public class InterceptorExecutor
    {
        //注入
        private HibernateTemplate hibernateTemplate;


        //构造函数
        public InterceptorExecutor(HibernateTemplate _hibernateTemplate)
        {
            hibernateTemplate = _hibernateTemplate;
            //同时初始化相关的配置信息
            ISession session = null;
            try
            {
                session = hibernateTemplate.SessionFactory.OpenSession();
                if (session == null) throw new AddinException("无法通过OpenSession获得session");
                //配置文件初始化
                AddinConfigure.Init(session);
                //初始化加载二开人员
                AddinOperator.Init(session);                                
                session.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (session.IsOpen)
                {
                    session.Close();
                }

            }
        }

        public bool ReloadConfigure()
        {
            ISession session = hibernateTemplate.SessionFactory.GetCurrentSession();
            if (session == null) throw new AddinException("无法通过GetCurrentSession获得session");
            //重新加载
            return AddinConfigure.ReloadConfigure(session);
        }

        public bool ReloadOperator()
        {
            ISession session = hibernateTemplate.SessionFactory.GetCurrentSession();
            if (session == null) throw new AddinException("无法通过GetCurrentSession获得session");
            //重新加载
            return AddinOperator.ReloadOperator(session);
        }

        /// <summary>
        /// 发布配置
        /// </summary>
        /// <param name="mstPhid"></param>
        /// <returns></returns>
        public bool DeployConfigure(long mstPhid)
        {
            return AddinConfigure.DeployConfigure(mstPhid);
        }

        /// <summary>
        /// 取消发布配置
        /// </summary>
        /// <param name="mstPhid"></param>
        /// <returns></returns>
        public bool UnDeployConfigure(long mstPhid)
        {
            return AddinConfigure.UnDeployConfigure(mstPhid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<ServiceUIParamBizModel> GetServiceRequestParameters()
        {
            return AddinEnvironment.GetServiceUIParams();
        }

        /// <summary>
        /// 方法前调用 
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public  bool ExecuteBeforeMethod(MethodInfo method, object[] args, object target)
        {
            bool rtn = false;
            ISession session = null;
            try
            {

                //判断是否能够注入方法
                var entity = AddinConfigure.GetPluginConfigure(method.Name, args, target, EnumInterceptorType.Before);
                if (entity == null)
                {
                    LogHelper<AfterAdvice>.Info(string.Format("无法取得注入方法的配置信息 class:{0},method:{1}", target.ToString(), method.Name));
                    return false;
                }

                AddinMethodInvocation invocation = new AddinMethodInvocation(method, args, target);

                if (entity != null)
                {
                    //取得当前服务方法的session
                    session = hibernateTemplate.SessionFactory.GetCurrentSession();
                    if (session == null) throw new AddinException("无法通过GetCurrentSession获得session");

                    IBeforeInterceptor[] executors = AddinInterceptorFactory.GetBeforeExecutor(entity, session);

                    foreach (var executor in executors)
                    {
                        if (executor != null)
                        {
                            rtn = executor.Before(invocation);
                        }
                    }
                    //session.Close();
                }

                return rtn;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //if (session!=null && session.IsOpen)
                //{
                //    session.Close();
                //}
            }                        

        }

        /// <summary>
        /// 方法后调用
        /// </summary>
        /// <param name="returnObject"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public  bool ExecuteAfterMethod(object returnObject, MethodInfo method, object[] args, object target)
        {
            bool rtn = false;


            //判断是否能够注入方法
            var entity = AddinConfigure.GetPluginConfigure(method.Name, args, target, EnumInterceptorType.After);
            if (entity == null)
            {
                LogHelper<BeforeAdvice>.Info(string.Format("无法取得注入方法的配置信息 class:{0},method:{1}", target.ToString(), method.Name));
                return false;
            }

            AddinMethodInvocation invocation = new AddinMethodInvocation(method, args, target);

            if (returnObject!=null)
            {

                var result = returnObject as SavedResult<long>;
                if(result!= null)
                {                    
                    //将结果注入到线程上下文
                    CallContext.SetData("returnobject", (result.KeyCodes.ToList())[0]);
                    LogHelper<InterceptorExecutor>.Info("取得的主键信息是"+ (result.KeyCodes.ToList())[0]);
                }else
                {
                    //为空的情况下判断是不是ResponseResult
                    var result2 = returnObject as ResponseResult;

                }
            }
           
            if (entity != null)
            {
                ISession session = hibernateTemplate.SessionFactory.GetCurrentSession();
                if (session == null) throw new AddinException("无法通过GetCurrentSession获得session");

                IAfterInterceptor[] executors = AddinInterceptorFactory.GetAfterExecutor(entity, session);
                LogHelper<InterceptorExecutor>.Info("待处理的后处理事件是有" + executors.Length);
                foreach (var executor in executors)
                {
                    rtn = executor.After(returnObject, invocation);
                }
                                    
            }
          
            return rtn;
                                   
           
        }

    }
}
