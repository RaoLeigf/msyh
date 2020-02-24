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
    public class MyFuncTreeFacade : IMyFuncTreeFacade
    {
        private MyFuncTreeRule rule = new MyFuncTreeRule();
        [DBControl]
        public IList<TreeJSONBase> LoadMyFuncTree(long userid, string nodeid)
        {
            return rule.LoadMyFuncTree(userid, nodeid);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int Save(DataTable myFuncTreeTable, long userid)
        {
            return rule.Save(myFuncTreeTable,userid);
        }
    }
}
