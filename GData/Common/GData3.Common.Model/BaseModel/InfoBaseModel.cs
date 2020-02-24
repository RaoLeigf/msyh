using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GData3.Common.Model
{
    /// <summary>
    ///对象操作基础请求参数
    /// </summary>
    public class InfoBaseModel<T>: BaseModel where T:class
    {
        /// <summary>
        ///对象序列化信息
        /// </summary>
        [Description("对象序列化信息")]
        public T infoData { set; get; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Description("值")]
        public string value { get; set; }

    }
}
