using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GJX3.JX.Model.Enums
{
    /// <summary>
    /// 绩效相关状态（1-未上报，2-已上报，3-未审核，4-已审核）
    /// </summary>
    public class EnumPerStatus
    {
        /// <summary>
        /// 未上报
        /// </summary>
        public static string NoCheck = "1";

        /// <summary>
        /// 已上报
        /// </summary>
        public static string Check = "2";

        /// <summary>
        /// 未审核
        /// </summary>
        public static string NoValid = "3";

        /// <summary>
        /// 已审核
        /// </summary>
        public static string Valid = "4";

        /// <summary>
        /// 第三方评价未审核
        /// </summary>
        public static string NoThird = "5";

        /// <summary>
        /// 第三方评价已审核
        /// </summary>
        public static string Third = "6";

    }
}