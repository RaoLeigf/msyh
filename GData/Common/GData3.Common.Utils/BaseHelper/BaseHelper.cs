using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    /// <summary>
    /// 通用帮助基类
    /// </summary>
    public class BaseHelper<T> where T: class
    {
        /// <summary>
        /// 对应唯一主键
        /// </summary>
        [JsonProperty("Id")]
        public Int64 Id { set; get; }

        /// <summary>
        /// 对应代码
        /// </summary>
        [JsonProperty("Code")]
        public String Code { set; get; }

        /// <summary>
        /// 对应名称
        /// </summary>
        [JsonProperty("Name")]
        public String Name { set; get; }

        public BaseHelper() { }

        /// <summary>
        /// 构造通用帮助对象
        /// </summary>
        /// <param name="t">对象（Model）</param>
        /// <param name="pers">对应参数的字典（例：{Id:"Phid",Code:"EnCode",Name:"OrgName"}）,顺序按照主键，编码，名称来</param>
        /// <returns>返回需要查找的SysOrganizeModel对象</returns>
        public BaseHelper(T t,Dictionary<string,string> pros) {
            this.Id=Int64.Parse(typeof(T).GetProperty(pros["Id"]).GetValue(t).ToString());
            this.Code = typeof(T).GetProperty(pros["Code"]).GetValue(t).ToString();
            this.Name = typeof(T).GetProperty(pros["Name"]).GetValue(t).ToString();
        }

        /// <summary>
        /// 构造通用帮助对象
        /// </summary>

        public BaseHelper(Int64 Id,object Code,object Name)
        {
            this.Id = Id;
            this.Code = Code.ToString();
            this.Name = Name.ToString();
        }
    }
}
