using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3;
using NG3.Aop;
using NG3.Data.Service;
using SUP.Common.DataEntity;
using SUP.CustomForm.DataAccess;
using SUP.CustomForm.DataEntity;
using SUP.CustomForm.Facade.Interface;
using SUP.CustomForm.Rule;

namespace SUP.CustomForm.Facade
{
    public class HelpFacade:IHelpFacade
    {
        private HelpDac dac = new HelpDac();

        [DBControl]
        public DataTable GetHelpList(string helpId, int pageSize, int pageIndex, ref int totalRecord,
                                     string clientFilter, bool isAutoComplete, string outJsonQuery)
        {
            return dac.GetHelpList(helpId, pageSize, pageIndex, ref totalRecord,
                                   clientFilter, isAutoComplete, outJsonQuery);
        }

        [DBControl]
        public HelpEntity GetHelpItem(string helpId)
        {
            return dac.GetHelpItem(helpId);
        }

        [DBControl]
        public string GetName(string helpId, string codeValue, string selectMode)
        {
            return dac.GetName(helpId, codeValue, selectMode);
        }

        [DBControl]
        public HelpValueNameEntity[] GetAllNames(IList<HelpValueNameEntity> list)
        {
            return new HelpRule().GetAllNames(list);
        }

        [DBControl]
        public bool ValidateData(string helpId , string inputValue)
        {
            return dac.ValidateData(helpId , inputValue);
        }
    }
}
