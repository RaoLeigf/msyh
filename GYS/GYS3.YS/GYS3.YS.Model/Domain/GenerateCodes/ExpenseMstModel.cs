#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseMstModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        ExpenseMstModel.cs
    * 创建时间：        2019/1/24 
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

using GYS3.YS.Model.Enums;

namespace GYS3.YS.Model.Domain
{
    /// <summary>
    /// ExpenseMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ExpenseMstModel : EntityBase<ExpenseMstModel>
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
		public virtual System.String FProjcode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjname
		{
			get;
			set;
		}

		/// <summary>
		/// 申报单位
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationunit
		{
			get;
			set;
		}

		/// <summary>
		/// 申报单位名称
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationunit_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 预算部门
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationDept
		{
			get;
			set;
		}

        /// <summary>
        /// 预算部门名称
        /// </summary>
        [DataMember]
		public virtual System.String FDeclarationDept_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 申报部门
		/// </summary>
		[DataMember]
		public virtual System.String FBudgetDept
		{
			get;
			set;
		}

		/// <summary>
		/// 申报部门名称
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
		/// 项目类型名称
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
		public virtual System.Int32 FProjstatus
		{
			get;
			set;
		}

		/// <summary>
		/// 开始日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FStartdate
		{
			get;
			set;
		}

		/// <summary>
		/// 结束日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FEnddate
		{
			get;
			set;
		}

		/// <summary>
		/// 申报日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FDateofdeclaration
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
		/// 年初预算金额金额
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
		public virtual System.Decimal FBudgetamount
		{
			get;
			set;
		}

        /// <summary>
        /// 项目代码的数量标识
        /// </summary>
        [DataMember]
		public virtual System.Int32 FIfperformanceappraisal
		{
			get;
			set;
		}

		/// <summary>
		/// 是否额度核销（0否1是）
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
		public virtual System.DateTime? FMeetingtime
		{
			get;
			set;
		}

		/// <summary>
		/// 会议纪要编号
		/// </summary>
		[DataMember]
		public virtual System.String FMeetiingsummaryno
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
		public virtual System.String FApprovestatus
		{
			get;
			set;
		}

		/// <summary>
		/// 调整版本号
		/// </summary>
		[DataMember]
		public virtual System.String FVerno
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
		public virtual System.Int32 FCarryover
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String FBillno
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
		public virtual System.DateTime? FApprovedate
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String FMidyearchange
		{
			get;
			set;
		}

		/// <summary>
		/// 单据状态（0正常单据;1作废(额度返还的原始单据);2额度返还单据）
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfpurchase
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String FPerformtype
		{
			get;
			set;
		}

		/// <summary>
		/// 单据号
		/// </summary>
		[DataMember]
		public virtual System.String FPerformevaltype
		{
			get;
			set;
		}

		/// <summary>
		/// 预计支出金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FSurplusamount
		{
			get;
			set;
		}

		/// <summary>
		/// 预计返回金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FReturnamount
		{
			get;
			set;
		}

		/// <summary>
		/// 可编报数
		/// </summary>
		[DataMember]
		public virtual System.Decimal FPlayamount
		{
			get;
			set;
		}

		/// <summary>
		/// 服务意向单位
		/// </summary>
		[DataMember]
		public virtual System.String FServiceDept
		{
			get;
			set;
		}

		/// <summary>
		/// 经费标准
		/// </summary>
		[DataMember]
		public virtual System.String FFundStandard
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
        /// 备注
        /// </summary>
        [DataMember]
        public virtual System.String Bz
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