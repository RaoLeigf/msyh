using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Interface;
using NG3.Exceptions;

namespace SUP.Right.Rules
{
    
    public class FieldRight : IRight
    {

        protected FieldServices services;

        public bool Add(IRightEntity entity)
        {
            return false;
        }

        public bool Remove(IRightEntity entity)
        {
            return false;
        }

        public bool Update(IRightEntity entity)
        {
            return false;
        }

        public virtual bool Validate(string ocode, string logid, string rightname, string funcname, params object[] objs)
        {
            return false;
        }   
          
        /// <summary>
        ///  获取域权限信息
        /// </summary>
        /// <typeparam name="CType">string 类型</typeparam>
        /// <param name="ocode">组织号</param>
        /// <param name="logid">登录账户</param>
        /// <param name="objs">目标sql</param>
        /// <returns></returns>
        public CType GetInfo<CType>(string ocode, string logid, params object[] objs)
        {
            if (objs == null) return default(CType);

            try
            {
                services = new FieldServices(logid, ocode);
                object sql = services.LimitBuilder(objs[0].ToString());
                CType type = default(CType);
                type = (CType)sql;
                return type;
            }
            catch (Exception ex)
            {
                //TODO: 记录出错日志
                //NG3.Log.Log4Net.ILogger logger = (new NG3.Log.Log4Net.Log4NetLoggerFactory()).CreateLogger(typeof(NoLoginException), NG3.Log.Log4Net.LogType.authorization);
                //logger.Info(ex.Message, ex);

                return default(CType);
            }
        }
    }

}
