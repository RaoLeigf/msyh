using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.Rule;
using NG3;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public class ReportListFacade : IReportListFacade
    {
        private ReportListRule rule = new ReportListRule();
        [DBControl]
        public IList<TreeJSONBase> LoadReportList(string userid, long orgid, string page = "")
        {
            return rule.LoadReportList(userid, orgid, page);
        }
    }
}
