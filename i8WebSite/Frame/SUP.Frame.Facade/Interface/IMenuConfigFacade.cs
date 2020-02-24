using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public interface IMenuConfigFacade
    {
        string Load(long userid);
        string LoadEnFuncTreeRight();
        
        bool Save(long userid, string masterdt);

        DataSet ConvertXMLToDataSet(string xmlFile);

        bool SaveDockControl(int isDockControl, long userid);

        string GetDockControl(long userid);
        bool SaveUITheme(int UITheme, long userid);

        string GetUITheme(long userid);
        void SaveRequestRecord(RequestRecordEntity record);     
    }
}
