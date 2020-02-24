using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Enterprise3.WebApi.GJX3.JX.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class BaseListModel
    {
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
        /// 用户id
        /// </summary>
        [DataMember]
        public long UserId { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [DataMember]
        public long OrgId { get; set; }

        /// <summary>
        /// 组织编码
        /// </summary>
        [DataMember]
        public string OrgCode { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        [DataMember]
        public string Year { get; set; }

        /// <summary>
        ///queryfilter的格式：{"属性1*str*判断规则{B}*是否{C}":"值","Mc*str*eq*1":"x"}，直接调用I8的接口
        /// </summary>
        [DataMember]
        public string QueryFilter { get; set; }

        /// <summary>
        ///排序格式："属性名 升序/降序"
        /// </summary>
        [DataMember]
        public string[] Sort { get; set; }

        /// <summary>
        /// 组织集合
        /// </summary>
        [DataMember]
        public List<long> OrgIds { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        [DataMember]
        public string Ucode { get; set; }

        /// <summary>
        /// 单据号集合
        /// </summary>
        [DataMember]
        public List<long> fPhIdList
        {
            set;
            get;
        }

        /// <summary>
        /// 绩效主表主键
        /// </summary>
        [DataMember]
        public long performancePhid
        {
            set;
            get;
        }

        /// <summary>
        /// 预算表主键
        /// </summary>
        [DataMember]
        public long fBudgetPhid
        {
            set;
            get;
        }
    }
}