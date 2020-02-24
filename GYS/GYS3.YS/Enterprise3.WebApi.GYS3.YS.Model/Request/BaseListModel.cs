using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GYS3.YS.Model
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
        public long uid { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [DataMember]
        public long orgid { get; set; }

        /// <summary>
        /// 组织编码
        /// </summary>
        [DataMember]
        public string orgCode { get; set; }

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
        /// 年度
        /// </summary>
        [DataMember]
        public string Year { get; set; }

        /// <summary>
        /// 操作员代码
        /// </summary>
        [DataMember]
        //public string UserCode { get; set; }
        public string UserCode { get; set; }


        /// <summary>
        /// 操作员代码
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 区分上报时间（1、年初 2、年中  3、年末）
        /// </summary>
        [DataMember]
        public string value
        {
            set;
            get;
        }

        /// <summary>
        /// 是否默认勾选本级组织(0-勾选，1-不勾选)
        /// </summary>
        [DataMember]
        public int ChooseOwn
        {
            set;
            get;
        }

        /// <summary>
        /// 模糊搜索字段
        /// </summary>
        [DataMember]
        public string Search
        {
            set;
            get;
        }

        /// <summary>
        /// 是否上报(1-上报，0-未上报)
        /// </summary>
        [DataMember]
        public int Verify
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

        /// <summary>
        /// 收入预算主键
        /// </summary>
        [DataMember]
        public long FPhId
        {
            set;
            get;
        }

        /// <summary>
        /// 单据号集合
        /// </summary>
        [DataMember]
        public List<long> FPhIds
        {
            set;
            get;
        }
    }
}
