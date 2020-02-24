using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public interface IMainTreeFacade
    {

        IList<TreeJSONBase> LoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter);
         IList<TreeJSONBase> Query(string product,bool isusbuser, string usertype, Int64 orgID, Int64 userID, string condition, bool rightFlag, string treeFilter);
        IList<SuiteInfoEntity> GetSuiteList(bool rightFlag, string treeFilter);
        DataTable GetMenuByBusphid(long busphid);
    }
}
