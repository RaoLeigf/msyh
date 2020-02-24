using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.DataEntity;

namespace SUP.Common.Facade
{
    public interface ICommonHelpFacade
    {

        CommonHelpEntity GetCommonHelpItem(string flag);

        CommonHelpEntity GetCommonHelpItem(string flag ,bool ormMode);

        DataTable GetList(string helpid, int pageSize, int pageIndex, ref int totalRecord, string clientQuery, string outJsonQuery, string leftLikeJsonQuery, string clientSqlFilter, bool isAutoComplete, bool ormMode);


        string GetListTemplate(string helpflag);

        string GetQueryTemplate(string helpflag);

        string GetJsonTemplate(string helpid);

        string GetName(string helpid, string code,string helptype, string clientQuery, string outJsonQuery);

        HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list);

        DataTable GetSelectedData(string helpid, string codes, bool mode);

        bool ValidateData(string helpid, string inputValue, string clientSqlFilter, string selectMode, string helptype);
    }
}
