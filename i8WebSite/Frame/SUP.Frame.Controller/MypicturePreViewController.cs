using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3.Aop.Transaction;
using NG3.Bill.Base;
using NG3.Web.Controller;
using SUP.Common.Base;
using SUP.Frame.Facade;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MypicturePreViewController : AFController
    {

        public MypicturePreViewController()
        {        
            
        }

        public ActionResult MypicturePreView()
        {
            ViewBag.fid = System.Web.HttpContext.Current.Request.Params["fid"];
            ViewBag.extention = System.Web.HttpContext.Current.Request.Params["extention"];
            return View("MypicturePreView");
        }

        /// <summary>
        /// 我的照片protal web版预览
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public string GetPicSrc()
        {
            try
            {
                string fid = System.Web.HttpContext.Current.Request.Params["fid"];
                string extention = System.Web.HttpContext.Current.Request.Params["extention"];

                string strPath = "NG3Resource\\MyPicPortalView";
                string dirPath = Request.PhysicalApplicationPath + strPath;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string fileName = fid + "." + extention;
                string picFile = dirPath + Path.DirectorySeparatorChar + fileName;

                if (System.IO.File.Exists(picFile))
                {
                    System.IO.File.Delete(picFile);
                }

                string base64 = NG3.Bill.Base.NG3UploadFileService.PicInvoke("NG.MyPicture", "NG.MyPicture.NGMyPicture", "GetResourceImage", new object[] { fid, NG3.AppInfoBase.UserConnectString });
                if (!string.IsNullOrEmpty(base64))
                {
                    byte[] buffers = Convert.FromBase64String(base64);
                    FileStream fs = new FileStream(picFile, FileMode.Create);
                    fs.Write(buffers, 0, buffers.Length);
                    fs.Close();
                }

                return DataConverterHelper.SerializeObject(new
                {
                    success = true,
                    filename = fileName
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
