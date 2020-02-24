using GSP3.SP.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GSP3.SP.Model.Response
{
    /// <summary>
    /// 审批信息的view model
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class AppvalRecordVo: GAppvalRecordModel
    {

        /// <summary>
        /// 申报组织id
        /// </summary>
        [DataMember]
        public long OrgId { get; set; }

        /// <summary>
        /// 申报组织名称
        /// </summary>
        [DataMember]
        public string OrgName { get; set; }

        /// <summary>
        /// 申报组织编码
        /// </summary>
        [DataMember]
        public string OrgCode { get; set; }

        /// <summary>
        /// 申请单编号
        /// </summary>
        [DataMember]
        public string BNum { get; set; }

        /// <summary>
        /// 支付单编号
        /// </summary>
        [DataMember]
        public string PayNum { get; set; }

        /// <summary>
        /// 申请单名称
        /// </summary>
        [DataMember]
        public string BName { get; set; }

        /// <summary>
        /// 申请单名称
        /// </summary>
        [DataMember]
        public string BYear { get; set; }

        /// <summary>
        /// 申请金额
        /// </summary>
        [DataMember]
        public decimal BAccount { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [DataMember]
        public decimal PayAccount { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        [DataMember]
        public DateTime? BDate { get; set; }

        /// <summary>
        /// 单据的审批状态(0- 未审批 1-待审批 2- 未通过 9-审批通过)
        /// </summary>
        [DataMember]
        public int BStatus { get; set; }

        /// <summary>
        /// 申报说明
        /// </summary>
        [DataMember]
        public string BDescribe { get; set; }

        /// <summary>
        /// 停留时长
        /// </summary>
        [DataMember]
        public double StopHour { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        [DataMember]
        public string BBilltype { get; set; }

        /// <summary>
        /// 申报部门id
        /// </summary>
        [DataMember]
        public long DepId { get; set; }

        /// <summary>
        /// 申报部门名称
        /// </summary>
        [DataMember]
        public string DepName { get; set; }

        /// <summary>
        /// 申报部门编码
        /// </summary>
        [DataMember]
        public string DepCode { get; set; }

        /// <summary>
        /// 是否集中采购
        /// </summary>
        [DataMember]
        public string IfPurchase { get; set; }

        /// <summary>
        /// 绩效项目类型代码
        /// </summary>
        [DataMember]
        public string PerformType { get; set; }

        /// <summary>
        /// 单据类型（判断年初或年中的）
        /// </summary>
        [DataMember]
        public string djType { get; set; }

        
        /// <summary>
        /// 调整版本号（判断年初或年中的）
        /// </summary>
        [DataMember]
        public string djVerno { get; set; }



        /// <summary>
        /// 项目状态
        /// </summary>
        [DataMember]
        public string ProjStatus { get; set; }

        /// <summary>
        /// 预算部门(code)
        /// </summary>
        [DataMember]
        public string BudgetDept { get; set; }

        /// <summary>
        /// 预算部门
        /// </summary>
        [DataMember]
        public string BudgetName { get; set; }

        /// <summary>
        /// 申报人
        /// </summary>
        [DataMember]
        public string Declarer { get; set; }
    }
}
