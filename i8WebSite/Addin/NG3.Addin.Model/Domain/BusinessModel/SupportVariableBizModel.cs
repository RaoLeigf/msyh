using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Model.Domain.BusinessModel
{
    /// <summary>
    /// 支持的变量
    /// </summary>
   
    [DataContract(Namespace = "")]
    public class SupportVariableBizModel
    {

        private string _varName;
        private string _varDesc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        public SupportVariableBizModel(string name, string desc)
        {
            _varName = name;
            _varDesc = desc;
        }

        /// <summary>
        /// 函数名称
        /// </summary>
        [DataMember]
        public string VarName
        {
            set
            { _varName = value; }
            get
            { return _varName; }
        }
        /// <summary>
        /// 函数描述
        /// </summary>
        [DataMember]
        public string VarDesc
        {
            set
            { VarDesc = value; }
            get
            { return _varDesc; }
        }
    }
}
