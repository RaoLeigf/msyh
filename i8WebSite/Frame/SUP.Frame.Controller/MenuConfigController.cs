using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Frame.DataEntity;
using System.Data;
using System.Web.SessionState;
using System.Web;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MenuConfigController : AFController
    {
        private IMenuConfigFacade proxy;
        public MenuConfigController()
        {
            proxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
            //string xmlFile = "TabPageChoose.xml";
            //DataSet ds = proxy.ConvertXMLToDataSet(xmlFile); //路径怎么破
            //DataTable dt = ds.Tables[0];
            //int totalRecord = 0;
            //string json = DataConverterHelper.ToJson(dt, totalRecord);
            //ViewBag.DT = json;

        }
        public string Load()
        {
            //string userid = System.Web.HttpContext.Current.Request.Params["userid"];
            long userid = AppInfoBase.UserID;
            //string useridString = System.Web.HttpContext.Current.Request.Params["userid"];
            //int userid = int.Parse(useridString);

            string json = proxy.Load(userid);
            return json;

        }
        public string Save()
        {
            string data = System.Web.HttpContext.Current.Request.Form["data"];
            string isDockControlString = System.Web.HttpContext.Current.Request.Form["isDockControl"];
            
            long userid = AppInfoBase.UserID;

            bool iret = proxy.Save(userid, data);
            bool iret2 = true;
            //保存管理对象要刷新左侧面板，所有要同时保存锚定状态
            if (isDockControlString!=""&& isDockControlString !=null)
            {                
                int isDockControl;
                if (isDockControlString == "true")
                {
                    isDockControl = 1;
                }
                else
                {
                    isDockControl = 0;
                }
                iret2 = proxy.SaveDockControl(isDockControl, userid);
            }
            
            if (iret && iret2)
            {
                return "{status : \"ok\"}";
            }
            else
            {
                return "{status : \"error\"}";
            }

        }

        //public string ConvertXMLToDataSet()
        //{
        //    //string xmlFile = System.Web.HttpContext.Current.Request.Form["xmlFile"];
        //    string xmlFile = "TabPageChoose.xml";
        //    DataSet ds = proxy.ConvertXMLToDataSet(xmlFile); //路径怎么破
        //    DataTable dt = ds.Tables[0];   
        //    int totalRecord = 0;
        //    string json = DataConverterHelper.ToJson(dt, totalRecord);
        //    return json;
        //}

        public void SaveRequestRecord()
        {
            string moduleno = System.Web.HttpContext.Current.Request.Form["moduleno"];
            string url = System.Web.HttpContext.Current.Request.Form["url"];
            RequestRecordEntity record = new RequestRecordEntity();
            record.Moduleno = moduleno;
            record.Url = url;
            proxy.SaveRequestRecord(record);         
        }
    }
}
