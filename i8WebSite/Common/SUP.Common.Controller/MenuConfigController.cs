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

namespace SUP.Frame.Controller
{
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

            string json = proxy.Load(userid.ToString());
            return json;

        }
        public string Save()
        {
            string data = System.Web.HttpContext.Current.Request.Form["data"];
            //string userid = System.Web.HttpContext.Current.Request.Form["userid"];
            long userid = AppInfoBase.UserID;
            bool iret = proxy.Save(userid.ToString(), data);
            if (iret)
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
    }
}
