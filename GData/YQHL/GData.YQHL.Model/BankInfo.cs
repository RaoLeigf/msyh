using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 银行信息
    /// </summary>
    public class BankInfo
    {

        /// <summary>
        /// 银行号
        /// </summary>
        [DataMember]
        public string bankCode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [DataMember]
        public string bankName { get; set; }

        /// <summary>
        /// 银行英文简写
        /// </summary>
        [DataMember]
        public string shortName { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [DataMember]
        public string bankKeys { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string description { get; set; }

    }
}
