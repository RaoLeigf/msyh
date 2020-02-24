using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GBK3.BK.Model.Enums
{
    /// <summary>
    /// IsPayType(支付状态)
    /// </summary>
    public enum IsPayType
    {
        /// <summary>
        /// 0-否
        /// </summary>
        no = 0,

        /// <summary>
        /// 1-待支付
        /// </summary>
        pend = 1,


        /// <summary>
        /// 9-支付完成
        /// </summary>
        yes = 9
    }
}