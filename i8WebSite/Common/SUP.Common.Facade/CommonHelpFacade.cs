using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3;

using SUP.Common.DataAccess;
using SUP.Common.Rule;
using SUP.Common.DataEntity;

namespace SUP.Common.Facade
{
    public class CommonHelpFacade : ICommonHelpFacade
    {

        private CommonHelpDac dac = null;

        public CommonHelpFacade()
        {
            dac = new CommonHelpDac();
        }

        /// <summary>
        /// 获取查询区模板
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetQueryTemplate(string helpid)
        {
            return  dac.GetQueryTemplate(helpid);
        }
        
        /// <summary>
        /// 取得列表的模板信息
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetListTemplate(string helpid)
        {
            return dac.GetListTemplate(helpid);
        }

        /// <summary>
        /// 获取Json模板描述内容
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns></returns>
        public string GetJsonTemplate(string helpid)
        {
            return dac.GetJsonTemplate(helpid);
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
        /// 取得帮助节点信息
        /// </summary>
        /// <param name="helpflag"></param>
        /// <returns></returns>
        [DBControl]
        public CommonHelpEntity GetCommonHelpItem(string helpid, bool ormMode)
        {
            if (ormMode)
            {
                return dac.GetHelpItem(helpid);
            }
            return dac.GetCommonHelpItem(helpid);
        }
        
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="helpflag">帮助标记</param>
        /// <param name="clientQuery">客户端查询条件</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="isAutoComplete">联想搜索</param>
        /// <returns></returns>
        [DBControl]
        public DataTable GetList(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, bool isAutoComplete,bool ORMMode)
        {
            return dac.GetList(helpid, pageSize, pageIndex, ref totalRecord,clientQuery,outJsonQuery,leftLikeJsonQuery,clientSqlFilter,isAutoComplete,ORMMode);
         
        }

        /// <summary>
        /// 根据代码获取名称
        /// </summary>
        /// <param name="helpflag">帮助标记</param>
        /// <param name="code">代码值</param>
        /// <param name="clientQuery">查询条件</param>
        /// <returns></returns>
        [DBControl]
        public string GetName(string helpid, string code,string helptype, string clientQuery, string outJsonQuery)
        {
            try
            {
                return dac.GetName(helpid, code,string.Empty, clientQuery, outJsonQuery,helptype);
            }
            catch (Exception)
            {                
               
            }

            return string.Empty;
            
        }

        [DBControl]
        public HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list)
        {
            try
            {
                return new CommonHelpRule().GetAllNames(list);
            }
            catch (Exception)
            {
                
                throw;
            }

            return null;
        }

        [DBControl]
        public DataTable GetSelectedData(string helpid, string codes, bool mode)
        {
            return dac.GetSelectedData(helpid, codes,mode);
        }

        /// <summary>
        /// 验证用户输入数据的合法性
        /// </summary>
        /// <param name="helpid"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        [DBControl]
        public bool ValidateData(string helpid, string inputValue, string clientSqlFilter, string selectMode, string helptype)
        {
            return dac.ValidateData(helpid, inputValue, clientSqlFilter,selectMode, helptype);
        }
    }
}
