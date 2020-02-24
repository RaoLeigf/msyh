using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SUP.Common.Base
{
    public interface ICommonHelp 
    {
       
        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="clientQuery">查询条件</param>
        /// <param name="outJsonQuery">外部条件</param>
        /// <param name="isAutoComplete">是否智能搜索</param>
        /// <returns></returns>
        DataTable GetHelpList(int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, bool isAutoComplete);
       
    }
}
