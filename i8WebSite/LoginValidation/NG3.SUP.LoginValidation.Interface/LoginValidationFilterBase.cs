using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.SUP.LoginValidation.Interface
{
    public class  LoginValidationFilterBase<FilterResult, LoginValidationParam> : FilterBase<FilterResult, LoginValidationParam>
    {
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

        public override FilterResult Filter(LoginValidationParam param)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public LoginValidationFilterBase()
        { }

        public void FireSubscribe(object obj)
        {
            if (this._onSubscribe != null)
            {
                this._onSubscribe(obj);
            }
        }
    }
}
