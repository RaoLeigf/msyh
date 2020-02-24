using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.SUP.LoginValidation.Interface;
using NG.Aop;
using System.IO;
using System.Xml;
using System.Threading;

namespace NG3.SUP.LoginValidation
{
    /// <summary>
    /// Login Validation Pipe
    /// </summary>
    public sealed class LoginValidationPipe : PipeBase<LoginValidationFilterBase<FilterResult, LoginValidationParam>, LoginValidationParam>
    {
        private List<LoginValidationFilterDetail> _loginValidationFilterDetailList = null;

        private static LoginValidationParam _loginValidationParam = null;

        private static int _threadSignal = 0;

        /// <summary>
        /// 配置文件路径（测试）
        /// </summary>
        private static readonly string LoginValidationConfigFile = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + @"LoginValidation.config";

        private INGAopContainer _container = null;

        public delegate void SubscribeHandle(object obj);

        private event SubscribeHandle _onSubscribe;

        /// <summary>
        /// 消息订阅
        /// </summary>
        public event SubscribeHandle OnSubscribe
        {
            add
            {
                _onSubscribe += value;
            }
            remove
            {
                _onSubscribe -= value;
            }
        }

        public delegate void ThrowErrHandle(string errMsg);

        private event ThrowErrHandle _onThrowErr;

        /// <summary>
        /// 抛出错误信息
        /// </summary>
        public event ThrowErrHandle OnThrowErr
        {
            add
            {
                _onThrowErr += value;
            }
            remove
            {
                _onThrowErr -= value;
            }
        }

        private bool _endAllCheck = false;
        /// <summary>
        /// 所以验证结束
        /// </summary>
        public bool EndAllCheck
        {
            get { return _endAllCheck; }
            set { _endAllCheck = value; }
        }

        private bool _checkSuccess = true;
        /// <summary>
        /// 所有验证成功
        /// </summary>
        public bool CheckSuccess
        {
            get { return _checkSuccess; }
            set { _checkSuccess = value; }
        }


        /// <summary>
        /// 读取过滤器集合(从配置的XML文件中)
        /// </summary>
        private void ReadFilter()
        {
            XmlDocument doc = new XmlDocument();
            _loginValidationFilterDetailList = new List<LoginValidationFilterDetail>();
            try
            {
                doc.Load(LoginValidationConfigFile);
                XmlNodeList parentNode = doc.SelectNodes("configuration/LoginValidationFilterSeq/LoginValidationFilter");
                if (parentNode == null)
                    return;

                foreach (XmlElement element in parentNode)
                {
                    LoginValidationFilterDetail detail = new LoginValidationFilterDetail(element.Attributes["id"].Value,
                                                                     element.Attributes["name"].
                                                                         Value,
                                                                     element.Attributes[
                                                                         "description"].Value,
                                                                     Convert.ToInt32(element.Attributes["seq"].Value),
                                                                     element.Attributes["fullType"].
                                                                         Value,
                                                                     Convert.ToBoolean(element.Attributes["isNeedUi"].
                                                                         Value));

                    _loginValidationFilterDetailList.Add(detail);
                }

                _loginValidationFilterDetailList.Sort(
                    delegate(LoginValidationFilterDetail detailA, LoginValidationFilterDetail detailB)
                    {
                        return detailA.Seq - detailB.Seq;
                    });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取最终的执行序列
        /// </summary>
        /// <returns></returns>
        private IList<IList<LoginValidationFilterDetail>> GetExecuteList()
        {
            IList<IList<LoginValidationFilterDetail>> executeList = new List<IList<LoginValidationFilterDetail>>();
            IList<LoginValidationFilterDetail> details = new List<LoginValidationFilterDetail>();

            if (_loginValidationFilterDetailList == null)
                return executeList;

            foreach (LoginValidationFilterDetail detail in _loginValidationFilterDetailList)
            {
                //如果需要展示UI或者是单核的机器,都不考虑启用多线程队列
                if (detail.IsNeedUi || Environment.ProcessorCount < 2)
                {
                    if (details.Count > 0)
                    {
                        executeList.Add(details);
                        details = new List<LoginValidationFilterDetail>();
                    }

                    details.Add(detail);
                    executeList.Add(details);
                    details = new List<LoginValidationFilterDetail>();
                }
                else
                {
                    details.Add(detail);
                }
            }
            if (details.Count > 0)
            {
                executeList.Add(details);
            }

            return executeList;
        }

        /// <summary>
        /// 执行验证队列
        /// </summary>
        private FilterResult ExecuteLoginValidationSeq()
        {
            if (_loginValidationFilterDetailList == null)
            {
                ReadFilter();
            }

            IList<IList<LoginValidationFilterDetail>> executeList = GetExecuteList();

            //等待Container预加载完毕
            while (_container == null)
            {
                Thread.Sleep(10);
            }
            FilterResult filterResult = new FilterResult() { Success=true, ErrorMsg=string.Empty };
            foreach (IList<LoginValidationFilterDetail> details in executeList)
            {
                foreach (LoginValidationFilterDetail detail in details)
                {
                    //INGAopContainer container = new NGAopContainer(LoginValidationConfigFile, "ng");
                    LoginValidationFilterBase<FilterResult, LoginValidationParam> loginValidationAction = _container.Resolve<LoginValidationFilterBase<FilterResult, LoginValidationParam>>(detail.FullType);
                    //_threadSignal++;
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), obj);
                    if (loginValidationAction == null)
                        break;

                    if (loginValidationAction is LoginValidationFilterBase<FilterResult, LoginValidationParam>)
                    {
                        LoginValidationFilterBase<FilterResult, LoginValidationParam> action = loginValidationAction as LoginValidationFilterBase<FilterResult, LoginValidationParam>;
                        filterResult = action.Filter(_loginValidationParam) as FilterResult;
                        if (filterResult != null)
                        {
                            if (!filterResult.Success)
                            {
                                return filterResult;
                            }
                        }
                    }

                }
            }
            return filterResult;
        }


        private void ThreadProc(Object loginValidationAction)
        {
            if (loginValidationAction == null)
                return;

            if (loginValidationAction is LoginValidationFilterBase<FilterResult, LoginValidationParam>)
            {
                LoginValidationFilterBase<FilterResult, LoginValidationParam> action =
                    loginValidationAction as LoginValidationFilterBase<FilterResult, LoginValidationParam>;
                action.OnSubscribe += new LoginValidationFilterBase<FilterResult, LoginValidationParam>.SubscribeHandle(Action_OnSubscribe);
                FilterResult filterResult = action.Filter(_loginValidationParam) as FilterResult;
                if (filterResult != null)
                {
                    if (!filterResult.Success)
                    {
                        if (!string.IsNullOrEmpty(filterResult.ErrorMsg) && this._onThrowErr != null)
                        {
                            this._onThrowErr(filterResult.ErrorMsg);
                        }
                        this._checkSuccess = false;
                    }
                }
            }
            _threadSignal--;
        }

        private void Action_OnSubscribe(object obj)
        {
            if (this._onSubscribe != null)
            {
                this._onSubscribe(obj);
            }
        }

        public override FilterResult Execute(LoginValidationParam param)
        {
            this.CheckSuccess = true;
            this.EndAllCheck = false;
            _loginValidationParam = param;
            return  ExecuteLoginValidationSeq();
        }

        /// <summary>
        /// 预编译管道
        /// </summary>
        public void PreCompilerPipe()
        {
            _container = new NGAopContainer(LoginValidationConfigFile, "ng");
        }
    }
}
