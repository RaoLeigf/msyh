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

namespace SUP.Frame.Controller
{
    public class EnFuncTreeController : AFController
    {

        private IEnFuncTreeFacade proxy;
        public EnFuncTreeController()
        {
            proxy = AopObjectProxy.GetObject<IEnFuncTreeFacade>(new EnFuncTreeFacade());
        }
        //系统功能树
   
        public JsonResult LoadMenu()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            string suite = System.Web.HttpContext.Current.Request.Params["suite"];
            SUP.Common.Base.ProductInfo prdInfo = new SUP.Common.Base.ProductInfo();
            IList<TreeJSONBase> list = proxy.LoadMenu(prdInfo.ProductCode + prdInfo.Series, suite, false, "orguser", AppInfoBase.OrgID, AppInfoBase.UserID, nodeid);

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSuiteList()
        {
            IList<SUP.Frame.DataEntity.SuiteInfoEntity> list = proxy.GetSuiteList();
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Query()
        {

            string condition = System.Web.HttpContext.Current.Request.Params["condition"];

            SUP.Common.Base.ProductInfo prdInfo = new SUP.Common.Base.ProductInfo();
            IList<TreeJSONBase> list = proxy.Query(prdInfo.ProductCode + prdInfo.Series, false, "orguser", AppInfoBase.OrgID, AppInfoBase.UserID, condition);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }
  
    }
}
