using System.Web.Mvc;
using NG3.Web.Controller;
using SUP.Frame.Facade;
using NG3.Aop.Transaction;
using System.Data;
using SUP.Common.Base;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MyDesktopSetController : AFController
    {

        private IMyDesktopSetFacade proxy;

        public MyDesktopSetController()
        {
            proxy = AopObjectProxy.GetObject<IMyDesktopSetFacade>(new MyDesktopSetFacade());
        }

        public ActionResult Index()
        {
            try
            {
                IMenuConfigFacade MenuConfigProxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
                string json = MenuConfigProxy.LoadEnFuncTreeRight();
                ViewBag.LoadEnFuncTreeRight = json;
                proxy.InitMyDesktopData();
            }
            catch { }
            return View("MyDesktopSet");
        }

        public string GetMyDesktopFuncIconData()
        {
            return proxy.GetMyDesktopFuncIconData();
        }

        public string GetMyDesktopGroup()
        {
            int totalRecord = 0;
            DataTable dt = proxy.GetMyDesktopGroup(ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        public string GetMyDesktopGroupEx(string index)
        {
            int totalRecord = 0;
            DataTable dt = proxy.GetMyDesktopGroupEx(index, ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        public string GetMyDesktopNode(string index)
        {
            int totalRecord = 0;
            DataTable dt = proxy.GetMyDesktopNode(index,ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        public string GetColor()
        {
            return proxy.GetColor();
        }

        public bool AddMyDesktopGroup()
        {
            return proxy.AddMyDesktopGroup();
        }

        public bool DelMyDesktopGroup(string index)
        {
            return proxy.DelMyDesktopGroup(index);
        }

        public bool UpMyDesktopGroup(string index)
        {
            return proxy.UpMyDesktopGroup(index);
        }

        public bool DownMyDesktopGroup(string index)
        {
            return proxy.DownMyDesktopGroup(index);
        }

        public bool UpMyDesktopNode(string groupindex, string nodeindex)
        {
            return proxy.UpMyDesktopNode(groupindex, nodeindex);
        }

        public bool DownMyDesktopNode(string groupindex, string nodeindex)
        {
            return proxy.DownMyDesktopNode(groupindex, nodeindex);
        }

        public string AddMyDesktopNode(string json, string groupname, string index)
        {
            return proxy.AddMyDesktopNode(json, groupname, index);
        }

        public string AddMyDesktopNodeEx(string json, string groupname)
        {
            return proxy.AddMyDesktopNodeEx(json, groupname);
        }

        public bool DelMyDesktopNode(string groupindex,string nodeindex)
        {
            return proxy.DelMyDesktopNode(groupindex, nodeindex);
        }

        public bool ChangeMyDesktopInfo(string json)
        {
            return proxy.ChangeMyDesktopInfo(json);
        }

        public string SaveMyDesktopInfo(string json)
        {
            return proxy.SaveMyDesktopInfo(json);
        }

        public bool ChangeMyDesktopSet(string id, string type, string value)
        {
            return proxy.ChangeMyDesktopSet(id, type, value);
        }

    }
}
