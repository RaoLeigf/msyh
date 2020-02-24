using NG3;
using SUP.Frame.Rule;
using System.Data;

namespace SUP.Frame.Facade
{
    public class MyDesktopSetFacade : IMyDesktopSetFacade
    {
        private MyDesktopSetRule rule = null;

        public MyDesktopSetFacade()
        {
            rule = new MyDesktopSetRule();
        }

        [DBControl]
        public string GetMyDesktopFuncIconData()
        {
            return rule.GetMyDesktopFuncIconData();
        }

        [DBControl]
        public DataTable GetMyDesktopGroup(ref int totalRecord)
        {
            return rule.GetMyDesktopGroup(ref totalRecord);
        }

        [DBControl]
        public DataTable GetMyDesktopGroupEx(string index, ref int totalRecord)
        {
            return rule.GetMyDesktopGroupEx(index, ref totalRecord);
        }

        [DBControl]
        public DataTable GetMyDesktopNode(string index, ref int totalRecord)
        {
            return rule.GetMyDesktopNode(index, ref totalRecord);
        }

        public string GetColor()
        {
            return rule.GetColor();
        }

        [DBControl]
        public bool AddMyDesktopGroup()
        {
            return rule.AddMyDesktopGroup();
        }

        [DBControl]
        public bool DelMyDesktopGroup(string index)
        {
            return rule.DelMyDesktopGroup(index);
        }

        [DBControl]
        public bool UpMyDesktopGroup(string index)
        {
            return rule.UpMyDesktopGroup(index);
        }

        [DBControl]
        public bool DownMyDesktopGroup(string index)
        {
            return rule.DownMyDesktopGroup(index);
        }

        [DBControl]
        public bool UpMyDesktopNode(string groupindex, string nodeindex)
        {
            return rule.UpMyDesktopNode(groupindex, nodeindex);
        }

        [DBControl]
        public bool DownMyDesktopNode(string groupindex, string nodeindex)
        {
            return rule.DownMyDesktopNode(groupindex, nodeindex);
        }

        [DBControl]
        public string AddMyDesktopNode(string json, string groupname, string index)
        {
            return rule.AddMyDesktopNode(json, groupname, index);
        }

        [DBControl]
        public string AddMyDesktopNodeEx(string json, string groupname)
        {
            return rule.AddMyDesktopNodeEx(json, groupname);
        }

        [DBControl]
        public bool DelMyDesktopNode(string groupindex, string nodeindex)
        {
            return rule.DelMyDesktopNode(groupindex, nodeindex);
        }

        [DBControl]
        public bool ChangeMyDesktopInfo(string json)
        {
            return rule.ChangeMyDesktopInfo(json);
        }

        [DBControl]
        public string SaveMyDesktopInfo(string json)
        {
            return rule.SaveMyDesktopInfo(json);
        }

        [DBControl]
        public bool ChangeMyDesktopSet(string id, string type, string value)
        {
            return rule.ChangeMyDesktopSet(id, type, value);
        }

        public void InitMyDesktopData()
        {
            rule.InitMyDesktopData();
        }
    }

    public interface IMyDesktopSetFacade
    {
        string GetMyDesktopFuncIconData();

        DataTable GetMyDesktopGroup(ref int totalRecord);

        DataTable GetMyDesktopGroupEx(string index, ref int totalRecord);

        DataTable GetMyDesktopNode(string index, ref int totalRecord);

        string GetColor();

        bool AddMyDesktopGroup();

        bool DelMyDesktopGroup(string index);

        bool UpMyDesktopGroup(string index);

        bool DownMyDesktopGroup(string index);

        bool UpMyDesktopNode(string groupindex, string nodeindex);

        bool DownMyDesktopNode(string groupindex, string nodeindex);

        string AddMyDesktopNode(string json, string groupname, string index);

        string AddMyDesktopNodeEx(string json, string groupname);

        bool DelMyDesktopNode(string groupindex, string nodeindex);

        bool ChangeMyDesktopInfo(string json);

        string SaveMyDesktopInfo(string json);

        bool ChangeMyDesktopSet(string id, string type, string value);

        void InitMyDesktopData();
    }
}
