using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.SUP.LoginValidation.Interface
{
    [Serializable]
    public class FilterResult
    {
        private bool _success = false;
        /// <summary>
        /// 检测是否成功
        /// </summary>
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        private string _errorMsg = string.Empty;
        /// <summary>
        /// 错误信息，失败的时候才有信息，否则为空
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }

        public FilterResult()
        {
        }
    }
}
