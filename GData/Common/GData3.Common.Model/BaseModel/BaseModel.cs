using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GData3.Common.Model
{
    /// <summary>
    ///请求参数基类
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        ///用户主键
        /// </summary>
        [Description("用户主键")]
        public string uid { get; set; }

        /// <summary>
        ///用户代码
        /// </summary>
        [Description("用户代码")]
        public string usercode { get; set; }

        /// <summary>
        ///组织主键
        /// </summary>
        [Description("组织主键")]
        public string orgid { get; set; }

        /// <summary>
        ///用户姓名
        /// </summary>
        [Description("用户姓名")]
        public string uname { get; set; }

        /// <summary>
        //当前年份
        /// </summary>
        [Description("当前年份")]
        public string Ryear { get; set; }

        /// <summary>
        // 请求时带的时间戳字段
        /// </summary>
        [Description("时间戳字段")]
        public string nowTimeRandom { get; set; }
    }
}
