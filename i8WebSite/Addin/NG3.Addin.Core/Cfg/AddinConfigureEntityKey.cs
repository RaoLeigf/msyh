
using NG3.Addin.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Cfg
{
    public class AddinConfigureEntityKey
    {
        private string _classFullName;
        private string _methodName;
        private EnumInterceptorType _type;

        public AddinConfigureEntityKey(string classFullName,string methodName,EnumInterceptorType type)
        {
            _classFullName = classFullName;
            _methodName = methodName;
            _type = type;
        }
      
        public string GetKey()
        {

            return _classFullName + "_" + _methodName+"_"+_type;
        }

    }
}
