using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise3.WebApi.GWF3.WF.Model.Request
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class BaseInfoModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        public string userid { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [DataMember]
        public string orgid { get; set; }
        /// <summary>
        /// 当前流程号
        /// </summary>
        public string piid { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string taskid { get; set; }

        /// <summary>
        /// 任务 类型
        /// </summary>
        [DataMember]
        public string flowType { set; get; }

        ///// <summary>
        /////对象序列化信息
        ///// </summary>
        //[Description("对象序列化信息")]
        //[DataMember]
        //public T infoData { set; get; }

    }
}
