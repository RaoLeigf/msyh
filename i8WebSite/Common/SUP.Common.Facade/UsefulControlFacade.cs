using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Rule;
using System.Data;
using NG3;

namespace SUP.Common.Facade
{
    public class UsefulControlFacade : IUsefulControlFacade
    {
        UsefulControlRule rule = new UsefulControlRule();

        [DBControl]
        public DataTable GetList(string userId, string controlId, string names)
        {
            return rule.GetList(userId, controlId, names);
        }

        [DBControl]
        public int Update(string userId, string controlId, string names)
        {
            return rule.Update(userId, controlId, names);
        }
    }
}
