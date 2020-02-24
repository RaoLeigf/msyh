using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SUP.Common.Facade
{
    public interface IIndividualUIFacade
    {

        int Save(DataTable dt, List<string> dellist);

        DataTable GetIndividualInfoList(string bustype);

        string GetIndividualUI(string bustype);

        string GetIndividualUIbyCode(string id);

        int Delete(string code);

        DataTable GetIndividualRegList(string clientJson, int pageSize, int PageIndex, ref int totalRecord);

        string GetIndividualRegUrl(string bustype);

        string GetUserDefScriptUrl(string bustype);

        int SaveIndividualUI(string id, string uiinfo);

        string GetIndividualColumnForList(string bustype, string tablename);

        string GetIndividualColumnForList(string bustype, List<string> tables);


        string GetScriptCode(Int64 phid);
        int SaveScript(string busType, Int64 phid, string scriptCode);

        int PublishScript(string busType,Int64 phid, string scriptCode);

        string GetSchemaName(Int64 phid);


        DataTable GetToCheckList(string bustype);

        string CheckUIInfo(ref List<Int64> idList, string bustype, string ids);

        string UpdateUIInfo(string ids);

        string CheckSysTemplate();
        string CheckAllUserTemplate();

        int Copy(Int64 phid);

        DataTable GetOrgNumberByPhid(string bustypephid);

        int SaveOrg(DataTable dtAddOrg, List<string> listDelOrg, string phid);

        int SyncScript();  //脚本同步



    }
}
