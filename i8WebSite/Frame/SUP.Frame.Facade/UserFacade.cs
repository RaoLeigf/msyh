using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3;
using SUP.Frame.DataAccess;
using SUP.Frame.Rule;

namespace SUP.Frame.Facade
{
    public class UserFacade : IUserFacade
    {
        private UserDac dac = new UserDac();
        private UserRule rule = new UserRule();

        [DBControl]
        public string ChangePwd(string logid, string oldpwd, string newpwd,ref string msg)
        {
            return dac.ChangePwd(logid, oldpwd, newpwd,ref msg);
        }

        [DBControl]
        public string ResetPwd(string logid, string newpwd, ref string msg)
        {
            return dac.ResetPwd(logid, newpwd, ref msg);
        }

        [DBControl]
        public string GetPwdLimit()
        {
            return dac.GetPwdLimit();
        }

        public bool Login(string svrName, string account, string logid, string pwd, ref string msg)
        {
            return dac.Login(svrName, account, logid, pwd, ref msg);
        }

        [DBControl(ControlOption = DbControlOption.BeginTransaction)]
        public bool AddUser(string logid, string username, string pwd)
        {
            return rule.AddUser(logid,username,pwd);
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public bool LockUser(string logid)
        {
            return dac.LockUser(logid);
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        [DBControl]
        public bool UnLockUser(string logid)
        {
            return dac.UnLockUser(logid);
        }
    }
}
