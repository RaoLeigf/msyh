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
    public class MyCommonFuncFacade : IMyCommonFuncFacade
    {
        private MyCommonFuncRule rule = new MyCommonFuncRule();
        [DBControl]
        public DataTable LoadMyMenu()
        {
            return rule.LoadMyMenu();
        }
        [DBControl]
        public DataTable LoadMyMenuByType(string typecode)
        {
            return rule.LoadMyMenuByType(typecode);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveMyMenu(DataTable masterdt)
        {
            return rule.SaveMyMenu(masterdt);
        }
        [DBControl]
        public DataTable LoadMyMenuType()
        {
            return rule.LoadMyMenuType();
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public int SaveMyMenuType(string masterdt)
        {
            return rule.SaveMyMenuType(masterdt);
        }
    }
}
