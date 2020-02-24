#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceMstModel
    * 命名空间：        GJX3.JX.Model.Domain
    * 文 件 名：        PerformanceMstModel.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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
using GJX3.JX.Model.Enums;

namespace GJX3.JX.Model.Domain
{
    /// <summary>
    /// PerformanceMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PerformanceMstModel : EntityBase<PerformanceMstModel>
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
		/// 预算主表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 YSMstPhId
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
		/// 实施单位
		/// </summary>
		[DataMember]
		public virtual System.String FExploitingEntity
		{
			get;
			set;
		}

		/// <summary>
		/// 项目负责人
		/// </summary>
		[DataMember]
		public virtual System.String FProjectLeader
		{
			get;
			set;
		}

		/// <summary>
		/// 联系电话
		/// </summary>
		[DataMember]
		public virtual System.String FPhoneNumber
		{
			get;
			set;
		}

		/// <summary>
		/// 联系地址
		/// </summary>
		[DataMember]
		public virtual System.String FContactAddress
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
		/// 项目金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FProjAmount
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
		/// 会议时间
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
		/// 审核状态
		/// </summary>
		[DataMember]
		public virtual System.String FAuditStatus
		{
			get;
			set;
		}

		/// <summary>
		/// 审核人
		/// </summary>
		[DataMember]
		public virtual System.Int64 FAuditor
		{
			get;
			set;
		}

		/// <summary>
		/// 审核时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FAuditDate
		{
			get;
			set;
		}

		/// <summary>
		/// 评价日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FEvaluationDate
		{
			get;
			set;
		}

		/// <summary>
		/// 自评得分
		/// </summary>
		[DataMember]
		public virtual System.Decimal FEvaluationScore
		{
			get;
			set;
		}

		/// <summary>
		/// 评价结果
		/// </summary>
		[DataMember]
		public virtual System.String FEvaluationResult
		{
			get;
			set;
		}

		/// <summary>
		/// 填录人
		/// </summary>
		[DataMember]
		public virtual System.Int64 FInformant
		{
			get;
			set;
		}

		/// <summary>
		/// 填录人 名称
		/// </summary>
		[DataMember]
		public virtual System.String FInformantName
		{
			get;
			set;
		}
        /// <summary>
		/// 指标类型
		/// </summary>
		[DataMember]
        public virtual System.String FTargetTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 指标类型_名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode_EXName
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
		/// 项目类型名称
		/// </summary>
		[DataMember]
        public virtual System.String FExpenseCategory_EXName
        {
            get;
            set;
        }

        /// <summary>
		/// 邮编
		/// </summary>
		[DataMember]
        public virtual System.String FPostCode
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
        /// 评价部门
        /// </summary>
        [DataMember]
        public virtual System.String FEvaluationDept
        {
            get;
            set;
        }

        /// <summary>
        /// 评价部门名称
        /// </summary>
        [DataMember]
        public virtual System.String FEvaluationDept_EXName
        {
            get;
            set;
        }

        /// <summary>
		/// 抽评得分
		/// </summary>
		[DataMember]
        public virtual System.Decimal FCheckEvaluationScore
        {
            get;
            set;
        }

        /// <summary>
        /// 抽评结果
        /// </summary>
        [DataMember]
        public virtual System.String FCheckEvaluationResult
        {
            get;
            set;
        }

        ///// <summary>
        ///// 第三方评价部门
        ///// </summary>
        //[DataMember]
        //public virtual System.String FThirdEvaluationDept
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 第三方评级部门名称
        ///// </summary>
        //[DataMember]
        //public virtual System.String FThirdEvaluationDept_EXName
        //{
        //    get;
        //    set;
        //}

        /// <summary>
		/// 第三方评价得分
		/// </summary>
		[DataMember]
        public virtual System.Decimal FThirdCheckEvaluationScore
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方评价结果
        /// </summary>
        [DataMember]
        public virtual System.String FThirdCheckEvaluationResult
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
        /// 绩效项目类型代码名称
        /// </summary>
        [DataMember]
        public virtual System.String FPerformType_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方评价（1是2否）
        /// </summary>
        [DataMember]
        public virtual System.String FThird
        {
            get;
            set;
        }

        #region 绩效list页面显示用  数据库不存在实体列
        /// <summary>
		/// 实际执行数
		/// </summary>
		[DataMember]
        public virtual System.Decimal FActualAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 结余金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FBalanceAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 执行率
        /// </summary>
        [DataMember]
        public virtual System.Decimal FImplRate
        {
            get;
            set;
        }
        #endregion
    }

}