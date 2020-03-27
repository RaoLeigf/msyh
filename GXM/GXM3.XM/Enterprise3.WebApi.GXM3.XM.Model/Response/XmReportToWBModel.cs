using GXM3.XM.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GXM3.XM.Model.Response
{
    /// <summary>
    /// 返回给java网报系统的签报数据
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class XmReportToWBModel
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
		/// 签报单名称（签报事项）
		/// </summary>
		[DataMember]
        public virtual System.String FTitle
        {
            get;
            set;
        }


        /// <summary>
        /// 签报单编码
        /// </summary>
        [DataMember]
        public virtual System.String FCode
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
        /// 预算部门（网报中的报账部门）
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetDept
        {
            get;
            set;
        }

        /// <summary>
        /// 预算部门名称（网报中的报账部门）
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetDept_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 该签报单申请金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 固定额度
        /// </summary>
        [DataMember]
        public virtual System.Decimal FixedAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 变动额度
        /// </summary>
        [DataMember]
        public virtual System.Decimal VariableAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 明细集合
        /// </summary>
        [DataMember]
        public List<XmReportDtlToWBModel> xmReportDtls
        {
            get;
            set;
        }
        
    }
}
