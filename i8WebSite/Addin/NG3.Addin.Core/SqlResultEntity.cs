using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core
{
    [Serializable]
    [DataContract(Namespace = "")]
    public class SqlResultEntity
    {
        private Dictionary<string, object> _dic;
        public Dictionary<string, object> ExtendObjects
        {
            set { _dic = value; }
            get { return _dic; }
        }
    }
}
