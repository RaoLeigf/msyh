using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Frame.DataEntity
{
    /// <summary>
    /// 客户端返回状态参数
    /// </summary>
    public class ResponseResult
    {
        protected string status = ResponseStatus.Success;
        public object Msg { get; set; }
        public string Status 
        { 
            get{return status;}
            set { status = value; }
        }
        public object Data { get; set; }
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
