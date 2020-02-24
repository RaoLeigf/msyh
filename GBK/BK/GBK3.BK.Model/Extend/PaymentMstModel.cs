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
    /// PaymentMst实体定义类
    /// </summary>
    public partial class PaymentMstModel : EntityBase<PaymentMstModel>
    {
        /// <summary>
        /// 0- 未审批 1-待审批 2- 未通过 9-审批通过
        /// </summary>
        [DataMember]
        public virtual String ApprovalBz
        {
            get;
            set;
        }

        /// <summary>
        /// 0- 否 1-待支付 9-支付完成
        /// </summary>
        [DataMember]
        public virtual String PayBz
        {
            get;
            set;
        }

        /// <summary>
        /// 0- 否 1-待支付 9-支付完成(多个筛选条件)
        /// </summary>
        [DataMember]
        public virtual List<byte> PayBzs
        {
            get;
            set;
        }

        /// <summary>
        /// 0- 未审批 1-待审批 2- 未通过 9-审批通过(多个筛选条件)
        /// </summary>
        [DataMember]
        public virtual List<byte> ApprovalBzs
        {
            get;
            set;
        }
        /// <summary>
        /// 最新关联单据编码
        /// </summary>
        [DataMember]
        public virtual String GkPaymentCode
        {
            get;
            set;
        }
        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        public virtual String UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 当前该用户是否能审核
        /// </summary>
        [DataMember]
        public virtual int IsApprovalNow
        {
            get;
            set;
        }
        /// <summary>
		/// 关联单据主键
		/// </summary>
		[DataMember]
        public virtual System.Int64 RefbillPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 关联单据类型
        /// </summary>
        [DataMember]
        public virtual System.String FBilltype
        {
            get;
            set;
        }

        /// <summary>
        /// 流程表主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 ProcPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位表主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 PostPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 审批记录主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 AppvalPhid
        {
            get;
            set;
        }
        /// <summary>
        /// 审批操作员主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 OperaPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 是否能批量审批的标志（1-是，0-否）
        /// </summary>
        [DataMember]
        public virtual System.Int64 BatchPracBz 
        {
            get;
            set;
        }
    }
}