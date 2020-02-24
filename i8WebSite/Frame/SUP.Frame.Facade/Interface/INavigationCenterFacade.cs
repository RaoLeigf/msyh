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
    public interface INavigationCenterFacade
    {   
        IList<TreeJSONBase> LoadTree(string type);
        int SaveTree(DataTable myTreeTable);

        string LoadChart(string phid);

        string SaveChart(string svgConfig, string phid);

        DataTable FindProcessByWiki(IList<long> phids);
    }
}
