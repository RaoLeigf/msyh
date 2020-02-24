using NG3;
using SUP.Frame.DataAccess;
using SUP.Frame.Facade.Interface;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade
{
    public class SchemeAllocationFacade: ISchemeAllocationFacade
    {
        private SchemeAllocationDac dac = new SchemeAllocationDac();
        private SchemeAllocationRule rule = new SchemeAllocationRule();

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public DataTable GetUserSchemeAllocation()
        {
            return rule.GetUserSchemeAllocation();
        }

        [DBControl]
        public DataTable GetRoleList()
        {
            return dac.GetRoleList();
        }

        [DBControl]
        public DataTable GetUserList()
        {
            return dac.GetUserList();
        }


        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public string SaveUserSchemeAllocation(string oriuserid,string oriusertype,string userid,string usertype)
        {
            return rule.SaveUserSchemeAllocation(oriuserid,oriusertype,userid,usertype);
        }


        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public string DeleteUserSchemeAllocation(string phid)
        {
            return rule.DeleteUserSchemeAllocation(phid);
        }
    }
}
