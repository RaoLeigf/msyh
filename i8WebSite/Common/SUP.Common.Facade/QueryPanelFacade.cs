using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Rule;
using System.Data;
using NG3;
using SUP.Common.DataEntity.Individual;
using Newtonsoft.Json;

namespace SUP.Common.Facade
{
    public class QueryPanelFacade : IQueryPanelFacade
    {
        /// <summary>
        /// 
        /// </summary>
        private QueryPanelRule queryPanelRule;
        /// <summary>
        /// 
        /// </summary>
        public QueryPanelFacade()
        {
            queryPanelRule = new QueryPanelRule();
        }

        /// <summary>
        /// 保存用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <param name="ClientJsonString"></param>
        /// <returns></returns>
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SetQueryPanelData(string PageId, string ClientJsonString)
        {
            return queryPanelRule.SetQueryPanelData(PageId, ClientJsonString);
        }

        /// <summary>
        /// 获取用户查询数据
        /// </summary>
        /// <param name="PageId"></param>
        /// <returns></returns>
        [DBControl]
        public DataTable GetQueryPanelData(string PageId)
        {
            return queryPanelRule.GetQueryPanelData(PageId);
        }

        /// <summary>
        /// 获取用户排序数据
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public string GetSortFilterData(string pageid, string ocode, string logid)
        {
            return queryPanelRule.GetSortFilterData(pageid, ocode, logid);
        }

        /// <summary>
        /// 获取内嵌查询面板的记忆搜索按钮的check状态
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public string GetCheckData(string pageid, string ocode, string logid)
        {
            return queryPanelRule.GetCheckData(pageid, ocode, logid);
        }

        /// <summary>
        /// 保存内嵌查询面板的记忆搜索按钮的check状态
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public int SaveCheckData(string pageid, string ocode, string logid, string ischeck)
        {
            return queryPanelRule.SaveCheckData(pageid, ocode, logid,ischeck);
        }

        /// <summary>
        /// 获取注册的查询面板
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        [DBControl]
        public IList<ExtControlInfoBase> GetIndividualQueryPanel(string pageId, string ocode, string logid)
        {
            return queryPanelRule.GetIndividualQueryPanel(pageId, ocode, logid);
        }

        /// <summary>
        /// 获取用户自定义查询信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public DataTable GetIndividualQueryPanelInfo(string pageid, string ocode, string logid)
        {
            return queryPanelRule.GetIndividualQueryPanelInfo(pageid, ocode, logid);
        }

        /// <summary>
        /// 保存查询面板自定义信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="helpid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public int SaveQueryInfo(DataTable dtchange, DataTable dt, string pageid, string ocode, string logid)
        {
            return queryPanelRule.SaveQueryInfo(dtchange, dt, pageid, ocode, logid);
        }

        [DBControl]
        public string GetLang()
        {
            Dictionary<string,string> ToolbarlangDic = queryPanelRule.GetLang();
            string langJson = JsonConvert.SerializeObject(ToolbarlangDic);
            return langJson;
        }

        //内嵌查询恢复默认
        [DBControl]
        public int RestoreDefault(string pageid, string ocode, string logid)
        {
            return queryPanelRule.RestoreDefault(pageid, ocode, logid);
        }
    }
}
