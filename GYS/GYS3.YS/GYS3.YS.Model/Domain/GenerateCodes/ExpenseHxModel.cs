#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseHxModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        ExpenseHxModel.cs
    * 创建时间：        2019/5/5 
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

namespace GYS3.YS.Model.Domain
{
    /// <summary>
    /// ExpenseHx实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ExpenseHxModel : EntityBase<ExpenseHxModel>
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
		/// 用款主表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 单据号
		/// </summary>
		[DataMember]
		public virtual System.String FCode
		{
			get;
			set;
		}

		/// <summary>
		/// 关联报销单据号
		/// </summary>
		[DataMember]
		public virtual System.String FContentCode
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Decimal FQty
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Decimal FPrice
		{
			get;
			set;
		}

		/// <summary>
		/// 核销金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String FSpecification
		{
			get;
			set;
		}

		/// <summary>
		/// 报销事由及相关说明
		/// </summary>
		[DataMember]
		public virtual System.String FRemark
		{
			get;
			set;
		}

		/// <summary>
		/// 核销日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FHxdateTime
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 Defint1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 Defint2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.DateTime? DefDate1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.DateTime? DefDate2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DefStr1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DefStr2
		{
			get;
			set;
		}

	}

}