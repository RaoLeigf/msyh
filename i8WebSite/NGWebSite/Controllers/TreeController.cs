using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExtJsSystemDemo.Controllers
{
    public class TreeController : Controller
    {
        //
        // GET: /Tree/

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult LoadTree()
        {

            string app = System.Web.HttpContext.Current.Request.ApplicationPath;

            if (app == "/")
            {
                app = string.Empty;//
            }

            IList<TreeJSON> rootlist = new List<TreeJSON>();

            IList<TreeJSON> list = new List<TreeJSON>();

            TreeJSON root = new TreeJSON();
            root.id = "root";
            root.cls = "folder";
            root.text = "ExtJS 4.x";
            root.expanded = true;
           // root.@checked = false;
            root.children = list;

            TreeJSON firstchild = new TreeJSON();
            firstchild.id = "first";
            firstchild.cls = "file";
            firstchild.text = "Grid";
            firstchild.leaf = true;
            firstchild.url = app + "/HR/StudentInfo";
           // firstchild.@checked = false;

            TreeJSON secchild = new TreeJSON();
            secchild.id = "secchild";
            secchild.cls = "file";
            secchild.text = "Form";
            secchild.leaf = true;
            secchild.url = app + "/HR/StudentInfo/StudentInfoDetail";
           // secchild.@checked = false;

            TreeJSON thirdchild = new TreeJSON();
            thirdchild.id = "third";
            thirdchild.cls = "file";
            thirdchild.text = "Combox";
            thirdchild.leaf = true;
            thirdchild.url = app + "/Help";

            TreeJSON fourchild = new TreeJSON();
            fourchild.id = "four";
            fourchild.cls = "file";
            fourchild.text = "员工信息列表";
            fourchild.leaf = true;
            fourchild.url = app + "/HR/EmpInfoList";

            TreeJSON fivechild = new TreeJSON();
            fivechild.id = "five";
            fivechild.cls = "file";
            fivechild.text = "Designer";
            fivechild.leaf = true;
            fivechild.url = app + "/SUP/Designer";

            TreeJSON sixchild = new TreeJSON();
            sixchild.id = "six";
            sixchild.cls = "six";
            sixchild.text = "UserInfo";
            sixchild.leaf = true;
            sixchild.url = app + "/HR/EmpInfoList/UserInfoList";

       
            root.children.Add(sixchild);
            root.children.Add(fivechild);
            root.children.Add(fourchild);
            root.children.Add(thirdchild);
            root.children.Add(secchild);
            root.children.Add(firstchild);           
           

            rootlist.Add(root);

            JsonResult jsons = this.Json(rootlist, JsonRequestBehavior.AllowGet);
            return jsons;  

        }        
    }

    //ext树的数据格式
    [Serializable]
    public class TreeJSON
    {
        public virtual string id { get; set; }

        public virtual string text { get; set; }

        public virtual string url { get; set; }

        public virtual string cls { get; set; }

        public virtual bool expanded { get; set; }

        public virtual IList<TreeJSON> children { get; set; }

        public virtual bool leaf { get; set; }

        //public virtual bool @checked { get; set; }

        public virtual string hrefTarget { get; set; }

    } 
 

}
