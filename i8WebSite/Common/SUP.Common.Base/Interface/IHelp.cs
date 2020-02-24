using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SUP.Common.Base
{
    public interface IHelp
    {

        /// <summary>
        /// 代码转名称
        /// </summary>
        /// <param name="helpid">帮助id</param>
        /// <param name="code">代码</param>
        /// <param name="helptype">帮助类型</param>
        /// <param name="clientQuery">客户端查询条件</param>
        /// <param name="outJsonQuery">外部条件</param>
        /// <returns></returns>
        string GetName(string helpid, string code, string helptype, string clientQuery, string outJsonQuery);

        /// <summary>
        /// 代码转名称
        /// </summary>
        /// <param name="InExpression">in语句后的内容：('1','2')</param>
        /// <returns></returns>
        DataTable CodeToName(string InExpression);
    }
}
