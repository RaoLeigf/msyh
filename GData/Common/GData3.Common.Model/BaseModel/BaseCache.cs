using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Model
{
    public class BaseCache
    {
        /// <summary>
        /// token
        /// </summary>
        public System.String Token { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public System.String AppKey { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        public System.String AppSecret { get; set; }
    }
}
