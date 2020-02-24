using SUP.Frame.DataAccess;
using SUP.Frame.Rule;
using NG3;

namespace SUP.Frame.Facade
{
    public class ChangePwdFacade : IChangePwdFacade
    {

        private ChangePwdDac dac = null;
        private ChangePwdRule rule = null;

        public ChangePwdFacade()
        {
            dac = new ChangePwdDac();
            rule = new ChangePwdRule();
        }

        [DBControl]
        public string GetPwdLimit()
        {
            return dac.GetPwdLimit();
        }

        [DBControl]
        public string SavePwd(string oldpwd, string newpwd)
        {
            return rule.SavePwd(oldpwd, newpwd);
        }

    }

    public interface IChangePwdFacade
    {
        string GetPwdLimit();

        string SavePwd(string oldpwd, string newpwd);
    }

}
