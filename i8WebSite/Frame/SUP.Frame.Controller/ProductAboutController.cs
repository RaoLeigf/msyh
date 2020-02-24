using NG.KeepConn;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Resources;


namespace SUP.Frame.Controller
{
    public class ProductAboutController : AFController
    {

        public ActionResult ProductAbout()
        {
            string Copyright = string.Empty;
            NGCOM ngcom = new NGCOM();
            ViewBag.SN = ngcom.SN;
            ViewBag.AllUsers = ngcom.AllUsers;
            ViewBag.OAUsers = ngcom.OAUsers;
            ViewBag.UserName = ngcom.UserName;
            ViewBag.FullName = GetProductAboutMsg();
            return View("ProductAbout");
        }

        /// <summary>
        /// 获取关于的详细信息
        /// </summary>
        /// <returns></returns>
        private string GetProductAboutMsg()
        {

            string result = string.Empty;
            string fullName = string.Empty;//产品名称加版本
            string moduleInfo = string.Empty;//模块列表
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/") + "product.xml");
            fullName = doc.GetElementsByTagName("FullName")[0].InnerText + doc.GetElementsByTagName("Version")[0].InnerText;
            XmlNodeList topNode = doc.SelectNodes("/Newgrand");
            XmlNodeList suiteNodes = topNode[0].SelectNodes("SuitInfo");
            foreach (XmlNode nodes in suiteNodes)
            {
                XmlNode nameNode = nodes.SelectSingleNode("Name");
                XmlNode versionNode = nodes.SelectSingleNode("Version");
                moduleInfo += nameNode.InnerText + versionNode.InnerText + ";";
            }
            return fullName + "#" + moduleInfo;
        }
    }
}
