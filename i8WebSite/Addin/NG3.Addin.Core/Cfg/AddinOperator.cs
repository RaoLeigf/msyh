using NG3.Addin.Model.Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Cfg
{
    public static class AddinOperator
    {
        private static object operlockobject = new object();
        private static object lockobject = new object();

        private static bool _inited = false;

        /// <summary>
        /// 初始化加载二开操作员
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static void Init(ISession session)
        {
            if (_inited) return;
            lock (lockobject)
            {
                if (_inited) return;               
                //操作员
                var operators = GetOperators(session);
                if (operators == null) return;
                operators.ToList().ForEach(p => { AddinCache.AddOper(p); });              
                _inited = true; //设置初始化完成标志
            }
        }
        /// <summary>
        /// 重新加载
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static bool ReloadOperator(ISession session)
        {
            var data = GetOperators(session);
            if (data == null) return false;

            lock (operlockobject)
            {
                AddinCache.ClearAllOpers();
                data.ToList().ForEach(p => { AddinCache.AddOper(p); });              
            }
            return true;
        }

        /// <summary>
        /// 判断当前用户是否是操作员
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public static bool CurrentUserIsOperator()
        {
            if (string.IsNullOrEmpty(NG3.AppInfoBase.LoginID)) return false;
            string loginId = NG3.AppInfoBase.LoginID.ToLower();

            return AddinCache.ContainOper(loginId.ToLower());
        }

        /// <summary>
        /// 取操作员
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private static IList<AddinOperatorModel> GetOperators(ISession session)
        {
            if (session == null) return null;
            var operators = session.CreateQuery("from AddinOperatorModel a ").List<AddinOperatorModel>();
            return operators;
        }


    


    }
}
