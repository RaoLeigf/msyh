using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity.Container;

namespace SUP.CustomForm.ClientGen
{
    public partial class PageListJsTemplate
    {
        public string NameSpace { get; set; }
        public string NameSpacePrefix { get; set; }
        public string NameSpaceSuffix { get; set; }
        public string ClassName { get; set; }
        public string EForm { get; set; }
        public string PForm { get; set; }
        public string QForm { get; set; }
        public string IsTask { get; set; }
        public string PkPropertyname { get; set; }
        public DataTable SourceTable { get; set; }
        public string Title { get; set; }
        public string Area { get; set; }
        public GridPanel gridPanel { get; set; }
        public Toolbar Toolbar { get; set; }
        public string HasBlobdoc { get; set; }
        public string HasEppocx { get; set; }
        public string HasReport { get; set; }
        public long defaultPc { get; set; }

        public void WriteEx(string sDirectory)
        {
            this.NameSpace = NameSpacePrefix + ".Controller";
            sDirectory = AppDomain.CurrentDomain.BaseDirectory + "NG3Resource\\js\\eformJs\\";
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sDirectory)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sDirectory));

            string sFile = sDirectory + this.ClassName + "List.js";
            System.IO.File.WriteAllText(sFile, this.TransformText(), Encoding.UTF8);
        }
    }
}
