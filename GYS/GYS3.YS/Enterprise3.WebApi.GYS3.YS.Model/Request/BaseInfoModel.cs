using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class BaseInfoModel<T> : BaseListModel where T : class
    {

        /// <summary>
        ///对象序列化信息
        /// </summary>
        [Description("对象序列化信息")]
        public T infoData { set; get; }

        ///// <summary>
        ///// 申请单据主键
        ///// </summary>
        //[DataMember]
        //public string fPhId
        //{
        //    set;
        //    get;
        //}

        ///// <summary>
        ///// 单据号集合
        ///// </summary>
        //[DataMember]
        //public string[] fPhIdList
        //{
        //    set;
        //    get;
        //}

    }
}
