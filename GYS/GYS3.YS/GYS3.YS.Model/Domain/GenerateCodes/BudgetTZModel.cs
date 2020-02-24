#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetMstModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        BudgetMstModel.cs
    * 创建时间：        2018/8/30 
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
    /// BudgetMst实体定义类
    /// </summary>
    [Serializable] 
	[DataContract(Namespace = "")]
    public partial class BudgetTZModel : EntityBase<BudgetTZModel>
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
		/// 项目编码
		/// </summary>
		[DataMember]
        public virtual System.String FProjDtlCode
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
        /// 序号
        /// </summary>
        [DataMember]
        public virtual System.String FNum
        {
            get;
            set;
        }

        /// <summary>
        /// 项目明细
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
        /// 核算单位
        /// </summary>
        [DataMember]
        public virtual System.String FExpensesChannel
        {
            get;
            set;
        }
        /// <summary>
		/// 核算单位名称
		/// </summary>
		[DataMember]
        public virtual System.String FExpensesChannel_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 核定年初预算数
        /// </summary>
        [DataMember]
        public virtual System.Decimal FBudgetAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 调增数
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAmountAddEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 调减数
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAmountCutEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 调整后预算数
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAmountAfterEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 核定预算数
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAmountAfterEditApprove
        {
            get;
            set;
        }

        /// <summary>
        /// 项目已使用金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal UseAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 项目剩余金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal RemainAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 执行率
        /// </summary>
        [DataMember]
        public virtual System.Decimal FUserPer
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

    }

}