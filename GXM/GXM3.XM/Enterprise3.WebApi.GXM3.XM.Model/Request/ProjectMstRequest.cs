using GXM3.XM.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GXM3.XM.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectMstRequest : BaseListModel
    {
        /// <summary>
        /// 项目（1-预立项；2-立项；其他）
        /// </summary>
        [DataMember]
        public string ProjStatus { get; set; }

        /// <summary>
        /// 审批状态（0-全部；1-待上报；2-审批中；3-审批通过；4-未通过）
        /// </summary>
        [DataMember]
        public string FApproveStatus { get; set; }

        /// <summary>
        /// 支出类别（0-全部；1-主业类；2-企事业类；3-机关行政类）
        /// </summary>
        [DataMember]
        public string FExpenseCategory { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember]
        public string FProjName { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember]
        public string FProjCode { get; set; }

        /// <summary>
        /// 项目主键
        /// </summary>
        [DataMember]
        public long FProjPhId { get; set; }

        /// <summary>
        /// 项目编码集合
        /// </summary>
        [DataMember]
        public List<string> FProjCodeList { get; set; }

        /// <summary>
        /// 项目主键集合
        /// </summary>
        [DataMember]
        public List<long> FProjPhIdList { get; set; }

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
		/// 预算部门
		/// </summary>
		[DataMember]
        public virtual System.String FBudgetDept
        {
            get;
            set;
        }

        /// <summary>
		/// 项目编码/项目名称
		/// </summary>
		[DataMember]
        public virtual System.String SearchValue
        {
            get;
            set;
        }

        /// <summary>
        /// 项目金额起始
        /// </summary>
        [DataMember]
        public virtual string FProjAmountBegin
        {
            get;
            set;
        }
        /// <summary>
        /// 项目金额结束
        /// </summary>
        [DataMember]
        public virtual string FProjAmountEnd
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
		/// 是否绩效评价
		/// </summary>
		[DataMember]
        public virtual EnumYesNo FIfPerformanceAppraisal
        {
            get;
            set;
        }
        /// <summary>
        /// 项目状态集合
        /// </summary>
        [DataMember]
        public List<int>  projectarea
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

    }
}