using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace NG3.SUP.LoginValidation.Interface
{
    /// <summary>
    /// 登陆验证参数
    /// </summary>
    [Serializable]
    public sealed class LoginValidationParam
    {
        /// <summary>
        /// 验证参数
        /// </summary>
        private ConcurrentDictionary<string, object> _validationParams = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                object value = null;
                if (this._validationParams.ContainsKey(key))
                {
                    value = this._validationParams[key];
                }
                else
                {
                    throw new Exception("this key not exists");
                }
                return value;
            }

            set
            {
                this._validationParams.TryAdd(key, value);
            }
        }

        public LoginValidationParam()
        {

        }

        /// <summary>
        /// 增加指定的键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            this._validationParams.TryAdd(key, value);
        }

        /// <summary>
        /// 删除指定的键
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            Object value;
            this._validationParams.TryRemove(key, out value);
        }
    }
}
