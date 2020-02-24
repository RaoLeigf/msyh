#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesModel
    * 命名空间：        GYS3.YS.Model
    * 文 件 名：        ExamplesModel.cs
    * 创建时间：        2015/9/21 
    * 作    者：        丰立新    
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

namespace GYS3.YS.Model.Extend
{
    /// <summary>
    /// Examples实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class BuDeptReportModel : EntityBase<BuDeptReportModel>
    {
        /// <summary>
        /// 项目年度
        /// </summary>
        [DataMember]
        public virtual System.String f_year
        {
            get;
            set;
        }

        /// <summary>
        /// 申报单位
        /// </summary>
        [DataMember]
        public virtual System.String f_DeclarationUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 申报单位名称
        /// </summary>
        [DataMember]
        public virtual System.String f_DeclarationUnitName
        {
            get;
            set;
        }

        /// <summary>
        /// 预算部门
        /// </summary>
        [DataMember]
        public virtual System.String f_BudgetDept
        {
            get;
            set;
        }
        
        /// <summary>
        /// 预算部门名称
        /// </summary>
        [DataMember]
        public virtual System.String f_BudgetDeptName
        {
            get;
            set;
        }
                
        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember]
        public virtual System.String f_ProjCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember]
        public virtual System.String f_ProjName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_ProjAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 预算金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_BudgetAmount
        {
            get;
            set;
        }
        
        /// <summary>
        /// 明细项目编码
        /// </summary>
        [DataMember]
        public virtual System.String f_DtlCode
        {
            get;
            set;
        }

        /// <summary>
        /// 明细项目名称
        /// </summary>
        [DataMember]
        public virtual System.String f_DtlName
        {
            get;
            set;
        }

        /// <summary>
        /// 明细项目金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 明细预算金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_DtlBuAmount
        {
            get;
            set;
        }
        

        /// <summary>
        /// 支出渠道
        /// </summary>
        [DataMember]
        public virtual System.String f_ExpensesChannel
        {
            get;
            set;
        }
        
        /// <summary>
        /// 支出渠道名称
        /// </summary>
        [DataMember]
        public virtual System.String f_ExpensesChannelName
        {
            get;
            set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        [DataMember]
        public virtual System.String f_Description
        {
            get;
            set;
        }
        
        /// <summary>
        /// 预算科目
        /// </summary>
        [DataMember]
        public virtual System.String f_BudgetAccounts
        {
            get;
            set;
        }

        /// <summary>
        /// 预算科目名称
        /// </summary>
        [DataMember]
        public virtual System.String f_BudgetAccountsName
        {
            get;
            set;
        }

        /// <summary>
        /// 上年预算金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_LastBudgetAmount
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
        /// 调增数
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_IncrementAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 调减数
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_DecrementAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 已用额度
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_UsedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 执行率
        /// </summary>
        [DataMember]
        public virtual System.Decimal f_ImplRate
        {
            get;
            set;
        }

    }

}