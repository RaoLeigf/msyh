using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade.Interface
{
    public interface IAppAutoAuthorizeFacade
    {
        DataTable GetAppAutoAuthorizeList(string rolename);

        int Save(DataTable dt);
    }
}
