using GBK3.BK.Model.Domain;
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
    public partial class PaymentMstAndXmModel
    {
        /// <summary>
        /// 资金拨付主表列表
        /// </summary>
        [DataMember]
        public virtual PaymentMstModel PaymentMst
        {
            get;
            set;
        }

        /// <summary>
        /// 资金拨付项目
        /// </summary>
        [DataMember]
        public virtual IList<PaymentXmAndDtlModel> PaymentXmDtl
        {
            get;
            set;
        }
    }
}