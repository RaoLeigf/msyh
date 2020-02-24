using NG3;
using SUP.Frame.DataAccess;
using SUP.Frame.Facade.Interface;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade
{
    public class ShortcutMenuFacade: IShortcutMenuFacade
    {
        private ShortcutMenuDac dac = new ShortcutMenuDac();
        private ShortcutMenuRule rule = new ShortcutMenuRule();

        [DBControl]
        public DataTable GetShortcutMenuList(Int64 phid, int pageSize, int pageIndex, ref int totalRecord)
        {
            return dac.GetShortcutMenuList(phid, pageSize, pageIndex, ref totalRecord);
        }

        [DBControl]
        public DataTable GetShortcutMenuForWeb(Int64 userid)
        {
            return dac.GetShortcutMenuForWeb(userid);
        }

        [DBControl]
        public DataTable GetShortcutKey()
        {
            return dac.GetShortcutKey();
        }

        [DBControl]
        public DataTable SaveShortcutMenu(DataTable dt)
        {
            return dac.SaveShortcutMenu(dt);
        }

        [DBControl]
        public string AddShortcutMenu(string originalcode, string name, string url,string busphid)
        {
            return dac.AddShortcutMenu(originalcode, name, url,busphid);
        }
    }
}
