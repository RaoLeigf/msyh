using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.Facade.Interface
{
    public interface IServerFuncParserFacade
    {
        string FuncParser(string busname, string funcname, string paramstr, string rtntype);
    }
}
