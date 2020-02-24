using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Frame.Facade
{
    public interface IUserFacade
    {

        string ChangePwd(string logid, string oldpwd, string newpwd, ref string msg);

        string GetPwdLimit();

        bool Login(string svrName, string account, string logid, string pwd, ref string msg);

        bool AddUser(string logid, string username, string pwd);

        bool LockUser(string logid);

        bool UnLockUser(string logid);
    }
}
