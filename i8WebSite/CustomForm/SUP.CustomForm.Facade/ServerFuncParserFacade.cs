using NG3;
using SUP.CustomForm.Facade.Interface;
using SUP.CustomForm.Rule;

namespace SUP.CustomForm.Facade
{
    public class ServerFuncParserFacade : IServerFuncParserFacade
    {
        private ServerFuncParserRule rule = new ServerFuncParserRule();

        [DBControl]
        public string FuncParser(string busname, string funcname, string paramstr, string rtntype)
        {
            return rule.FuncParser(busname, funcname, paramstr, rtntype);
        }
    }
}
