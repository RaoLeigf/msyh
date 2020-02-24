using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public interface IIndividualNavigationFacade
    {
        DataTable LoadTree();
        string Load(string text);

        string Save(string svgConfig, string text, string saveType);
        bool Delete(string text);
      
    }
}
