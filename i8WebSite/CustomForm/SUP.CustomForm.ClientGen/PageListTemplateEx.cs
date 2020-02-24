using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity.Container;

namespace SUP.CustomForm.ClientGen
{
    public partial class PageListTemplate
    {
        public string NameSpace { get; set; }
        public string NameSpacePrefix { get; set; }
        public string NameSpaceSuffix { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }

        public void WriteEx(string sDirectory)
        {
            this.NameSpace = NameSpacePrefix + ".Controller";
            sDirectory = AppDomain.CurrentDomain.BaseDirectory + "Areas\\" + sDirectory + "\\Views\\";
            if(!string.IsNullOrEmpty(this.NameSpaceSuffix))
            {
                this.NameSpace += "." + NameSpaceSuffix;
                sDirectory += this.ClassName + "List\\";
            }
            if(!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sDirectory)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sDirectory));

            string sFile = sDirectory + this.ClassName + "List.cshtml";
            System.IO.File.WriteAllText(sFile , this.TransformText() , Encoding.UTF8);
        }
    }
}
