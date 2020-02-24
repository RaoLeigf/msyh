using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.Facade
{
    public interface IUserOnlineInforFacade
    {

        bool RemoveLoginUser(string sSessionID);

        void KillLoginUser(string sSessionID);

        string Refresh();
    }
}
