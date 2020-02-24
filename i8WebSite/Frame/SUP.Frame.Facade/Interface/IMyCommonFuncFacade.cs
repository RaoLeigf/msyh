using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public interface IMyCommonFuncFacade
    {
        DataTable LoadMyMenu();
        DataTable LoadMyMenuByType(string typecode);
        int SaveMyMenu(DataTable masterdt);

        DataTable LoadMyMenuType();

        int SaveMyMenuType(string masterdt);
    }
}
