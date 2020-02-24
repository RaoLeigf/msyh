using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3.Data.Service;

namespace SUP.CustomForm.ClientGen
{
    public partial class PageJsExtTemplate
    {
        public string ClassName { get; set; }
        public string ExtJsStr { get; set; }

        public void WriteEx(string sDirectory)
        {
            sDirectory = AppDomain.CurrentDomain.BaseDirectory + "NG3Resource\\js\\eformJs\\";
            string sFile = sDirectory + this.ClassName + "Ext.js";

            if (string.IsNullOrEmpty(ExtJsStr))
            {
                ExtJsStr = this.TransformText();                
            }

            System.IO.File.WriteAllText(sFile, ExtJsStr, Encoding.UTF8);
        }
    }
}
