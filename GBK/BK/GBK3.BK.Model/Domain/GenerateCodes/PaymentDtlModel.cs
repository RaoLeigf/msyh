#region Summary
/**************************************************************************************
    * 类 名 称：        PaymentDtlModel
    * 命名空间：        GBK3.BK.Model.Domain
    * 文 件 名：        PaymentDtlModel.cs
    * 创建时间：        2019/5/23 
    * 作    者：        刘杭    
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

namespace GBK3.BK.Model.Domain
{
    /// <summary>
    /// PaymentDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PaymentDtlModel : EntityBase<PaymentDtlModel>
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
		/// 资金拨付主表
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 资金拨付项目
		/// </summary>
		[DataMember]
		public virtual System.Int64 PayXmPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 预算项目主表
		/// </summary>
		[DataMember]
		public virtual System.Int64 XmMstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 预算明细主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 BudgetdtlPhid
		{
			get;
			set;
		}

        /// <summary>
        /// 预算明细名称
        /// </summary>
        [DataMember]
        public virtual System.String BudgetdtlName
        {
            get;
            set;
        }

        /// <summary>
        /// 补助单位/部门
        /// </summary>
        [DataMember]
		public virtual System.String FDepartmentcode
		{
			get;
			set;
		}

		/// <summary>
		/// 补助单位名称
		/// </summary>
		[DataMember]
		public virtual System.String FDepartmentname
		{
			get;
			set;
		}

		/// <summary>
		/// 申请金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 0-待支付 1-支付异常  9-支付成功
		/// </summary>
		[DataMember]
		public virtual System.Int32 FPayment
		{
			get;
			set;
		}

		/// <summary>
		/// 支付日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FPaymentdate
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String FRemarks
		{
			get;
			set;
		}

		/// <summary>
		/// 预算科目代码
		/// </summary>
		[DataMember]
		public virtual System.String QtKmdm
		{
			get;
			set;
		}

		/// <summary>
		/// 1-同行 2-跨行
		/// </summary>
		[DataMember]
		public virtual System.Int32 FTransfertype
		{
			get;
			set;
		}

		/// <summary>
		/// 银行类型
		/// </summary>
		[DataMember]
		public virtual System.String FBanktype
		{
			get;
			set;
		}

		/// <summary>
		/// 收款方账户名称
		/// </summary>
		[DataMember]
		public virtual System.String FBankname
		{
			get;
			set;
		}

		/// <summary>
		/// 收款账号
		/// </summary>
		[DataMember]
		public virtual System.String FAccount
		{
			get;
			set;
		}

		/// <summary>
		/// 银行行号
		/// </summary>
		[DataMember]
		public virtual System.String FBankcode
		{
			get;
			set;
		}

        /// <summary>
        /// 预算科目名称
        /// </summary>
        [DataMember]
        public virtual System.String QtKmmc
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