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
    public class NavigationCenterFacade : INavigationCenterFacade
    {
        private NavigationCenterRule rule = new NavigationCenterRule();
        [DBControl]
        public IList<TreeJSONBase> LoadTree(string type)
        {
            return rule.LoadTree(type);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveTree(DataTable myTreeTable)
        {
            return rule.SaveTree(myTreeTable);
        }
        [DBControl]
        public string LoadChart(string phid)
        {
            return rule.LoadChart(phid);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public string SaveChart(string svgConfig, string phid)
        {
            return rule.SaveChart(svgConfig, phid);
        }

        [DBControl]
        public DataTable FindProcessByWiki(IList<long> phids)
        {
            return rule.FindProcessByWiki(phids);
        }
    }
}
