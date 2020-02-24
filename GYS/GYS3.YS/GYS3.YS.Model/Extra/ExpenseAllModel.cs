using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GYS3.YS.Model.Extra
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class ExpenseAllModel
    {
        /// <summary>
        /// 用款计划主表对象
        /// </summary>
        [DataMember]
        public virtual ExpenseMstModel ExpenseMst
        {
            get;
            set;
        }

        /// <summary>
        /// 用款计划明细对象
        /// </summary>
        [DataMember]
        public virtual List<ExpenseDtlModel> ExpenseDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 用款计划额度核销对象
        /// </summary>
        [DataMember]
        public virtual List<ExpenseHxModel> ExpenseHxs
        {
            get;
            set;
        }

        /// <summary>
        /// 用款计划对应的附件
        /// </summary>
        [DataMember]
        public virtual List<QtAttachmentModel> QtAttachments
        {
            get;
            set;
        }

        /// <summary>
        /// 返回总金额
        /// </summary>
        [DataMember]
        public virtual decimal ReturnAllAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 单据预计支出总金额
        /// </summary>
        [DataMember]
        public virtual decimal PayAllAmount
        {
            get;
            set;
        }
    }
}
