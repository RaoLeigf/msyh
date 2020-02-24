using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3;
using SUP.Common.DataAccess;
using System.Data;
using SUP.Common.DataEntity;
using SUP.Common.Rule;
using SUP.Common.Base;
using Newtonsoft.Json.Linq;

namespace SUP.Common.Facade
{
    public class RichHelpFacade : IRichHelpFacade
    {

        private RichHelpDac dac = null;
        private RichHelpRule rule = null;

        public RichHelpFacade() 
        {
            dac = new RichHelpDac();
            rule = new RichHelpRule();
        }

        /// <summary>
        /// 取得帮助节点信息
        /// </summary>
        /// <param name="helpflag"></param>
        /// <returns></returns>
        [DBControl]
        public CommonHelpEntity GetCommonHelpItem(string helpid)
        {
            return dac.GetCommonHelpItem(helpid);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="helpid">帮助标记</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="keys">搜索条件</param>
        /// <param name="treerefkey">树搜索字段</param>
        /// <param name="treesearchkey">树搜索条件</param>
        /// <param name="outJsonQuery">外部过滤条件</param>
        /// <param name="leftLikeJsonQuery">左like</param>
        /// <param name="clientSqlFilter">客户端查询条件</param>
        /// <param name="isAutoComplete">是否智能搜索</param>
        /// <returns></returns>                 
        [DBControl]
        public Object GetList(RichHelpListArgEntity entity, ref int totalRecord,bool ormMode)
        {
            return dac.GetList(entity, ref totalRecord, ormMode);
        }

        [DBControl]
        public DataTable GetDetailList(string helpid, string masterCode, bool ormMode)
        {
            return dac.GetDetailList(helpid, masterCode, ormMode);
        }


        /// <summary>
        /// 根据代码获取名称
        /// </summary>
        /// <param name="helpflag">帮助标记</param>
        /// <param name="code">代码值</param>
        /// <param name="clientQuery">查询条件</param>
        /// <returns></returns>
        [DBControl]
        public string GetName(string helpid, string code, string helptype, string clientQuery, string outJsonQuery)
        {
            return dac.GetName(helpid, code, string.Empty, clientQuery, outJsonQuery,helptype);
        }

        [DBControl]
        public HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list)
        {
            try
            {
                return new RichHelpRule().GetAllNames(list);
            }
            catch (Exception)
            {

                throw;
            }            
        }

        [DBControl]
        public IList<TreeJSONBase> GetTreeList(string helpid, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, string nodeid,bool ormMode)
        {
            return rule.GetTreeList(helpid, clientQuery, outJsonQuery, leftLikeJsonQuery, clientSqlFilter, nodeid, ormMode);
        }

        /// <summary>
        /// 获取分类的树型数据
        /// </summary>
        /// <param name="code">主键</param>
        /// <param name="nodeid">节点id</param>
        /// <returns></returns>
        [DBControl]
        public IList<TreeJSONBase> GetQueryProTree(string code, string nodeid)
        {
            return rule.GetQueryProTree(code, nodeid);
        }

        [DBControl]
        public DataTable GetCommonUseList(string helpid, string logid, string org, bool ormMode)
        {
            return dac.GetCommonUseList(helpid, logid, org, ormMode);
        }

        [DBControl]
        public int SaveCommonUseData(string helpid, string codeValue, string logid, string org)
        {
            return dac.SaveCommonUseData(helpid, codeValue, logid, org);
        }

        [DBControl]
        public int DeleteCommonUseData(string helpid, string codeValue, string logid, string org)
        {
            return dac.DeleteCommonUseData(helpid,codeValue,logid,org);
        }

        /// <summary>
        /// 获取树节点的记忆状态
        /// </summary>
        /// <param name="TreeType"></param>
        /// <param name="BussType"></param>
        /// <returns></returns>
        [DBControl]
        public DataEntity.TreeMemoryEntity GetTreeMemory(string type, string BussType = "all")
        {
            return dac.GetTreeMemory(type, BussType);
        }

        /// <summary>
        /// 更新树节点的记忆状态
        /// </summary>
        /// <param name="treeMemoryEntity"></param>
        /// <returns></returns>
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int UpdataTreeMemory(DataEntity.TreeMemoryEntity treeMemoryEntity)
        {
            return dac.UpdataTreeMemory(treeMemoryEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        [DBControl]
        public JArray GetRichQueryItems(string helpid)
        {
            return rule.GetRichQueryItems(helpid);
        }

        [DBControl]
        //public object GetRichQueryList(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, bool ormMode)
        public object GetRichQueryList(RichHelpListArgEntity entity, ref int totalRecord,bool ormMode)
        {
            return dac.GetRichQueryList(entity,ref totalRecord,ormMode);
        }

        [DBControl]
        public DataTable GetRichQueryUIInfo(string helpid, string ocode, string logid)
        {
            return dac.GetRichQueryUIInfo(helpid, ocode, logid);
        }

        [DBControl]
        public int SaveQueryInfo(DataTable dt, string helpid, string ocode, string logid)
        {
            return dac.SaveQueryInfo(dt, helpid, ocode, logid);
        }

        /// <summary>
        /// 查询条件记忆
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [DBControl]
        public int SaveQueryFilter(string helpid, string json)
        {
            return dac.SaveQueryFilter(helpid,json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="ocode"></param>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public JObject GetQueryFilter(string helpid, string ocode, string logid)
        {
            return rule.GetQueryFilter(helpid, ocode, logid);
        }

        [DBControl]
        public JObject GetListExtendInfo(string code)
        {
            return rule.GetListExtendInfo(code);
        }

        /// <summary>
        /// 验证用户输入数据的合法性
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        [DBControl]
        public bool ValidateData(string helpid, string inputValue,string clientSqlFilter, string selectMode, string helptype)
        {
            return dac.ValidateData(helpid, inputValue, clientSqlFilter,selectMode,helptype);
        }

        [DBControl]
        public DataTable GetSelectedData(string helpid, string codes, bool mode)
        {
            return dac.GetSelectedData(helpid, codes, mode);
        }
    }


    public interface IRichHelpFacade
    {
        object GetList(RichHelpListArgEntity entity, ref int totalRecord,bool ormMode);

        DataTable GetDetailList(string helpid, string masterCode, bool ormMode);

        string GetName(string helpid, string code, string helptype, string clientQuery, string outJsonQuery);

        HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list);

        CommonHelpEntity GetCommonHelpItem(string helpid);

        IList<TreeJSONBase> GetTreeList(string helpid, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, string nodeid,bool ormMode);

        IList<TreeJSONBase> GetQueryProTree(string code, string nodeid);

        DataTable GetCommonUseList(string helpid, string logid, string org, bool ormMode);

        int SaveCommonUseData(string helpid, string codeValue, string logid, string org);

        int DeleteCommonUseData(string helpid, string codeValue, string logid, string org);

        /// <summary>
        /// 获取树节点的记忆状态
        /// </summary>
        /// <param name="TreeType"></param>
        /// <param name="BussType"></param>
        /// <returns></returns>
        TreeMemoryEntity GetTreeMemory(string type, string BussType = "all");

        /// <summary>
        /// 更新树节点的记忆状态
        /// </summary>
        /// <param name="treeMemoryEntity"></param>
        /// <returns></returns>
        int UpdataTreeMemory(TreeMemoryEntity treeMemoryEntity);

        JArray GetRichQueryItems(string helpid);

        //Object GetRichQueryList(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, bool ormMode);
        object GetRichQueryList(RichHelpListArgEntity entity, ref int totalRecord, bool ormMode);

        DataTable GetRichQueryUIInfo(string helpid, string ocode, string logid);

        int SaveQueryInfo(DataTable dt, string helpid, string ocode, string logid);

        int SaveQueryFilter(string helpid, string json);

        JObject GetQueryFilter(string helpid, string ocode, string logid);

        JObject GetListExtendInfo(string code);

        bool ValidateData(string helpid, string inputValue, string clientSqlFilter, string selectMode, string helptype);

        DataTable GetSelectedData(string helpid, string codes, bool mode);
    }
}
