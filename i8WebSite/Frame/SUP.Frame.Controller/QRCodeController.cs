using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using System.Web.SessionState;
using WM3.KM.Service.Interface;
using Newtonsoft.Json.Linq;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class QRCodeController : AFController
    {
        private IQRCodeFacade proxy;

        public QRCodeController()
        {
            proxy = AopObjectProxy.GetObject<IQRCodeFacade>(new QRCodeFacade());           
        }
        public string getUrlByCode(string code)
        {
            DataTable dt = proxy.getUrlByCode(code);
            return DataConverterHelper.ToJson(dt, 1);
        }
    }
}
