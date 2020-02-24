using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GQT3.QT.Model
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
        public string uid { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [DataMember]
        public string orgid { get; set; }

        /// <summary>
        ///queryfilter的格式：{"属性1*str*判断规则{B}*是否{C}":"值","Mc*str*eq*1":"x"}，直接调用I8的接口
        /// </summary>
        [DataMember]
        public string queryfilter { get; set; }

        /// <summary>
        ///排序格式："属性名 升序/降序"
        /// </summary>
        [DataMember]
        public string[] sort { get; set; }

        /// <summary>
        /// 组织编码
        /// </summary>
        [DataMember]
        public string orgCode { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        [DataMember]
        public string uCode { get; set; }

        /// <summary>
        /// 工作流类型（与单据类型一一对应）
        /// </summary>
        [DataMember]
        public string workType { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        [DataMember]
        public string Year { get; set; }

        /// <summary>
        /// 过程编码
        /// </summary>
        [DataMember]
        public string processCode { get; set; }

        /// <summary>
        /// 对应每个列表的编码
        /// </summary>
        [DataMember]
        public string TableCode { get; set; }

        /// <summary>
        /// 对应每个列表的名称
        /// </summary>
        [DataMember]
        public string TableName { get; set; }
    }
}
