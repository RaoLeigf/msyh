using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity.Container;

namespace SUP.CustomForm.ClientGen
{
    public partial class PageAppTemplate
    {
        public string NameSpace { get; set; }
        public string NameSpacePrefix { get; set; }
        public string NameSpaceSuffix { get; set; }
        public string ClassName { get; set; }
        public string PkPropertyname { get; set; }

        public string Title { get; set; }
        public string Area { get; set; }
        public List<FieldSet> fieldSets { get; set; }
        public List<GridPanel> panels { get; set; }
        public TableLayoutForm tableLayouts { get; set; }
        public List<String> PanelNames { get; set; }
        public Dictionary<String, String> Expressions { get; set; }
        public Toolbar Toolbar { get; set; }

        public void WriteEx(string sDirectory)
        {
            this.NameSpace = NameSpacePrefix + ".Controller";
            sDirectory = AppDomain.CurrentDomain.BaseDirectory + "Areas\\" + sDirectory + "\\Views\\";
            if (!string.IsNullOrEmpty(this.NameSpaceSuffix))
            {
                this.NameSpace += "." + NameSpaceSuffix;
                sDirectory += this.ClassName + "App\\"; //NameSpaceSuffix + "\\";
            }
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sDirectory)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sDirectory));

            string sFile = sDirectory + this.ClassName + "App.cshtml";
            System.IO.File.WriteAllText(sFile, this.TransformText(), Encoding.UTF8);
        }

    }
}
