using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Model.Domain.BusinessModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "")]
    public class InterceptedServiceBizModel
    {
        /// <summary>
        /// 服务名
        /// </summary>
        [DataMember]
        public System.String ServiceName { set; get; }
        /// <summary>
        /// 服务功能名
        /// </summary>
        [DataMember]
        public System.String ServiceFuncName { set; get; }
        /// <summary>
        /// 目标Assembly名
        /// </summary>
        [DataMember]
        public System.String TargetAssemblyName { set; get; }

        /// <summary>
        /// 目标类名
        /// </summary>
        [DataMember]
        public System.String TargetClassName { set; get; }
        /// <summary>
        /// 目标方法名
        /// </summary>
        [DataMember]
        public System.String TargetMethodName { set; get; }

        /// <summary>
        /// 匹配条件
        /// </summary>
        [DataMember]
        public System.String MatchCondition { set; get; }
    }
}
