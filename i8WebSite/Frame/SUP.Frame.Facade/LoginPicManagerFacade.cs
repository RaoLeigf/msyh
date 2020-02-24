using NG3;
using SUP.Frame.DataAccess;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Frame.Facade
{
    public class LoginPicManagerFacade : ILoginPicManagerFacade
    {
        private LoginPicManagerDac dac = null;
        private LoginPicManagerRule rule = null;

        public LoginPicManagerFacade()
        {
            dac = new LoginPicManagerDac();
            rule = new LoginPicManagerRule();
        }

        [DBControl]
        public DataTable GetLoginPictureTree()
        {
            return dac.GetLoginPictureTree();
        }

        [DBControl]
        public void AddNode(string id, string name, string src, string attachid)
        {
            rule.AddNode(id, name, src, attachid);
        }

        [DBControl]
        public void DelNode(string phid)
        {
            rule.DelNode(phid);
        }

        [DBControl]
        public DataTable GetLoginPicSet()
        {
            return dac.GetLoginPicSet();
        }

        [DBControl]
        public string ChangeShowType(string showtype, string showpic)
        {
            return dac.ChangeShowType(showtype, showpic);
        }

        [DBControl]
        public void SaveLoginPicSet(string showtype, string showlogo, string allowuser, string showpic)
        {
            dac.SaveLoginPicSet(showtype, showlogo, allowuser, showpic);
        }

    }

    public interface ILoginPicManagerFacade
    {
        DataTable GetLoginPictureTree();

        void AddNode(string id, string name, string src, string attachid);

        void DelNode(string phid);

        DataTable GetLoginPicSet();

        string ChangeShowType(string showtype, string showpic);

        void SaveLoginPicSet(string showtype, string showlogo, string allowuser, string showpic);

    }

}
