using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GQT3.QT.Model.Extra
{
    /// <summary>
    /// 申报部门项目汇总表的对象
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class QTProjectMstHZModel
    {
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
		/// 项目名称
		/// </summary>
		[DataMember]
        public virtual System.String FProjName
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
        /// 原预算科目
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetAccounts
        {
            get;
            set;
        }
        /// <summary>
        /// 原预算科目
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetAccounts_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 新预算科目
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetAccounts2
        {
            get;
            set;
        }
        /// <summary>
        /// 新预算科目
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetAccounts2_EXName
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
		/// 一上金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountTop1
        {
            get;
            set;
        }

        /// <summary>
		/// 调整金额（1下-1上）
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAdjustAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 调整金额（2上-1上）
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAdjustAmountS2S1
        {
            get;
            set;
        }

        /// <summary>
        /// 调整金额（2下-1上）
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAdjustAmountX2S1
        {
            get;
            set;
        }

        /// <summary>
		/// 一下金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountBottom1
        {
            get;
            set;
        }
        /// <summary>
        /// 调整金额（2上-1下）
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAdjustAmount2
        {
            get;
            set;
        }

        /// <summary>
        /// 调整金额（2下-1下）
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAdjustAmountX2X1
        {
            get;
            set;
        }

        /// <summary>
		/// 二上金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountTop2
        {
            get;
            set;
        }
        /// <summary>
        /// 调整金额（2下-2上）
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAdjustAmount3
        {
            get;
            set;
        }

        /// <summary>
		/// 二下金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountBottom2
        {
            get;
            set;
        }
    }
}
