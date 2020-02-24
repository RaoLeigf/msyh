using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NG3.Web.Controller;
using NG3.Web.Mvc;
using System.Web.Mvc;

using SUP.Common.Controller.WebReference;
using System.Web.SessionState;
using System.Data;

namespace SUP.Common.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AttachmentController : AFController 
    {

        private UploadFileService proxy = new UploadFileService();

        SUP.Common.Base.ProductInfo productInfo = new SUP.Common.Base.ProductInfo();

        private string product = string.Empty;

        public AttachmentController()
        {
            product = productInfo.ProductCode + productInfo.Series;

            string port = System.Web.HttpContext.Current.Request.Url.Port.ToString();

            //web.config配置节点标识附件服务器的站点
            //string busWebsite = System.Configuration.ConfigurationManager.AppSettings["BusWebSite"];
            string attachSrvURL = System.Configuration.ConfigurationManager.AppSettings["AttachServiceURL"];

            //if (!string.IsNullOrWhiteSpace(busWebsite))
            //{
            //    if (string.IsNullOrWhiteSpace(port))
            //    {
            //        proxy.Url = "http://127.0.0.1/" + busWebsite + "FileSrv/UploadFileService.asmx";
            //    }
            //    else
            //    {
            //        proxy.Url = "http://127.0.0.1:" + port + "/" + busWebsite + "FileSrv/UploadFileService.asmx";
            //    }   
            //}
            if (!string.IsNullOrWhiteSpace(attachSrvURL))
            {
                proxy.Url = attachSrvURL;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(port))
                {
                    proxy.Url = "http://127.0.0.1/" + product + "FileSrv/UploadFileService.asmx";
                }
                else
                {
                    proxy.Url = "http://127.0.0.1:" + port + "/" + product + "FileSrv/UploadFileService.asmx";
                }
            }
            
        }


        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index(string helpid)
        {
            this.InitialParam();

            return View("Attachment");
        }

        private void InitialParam()
        {
            string logid = string.Empty;
            string userName = string.Empty;

            if (NG3.AppInfoBase.UP != null)
            {
                logid = NG3.AppInfoBase.LoginID;
                userName = NG3.AppInfoBase.UserName;
            }

            //web.config配置节点标识附件服务器的站点
            string busWebsite = System.Configuration.ConfigurationManager.AppSettings["BusWebSite"];
            if (!string.IsNullOrWhiteSpace(busWebsite))
            {
                ViewBag.product = busWebsite;
            }
            else
            {
                ViewBag.product = product;
            }
                       
            ViewBag.fill = logid;
            ViewBag.fillname = userName;
            ViewBag.btnAdd = System.Web.HttpContext.Current.Request.Params["btnAdd"];//是否有新增按钮，'1'或者'0'
            ViewBag.status = System.Web.HttpContext.Current.Request.Params["status"];
            ViewBag.btnwebok = System.Web.HttpContext.Current.Request.Params["btnWebOk"];
            ViewBag.autosave = System.Web.HttpContext.Current.Request.Params["autoSave"];
            ViewBag.fileNum = System.Web.HttpContext.Current.Request.Params["fileNum"];//附件数量
            ViewBag.Height = System.Web.HttpContext.Current.Request.Params["Height"];//窗口高度
            ViewBag.btnScan = System.Web.HttpContext.Current.Request.Params["btnScan"];
            ViewBag.btnDelete = System.Web.HttpContext.Current.Request.Params["btnDelete"];//删除按钮
            ViewBag.btnEdit = System.Web.HttpContext.Current.Request.Params["btnEdit"];//修改按钮
            ViewBag.btnView = System.Web.HttpContext.Current.Request.Params["btnView"];//查看按钮
            ViewBag.btnDownload = System.Web.HttpContext.Current.Request.Params["btnDownload"];//查看按钮
            
            string attachTableName = System.Web.HttpContext.Current.Request.Params["attachTName"];
            string busTableName = System.Web.HttpContext.Current.Request.Params["busTName"];
            string busid = System.Web.HttpContext.Current.Request.Params["busid"];
            ViewBag.guid = busid;

            string attachGuid = System.Web.HttpContext.Current.Request.Params["attachguid"];//由客户端生成传过来
            string guid = string.Empty;//
            if (!string.IsNullOrEmpty(attachGuid))
            {
                guid = attachGuid;
            }
            else 
            {
                guid = Guid.NewGuid().ToString();//查看时客户端可能不传guid，自己生成guid，自己控制,init方法也返回这个guid
            }

            string fileConnect = ConfigurationManager.AppSettings["FileDBConnectString"];

            string bustypecode = System.Web.HttpContext.Current.Request.Params["bustypecode"] == null ? string.Empty : System.Web.HttpContext.Current.Request.Params["bustypecode"];//业务类型编码 
            string orgid = System.Web.HttpContext.Current.Request.Params["orgid"] == null ? "0" : System.Web.HttpContext.Current.Request.Params["orgid"];//组织phid
            string busurl = System.Web.HttpContext.Current.Request.Params["busurl"] == null ? string.Empty : System.Web.HttpContext.Current.Request.Params["busurl"];//业务单据url
            string asr_where = string.Empty;

            ViewBag.guid = proxy.Init3(guid, attachTableName, busTableName, busid, logid, userName, string.IsNullOrWhiteSpace(fileConnect) ? NG3.AppInfoBase.UserConnectString : fileConnect, bustypecode, orgid, busurl, asr_where);
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string Save(string attachguid, string buscode)
        {
            string str = string.Empty;
            try
            {
                str = proxy.Save(attachguid, buscode, "0");
                return "{\"status\":\"ok\"}";
            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\",\"msg\":\"" + ex.Message +"\"}";
                throw ex;
            }
                       
        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string ClearTempData(string attachguid)
        {
            string str = string.Empty;
            try
            {
                str = proxy.ClearCache(attachguid);
                return "{\"status\":\"ok\"}";
            }
            catch (Exception ex)
            {
                return "{\"status\":\"error\", \"msg\":\"" + ex.Message + "\"}";
                throw ex;
            }
          
        }

        public string GetAttachmentInfo(string guid)
        {
            DataTable dt;
            try
            {
                dt = proxy.GetAttachInfo(guid);
                return "{\"Status\":\"success\",\"AttachmentInfo\":" + Newtonsoft.Json.JsonConvert.SerializeObject(dt) + "}";
            }
            catch (Exception exception)
            {
                return "{\"Status\":\"error\", \"msg\":\"" + exception.Message + "\"}";
            }

        }

        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string AttachmentInit()
        {
            string logid = string.Empty;
            string userName = string.Empty;

            if (NG3.AppInfoBase.UP != null)
            {
                logid = NG3.AppInfoBase.LoginID;
                userName = NG3.AppInfoBase.UserName;
            }        

            string attachTableName = System.Web.HttpContext.Current.Request.Params["attachTName"];
            string busTableName = System.Web.HttpContext.Current.Request.Params["busTName"];
            string busid = System.Web.HttpContext.Current.Request.Params["busid"];

            string attachGuid = System.Web.HttpContext.Current.Request.Params["attachguid"];//由客户端生成传过来
            string guid = string.Empty;//
            if (!string.IsNullOrEmpty(attachGuid))
            {
                guid = attachGuid;
            }
            else
            {
                guid = Guid.NewGuid().ToString();//查看时客户端可能不传guid，自己生成guid，自己控制,init方法也返回这个guid
            }

            string fileConnect = ConfigurationManager.AppSettings["FileDBConnectString"];
            string bustypecode = System.Web.HttpContext.Current.Request.Params["bustypecode"];
            string orgid = System.Web.HttpContext.Current.Request.Params["orgid"] == null ? "0" : System.Web.HttpContext.Current.Request.Params["orgid"];
            string busurl = System.Web.HttpContext.Current.Request.Params["busurl"] == null ? string.Empty : System.Web.HttpContext.Current.Request.Params["busurl"];
            string phidsfilter = System.Web.HttpContext.Current.Request.Params["phidsfilter"] == null ? string.Empty : System.Web.HttpContext.Current.Request.Params["phidsfilter"];

            string result = string.Empty;
            string asr_where = string.Empty;
            string strqryphid = string.Empty;

            if (!string.IsNullOrEmpty(phidsfilter))
            {
                string[] aryphid = phidsfilter.Split(',');

                for (int i = 0; i < aryphid.Length; i++)
                {
                    if (string.IsNullOrEmpty(strqryphid))
                    {
                        strqryphid = aryphid[i];
                    }
                    else
                    {
                        strqryphid += "," + aryphid[i];
                    }
                }

                asr_where = "phid in (" + strqryphid + ")";
            }

            try
            {
                result = proxy.Init3(guid, attachTableName, busTableName, busid, logid, userName, string.IsNullOrWhiteSpace(fileConnect) ? NG3.AppInfoBase.UserConnectString : fileConnect, bustypecode, orgid, busurl, asr_where);

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            if (string.IsNullOrEmpty(result) || result == "0")
            {
                return "{\"status\":\"error\",msg:\"" + result + "\"}";
            }
            else
            {
                return "{\"status\":\"success\",\"msg\":\"" + result + "\"}";
            }
        }
    }
}
