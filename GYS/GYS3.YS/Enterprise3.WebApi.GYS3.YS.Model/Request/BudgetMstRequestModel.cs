using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Extra;
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
    public class BudgetMstRequestModel : BaseListModel
    {
        /// <summary>
        /// 申报组织PHID
        /// </summary>
        [DataMember]
        public string UnitId { get; set; }

        /// <summary>
        /// 预算部门PHID
        /// </summary>
        [DataMember]
        public string DeptId { get; set; }
        
    }
    /// <summary>
    /// 
    /// </summary>
    public class BudgetMstListsRequestModel : BudgetMstModel
    {
        /// <summary>
        /// 用户userId
        /// </summary>
        [DataMember]
        public string UserId { get; set; }


        /// <summary>
        /// 用户Ucode
        /// </summary>
        [DataMember]
        public string Ucode { get; set; }


        /// <summary>
        /// 组织id
        /// </summary>
        [DataMember]
        public long OrgId { get; set; }

        /// <summary>
        /// 组织code
        /// </summary>
        [DataMember]
        public string OrgCode { get; set; }
        ///// <summary>
        ///// 审批状态（0-全部；1-待上报；2-审批中；3-审批通过；4-未通过）
        ///// </summary>
        //[DataMember]
        //public string FApproveStatus { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [DataMember]
        public string Year { get; set; }

        /// <summary>
        /// 预算主键
        /// </summary>
        [DataMember]
        public long FBudgetPhId { get; set; }

        /// <summary>
        /// 预算主键集合
        /// </summary>
        [DataMember]
        public List<long> FBudgetPhIdList { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        [DataMember]
        public int PageSize { set; get; }
        /// <summary>
        /// 当前页数
        /// </summary>
        [DataMember]
        public int PageIndex { set; get; }

        /// <summary>
        /// 项目金额起始
        /// </summary>
        [DataMember]
        public Decimal FProjAmountBegin { get; set; }
        /// <summary>
        /// 项目金额结束
        /// </summary>
        [DataMember]
        public Decimal FProjAmountEnd { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [DataMember]
        public string OrderBy { get; set; }

        /// <summary>
        /// 获取不同类型的预算数据（0-全部，1-年初申报， 2-年中新增，3-年中调整）
        /// </summary>
        [DataMember]
        public string BudgetType { set; get; }

        /// <summary>
        /// 单据类型
        /// </summary>
        [DataMember]
        public string workType { get; set; }

        /// <summary>
        /// 预算内容对象
        /// </summary>
        [DataMember]
        public BudgetDtlTextContentModel BudgetDtlTextContent { get; set; }

        /// <summary>
        /// 项目状态集合
        /// </summary>
        [DataMember]
        public List<int> projectarea
        {
            get;
            set;
        }
        /// <summary>
        /// 审批集合
        /// </summary>
        [DataMember]
        public List<string> fapprovearea
        {
            get;
            set;
        }

        /// <summary>
        /// 用来筛选的审批流
        /// </summary>
        [DataMember]
        public long ProcPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 搜索框
        /// </summary>
        [DataMember]
        public string SearchValue { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class BudgetAllDataRequest : BaseListModel
    {
        /// <summary>
        /// 项目主表对象
        /// </summary>
        [DataMember]
        public BudgetMstModel BudgetMst
        {
            get;
            set;
        }

        /// <summary>
        /// 项目明细对象
        /// </summary>
        [DataMember]
        public List<BudgetDtlBudgetDtlModel> BudgetDtlBudgetDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 项目资金申请明细
        /// </summary>
        [DataMember]
        public List<BudgetDtlFundApplModel> BudgetDtlFundAppls
        {
            get;
            set;
        }
        /// <summary>
        /// 项目实施计划
        /// </summary>
        [DataMember]
        public List<BudgetDtlImplPlanModel> BudgetDtlImplPlans
        {
            get;
            set;
        }

        /// <summary>
        /// 项目绩效目标
        /// </summary>
        [DataMember]
        public List<BudgetDtlPerformTargetModel> BudgetDtlPerformTargets
        {
            get;
            set;
        }

        /// <summary>
        /// 项目采购明细
        /// </summary>
        [DataMember]
        public List<BudgetDtlPurchaseDtlModel> BudgetDtlPurchaseDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 项目采购明细-资金来源
        /// </summary>
        [DataMember]
        public List<BudgetDtlPurDtl4SOFModel> BudgetDtlPurDtl4SOFs
        {
            get;
            set;
        }

        /// <summary>
        /// 项目可研（文字内容）
        /// </summary>
        [DataMember]
        public List<BudgetDtlTextContentModel> BudgetDtlTextContents
        {
            get;
            set;
        }

        /// <summary>
        /// 年中调整标志
        /// </summary>
        [DataMember]
        public string midYearEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 是否点击了缓存按钮
        /// </summary>
        [DataMember]
        public string IsCache
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
        /// 预算项目的绩效跟踪集合
        /// </summary>
        [DataMember]
        public virtual List<JxTrackingModel> JxTrackings
        {
            get;
            set;
        }

        /// <summary>
        /// 预算项目以及明细集合
        /// </summary>
        [DataMember]
        public virtual List<BudgetAllDataModel> BudgetAllDataModels
        {
            get;
            set;
        }
    }
}
