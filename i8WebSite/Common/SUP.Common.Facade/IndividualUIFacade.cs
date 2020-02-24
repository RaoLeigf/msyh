using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3;
using SUP.Common.DataAccess;
using SUP.Common.Rule;

namespace SUP.Common.Facade
{
    public  class IndividualUIFacade : IIndividualUIFacade
    {

        private IndividualUIDac dac;
        private IndividualUIRule rule;

        public IndividualUIFacade()
        {
            dac = new IndividualUIDac();
            rule = new IndividualUIRule();
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int Save(DataTable dt, List<string> dellist)
        {
            return rule.Save(dt,dellist);
        }

        [DBControl]
        public DataTable GetIndividualInfoList(string bustype)
        {
            return dac.GetIndividualInfoList(bustype);
        }

        /// <summary>
        /// 根据业务类型获取自定义界面信息
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        [DBControl]
        public string GetIndividualUI(string bustype)
        {
            return rule.GetIndividualUI(bustype);
        }

        /// <summary>
        /// 获取某一个界面
        /// </summary>
        /// <param name="code">主键</param>
        /// <returns></returns>
        [DBControl]
        public string GetIndividualUIbyCode(string id)
        {
            Int64 phid = Convert.ToInt64(id);
                
           return dac.GetIndividualUIbyCode(phid);
        }

        /// <summary>
        /// 删除自定义界面信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int Delete(string code)
        {
            return dac.Delete(code);
        }

        [DBControl]
        public DataTable GetIndividualRegList(string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            return dac.GetIndividualRegList(clientJson, pageSize, PageIndex, ref totalRecord);
        }

        [DBControl]
        public string GetIndividualRegUrl(string bustype)
        {
            return dac.GetIndividualRegUrl(bustype);
        }

        [DBControl]
        public string GetUserDefScriptUrl(string bustype)
        {
            string app = System.Web.HttpContext.Current.Request.ApplicationPath;
            string fileurl = dac.GetUserDefScriptUrl(bustype);
            string url = string.Empty;
            if (app == "/")
            {
                url = "/NG3Resource/IndividualInfo/" + fileurl;
            }
            else
            {
                url = app + "/NG3Resource/IndividualInfo/" + fileurl;
            }

            return url;            
        }

        [DBControl(ControlOption= DbControlOption.BeginTransaction)]
        public int SaveIndividualUI(string id, string uiinfo)
        {
            Int64 phid = Convert.ToInt64(id);
            return dac.SaveIndividualUI(phid,uiinfo);
        }


        [DBControl]
        public string GetIndividualColumnForList(string bustype, string tablename)
        {
            List<string> ls = new List<string>();
            ls.Add(tablename);
            return GetIndividualColumnForList(bustype, ls);
        }

        [DBControl]
        public string GetIndividualColumnForList(string bustype, List<string> tables)
        {
            return rule.GetIndividualColumnForList(bustype, tables);
        }

        [DBControl]
        public string GetScriptCode(Int64 phid)
        {
            return dac.GetScriptCode(phid);
        }


        [DBControl]
        public int SaveScript(string busType, Int64 phid,string scriptCode)
        {
            return dac.SaveScript(busType,phid, scriptCode);
        }

        [DBControl]
        public int PublishScript(string busType, Int64 phid, string scriptCode)
        {
            return dac.PublishScript(busType,phid, scriptCode);
        }

        [DBControl]
        public string GetSchemaName(Int64 phid)
        {
            return dac.GetSchemaName(phid);
        }

        /// <summary>
        /// 检测系统模板
        /// </summary>
        /// <returns></returns>
        [DBControl]
        public string CheckSysTemplate()
        {
            return rule.CheckSysTemplate();
        }

        /// <summary>
        /// 检测用户自定义模板
        /// </summary>
        /// <returns></returns>
        [DBControl]
        public string CheckAllUserTemplate()
        {
            return rule.CheckAllUserTemplate();
        }

        [DBControl]
        public DataTable GetToCheckList(string bustype)
        {
            return dac.GetToCheckList(bustype);
        }

        [DBControl]
        public string CheckUIInfo(ref List<Int64> idList, string bustype, string ids)
        {
            return rule.CheckUIInfo(ref idList,bustype,ids);
        }

        [DBControl]
        //更新
        public string UpdateUIInfo(string ids)
        {
            return rule.UpdateUIInfo(ids);
        }

        [DBControl]
        public int Copy(Int64 phid)
        {
            return rule.Copy(phid);
        }

        [DBControl]
        public DataTable GetOrgNumberByPhid(string bustypephid)
        {
            return dac.GetOrgNumberByPhid(bustypephid);
        }

        [DBControl]
        public int SaveOrg(DataTable dtAddOrg, List<string> listDelOrg, string phid)
        {
            return rule.SaveOrg(dtAddOrg, listDelOrg, phid);
        }

        [DBControl]
        public int SyncScript()
        {
            return rule.SyncScript();
        }
    }
}
