using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GBK3.BK.Model.Request
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
        /// 单据主键
        /// </summary>
        [DataMember]
        public long mstPhid { get; set; }

        
        /// <summary>
        /// 申请单据主键
        /// </summary>
        [DataMember]
        public string fPhId
        {
            set;
            get;
        }

        /// <summary>
        /// 单据号集合
        /// </summary>
        [DataMember]
        public string[] fPhIdList
        {
            set;
            get;
        }

    }
}
