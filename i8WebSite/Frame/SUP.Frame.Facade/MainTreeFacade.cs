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
    public class MainTreeFacade : IMainTreeFacade
    {

        private MainTreeRule rule = new MainTreeRule();

        public MainTreeFacade()
        {
 
        }

        [DBControl]
        public IList<TreeJSONBase> LoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter)
        {
            return  rule.LoadMenu(product, suite, isusbuser, usertype, orgID, userID, nodeid,rightFlag,lazyLoadFlag, treeFilter);
        }

    
        [DBControl]
        public IList<TreeJSONBase> Query(string product,  bool isusbuser, string usertype, Int64 orgID, Int64 userID, string condition, bool rightFlag, string treeFilter)
        {
            return rule.Query(product, isusbuser, usertype, orgID, userID,  condition, rightFlag,treeFilter);
        }
        [DBControl]
        public IList<SuiteInfoEntity> GetSuiteList(bool rightFlag, string treeFilter)
        {
            return rule.GetSuiteList(rightFlag, treeFilter);
        }
        [DBControl]
        //根据busphid取fg3-menu数据的接口
        public DataTable GetMenuByBusphid(long busphid)
        {
            return rule.GetMenuByBusphid(busphid);
        }
    }
}
