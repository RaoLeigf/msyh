using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SUP.Common.Base
{

    public interface IRichHelpList 
    {
        
        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="keys">查询关键字</param>
        /// <param name="treeSearchkey">左树过滤条件</param>
        /// <param name="outJsonQuery">外部注入条件</param>
        /// <param name="isAutoComplete">是否智能搜索</param>
        /// <returns></returns>
        DataTable GetHelpList(int pageSize, int pageIndex, ref int totalRecord, string[] keys, string treeSearchkey, string clientQuery, string outJsonQuery, bool isAutoComplete);
                
    }    

    public interface IRichHelpRichQueryList
    {        
        /// <summary>
        /// 获取高级查询额外组装的条件
        /// </summary>
        /// <param name="tables">表集合，以逗号分隔</param>
        /// <param name="query">附加的查询条件</param>
        //void GetRichQueryInfo(ref string tables, ref string query);

        /// <summary>
        /// 获取高级查询列表的数据
        /// </summary>
        /// <param name="helpid">帮助id</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="totalRecord">总行数</param>
        /// <param name="clientQuery">客户端json查询条件</param>
        /// <param name="outJsonQuery">外部查询条件</param>
        /// <returns></returns>
        DataTable GetRichQueryList(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery);
    }

    public interface IRichHelpProTree
    {        
        /// <summary>
        /// 获取查询属性树列表
        /// </summary>
        /// <param name="code">查询属性的主键fg_helpinfo_queryprop.code</param>
        /// <returns></returns>
        DataTable GetPropertyTreeList(string code);
    }

   
}
