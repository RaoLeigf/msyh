using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GQT3.QT.Controller
{
    /// <summary>
    /// 快捷方式下载
    /// </summary>
    public class DownloadkuaijieController: System.Web.Mvc.Controller
    {
        /// <summary>
        /// 快捷方式下载
        /// </summary>
        /// <returns></returns>
        public FileContentResult ShortCut()
        {
            string HostAddr = "";
            string icoPath = Server.MapPath("~/NG3Resource/Desk/img/bitbug_favicon.ico");//初始化为绝对路径
            string icoName = "/G6H/NG3Resource/Desk/img/bitbug_favicon.ico";//修改此处更改url图标或者图标路径，当前路径为根目录，只用修改相对路径，图标的完整路径由下方会自动生成
            if (!string.IsNullOrEmpty(Request.Url.Host))//如果服务器的域名地址存在则用域名地址，否则用IP地址
            {
                icoPath = "http://" + Request.Url.Host +icoName;//获取服务器域名地址，为快捷方式的地址
                HostAddr = "http://" + Request.Url.Host + "/G6H/web";//快捷方式的外部链接
                if (!string.IsNullOrEmpty(Request.ServerVariables["SERVER_PORT"].ToString()))
                {//带服务器端口的图标路径
                    icoPath = "http://" + Request.Url.Host + ":" + Request.ServerVariables["SERVER_PORT"] + icoName;
                    if (Request.ServerVariables["SERVER_PORT"].ToString() != "80")
                    {//端口不为默认值80时，域名添加上端口
                        HostAddr = "http://" + Request.Url.Host + ":" + Request.ServerVariables["SERVER_PORT"] + "/G6H/web";
                    }
                }
            }
            //string HostAddr = Configs.GetValue("weburl");
            //string icoPath = HostAddr + "/favicon.ico";//修改此处更改url图标或者图标路径，当前路径为根目录，只用修改相对路径，图标的完整路径由下方会自动生成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[InternetShortcut]");
            sb.AppendLine("URL=" + HostAddr); //快捷方式的外部链接
            sb.AppendLine("IDList= ");
            sb.AppendLine("IconFile=" +icoPath); //图标文件
            sb.AppendLine("IconIndex=1 ");
            sb.AppendLine("[{000214A0-0000-0000-C000-000000000046}]");
            sb.AppendLine("Prop3=19,2 ");
            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.Default.GetBytes(sb.ToString());
            string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
            string fileName = "浙江省总工会财务集中管控平台.url";
            if (UserAgent.IndexOf("firefox") == -1)
            {
                fileName = System.Web.HttpUtility.UrlEncode("浙江省总工会财务集中管控平台", System.Text.Encoding.UTF8) + ".url";
            }
            //string fileName = System.Web.HttpUtility.UrlEncode("工会预算项目库系统", System.Text.Encoding.UTF8) + ".url";
            return File(fileContents, "APPLICATION/OCTET-STREAM", fileName);
        }
    }
}
