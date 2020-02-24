using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade.Interface
{
    public interface ISchemeAllocationFacade
    {
        DataTable GetUserSchemeAllocation();
        DataTable GetRoleList();
        DataTable GetUserList();

        string SaveUserSchemeAllocation(string oriuserid, string oriusertype, string userid, string usertype);

        string DeleteUserSchemeAllocation(string phid);
    }
}
