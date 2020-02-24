using GBK3.BK.Model.Domain;
using GQT3.QT.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GBK3.BK.Model.Extend
{
    /// <summary>
    /// Examples实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class PaymentXmAndDtlModel
    {
        /// <summary>
        /// 资金拨付项目
        /// </summary>
        [DataMember]
        public virtual PaymentXmModel PaymentXm
        {
            get;
            set;
        }

        /// <summary>
        /// 资金拨付明细
        /// </summary>
        [DataMember]
        public virtual IList<PaymentDtlModel> PaymentDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 附件列表集合
        /// </summary>
        [DataMember]
        public virtual IList<QtAttachmentModel> QtAttachments
        {
            get;
            set;
        }
    }
}