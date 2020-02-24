using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GYS3.YS.Model.Extra
{
    /// <summary>
    /// 关于项目的实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class BudgetAllDataModel
    {

        /// <summary>
        /// 预算主表对象
        /// </summary>
        [DataMember]
        public virtual BudgetMstModel BudgetMst
        {
            get;
            set;
        }

        /// <summary>
        /// 预算明细对象
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlBudgetDtlModel> BudgetDtlBudgetDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 预算明细对象2
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlBudgetDtlModel> BudgetDtlBudgetDtls2
        {
            get;
            set;
        }


        /// <summary>
        /// 预算资金申请明细
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlFundApplModel> BudgetDtlFundAppls
        {
            get;
            set;
        }
        /// <summary>
        /// 预算实施计划
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlImplPlanModel> BudgetDtlImplPlans
        {
            get;
            set;
        }

        /// <summary>
        /// 预算绩效目标
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlPerformTargetModel> BudgetDtlPerformTargets
        {
            get;
            set;
        }

        /// <summary>
        /// 预算采购明细
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlPurchaseDtlModel> BudgetDtlPurchaseDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 预算采购明细-资金来源
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlPurDtl4SOFModel> BudgetDtlPurDtl4SOFs
        {
            get;
            set;
        }

        /// <summary>
        /// 预算可研（文字内容）
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlTextContentModel> BudgetDtlTextContents
        {
            get;
            set;
        }

        /// <summary>
        /// 项目自身所带的附件
        /// </summary>
        [DataMember]
        public virtual List<QtAttachmentModel> BudgetAttachments
        {
            get;
            set;
        }

        /// <summary>
        /// 预算项目的绩效跟踪集合
        /// </summary>
        [DataMember]
        public virtual List<JxTrackingModel> JxTrackings
        {
            get;
            set;
        }

        /// <summary>
        /// 预算人员维护名单
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlPersonNameModel> BudgetDtlPersonNames
        {
            get;
            set;
        }
        /// <summary>
        /// 预算分摊金额合计
        /// </summary>
        [DataMember]
        public virtual List<BudgetDtlPersonnelModel> BudgetDtlPersonnels
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是修改
        /// </summary>
        //[DataMember]
        //public virtual string MidEdit
        //{
        //    get;
        //    set;
        //}
    }
}