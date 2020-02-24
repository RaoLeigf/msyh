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
    public class LoginPicManagerController : AFController
    {

        private ILoginPicManagerFacade proxy;

        public LoginPicManagerController()
        {        
            proxy = AopObjectProxy.GetObject<ILoginPicManagerFacade>(new LoginPicManagerFacade());
        }

        public ActionResult Index()
        {
            if (NG3.AppInfoBase.UserType != UserType.System)
            {
                string individualconfig = System.Web.HttpContext.Current.Request.Params["individualconfig"];
                if (string.IsNullOrEmpty(individualconfig) || individualconfig != "1")
                {
                    HttpContext.Response.Redirect("~/SUP/ErrorPage?msg=系统管理员才能设置！"); //重定向到错误页面
                }
            }

            return View("LoginPicManager");
        }

        public JsonResult GetTree()
        {
            DataTable dt = proxy.GetLoginPictureTree();

            string strPath = Request.PhysicalApplicationPath;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["type"].ToString() == "1")
                {
                    if(!System.IO.File.Exists(strPath + dt.Rows[i]["src"].ToString()) && dt.Rows[i]["attachid"] != DBNull.Value)
                    {
                        try
                        {
                            byte[] buffer = NG3UploadFileService.NG3GetEx("", (long)dt.Rows[i]["attachid"]);
                            System.IO.File.WriteAllBytes(strPath + dt.Rows[i]["src"].ToString(), buffer);
                        }
                        catch { }
                    }
                }
            }

            string filter = "(pid is null or pid='')";
            IList<TreeJSONBase> list = new LoginPicManagerSelectedTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public void AddNode(string id, string name)
        {
            proxy.AddNode(id, name, "", "");
        }

        public void DelNode(string phid)
        {
            proxy.DelNode(phid);
        }

        public string AddPicUpload()
        {
            try
            {
                string strPath = "NG3Resource\\LoginPic";
                string dirPath = Request.PhysicalApplicationPath + strPath;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                HttpPostedFileBase imgFile = Request.Files["addPic"];
                string fileName = Guid.NewGuid() + imgFile.FileName.Substring(imgFile.FileName.LastIndexOf("."));
                string filePath = dirPath + "\\" + fileName;
                imgFile.SaveAs(filePath);
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

        public void SavePicUpload(string name, string id, string src, string deletefiles)
        {
            CancelPicUpload(deletefiles);
            string attachid = AttachUpload("NG3Resource\\LoginPic\\" + src);
            proxy.AddNode(id, name, "NG3Resource/LoginPic/" + src, attachid);
        }
        
        public void CancelPicUpload(string deletefiles)
        {
            string[] files = deletefiles.Split('|');
            string path = Request.PhysicalApplicationPath + "NG3Resource\\LoginPic\\";
            for (int i = 0; i < files.Length - 1; i++)
            {
                if (System.IO.File.Exists(path + files[i]))
                {
                    System.IO.File.Delete(path + files[i]);
                }
            }
        }

        public string AttachUpload(string src)
        {
            string path = Request.PhysicalApplicationPath + src;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer;
            try
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            catch
            {
                return "";
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

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

        public string GetLoginPicSet()
        {
            string loginPicSet = JsonConvert.SerializeObject(proxy.GetLoginPicSet());            
            return "{\"UserType\": \"" + NG3.AppInfoBase.UserType + "\", \"LoginPicSet\": " + loginPicSet + "}";
        }

        public string ChangeShowType(string showtype, string showpic)
        {
            return proxy.ChangeShowType(showtype, showpic);
        }

        public void SaveLoginPicSet(string showtype, string showlogo, string allowuser, string showpic)
        {
            proxy.SaveLoginPicSet(showtype, showlogo, allowuser, showpic);
        }

        class LoginPicManagerSelectedTreeJSONBase : TreeJSONBase
        {
            public virtual string phid { get; set; }

            public virtual string pid { get; set; }

            public virtual string type { get; set; }

            public virtual string src { get; set; }

            public virtual string sysflg { get; set; }

            public virtual string userid { get; set; }           
        }

        public class LoginPicManagerSelectedTreeBuilder : ExtJsTreeBuilderBase
        {
            public override TreeJSONBase BuildTreeNode(DataRow dr)
            {
                LoginPicManagerSelectedTreeJSONBase node = new LoginPicManagerSelectedTreeJSONBase();
                node.phid = dr["phid"].ToString();
                node.pid = dr["pid"].ToString();
                node.expanded = string.IsNullOrEmpty(node.pid);
                node.id = dr["id"].ToString();
                node.text = dr["name"].ToString();
                node.type = dr["type"].ToString();
                node.src = dr["src"].ToString();
                node.sysflg = dr["sysflg"].ToString();
                node.userid = dr["userid"].ToString();
                return node;
            }
        }

    }
}
