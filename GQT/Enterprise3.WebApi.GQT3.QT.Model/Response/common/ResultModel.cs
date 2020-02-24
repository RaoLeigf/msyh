using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GQT3.QT.Model
{
    /// <summary>
    /// 返回类型的集合
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class ResultModel
    {
        /// <summary>
        /// 返回状态 success error
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string Msg { get; set; }
        /// <summary>
        /// 数据集
        /// </summary>
        [DataMember]
        public object Data { get; set; }
    }
}
