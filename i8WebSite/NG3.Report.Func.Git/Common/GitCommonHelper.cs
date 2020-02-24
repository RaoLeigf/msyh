using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Git.Common
{
    public class GitCommonHelper
    {
        /// <summary>
        /// 根据会计区间，取得会计期
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static int[] GetAccperFromArea(string area)
        {
            return new int[] { 1,2};
        }

        /// <summary>
        /// 获取到当前的年度
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentYear()
        {
            return "2017";
        }


        public static string ToValue(object value)
        {
            return null;
        }

        public static string ToArray(int[] accper)
        {
            return null;
        }
    }
}
