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
    public class SupportFunctionBizModel
    {
        private string _funcName;
        private string _funcDesc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        public SupportFunctionBizModel(string name, string desc)
        {
            _funcName = name;
            _funcDesc = desc;
        }

        /// <summary>
        /// 函数名称
        /// </summary>
        [DataMember]
        public string FuncName {
            set
            { _funcName = value; }
            get
            { return _funcName; }
        }
        /// <summary>
        /// 函数描述
        /// </summary>
        [DataMember]
        public string FuncDesc
        {
            set
            { FuncDesc = value; }
            get
            { return _funcDesc; }
        }
    }
}
