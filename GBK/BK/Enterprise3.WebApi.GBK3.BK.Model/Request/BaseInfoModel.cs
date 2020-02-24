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
    public class BaseInfoModel<T> : BaseListModel where T : class
    {

        /// <summary>
        ///对象序列化信息
        /// </summary>
        [Description("对象序列化信息")]
        [DataMember]
        public T infoData { set; get; }

    }
}
