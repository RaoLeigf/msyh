using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
namespace SUP.Frame.Facade
{
    public interface IEnFuncTreeFacade
    {
        IList<TreeJSONBase> LoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid);
        IList<TreeJSONBase> Query(string product, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string condition);
        IList<SuiteInfoEntity> GetSuiteList();
    }
}
