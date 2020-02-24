using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.DataEntity.Individual;

namespace SUP.Common.Facade
{
    public interface IQueryPanelFacade
    {
        /// <summary>
        /// 保存用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="ClientJsonString"></param>
        /// <returns></returns>
        int SetQueryPanelData(string PageId, string ClientJsonString);

        /// <summary>
        /// 获取用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <returns></returns>
        DataTable GetQueryPanelData(string PageId);

        string GetSortFilterData(string pageid, string ocode, string logid);

        IList<ExtControlInfoBase> GetIndividualQueryPanel(string pageId, string ocode, string logid);

        DataTable GetIndividualQueryPanelInfo(string pageid, string ocode, string logid);

        int SaveQueryInfo(DataTable dtchange, DataTable dt, string pageid, string ocode, string logid);
        string GetLang();

        string GetCheckData(string pageid, string ocode, string logid);

        int SaveCheckData(string pageid, string ocode, string logid, string ischeck);

        int RestoreDefault(string pageid, string ocode, string logid);
    }
}
