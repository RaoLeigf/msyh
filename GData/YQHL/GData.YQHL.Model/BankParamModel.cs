using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 银行参数信息
    /// </summary>
    public class BankParamModel<T> : object where T : class
    {
        /// <summary>
        /// 调用者
        /// </summary>
        public CallerInfo caller { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        public string currency { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime beginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endDate { get; set; }

        /// <summary>
        /// 序列化的对象
        /// </summary>
        public T infoData { get; set; }

        /// <summary>
        /// 指令包序列号
        /// </summary>
        public string fSeqNo { get; set; }

        /// <summary>
        /// 上限金额,单位元
        /// </summary>
        public decimal maxAmt { get; set; }

        /// <summary>
        /// 下限金额,单位元
        /// </summary>
        public decimal minAmt { get; set; }

        /// <summary>
        /// 下页标志
        /// </summary>
        public string nextTag { get; set; }
    }
}
