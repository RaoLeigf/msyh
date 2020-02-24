using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Model
{
    public class BaseToken
    {
        /// <summary>
        ///Token
        /// </summary>
        [JsonProperty("Token")]
        public String Token { set; get; }

        /// <summary>
        /// AppKey
        /// </summary>
        [JsonProperty("AppKey")]
        public String AppKey { set; get; }

        /// <summary>
        /// AppSecret
        /// </summary>
        [JsonProperty("AppSecret")]
        public String AppSecret { set; get; }
        /// <summary>
        /// 数据库信息
        /// </summary>
        [JsonProperty("DBName")]
        public String DBName { set; get; }
    }
}
