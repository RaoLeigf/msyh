using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.Facade
{
    public interface IIndividualFormFacade
    {

        int Save(DataTable dt);

        DataTable GetIndividualFormList(string bustype);

    }
}
