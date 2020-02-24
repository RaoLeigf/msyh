#region Summary
/**************************************************************************************
    * 类 名 称：        PaymentMstModel
    * 命名空间：        GBK3.BK.Model.Domain
    * 文 件 名：        PaymentMstModel.cs
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
    /// PaymentMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PaymentMstModel : EntityBase<PaymentMstModel>
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
		/// 年度
		/// </summary>
		[DataMember]
		public virtual System.String FYear
		{
			get;
			set;
		}

		/// <summary>
		/// 申请编号
		/// </summary>
		[DataMember]
		public virtual System.String FCode
		{
			get;
			set;
		}

		/// <summary>
		/// 申请单名称
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

		/// <summary>
		/// 申请单位主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 FOrgphid
		{
			get;
			set;
		}

		/// <summary>
		/// 申请单位编码
		/// </summary>
		[DataMember]
		public virtual System.String FOrgcode
		{
			get;
			set;
		}

		/// <summary>
		/// 申请单位名称
		/// </summary>
		[DataMember]
		public virtual System.String FOrgname
		{
			get;
			set;
		}

		/// <summary>
		/// 申报部门主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 FDepphid
		{
			get;
			set;
		}

		/// <summary>
		/// 申报部门编码
		/// </summary>
		[DataMember]
		public virtual System.String FDepcode
		{
			get;
			set;
		}

		/// <summary>
		/// 申报部门名称
		/// </summary>
		[DataMember]
		public virtual System.String FDepname
		{
			get;
			set;
		}


        /// <summary>
        /// 预算部门主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 FBudphid
        {
            get;
            set;
        }

        /// <summary>
        /// 预算部门编码
        /// </summary>
        [DataMember]
        public virtual System.String FBudcode
        {
            get;
            set;
        }

        /// <summary>
        /// 预算部门名称
        /// </summary>
        [DataMember]
        public virtual System.String FBudname
        {
            get;
            set;
        }

        /// <summary>
        /// 合计申请金额
        /// </summary>
        [DataMember]
		public virtual System.Decimal FAmountTotal
		{
			get;
			set;
		}

		/// <summary>
		/// 申请日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FDate
		{
			get;
			set;
		}

		/// <summary>
		/// 0- 未审批 1-待审批 2- 未通过 9-审批通过
		/// </summary>
		[DataMember]
		public virtual System.Byte FApproval
		{
			get;
			set;
		}

        /// <summary>
        /// 0- 否 1-支付异常 9-支付完成
        /// </summary>
        [DataMember]
		public virtual System.Byte IsPay
		{
			get;
			set;
		}

		/// <summary>
		/// 申报说明
		/// </summary>
		[DataMember]
		public virtual System.String FDescribe
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
        /// 是否作废
        /// </summary>
        [DataMember]
        public virtual System.Byte FDelete
        {
            get;
            set;
        }
        /// <summary>
        /// 开始筛选日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 结束筛选日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 筛选金额上线
        /// </summary>
        [DataMember]
        public virtual System.String MaxAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 筛选金额下线
        /// </summary>
        [DataMember]
        public virtual System.String MinAmount
        {
            get;
            set;
        }
    }

}