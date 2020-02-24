using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GYS3.YS.Model.Request
{
    /// <summary>
    /// 用款计划列表查询
    /// </summary>
    public class ExpenseListRequestModel:BaseListModel
    {
        /// <summary>
		/// 预算部门
		/// </summary>
		[DataMember]
        public virtual System.String FDeclarationDept
        {
            get;
            set;
        }

        /// <summary>
		/// 审批状态
		/// </summary>
		[DataMember]
        public virtual List<String> FApprovestatus
        {
            get;
            set;
        }

        /// <summary>
		/// 开始日期
		/// </summary>
		[DataMember]
        public virtual System.DateTime? FStartdate
        {
            get;
            set;
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FEnddate
        {
            get;
            set;
        }
        /// <summary>
        /// 最小金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal MinAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 最大金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal MaxAmount
        {
            get;
            set;
        }
    
        
        /// <summary>
        /// 搜索条件
        /// </summary>
        [DataMember]
        public virtual System.String searchValue
        {
            get;
            set;
        }

        /// <summary>
        /// 用来筛选的审批流
        /// </summary>
        [DataMember]
        public virtual System.Int64 ProcPhid
        {
            get;
            set;
        }
    }
}
