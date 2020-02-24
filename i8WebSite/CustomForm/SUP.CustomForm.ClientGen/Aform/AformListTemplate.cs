using System;
using System.Data;
using System.Text;
using SUP.CustomForm.DataEntity.Container;
using System.IO;

namespace SUP.CustomForm.ClientGen.Aform
{
    public static class AformListTemplate
    {
        public static string NameSpace { get; set; }
        public static string NameSpacePrefix { get; set; }
        public static string NameSpaceSuffix { get; set; }
        public static string ClassName { get; set; }
        public static string PkPropertyname { get; set; }
        public static string ListOrEdit { get; set; }

        public static DataTable SourceTable { get; set; }
        public static string Title { get; set; }
        public static string Area { get; set; }
        public static GridPanel gridPanel { get; set; }
        public static Toolbar Toolbar { get; set; }

        public static void WriteEx(string sDirectory)
        {
            NameSpace = NameSpacePrefix + ".Controller";
            sDirectory = AppDomain.CurrentDomain.BaseDirectory + "AppJS\\" + sDirectory + "\\";

            if (!string.IsNullOrEmpty(NameSpaceSuffix))
            {
                NameSpace += "." + NameSpaceSuffix;
                sDirectory += ClassName + "\\";
            }

            //创建前端mvc文件夹
            //aformxxxxxxxxxx 
            if (!Directory.Exists(Path.GetDirectoryName(sDirectory)))
                Directory.CreateDirectory(Path.GetDirectoryName(sDirectory));

            //aformxxxxxxxxxx\app
            if (!Directory.Exists(Path.GetDirectoryName(sDirectory + "app\\")))
                Directory.CreateDirectory(Path.GetDirectoryName(sDirectory + "app\\"));

            //aformxxxxxxxxxx\app\controller
            if (!Directory.Exists(Path.GetDirectoryName(sDirectory + "app\\controller\\")))
                Directory.CreateDirectory(Path.GetDirectoryName(sDirectory + "app\\controller\\"));

            //aformxxxxxxxxxx\app\model
            if (!Directory.Exists(Path.GetDirectoryName(sDirectory + "app\\model\\")))
                Directory.CreateDirectory(Path.GetDirectoryName(sDirectory + "app\\model\\"));

            //aformxxxxxxxxxx\app\store
            if (!Directory.Exists(Path.GetDirectoryName(sDirectory + "app\\store\\")))
                Directory.CreateDirectory(Path.GetDirectoryName(sDirectory + "app\\store\\"));

            //aformxxxxxxxxxx\app\view
            if (!Directory.Exists(Path.GetDirectoryName(sDirectory + "app\\view\\")))
                Directory.CreateDirectory(Path.GetDirectoryName(sDirectory + "app\\view\\"));

            //aformxxxxxxxxxx\list
            string listDirectory = AppDomain.CurrentDomain.BaseDirectory + "Areas\\SUP\\Views\\" + ClassName + "List\\";
            if (!Directory.Exists(Path.GetDirectoryName(listDirectory)))
                Directory.CreateDirectory(Path.GetDirectoryName(listDirectory));


            //生成前端文件
            string sFile = sDirectory + "app.js";
            File.WriteAllText(sFile, new PageAppTemplate().TransformText(), Encoding.UTF8);

            sFile = sDirectory + "app\\controller\\List.js";
            File.WriteAllText(sFile, new PageControllerListTemplate().TransformText(), Encoding.UTF8);

            sFile = sDirectory + "app\\model\\List.js";
            File.WriteAllText(sFile, new PageModelListTemplate().TransformText(), Encoding.UTF8);

            sFile = sDirectory + "app\\store\\List.js";
            File.WriteAllText(sFile, new PageStoreListTemplate().TransformText(), Encoding.UTF8);

            sFile = sDirectory + "app\\view\\List.js";
            File.WriteAllText(sFile, new PageViewListTemplate().TransformText(), Encoding.UTF8);

            sFile = listDirectory + ClassName + "List.cshtml";
            File.WriteAllText(sFile, new PageHtmlTemplate().TransformText(), Encoding.UTF8);
        }
    }

    //aformxxxxxxxxxx.cshtml
    public partial class PageHtmlTemplate
    {
        public string NameSpace { get { return AformListTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformListTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformListTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformListTemplate.ClassName; } }
        public string PkPropertyname { get { return AformListTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformListTemplate.ListOrEdit; } }

        public DataTable SourceTable { get { return AformListTemplate.SourceTable; } }
        public string Title { get { return AformListTemplate.Title; } }
        public string Area { get { return AformListTemplate.Area; } }
        public GridPanel gridPanel { get { return AformListTemplate.gridPanel; } }
        public Toolbar Toolbar { get { return AformListTemplate.Toolbar; } }
    }

    //app.js
    public partial class PageAppTemplate
    {
        public string NameSpace { get { return AformListTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformListTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformListTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformListTemplate.ClassName; } }
        public string PkPropertyname { get { return AformListTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformListTemplate.ListOrEdit; } }

        public DataTable SourceTable { get { return AformListTemplate.SourceTable; } }
        public string Title { get { return AformListTemplate.Title; } }
        public string Area { get { return AformListTemplate.Area; } }
        public GridPanel gridPanel { get { return AformListTemplate.gridPanel; } }
        public Toolbar Toolbar { get { return AformListTemplate.Toolbar; } }
    }

    //controller目录下list.js
    public partial class PageControllerListTemplate
    {
        public string NameSpace { get { return AformListTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformListTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformListTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformListTemplate.ClassName; } }
        public string PkPropertyname { get { return AformListTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformListTemplate.ListOrEdit; } }

        public DataTable SourceTable { get { return AformListTemplate.SourceTable; } }
        public string Title { get { return AformListTemplate.Title; } }
        public string Area { get { return AformListTemplate.Area; } }
        public GridPanel gridPanel { get { return AformListTemplate.gridPanel; } }
        public Toolbar Toolbar { get { return AformListTemplate.Toolbar; } }
    }

    //model目录下list.js
    public partial class PageModelListTemplate
    {
        public string NameSpace { get { return AformListTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformListTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformListTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformListTemplate.ClassName; } }
        public string PkPropertyname { get { return AformListTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformListTemplate.ListOrEdit; } }

        public DataTable SourceTable { get { return AformListTemplate.SourceTable; } }
        public string Title { get { return AformListTemplate.Title; } }
        public string Area { get { return AformListTemplate.Area; } }
        public GridPanel gridPanel { get { return AformListTemplate.gridPanel; } }
        public Toolbar Toolbar { get { return AformListTemplate.Toolbar; } }
    }

    //store目录下list.js
    public partial class PageStoreListTemplate
    {
        public string NameSpace { get { return AformListTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformListTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformListTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformListTemplate.ClassName; } }
        public string PkPropertyname { get { return AformListTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformListTemplate.ListOrEdit; } }

        public DataTable SourceTable { get { return AformListTemplate.SourceTable; } }
        public string Title { get { return AformListTemplate.Title; } }
        public string Area { get { return AformListTemplate.Area; } }
        public GridPanel gridPanel { get { return AformListTemplate.gridPanel; } }
        public Toolbar Toolbar { get { return AformListTemplate.Toolbar; } }
    }

    //view目录下list.js
    public partial class PageViewListTemplate
    {
        public string NameSpace { get { return AformListTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformListTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformListTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformListTemplate.ClassName; } }
        public string PkPropertyname { get { return AformListTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformListTemplate.ListOrEdit; } }

        public DataTable SourceTable { get { return AformListTemplate.SourceTable; } }
        public string Title { get { return AformListTemplate.Title; } }
        public string Area { get { return AformListTemplate.Area; } }
        public GridPanel gridPanel { get { return AformListTemplate.gridPanel; } }
        public Toolbar Toolbar { get { return AformListTemplate.Toolbar; } }
    }
}
