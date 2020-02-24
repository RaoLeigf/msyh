using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJX3.JX.Model.Enums
{
    /// <summary>
    /// 单据类型：1|c|年初;2|z|年中新增;3|x|专项
    /// </summary>
    public enum EnumProjBillType
    {
        /// <summary>
        /// 1-c 年初
        /// </summary>
        c = 1,

        /// <summary>
        /// 2-z 年中新增
        /// </summary>
        z = 2,

        /// <summary>
        /// 3-x 专项
        /// </summary>
        x = 3
    }
}
