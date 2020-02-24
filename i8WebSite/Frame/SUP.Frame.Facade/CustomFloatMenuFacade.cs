using System.Collections.Generic;
using System.Data;
using SUP.Frame.DataAccess;
using NG3;
using SUP.Frame.Rule;
using SUP.Frame.DataEntity;

namespace SUP.Frame.Facade
{
    public class CustomFloatMenuFacade : ICustomFloatMenuFacade
    {
        private CustomFloatMenuDac dac = null;
        private CustomFloatMenuRule rule = null;

        public CustomFloatMenuFacade()
        {
            dac = new CustomFloatMenuDac();
            rule = new CustomFloatMenuRule();
        }

        [DBControl]
        public DataTable LoadMenuData(string product, string suite, string language)
        {                      
            return dac.LoadMenuData(product, suite, language);
        }

        [DBControl]
        public DataTable GetFloatMenuTree(string code)
        {
            return rule.GetFloatMenuTree(code);
        }

        [DBControl]
        public DataTable GetFloatMenuIn(string code)
        {
            return dac.GetFloatMenuIn(code);
        }

        [DBControl]
        public DataTable GetFloatMenuOut(string code)
        {
            return dac.GetFloatMenuOut(code);
        }

        [DBControl]
        public string GetBusNameByCode(string code)
        {
            return rule.GetBusNameByCode(code);
        }

        [DBControl]
        public bool SaveFloatMenu(string code, List<FloatMenuEntity> floatMenuList)
        {
            return rule.SaveFloatMenu(code, floatMenuList);
        }

        [DBControl]
        public DataTable GetFloatMenuByCode(string code)
        {
            return rule.GetFloatMenuByCode(code);
        }

        [DBControl]
        public string LoadReportList()
        {
            return rule.LoadReportList();
        }

        [DBControl]
        public string LoadSearchReportList(string search)
        {
            return rule.LoadSearchReportList(search);
        }

        [DBControl]
        public DataTable GetSheet(string phid)
        {
            return dac.GetSheet(phid);
        }

        [DBControl]
        public DataTable GetDsc(string phid, string sheetid)
        { 
            return dac.GetDsc(phid, sheetid);
        }

        [DBControl]
        public DataTable GetPara(string phid, string sheetid, string ds_no)
        {
            return dac.GetPara(phid, sheetid, ds_no);
        }

    }

    public interface ICustomFloatMenuFacade
    {
        DataTable LoadMenuData(string product, string suite, string language);

        DataTable GetFloatMenuTree(string code);

        DataTable GetFloatMenuIn(string code);

        DataTable GetFloatMenuOut(string code);

        string GetBusNameByCode(string code);

        bool SaveFloatMenu(string code, List<FloatMenuEntity> floatMenuList);

        DataTable GetFloatMenuByCode(string code);

        string LoadReportList();

        string LoadSearchReportList(string search);

        DataTable GetSheet(string phid);

        DataTable GetDsc(string phid, string sheetid);

        DataTable GetPara(string phid, string sheetid, string ds_no);
    }
}
