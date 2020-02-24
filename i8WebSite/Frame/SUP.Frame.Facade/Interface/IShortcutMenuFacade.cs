using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade.Interface
{
    public interface IShortcutMenuFacade
    {
        DataTable GetShortcutMenuList(Int64 phid, int pageSize, int pageIndex, ref int totalRecord);

        DataTable GetShortcutMenuForWeb(Int64 phid);


        DataTable GetShortcutKey();

        DataTable SaveShortcutMenu(DataTable dt);

        string AddShortcutMenu(string originalcode, string name, string url,string busphid);

    }
}
