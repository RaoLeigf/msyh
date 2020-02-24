using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Model.Domain.BusinessModel
{
    /// <summary>
    /// 服务方法调用上传到服务端的UI参数
    /// </summary>
    [DataContract(Namespace = "")]
    public class ServiceUIParamBizModel
    {
        /// <summary>
        /// 类名
        /// </summary>
        [DataMember]
        public string ClassName { set; get; }

        /// <summary>
        /// 方法名
        /// </summary>
        [DataMember]
        public string MethodName { set; get; }

        /// <summary>
        /// 参数名
        /// </summary>
        [DataMember]
        public string ParamName { set; get; }

        /// <summary>
        /// 参数值
        /// </summary>
        [DataMember]
        public string ParamValue { set; get; }


    }
}
