using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 付款单信息
    /// </summary>    
    public class PaymentInfo
    {
        /// <summary>
        /// 付款单流水号
        /// </summary>
        [DataMember]
        public long packageID { get; set; }

        /// <summary>
        /// 付款账户
        /// </summary>
        [DataMember]
        public BankAcnt bankAcnt { get; set; }

        /// <summary>
        /// 收款账户
        /// </summary>
        [DataMember]
        public BankAcnt oppoBankAcnt { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        [DataMember]
        public Decimal amount { get; set; }

        /// <summary>
        /// 付款币别
        /// </summary>
        [DataMember]
        public string currency { get; set; }

        /// <summary>
        /// 期望交易日期
        /// </summary>
        [DataMember]
        public DateTime bookingDate { get; set; }

        /// <summary>
        /// 提交网银日期
        /// </summary>
        [DataMember]
        public DateTime submitDate { get; set; }

        /// <summary>
        /// 是否同城 同 enumYesNo
        /// </summary>
        [DataMember]
        public int sameCity { get; set; }

        /// <summary>
        /// 是否同行 同 enumYesNo
        /// </summary>
        [DataMember]
        public int sameBank { get; set; }

        /// <summary>
        /// 是否加急 同 enumYesNo
        /// </summary>
        [DataMember]
        public int isUrgent { get; set; }

        /// <summary>
        /// 付款单状态 0-待支付， 1 支付成功, 2 支付异常, 3-支付中
        /// </summary>
        [DataMember]
        public int payState { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [DataMember]
        public string explantion { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        [DataMember]
        public string usage { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        [DataMember]
        public string response { get; set; }

        /// <summary>
        /// 附言
        /// </summary>
        [DataMember]
        public string postScript { get; set; }

        /// <summary>
        /// 提交人ID
        /// </summary>
        [DataMember]
        public string submitterID { get; set; }

        /// <summary>
        /// 业务参考号
        /// </summary>
        [DataMember]
        public string businessRefNo { get; set; }

        /// <summary>
        /// 对应的支付单序列号
        /// </summary>
        [DataMember]
        public string paymentSeqNo { get; set; }

        //支付指令状态
        [DataMember]
        public string result { get; set; }

        //支付指令状态描述
        [DataMember]
        public string resultMsg { get; set; }

        //明细支付序号
        [DataMember]
        public string iSeqno { get; set; }

    }
}
