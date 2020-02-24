using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.Base
{
    public interface IValidate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="inputValue"></param>
        /// <param name="clientSqlFilter"></param>
        /// <param name="selectMode"></param>
        /// <returns></returns>
        bool ValidateData(string helpid, string inputValue, string clientSqlFilter, string selectMode);
    }
}
