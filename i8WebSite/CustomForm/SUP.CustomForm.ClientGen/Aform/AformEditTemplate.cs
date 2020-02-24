using System;
using System.Collections.Generic;
using System.Text;
using SUP.CustomForm.DataEntity.Container;
using System.IO;

namespace SUP.CustomForm.ClientGen.Aform
{
    public static class AformEditTemplate
    {
        public static string NameSpace { get; set; }
        public static string NameSpacePrefix { get; set; }
        public static string NameSpaceSuffix { get; set; }
        public static string ClassName { get; set; }
        public static string PkPropertyname { get; set; }
        public static string ListOrEdit { get; set; }

        public static string Title { get; set; }
        public static string Area { get; set; }
        public static GridPanel gridPanel { get; set; }
        public static List<FieldSet> fieldSets { get; set; }
        public static List<GridPanel> panels { get; set; }
        public static TableLayoutForm tableLayouts { get; set; }
        public static List<String> PanelNames { get; set; }
        public static Dictionary<String, String> Expressions { get; set; }
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


            //生成前端文件
            string sFile = string.Empty;
            if (AformListTemplate.ListOrEdit != "viewlist")  //list界面文件未生成
            {
                sFile = sDirectory + ClassName + ".cshtml";
                File.WriteAllText(sFile, new PageAppTemplate().TransformText(), Encoding.UTF8);

                sFile = sDirectory + "app.js";
                File.WriteAllText(sFile, new PageAppTemplate().TransformText(), Encoding.UTF8);
            }

            sFile = sDirectory + "app\\controller\\Edit.js";
            File.WriteAllText(sFile, new PageControllerEditTemplate().TransformText(), Encoding.UTF8);

            sFile = sDirectory + "app\\model\\Edit.js";
            File.WriteAllText(sFile, new PageModelEditTemplate().TransformText(), Encoding.UTF8);

            sFile = sDirectory + "app\\store\\Edit.js";
            File.WriteAllText(sFile, new PageStoreEditTemplate().TransformText(), Encoding.UTF8);

            sFile = sDirectory + "app\\view\\Edit.js";
            File.WriteAllText(sFile, new PageViewEditTemplate().TransformText(), Encoding.UTF8);
        }
    }

    //controller目录下edit.js
    public partial class PageControllerEditTemplate
    {
        public string NameSpace { get { return AformEditTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformEditTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformEditTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformEditTemplate.ClassName; } }
        public string PkPropertyname { get { return AformEditTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformEditTemplate.ListOrEdit; } }

        public string Title { get { return AformEditTemplate.Title; } }
        public string Area { get { return AformEditTemplate.Area; } }
        public GridPanel gridPanel { get { return AformEditTemplate.gridPanel; } }
        public List<FieldSet> fieldSets { get { return AformEditTemplate.fieldSets; } }
        public List<GridPanel> panels { get { return AformEditTemplate.panels; } }
        public TableLayoutForm tableLayouts { get { return AformEditTemplate.tableLayouts; } }
        public List<String> PanelNames { get { return AformEditTemplate.PanelNames; } }
        public Dictionary<String, String> Expressions { get; set; }
        public Toolbar Toolbar { get { return AformEditTemplate.Toolbar; } }
    }

    //model目录下edit.js
    public partial class PageModelEditTemplate
    {
        public string NameSpace { get { return AformEditTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformEditTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformEditTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformEditTemplate.ClassName; } }
        public string PkPropertyname { get { return AformEditTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformEditTemplate.ListOrEdit; } }

        public string Title { get { return AformEditTemplate.Title; } }
        public string Area { get { return AformEditTemplate.Area; } }
        public GridPanel gridPanel { get { return AformEditTemplate.gridPanel; } }
        public List<FieldSet> fieldSets { get { return AformEditTemplate.fieldSets; } }
        public List<GridPanel> panels { get { return AformEditTemplate.panels; } }
        public TableLayoutForm tableLayouts { get { return AformEditTemplate.tableLayouts; } }
        public List<String> PanelNames { get { return AformEditTemplate.PanelNames; } }
        public Dictionary<String, String> Expressions { get; set; }
        public Toolbar Toolbar { get { return AformEditTemplate.Toolbar; } }
    }

    //store目录下edit.js
    public partial class PageStoreEditTemplate
    {
        public string NameSpace { get { return AformEditTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformEditTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformEditTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformEditTemplate.ClassName; } }
        public string PkPropertyname { get { return AformEditTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformEditTemplate.ListOrEdit; } }

        public string Title { get { return AformEditTemplate.Title; } }
        public string Area { get { return AformEditTemplate.Area; } }
        public GridPanel gridPanel { get { return AformEditTemplate.gridPanel; } }
        public List<FieldSet> fieldSets { get { return AformEditTemplate.fieldSets; } }
        public List<GridPanel> panels { get { return AformEditTemplate.panels; } }
        public TableLayoutForm tableLayouts { get { return AformEditTemplate.tableLayouts; } }
        public List<String> PanelNames { get { return AformEditTemplate.PanelNames; } }
        public Dictionary<String, String> Expressions { get; set; }
        public Toolbar Toolbar { get { return AformEditTemplate.Toolbar; } }
    }

    //view目录下edit.js
    public partial class PageViewEditTemplate
    {
        public string NameSpace { get { return AformEditTemplate.NameSpace; } }
        public string NameSpacePrefix { get { return AformEditTemplate.NameSpacePrefix; } }
        public string NameSpaceSuffix { get { return AformEditTemplate.NameSpaceSuffix; } }
        public string ClassName { get { return AformEditTemplate.ClassName; } }
        public string PkPropertyname { get { return AformEditTemplate.PkPropertyname; } }
        public string ListOrEdit { get { return AformEditTemplate.ListOrEdit; } }

        public string Title { get { return AformEditTemplate.Title; } }
        public string Area { get { return AformEditTemplate.Area; } }
        public GridPanel gridPanel { get { return AformEditTemplate.gridPanel; } }
        public List<FieldSet> fieldSets { get { return AformEditTemplate.fieldSets; } }
        public List<GridPanel> panels { get { return AformEditTemplate.panels; } }
        public TableLayoutForm tableLayouts { get { return AformEditTemplate.tableLayouts; } }
        public List<String> PanelNames { get { return AformEditTemplate.PanelNames; } }
        public Dictionary<String, String> Expressions { get; set; }
        public Toolbar Toolbar { get { return AformEditTemplate.Toolbar; } }
    }
}
