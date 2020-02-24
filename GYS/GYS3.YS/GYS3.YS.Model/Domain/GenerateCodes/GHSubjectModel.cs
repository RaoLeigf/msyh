#region Summary
/**************************************************************************************
    * 类 名 称：        GHSubjectModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        GHSubjectModel.cs
    * 创建时间：        2018/11/26 
    * 作    者：        董泉伟    
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
    /// GHSubject实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GHSubjectModel : EntityBase<GHSubjectModel>
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
		/// 项目年度
		/// </summary>
		[DataMember]
		public virtual System.String FYear
		{
			get;
			set;
		}

		/// <summary>
		/// 项目代码
		/// </summary>
		[DataMember]
		public virtual System.String FProjCode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjName
		{
			get;
			set;
		}

		/// <summary>
		/// 申报单位
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationUnit
		{
			get;
			set;
		}

		/// <summary>
		/// 申报单位名称
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationUnit_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 申报部门
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationDept
		{
			get;
			set;
		}

		/// <summary>
		/// 申报部门名称
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationDept_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 预算部门
		/// </summary>
		[DataMember]
		public virtual System.String FBudgetDept
		{
			get;
			set;
		}

		/// <summary>
		/// 预算部门名称
		/// </summary>
		[DataMember]
		public virtual System.String FBudgetDept_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 项目属性
		/// </summary>
		[DataMember]
		public virtual System.String FProjAttr
		{
			get;
			set;
		}

		/// <summary>
		/// 存续期限
		/// </summary>
		[DataMember]
		public virtual System.String FDuration
		{
			get;
			set;
		}

		/// <summary>
		/// 支出
		/// </summary>
		[DataMember]
		public virtual System.String FExpenseCategory
		{
			get;
			set;
		}

		/// <summary>
		/// 项目状态
		/// </summary>
		[DataMember]
		public virtual System.Int32 FProjStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 开始日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FStartDate
		{
			get;
			set;
		}

		/// <summary>
		/// 结束日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FEndDate
		{
			get;
			set;
		}

		/// <summary>
		/// 申报日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FDateofDeclaration
		{
			get;
			set;
		}

		/// <summary>
		/// 申报人
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarer
		{
			get;
			set;
		}

		/// <summary>
		/// 项目金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FProjAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 预算金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FBudgetAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Int32 FifPerformanceAppraisal
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Int32 FifKeyEvaluation
		{
			get;
			set;
		}

		/// <summary>
		/// 会议日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FMeetingTime
		{
			get;
			set;
		}

		/// <summary>
		/// 会议编号
		/// </summary>
		[DataMember]
		public virtual System.String FMeetiingSummaryNo
		{
			get;
			set;
		}

		/// <summary>
		/// 单据类型
		/// </summary>
		[DataMember]
		public virtual System.String FType
		{
			get;
			set;
		}

		/// <summary>
		/// 审批状态
		/// </summary>
		[DataMember]
		public virtual System.String FApproveStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String FVerNo
		{
			get;
			set;
		}

		/// <summary>
		/// 版本号
		/// </summary>
		[DataMember]
		public virtual System.Int32 FLifeCycle
		{
			get;
			set;
		}

		/// <summary>
		/// 结转标志
		/// </summary>
		[DataMember]
		public virtual System.Int32 FCarryOver
		{
			get;
			set;
		}

		/// <summary>
		/// 单据编号
		/// </summary>
		[DataMember]
		public virtual System.String FBillNo
		{
			get;
			set;
		}

		/// <summary>
		/// 审批人
		/// </summary>
		[DataMember]
		public virtual System.Int64 FApprover
		{
			get;
			set;
		}

		/// <summary>
		/// 审批人名称
		/// </summary>
		[DataMember]
		public virtual System.String FApprover_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 审批时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FApproveDate
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String FmidYearChange
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Int32 FifPurchase
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String FPerformType
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String FPerformEvalType
		{
			get;
			set;
		}

        /// <summary>
		/// 下一审批岗位
		/// </summary>
		[DataMember]
        public virtual System.String FNextApprove
        {
            get;
            set;
        }


    }

}