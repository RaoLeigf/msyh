using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class IndividualNavigationController : AFController
    {
        private IIndividualNavigationFacade proxy;
        public IndividualNavigationController()
        {
            proxy = AopObjectProxy.GetObject<IIndividualNavigationFacade>(new IndividualNavigationFacade());
        }

        public string LoadTree()
        {
            string str = JsonConvert.SerializeObject(proxy.LoadTree());
            return str;
        }


        public string Save()
        {
            string param = System.Web.HttpContext.Current.Request.Params["param"];
            string text = System.Web.HttpContext.Current.Request.Params["text"];
            string saveType = System.Web.HttpContext.Current.Request.Params["saveType"];
            return proxy.Save(param, text, saveType);
        }

        public string Load()
        {
            string text = System.Web.HttpContext.Current.Request.Params["text"];
            return proxy.Load(text);
        }

        public bool Delete()
        {
            string text = System.Web.HttpContext.Current.Request.Params["text"];
            return proxy.Delete(text);
        }
    }
}
