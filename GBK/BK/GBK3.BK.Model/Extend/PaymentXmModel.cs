using Enterprise3.Common.Model.NHORM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace GBK3.BK.Model.Domain
{
    /// <summary>
    /// PaymentXm实体定义类
    /// </summary>
    public partial class PaymentXmModel: EntityBase<PaymentXmModel>
    {
        /// <summary>
        /// 项目总金额
        /// </summary>
        [DataMember]
        public virtual decimal Sum
        {
            get;
            set;
        }

        /// <summary>
        /// 已使用金额
        /// </summary>
        [DataMember]
        public virtual decimal Use
        {
            get;
            set;
        }

        /// <summary>
        /// 已冻结金额
        /// </summary>
        [DataMember]
        public virtual decimal Frozen
        {
            get;
            set;
        }

        /// <summary>
        /// 剩余金额
        /// </summary>
        [DataMember]
        public virtual decimal Surplus
        {
            get;
            set;
        }

    }
}