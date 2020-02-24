#region Summary
/**************************************************************************************
    * 类 名 称：        QTProjectMstModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProjectMstModel.cs
    * 创建时间：        2019/9/4 
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

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// QTProjectMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProjectMstModel : EntityBase<QTProjectMstModel>
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
		/// 项目编码
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
		/// 项目类型
		/// </summary>
		[DataMember]
		public virtual System.String FExpenseCategory
		{
			get;
			set;
		}
        /// <summary>
		/// 项目类型
		/// </summary>
		[DataMember]
        public virtual System.String FExpenseCategory_EXName
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
		/// 是否绩效评价
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfPerformanceAppraisal
		{
			get;
			set;
		}

		/// <summary>
		/// 是否重点评价
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfKeyEvaluation
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
		/// 会议纪要编号
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
		/// 调整版本号
		/// </summary>
		[DataMember]
		public virtual System.String FVerNo
		{
			get;
			set;
		}

		/// <summary>
		/// 版本标识
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
		/// 审批时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FApproveDate
		{
			get;
			set;
		}

		/// <summary>
		/// 单据调整判断
		/// </summary>
		[DataMember]
		public virtual System.String FMidYearChange
		{
			get;
			set;
		}

		/// <summary>
		/// 是否集中采购
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfPurchase
		{
			get;
			set;
		}

		/// <summary>
		/// 绩效项目类型代码
		/// </summary>
		[DataMember]
		public virtual System.String FPerformType
		{
			get;
			set;
		}

		/// <summary>
		/// 绩效评价类型代码
		/// </summary>
		[DataMember]
		public virtual System.String FPerformEvalType
		{
			get;
			set;
		}

		/// <summary>
		/// 生成到老G6H记录
		/// </summary>
		[DataMember]
		public virtual System.Int32 FSaveToOldG6h
		{
			get;
			set;
		}

		/// <summary>
		/// 项目级别
		/// </summary>
		[DataMember]
		public virtual System.String FLevel
		{
			get;
			set;
		}

		/// <summary>
		/// 自定义模板主键
		/// </summary>
		[DataMember]
		public virtual System.String FIndividualinfophid
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

		/// <summary>
		/// 执行年度
		/// </summary>
		[DataMember]
		public virtual System.String FGoYear
		{
			get;
			set;
		}

		/// <summary>
		/// 支出类别（1项目支出2基本支出）
		/// </summary>
		[DataMember]
		public virtual System.String FZcType
		{
			get;
			set;
		}

		/// <summary>
		/// 临时项目（1是2否）
		/// </summary>
		[DataMember]
		public virtual System.Int32 FTemporary
		{
			get;
			set;
		}

		/// <summary>
		/// 时间进度
		/// </summary>
		[DataMember]
		public virtual System.String FProcessstatus
		{
			get;
			set;
		}

		/// <summary>
		/// 明细阶段
		/// </summary>
		[DataMember]
		public virtual System.String FDtlstage
		{
			get;
			set;
		}

        /// <summary>
		/// 明细阶段
		/// </summary>
		[DataMember]
        public virtual System.String FDtlstage_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 暂存时间
        /// </summary>
        [DataMember]
		public virtual System.DateTime? FTemporarydate
		{
			get;
			set;
		}

        /// <summary>
        /// 账套
        /// </summary>
        [DataMember]
        public virtual System.String FAccount
        {
            get;
            set;
        }
        /// <summary>
		/// 年结标志
		/// </summary>
		[DataMember]
        public virtual System.String FReference
        {
            get;
            set;
        }
        /// <summary>
        /// 活动地点
        /// </summary>
        [DataMember]
        public virtual System.String FVenue
        {
            get;
            set;
        }
        /// <summary>
		/// 参加对象
		/// </summary>
		[DataMember]
        public virtual System.String FParticipants
        {
            get;
            set;
        }
    }

}