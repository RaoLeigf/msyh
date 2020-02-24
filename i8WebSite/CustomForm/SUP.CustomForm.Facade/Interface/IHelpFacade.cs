using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity;
using SUP.Common.DataEntity;

namespace SUP.CustomForm.Facade.Interface
{
    public interface IHelpFacade
    {
        DataTable GetHelpList(string helpId, int pageSize, int pageIndex, ref int totalRecord, string clientFilter,bool isAutoComplete, string outJsonQuery);

        HelpEntity GetHelpItem(string helpId);

        string GetName(string helpId, string codoValue, string selectMode);

        HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list);

        bool ValidateData(string helpId, string inputValue);
        
    }
}
