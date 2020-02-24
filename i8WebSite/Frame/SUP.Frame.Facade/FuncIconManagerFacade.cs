using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3;
using SUP.Frame.Rule;
using SUP.Frame.DataAccess;
using SUP.Frame.DataEntity;

namespace SUP.Frame.Facade
{
    public class FuncIconManagerFacade : IFuncIconManagerFacade
    {
        private FuncIconManagerRule rule = null;
        private FuncIconManagerDac dac = null;

        public FuncIconManagerFacade()
        {
            rule = new FuncIconManagerRule();
            dac = new FuncIconManagerDac();
        }

        [DBControl]
        public DataTable GetFuncIconGrid(string code, string suite, ref int totalRecord)
        {
            return rule.GetFuncIconGrid(code, suite, ref totalRecord);
        }

        [DBControl]
        public DataTable GetFuncIcons(string tag, ref int totalRecord)
        {
            return rule.GetFuncIcons(tag, ref totalRecord);
        }

        [DBControl]
        public DataTable GetFuncIconsEx(bool buildinIconShow, bool customIconShow, string tag, ref int totalRecord)
        {
            return rule.GetFuncIcons(buildinIconShow, customIconShow, tag, ref totalRecord);
        }

        [DBControl]
        public bool AddFuncIconTag(string name)
        {
            return rule.AddFuncIconTag(name);
        }

        [DBControl]
        public bool DelFuncIconTag(string name)
        {
            return dac.DelFuncIconTag(name);
        }

        [DBControl]
        public DataTable GetFuncIconTagGrid(string search,ref int totalRecord)
        {
            return dac.GetFuncIconTagGrid(search, ref totalRecord);
        }

        [DBControl]
        public bool AddFuncIcon(string name, string tag, string attachid)
        {
            return rule.AddFuncIcon(name, tag, attachid);
        }

        [DBControl]
        public bool DelFuncIcon(string id)
        {
            return rule.DelFuncIcon(id);
        }

        [DBControl]
        public bool ReplaceFuncIcon(string id, string name, string tag)
        {
            return dac.ReplaceFuncIcon(id,name,tag);
        }

        [DBControl]
        public bool Save(List<FuncIconEntity> funcIconList)
        {
            return rule.SaveFuncIconSet(funcIconList);
        }

    }

    public interface IFuncIconManagerFacade
    {
        DataTable GetFuncIconGrid(string code,string suite,ref int totalRecord);

        DataTable GetFuncIcons(string tag, ref int totalRecord);

        DataTable GetFuncIconsEx(bool buildinIconShow, bool customIconShow, string tag, ref int totalRecord);

        bool AddFuncIconTag(string name);

        bool DelFuncIconTag(string name);

        DataTable GetFuncIconTagGrid(string search,ref int totalRecord);

        bool AddFuncIcon(string name, string tag, string attachid);

        bool DelFuncIcon(string id);

        bool ReplaceFuncIcon(string id, string name, string tag);

        bool Save(List<FuncIconEntity> funcIconList);
    }
}
