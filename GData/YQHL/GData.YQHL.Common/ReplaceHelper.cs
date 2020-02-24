using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GData.YQHL.Common
{
    public static class ReplaceHelper
    {

        /// <summary>
        /// 会自动替换 变量   把str中形如 "{{varName}}" 替换成context中对应key的数值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string ReplaceStringVar(string str, InstallContext context)
        {
            Regex reg = new Regex(@"\{\{(.*?)\}\}");
            //var mat = reg.Matches(webcofnigstring2);

            str = reg.Replace(str,
                new MatchEvaluator(m =>
                     context.Get(m.Groups[1].Value) == string.Empty ? m.Value : context.Get(m.Groups[1].Value)
                ));
            return str;
        }
    }
}
