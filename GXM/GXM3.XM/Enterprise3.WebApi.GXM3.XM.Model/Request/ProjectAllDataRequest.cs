using GXM3.XM.Model.Domain;
using GXM3.XM.Model.Extra;
using SUP.Common.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GXM3.XM.Model.Request
{
    /// <summary>
    /// 保存立项数据
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class ProjectAllDataRequest: BaseListModel
    {
        /// <summary>
        /// 项目主表对象
        /// </summary>
        [DataMember]
        public ProjectMstModel ProjectMst
        {
            get;
            set;
        }

        /// <summary>
        /// 项目明细对象
        /// </summary>
        [DataMember]
        public List<ProjectDtlBudgetDtlModel> ProjectDtlBudgetDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 项目资金申请明细
        /// </summary>
        [DataMember]
        public List<ProjectDtlFundApplModel> ProjectDtlFundAppls
        {
            get;
            set;
        }
        /// <summary>
        /// 项目实施计划
        /// </summary>
        [DataMember]
        public List<ProjectDtlImplPlanModel> ProjectDtlImplPlans
        {
            get;
            set;
        }

        /// <summary>
        /// 项目绩效目标
        /// </summary>
        [DataMember]
        public List<ProjectDtlPerformTargetModel> ProjectDtlPerformTargets
        {
            get;
            set;
        }

        /// <summary>
        /// 项目采购明细
        /// </summary>
        [DataMember]
        public List<ProjectDtlPurchaseDtlModel> ProjectDtlPurchaseDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 项目采购明细-资金来源
        /// </summary>
        [DataMember]
        public List<ProjectDtlPurDtl4SOFModel> ProjectDtlPurDtl4SOFs
        {
            get;
            set;
        }

        /// <summary>
        /// 项目可研（文字内容）
        /// </summary>
        [DataMember]
        public ProjectDtlTextContentModel ProjectDtlTextContents
        {
            get;
            set;
        }

        /// <summary>
        /// 项目人员分摊
        /// </summary>
        [DataMember]
        public List<ProjectDtlPersonnelModel> ProjectDtlPersonnels
        {
            get;
            set;
        }

        /// <summary>
        /// 项目维护人员名单集合
        /// </summary>
        [DataMember]
        public List<ProjectDtlPersonNameModel> ProjectDtlPersonNames
        {
            get;
            set;
        }


        /// <summary>
        /// 是否是修改
        /// </summary>
        [DataMember]
        public string MidEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 统一保存列表数据（民生银行）
        /// </summary>
        [DataMember]
        public List<ProjectAllDataModel> ProjectAllDataModels
        {
            get;
            set;
        }

        /// <summary>
        /// 签报单主对象
        /// </summary>
        [DataMember]
        public XmReportMstModel XmReportMst
        {
            get;
            set;
        }


        /// <summary>
        /// 签报单明细对象集合
        /// </summary>
        [DataMember]
        public List<XmReportDtlModel> XmReportDtls
        {
            get;
            set;
        }

    }
}