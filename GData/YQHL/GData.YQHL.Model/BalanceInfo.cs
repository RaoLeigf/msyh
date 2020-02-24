using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 余额信息
    /// </summary>    
    public class BalanceInfo
    {
        /// <summary>
        /// 银行账户
        /// </summary>
        [DataMember]
        public BankAcnt bankAcnt { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        [DataMember]
        public string currency { get; set; }

        /// <summary>
        /// 账户类型 第3位数字为帐户属性： 1：基本户 2：一般结算户 3：临时户 4：专用户 6：集团二级户 7：协定存款户 8：保证金户     查询帐户为定期户数据字典：0 国有企业1 外企及金融机构
        /// </summary>
        [DataMember]
        public string acntType { get; set; }

        /// <summary>
        /// 余额日期
        /// </summary>
        [DataMember]
        public DateTime balanceDate { get; set; }

        /// <summary>
        /// 上日余额 单位：元
        /// </summary>
        [DataMember]
        public Decimal hisBalance { get; set; }

        /// <summary>
        /// 当前余额 单位：元
        /// </summary>
        [DataMember]
        public Decimal balance { get; set; }

        /// <summary>
        /// 可用余额 单位：元
        /// </summary>
        [DataMember]
        public Decimal availBalance { get; set; }

        /// <summary>
        /// 无效原因
        /// </summary>
        [DataMember]
        public String unavailReason { get; set; }

        /// <summary>
        /// 指令包序列号
        /// </summary>
        [DataMember]
        public String fSeqno { get; set; }


    }
}
