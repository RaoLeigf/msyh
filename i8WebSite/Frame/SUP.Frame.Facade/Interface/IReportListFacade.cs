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
    public interface IReportListFacade
    {
        IList<TreeJSONBase> LoadReportList(string userid, long orgid,string page="");
    }
}




