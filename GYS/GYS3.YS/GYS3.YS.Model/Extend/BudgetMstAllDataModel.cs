using GYS3.YS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GYS3.YS.Model.Extend
{
    /// <summary>
    /// 关于预算的实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class BudgetMstAllDataModel
    {

        /// <summary>
        /// 预算主表对象
        /// </summary>
        [DataMember]
        public virtual  BudgetMstModel BudgetMst 
        {
            get;
            set;
        }

        [DataMember]
        public virtual List<BudgetDtlImplPlanModel> BudgetDtlImplPlans
        {
            get;
            set;
        }

 
        [DataMember]
        public virtual List<BudgetDtlTextContentModel> BudgetDtlTextContents
        {
            get;
            set;
        }

        [DataMember]
        public virtual List<BudgetDtlFundApplModel> BudgetDtlFundAppls
        {
            get;
            set;
        }

        [DataMember]
        public virtual List<BudgetDtlBudgetDtlModel> BudgetDtlBudgetDtls
        {
            get;
            set;
        }

        [DataMember]
        public virtual List<BudgetDtlPurchaseDtlModel> BudgetDtlPurchaseDtls
        {
            get;
            set;
        }

        [DataMember]
        public virtual List<BudgetDtlPurDtl4SOFModel> BudgetDtlPurDtl4SOFs
        {
            get;
            set;
        }

        [DataMember]
        public virtual List<BudgetDtlPerformTargetModel> BudgetDtlPerformTargets
        {
            get;
            set;
        }

    }

}