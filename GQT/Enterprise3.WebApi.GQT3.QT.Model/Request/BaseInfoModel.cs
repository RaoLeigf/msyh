using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class BaseInfoModel<T> : BaseListModel where T : class
    {

        /// <summary>
        ///对象序列化信息
        /// </summary>
        [Description("对象序列化信息")]
        public T infoData { set; get; }

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

        /// <summary>
        /// 采购相关数据的类型（1-Catalog采购目录,2-Procedures采购程序,3-Type采购类型）
        /// </summary>
        [DataMember]
        public string ProcurType { get; set; }

        /// <summary>
        /// 绩效相关数据的类型 （1-Type评价类型，2-评价指标类别）
        /// </summary>
        [DataMember]
        public string PerformType { get; set; }


    }
}
