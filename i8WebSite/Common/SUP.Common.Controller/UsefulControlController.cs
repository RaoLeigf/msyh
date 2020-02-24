using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.Common.Facade;
using System.Data;
using Newtonsoft.Json;
using NG3.Web.Controller;
using NG3.Aop.Transaction;

namespace SUP.Common.Controller
{
    public class UsefulControlController : AFController
    {
        private UsefulControlFacade facade= null;
        private IUsefulControlFacade proxy;

        public UsefulControlController()
        {
            facade = new UsefulControlFacade();
            proxy = AopObjectProxy.GetObject<IUsefulControlFacade>(facade);
        }

        public string GetList()
        {
            var userId = NG3.AppInfoBase.LoginID;
            string controlId = System.Web.HttpContext.Current.Request.Params["controlId"];
            string names = System.Web.HttpContext.Current.Request.Params["names"];
            var json = JsonConvert.SerializeObject(proxy.GetList(userId, controlId, names));
            return json;
        }

        public string Update()
        {
            var userId = NG3.AppInfoBase.LoginID;
            string controlId = System.Web.HttpContext.Current.Request.Params["controlId"];
            string names = System.Web.HttpContext.Current.Request.Params["names"];
            var json = JsonConvert.SerializeObject(proxy.Update(userId, controlId, names));
            return json;
        }

    }
}
