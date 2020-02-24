using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Data.Service;

namespace SUP.Common.DataAccess
{
    /// <summary>
    /// 服务逻辑层多语言帮助类
    /// </summary>
    public class LangInfo : Enterprise3.Rights.AnalyticEngine.LangInfo
    {
        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <param name="busiType">ui元素业务类型</param>
        /// <returns>Lang,LangSln</returns>
        public static Dictionary<string, string> GetLabelLang(string busiType)
        {
            var dic = GetLang(busiType);
            if (dic == null || dic.Count == 0)
                return new Dictionary<string, string>();

            return dic.ToDictionary(t => t.Key, t => t.Value);
        }

        /// <summary>
        /// 获取多语言信息
        /// </summary>
        /// <param name="busiType">ui元素业务类型</param>
        /// <returns>Lang,LangSln</returns>
        public static Dictionary<string, string> GetLabelLangWithConn(string language, string conn, string busiType)
        {
            return null;
        }
    }
}