using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class BankAcnt
    {
        /// <summary>
        /// 开户行名称
        /// </summary>
        [DataMember]
        public string bankName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [DataMember]
        public string acntNo { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        [DataMember]
        public string acntName { get; set; }

        /// <summary>
        /// swift编码
        /// </summary>
        [DataMember]
        public string swiftCode { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [DataMember]
        public string country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [DataMember]
        public string province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [DataMember]
        public string city { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [DataMember]
        public BankInfo bankInfo { get; set; }

        /// <summary>
        /// 银行版本
        /// </summary>
        [DataMember]
        public BankVersionInfo bankVersionInfo { get; set; }

        /// <summary>
        /// 企业证书ID
        /// </summary>
        [DataMember]
        public string CertUserID { get; set; }
    }
}
