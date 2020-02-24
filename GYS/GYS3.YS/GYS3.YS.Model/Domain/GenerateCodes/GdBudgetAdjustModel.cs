using GYS3.YS.Model.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GYS3.YS.Model.Domain.GenerateCodes
{
    /// <summary>
    /// 广东调整分析表
    /// </summary>
  public  class GdBudgetAdjustModel: GdBaseListModel
    {
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
        /// 对应账套字段
        /// </summary>
        [DataMember]
        public virtual System.String FAccount
        {
            get;
            set;
        }

        /// <summary>
		/// 明细项目代码
		/// </summary>
		[DataMember]
        public virtual System.String FDtlCode
        {
            get;
            set;
        }

        /// <summary>
        /// 明细项目名称
        /// </summary>
        [DataMember]
        public virtual System.String FDtlName
        {
            get;
            set;
        }

        /// <summary>
        /// 预算科目
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetAccounts
        {
            get;
            set;
        }

        /// <summary>
        /// 预算科目名称
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetAccounts_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目预算金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FBudgetAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 项目预算已用金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FUseBudgetAmount
        {
            get;
            set;
        }


        /// <summary>
        /// 项目预算未用金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FNoUseBudgetAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 项目调整增加金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FZBudgetAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 项目调整减少金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FJBudgetAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 项目调整金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FBudgetAmountEdit
        {
            get;
            set;
        }

        /// <summary>
		/// 项目调整后金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FBudgetAmountAfterEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 项目执行率
        /// </summary>
        [DataMember]
        public virtual System.Decimal FBudgetRate
        {
            get;
            set;
        }
        /// <summary>
        /// 项目执行率(含在途)
        /// </summary>
        [DataMember]
        public virtual System.Decimal FZTBudgetRate
        {
            get;
            set;
        }


        /// <summary>
		/// 明细预算金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 明细预算已用金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FUseAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 明细预算未用金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FNoUseAmount
        {
            get;
            set;
        }
        /// <summary>
		/// 明细调整增加金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FZAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 调整减少金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FJAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 调整金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountEdit
        {
            get;
            set;
        }

        /// <summary>
		/// 调整后金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountAfterEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 项目明细执行率
        /// </summary>
        [DataMember]
        public virtual System.Decimal FRate
        {
            get;
            set;
        }
        /// <summary>
        /// 项目明细执行率(含在途)
        /// </summary>
        [DataMember]
        public virtual System.Decimal FZTRate
        {
            get;
            set;
        }

        /// <summary>
        /// 核算组织id
        /// </summary>
        [DataMember]
        public virtual System.String FAccountOrgId
        {
            get;
            set;
        }

        /// <summary>
        /// 核算组织编码
        /// </summary>
        [DataMember]
        public virtual System.String FAccountOrgCode
        {
            get;
            set;
        }

        /// <summary>
        /// 核算组织名称
        /// </summary>
        [DataMember]
        public virtual System.String FAccountOrgName
        {
            get;
            set;
        }

        #region//网报预留字段

        /// <summary>
        /// 借款申请占用金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FLoanAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 报销申请占用金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FSubmitAmount
        {
            get;
            set;
        }

        #endregion
    }
}
