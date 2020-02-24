using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXM3.XM.Model
{
    /// <summary>
    /// 条数获取
    /// </summary>
    public class ProjectCountModel
    {
        /// <summary>
        /// 项目库 条数
        /// </summary>
        public long ProjectCount { get; set; }
        /// <summary>
        /// 预算库 条数
        /// </summary>
        public long BudgetCount { get; set; }
        /// <summary>
        /// 项目库审批 条数
        /// </summary>
        public long ProjectApproval { get; set; }
        /// <summary>
        /// 预算库审批 条数
        /// </summary>
        public long BudgetApproval { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Notice
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string datetime { get; set; }
    }
}
