using GYS3.YS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GYS3.YS.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpenseAllRequestModel : BaseListModel
    {
        /// <summary>
        /// 用款计划主表对象
        /// </summary>
        [DataMember]
        public virtual ExpenseMstModel ExpenseMst
        {
            get;
            set;
        }

        /// <summary>
        /// 用款计划明细对象
        /// </summary>
        [DataMember]
        public virtual List<ExpenseDtlModel> ExpenseDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 用款计划额度核销对象
        /// </summary>
        [DataMember]
        public virtual List<ExpenseHxModel> ExpenseHxs
        {
            get;
            set;
        }

        /// <summary>
        /// 年初预算金额
        /// </summary>
        [DataMember]
        public virtual string NCmoney
        {
            get;
            set;
        }

        /// <summary>
        /// 本单据初始预计支出金额
        /// </summary>
        [DataMember]
        public virtual string beforeSum
        {
            get;
            set;
        }

        /// <summary>
        /// 本单据初始预计返还金额
        /// </summary>
        [DataMember]
        public virtual string beforeFReturnamount
        {
            get;
            set;
        }

        /// <summary>
        /// 是否额度返还
        /// </summary>
        [DataMember]
        public virtual string Ifreturn
        {
            get;
            set;
        }

        #region//用款计划--wgg
        /// <summary>
        /// 用款计划的主键
        /// </summary>
        [DataMember]
        public virtual long ExpensePhId
        {
            get;
            set;
        }


        /// <summary>
        /// 预算部门
        /// </summary>
        [DataMember]
        public virtual string FBudgetDept
        {
            get;
            set;
        }

        /// <summary>
        /// 核定预算数（小）
        /// </summary>
        [DataMember]
        public virtual string FBudgetAmountMin
        {
            get;
            set;
        }
        /// <summary>
        /// 核定预算数（大）
        /// </summary>
        [DataMember]
        public virtual string FBudgetAmountMax
        {
            get;
            set;
        }

        /// <summary>
        /// 已编报数（小）
        /// </summary>
        [DataMember]
        public virtual string FProjAmountMin
        {
            get;
            set;
        }
        /// <summary>
        /// 已编报数（大）
        /// </summary>
        [DataMember]
        public virtual string FProjAmountMax
        {
            get;
            set;
        }

        /// <summary>
        /// 剩余可编报数（小）
        /// </summary>
        [DataMember]
        public virtual string FSurplusAmountMin
        {
            get;
            set;
        }
        /// <summary>
        /// 剩余可编报数（大）
        /// </summary>
        [DataMember]
        public virtual string FSurplusAmountMax
        {
            get;
            set;
        }

        /// <summary>
        /// 实际发生数（小）
        /// </summary>
        [DataMember]
        public virtual string FHappenAmountMin
        {
            get;
            set;
        }
        /// <summary>
        /// 实际发生数（大）
        /// </summary>
        [DataMember]
        public virtual string FHappenAmountMax
        {
            get;
            set;
        }
        #endregion
    }
}
