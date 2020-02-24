#region Summary
/**************************************************************************************
    * 类 名 称：        GKPaymentMstModel
    * 命名空间：        GGK3.GK.Model.Domain
    * 文 件 名：        GKPaymentMstModel.cs
    * 创建时间：        2019/6/5 
    * 作    者：        李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;

namespace GGK3.GK.Model.Domain
{
    /// <summary>
    /// GKPaymentMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GKPaymentMstModel : EntityBase<GKPaymentMstModel>
    {
		/// <summary>
		/// 主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 组织主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 OrgPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 组织编码
		/// </summary>
		[DataMember]
		public virtual System.String OrgCode
		{
			get;
			set;
		}

		/// <summary>
		/// 关联业务单主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 RefbillPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 关联业务单号
		/// </summary>
		[DataMember]
		public virtual System.String RefbillCode
		{
			get;
			set;
		}

		/// <summary>
		/// 付款单号
		/// </summary>
		[DataMember]
		public virtual System.String FCode
		{
			get;
			set;
		}

		/// <summary>
		/// 支付方式
		/// </summary>
		[DataMember]
		public virtual System.Int64 FPaymethod
		{
			get;
			set;
		}


        /// <summary>
        /// 支付方式
        /// </summary>
        [DataMember]
        public virtual System.String FPaymethodCode
        {
            get;
            set;
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        [DataMember]
        public virtual System.String FPaymethodName
        {
            get;
            set;
        }

        /// <summary>
        /// 合计付款金额
        /// </summary>
        [DataMember]
		public virtual System.Decimal FAmountTotal
		{
			get;
			set;
		}

		/// <summary>
		/// 审批状态
		/// </summary>
		[DataMember]
		public virtual System.Byte FApproval
		{
			get;
			set;
		}

		/// <summary>
		/// 支付状态
		/// </summary>
		[DataMember]
		public virtual System.Byte FState
		{
			get;
			set;
		}

		/// <summary>
		/// 描述
		/// </summary>
		[DataMember]
		public virtual System.String FDescribe
		{
			get;
			set;
		}

		/// <summary>
		/// 支付时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FDate
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
		/// 年度
		/// </summary>
		[DataMember]
		public virtual System.String FYear
		{
			get;
			set;
		}

		/// <summary>
		/// 支付指令序号
		/// </summary>
		[DataMember]
		public virtual System.String FSeqno
		{
			get;
			set;
		}

		/// <summary>
		/// 提交支付操作员phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 FSubmitterId
		{
			get;
			set;
		}

		/// <summary>
		/// 提交支付时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FSubmitdate
		{
			get;
			set;
		}
        /// <summary>
        /// 是否作废
        /// </summary>
        [DataMember]
        public virtual System.Byte FDelete
        {
            get;
            set;
        }

    }

}