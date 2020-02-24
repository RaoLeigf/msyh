using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.ServerGen
{
    public partial class ControllerTemplate
    {
        private string nameSpace { get; set; }
        private string nameSpacePrefix { get; set; }
        private string className { get; set; }
        private string tableNameM { get; set; }
        private string PForm { get; set; }
        private string EForm { get; set; }
        private string QForm { get; set; }
        private string sqlM { get; set; }
        private string Title { get; set; }
        private string sqlList { get; set; }
        private IList<TemplateDetailInfo> detailInfoList { get; set; }
        private IList<CodeToNameInfo> CodeToNameList { get; set; }
        private IList<CodeToNameInfo> CodeToNameGrid { get; set; }
        private IList<string> bitmapNameList { get; set; }
        private string hasRule { get; set; }
        private string userbillno { get; set; }

        private string HasAsrGrid { get; set; }

        public ControllerTemplate(TemplateInfo tempInfo)
        {
            nameSpace = tempInfo.NameSpacePrefix + "." + tempInfo.ClassName;
            nameSpacePrefix = tempInfo.NameSpacePrefix;
            className = tempInfo.ClassName;
            tableNameM = tempInfo.TableMaster;
            sqlM = tempInfo.SqlMaster;
            PForm = tempInfo.PForm;
            EForm = tempInfo.EForm;
            QForm = tempInfo.QForm;
            Title = tempInfo.Title;
            sqlList = tempInfo.SqlList;
            detailInfoList = tempInfo.DetailInfoList;
            CodeToNameList = tempInfo.CodeToNameList;
            CodeToNameGrid = tempInfo.CodeToNameGrid;
            bitmapNameList = tempInfo.BitmapNameList;
            HasAsrGrid = tempInfo.HasAsrGrid;
        }

        public void WriteEx(ref string sDirectory)
        {
            sDirectory = sDirectory + "\\" + nameSpacePrefix + ".Controller\\";

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sDirectory)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sDirectory));

            string sFile = sDirectory + this.className + "Controller.cs";
            System.IO.File.WriteAllText(sFile, this.TransformText());
        }

        public string WriteEx()
        {
            return this.TransformText();
        }
    }
}
