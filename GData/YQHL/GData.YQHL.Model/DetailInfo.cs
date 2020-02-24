using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 交易明细
    /// </summary>    
    public class DetailInfo
    {
        /// <summary>
        /// 查询账户
        /// </summary>
        [DataMember]
        public BankAcnt bankAcnt { get; set; }

        /// <summary>
        /// 对方账户
        /// </summary>
        [DataMember]
        public BankAcnt oppoBankAcnt { get; set; }

        /// <summary>
        /// 付款币别
        /// </summary>
        [DataMember]
        public string currency { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        [DataMember]
        public DateTime transTime { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        [DataMember]
        public string bussinessName { get; set; }
                
        /// <summary>
        /// 业务参考号
        /// </summary>
        [DataMember]
        public string bussinessRefNo { get; set; }

        /// <summary>
        /// 交易摘要
        /// </summary>
        [DataMember]
        public string explantion { get; set; }

        /// <summary>
        /// 支出金额
        /// </summary>
        [DataMember]
        public decimal debitAmount { get; set; }

        /// <summary>
        /// 收入金额
        /// </summary>
        [DataMember]
        public decimal crebitAmount { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [DataMember]
        public decimal balance { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [DataMember]
        public string rawTransTime { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        [DataMember]
        public string usage { get; set; }

        /// <summary>
        /// 凭证号
        /// </summary>
        [DataMember]
        public string vouhNo { get; set; }

        /// <summary>
        /// 凭证种类
        /// </summary>
        [DataMember]
        public string cVouhType { get; set; }
                
        /// <summary>
        /// 附言
        /// </summary>
        [DataMember]
        public string postScript { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        [DataMember]
        public string addInfo { get; set; }
    }
}
