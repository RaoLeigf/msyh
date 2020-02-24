using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SUP.Common.Base
{
    public interface IRichHelpTreeList
    {

        /// <summary>
        /// 获取树列表
        /// </summary>
        /// <param name="helpid">帮助标记</param>
        /// <param name="clientQuery">客户端json查询条件</param>
        /// <param name="outJsonQuery">客户端json查询条件</param>
        /// <param name="leftLikeJsonQuery">客户端json查询条件，*%</param>
        /// <param name="clientSqlFilter">客户端sql查询条件</param>
        /// <param name="nodeid">懒加载时的根节点id</param>
        /// <returns></returns>
        DataTable GetTreeList(string helpid, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, string nodeid);
    }
}
