using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GJX3.JX.Model.Enums
{
    /// <summary>
    /// 绩效测评的类型（1-自评，2-抽评）
    /// </summary>
    public class EnumPerType
    {
        /// <summary>
        /// 自评
        /// </summary>
        public static string Self = "1";

        /// <summary>
        /// 抽评
        /// </summary>
        public static string Review = "2";

        /// <summary>
        /// 第三方评价
        /// </summary>
        public static string Third = "3";
    }
}