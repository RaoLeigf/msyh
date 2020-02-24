using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using System.Data;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MyFuncTreeController : AFController
    {
        private IMyFuncTreeFacade proxy;
        public MyFuncTreeController()
        {
            proxy = AopObjectProxy.GetObject<IMyFuncTreeFacade>(new MyFuncTreeFacade());
        }
        public JsonResult LoadMyFuncTree()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            long userid = AppInfoBase.UserID;//作为参数传进来
            //string useridString = System.Web.HttpContext.Current.Request.Params["userid"];
            //int userid = int.Parse(useridString);

            IList<TreeJSONBase> list = proxy.LoadMyFuncTree(userid, nodeid);
            if(list == null)
            {
                return null;
            }
            else
            {
                return this.Json(list, JsonRequestBehavior.AllowGet);
            }
            
        }
        public string Save()
        {
            string myFuncTree = System.Web.HttpContext.Current.Request.Form["myFuncTree"];
            //string userid = System.Web.HttpContext.Current.Request.Form["userid"];
            long userid = AppInfoBase.UserID;
            DataTable myFuncTreeTable = DataConverterHelper.ToDataTable(myFuncTree, "select * from fg3_myfunctree");
            int iret;
            try
            {
                 iret = proxy.Save(myFuncTreeTable, userid);
                if (iret > 0)
                {
                    return "{status : \"ok\"}";
                }
                else
                {
                    return "{status : \"error\",messsage:\"保存失败\"}";
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return "{status : \"error\",messsage:\""+ ex.Message + "\"}";
            }
           

        }
    }
}
