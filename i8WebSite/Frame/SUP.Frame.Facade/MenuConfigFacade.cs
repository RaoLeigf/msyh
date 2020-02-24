using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Base;
using SUP.Frame.Rule;
using SUP.Frame.DataAccess;
using NG3;
using SUP.Frame.DataEntity;
using System.Data;

namespace SUP.Frame.Facade
{
    public class MenuConfigFacade : IMenuConfigFacade
    {
        private MenuConfigRule rule = new MenuConfigRule();
        private MenuConfigDac dac = new MenuConfigDac();
        [DBControl]
        public string Load(long userid)
        {
            return rule.Load(userid);
        }
        [DBControl]
        public string LoadEnFuncTreeRight()
        {
            return rule.LoadEnFuncTreeRight();
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool Save(long userid, string masterdt)
        {
            return rule.Save(userid, masterdt);
        }
   

        [DBControl]
        public DataSet ConvertXMLToDataSet(string xmlFile)
        {
            return rule.ConvertXMLToDataSet(xmlFile);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool SaveDockControl(int isDockControl, long userid)
        {
            return rule.SaveDockControl(isDockControl, userid);
        }
        [DBControl]
        public string GetDockControl(long userid)
        {
            return rule.GetDockControl(userid);
        }
        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool SaveUITheme(int UITheme, long userid)
        {
            return rule.SaveUITheme(UITheme, userid);
        }
        [DBControl]
        public string GetUITheme(long userid)
        {
            return rule.GetUITheme(userid);
        }
        public void SaveRequestRecord(RequestRecordEntity record)
        {
            dac.SaveRequestRecord(record);
        }        
    }
}
