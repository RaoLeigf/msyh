using System;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Web.SessionState;
using Newtonsoft.Json;
using SUP.Common.Base;
using SUP.Frame.Facade;
using SUP.Frame.DataEntity;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using NG3.Bill.Base;
using Newtonsoft.Json.Linq;

namespace SUP.Frame.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class FuncIconManagerController : AFController
    {
        private IFuncIconManagerFacade proxy;

        public FuncIconManagerController()
        {
            proxy = AopObjectProxy.GetObject<IFuncIconManagerFacade>(new FuncIconManagerFacade());
        }

        //[PageAuthorize(RightKey = 39842)]
        public ActionResult Index()
        {
            try
            {
                IMenuConfigFacade MenuConfigProxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
                string json = MenuConfigProxy.LoadEnFuncTreeRight();
                ViewBag.LoadEnFuncTreeRight = json;
                ViewBag.UserType = NG3.AppInfoBase.UserType;
            }
            catch { }
            return View("FuncIconManager");
        }

        //[PageAuthorize(RightKey = 39843)]
        public ActionResult SysIconLib()
        {
            return View("SysIconLib");
        }

        public string AddIconUpload(string name, string tag)
        {
            try
            {
                name = Guid.NewGuid().ToString() + name.Substring(name.LastIndexOf("."));

                string strPath = "NG3Resource\\CustomIcons";
                string dirPath = Request.PhysicalApplicationPath + strPath;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                HttpPostedFileBase imgFile = Request.Files["addCustomIcon"];
                string filePath = dirPath + "\\" + name;
                imgFile.SaveAs(filePath);

                byte[] buffer = GetFileData(filePath);
                string attachid = AttachUpload(buffer);

                return JsonConvert.SerializeObject(proxy.AddFuncIcon(name, tag, attachid));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ReplaceIconUpload(string id, string name, string tag, string attachid)
        {
            try
            {
                name = Guid.NewGuid().ToString() + name.Substring(name.LastIndexOf("."));

                string strPath = "NG3Resource\\CustomIcons\\";
                string dirPath = Request.PhysicalApplicationPath + strPath;
                HttpPostedFileBase imgFile = Request.Files["replaceCustomIcon"];
                string filePath = dirPath + name;
                imgFile.SaveAs(filePath);

                if (!string.IsNullOrEmpty(attachid))
                {
                    byte[] buffer = GetFileData(filePath);
                    NG3UploadFileService.NG3ModifyEx("", long.Parse(attachid), buffer);
                }

                return JsonConvert.SerializeObject(proxy.ReplaceFuncIcon(id, name, tag));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        byte[] GetFileData(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer;
            try
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return buffer;
        }

        string AttachUpload(byte[] buffer)
        {
            if (buffer == null) return "";

            string result = NG3UploadFileService.NG3UploadEx("", buffer);
            JObject jo = (JObject)JsonConvert.DeserializeObject(result);

            if (jo["success"] != null && jo["success"].ToString() == "true")
            {
                return jo["phid"].ToString();
            }
            else
            {
                return "";
            }
        }

        public bool DelIcon(string id)
        {
            return proxy.DelFuncIcon(id);
        }

        public string GetGrid(string code,string suite)
        {
            int totalRecord = 0;
            DataTable dt = proxy.GetFuncIconGrid(code, suite, ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

        public string GetIcons(string tag)
        {
            int totalRecord = 0;

            DataTable dt = proxy.GetFuncIcons(tag, ref totalRecord);

            string json = JsonConvert.SerializeObject(dt);

            json = "{totalRows: " + totalRecord + ", items: " + json + "}";

            return json;
        }

        public string GetIconsEx(bool buildinIconShow, bool customIconShow, string tag)
        {
            int totalRecord = 0;

            DataTable dt = proxy.GetFuncIconsEx(buildinIconShow, customIconShow, tag, ref totalRecord);

            string json = JsonConvert.SerializeObject(dt);

            json = "{totalRows: " + totalRecord + ", items: " + json + "}";

            return json;
        }

        public bool AddTag(string name)
        {
            return proxy.AddFuncIconTag(name);
        }

        public bool DelTag(string name)
        {
            return proxy.DelFuncIconTag(name);
        }

        public string GetTagGrid(string search)
        {
            int totalRecord = 0;

            DataTable dt = proxy.GetFuncIconTagGrid(search, ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);

            return json;
        }

        public bool Save(string menuGrids)
        {
            List<FuncIconEntity> FuncIconList = JsonConvert.DeserializeObject<List<FuncIconEntity>>(menuGrids);            
            return proxy.Save(FuncIconList);
        }

    }
}
