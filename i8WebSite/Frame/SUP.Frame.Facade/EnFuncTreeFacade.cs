using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.Rule;
using NG3;
using SUP.Frame.DataEntity;

namespace SUP.Frame.Facade
{
    public class EnFuncTreeFacade : IEnFuncTreeFacade
    {

        private EnFuncTreeRule rule = new EnFuncTreeRule();

        public EnFuncTreeFacade()
        {

        }

        [DBControl]
        public IList<TreeJSONBase> LoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid)
        {
            return rule.LoadMenu(product, suite, isusbuser, usertype, orgID, userID, nodeid);
        }

        [DBControl]
        public IList<TreeJSONBase> Query(string product, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string condition)
        {
            return rule.Query(product, isusbuser, usertype, orgID, userID, condition);
        }
        [DBControl]
        //public IList<IndivadualSuiteInfoEntity> GetSuiteList()
        //{
        //    return rule.GetSuiteList();
        //}

        public IList<SuiteInfoEntity> GetSuiteList()
        {
            return rule.GetSuiteList();
        }

    }
}
