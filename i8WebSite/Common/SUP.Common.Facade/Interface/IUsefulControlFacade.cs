using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SUP.Common.Facade
{
    public interface IUsefulControlFacade
    {
        DataTable GetList(string userId, string controlId, string names);

        int Update(string userId, string controlId, string names);
    }
}
