using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SUP.Common.DataEntity
{
    /// <summary>
    /// 客户端返回状态参数
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]   
    public class ResponseResult
    {
        protected string status = ResponseStatus.Success;

        [DataMember]
        public object Msg { get; set; }
        [DataMember]
        public string Status 
        { 
            get{return status;}
            set { status = value; }
        }
        [DataMember]
        public object Data { get; set; }
    }

    /// <summary>
    /// 客户端返回状态参数
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]   
    public class ResponseResult<T>
    {
        protected string status = ResponseStatus.Success;
        [DataMember]
        public object Msg { get; set; }
        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        [DataMember]
        public T Data { get; set; }
    }

    /// <summary>
    /// 响应状态类
    /// </summary>
    public struct ResponseStatus
    {
       public static readonly string  Success = "success";
       public static readonly string  Error = "error";        
    }
}
