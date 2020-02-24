using GQT3.QT.Model.Domain;
using GXM3.XM.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GXM3.XM.Model.Extra
{
    /// <summary>
    /// 关于项目的实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class ProjectAllDataModel
    {

        /// <summary>
        /// 预算主表对象
        /// </summary>
        [DataMember]
        public virtual ProjectMstModel ProjectMst
        {
            get;
            set;
        }

        /// <summary>
        /// 预算明细对象
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlBudgetDtlModel> ProjectDtlBudgetDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 预算明细对象2(为民生银行做准备的)
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlBudgetDtlModel> ProjectDtlBudgetDtls2
        {
            get;
            set;
        }

        /// <summary>
        /// 预算资金申请明细
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlFundApplModel> ProjectDtlFundAppls
        {
            get;
            set;
        }
        /// <summary>
        /// 预算实施计划
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlImplPlanModel> ProjectDtlImplPlans
        {
            get;
            set;
        }

        /// <summary>
        /// 预算绩效目标
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPerformTargetModel> ProjectDtlPerformTargets
        {
            get;
            set;
        }

        /// <summary>
        /// 预算采购明细
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurchaseDtlModel> ProjectDtlPurchaseDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 预算采购明细-资金来源
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurDtl4SOFModel> ProjectDtlPurDtl4SOFs
        {
            get;
            set;
        }

        /// <summary>
        /// 预算可研（文字内容）
        /// </summary>
        [DataMember]
        public virtual ProjectDtlTextContentModel ProjectDtlTextContents
        {
            get;
            set;
        }

        /// <summary>
        /// 项目自身所带的附件
        /// </summary>
        [DataMember]
        public virtual List<QtAttachmentModel> ProjectAttachments
        {
            get;
            set;
        }

        /// <summary>
        /// 项目分摊详情
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPersonnelModel> ProjectDtlPersonnels
        {
            get;
            set;
        }


        /// <summary>
        /// 项目追加人员名单
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPersonNameModel> ProjectDtlPersonNames
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