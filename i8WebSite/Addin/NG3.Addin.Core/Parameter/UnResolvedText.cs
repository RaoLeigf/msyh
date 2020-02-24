using NG3.Addin.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Parameter
{
    /// <summary>
    /// 原始的UI参数
    /// </summary>
    public class UnResolvedText
    {
        public string RequestParam { set; get; }

        public EnumUIDataSourceType RowsType { set; get; }

        /// <summary>
        /// 待解析的文本
        /// </summary>
        public string RawText { set; get; }
    }
}
