using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity.Container;
using SUP.CustomForm.DataEntity.Control;

namespace SUP.CustomForm.ClientGen
{
    public partial class PageEditJsTemplate
    {
        public string NameSpace { get; set; }
        public string NameSpacePrefix { get; set; }
        public string NameSpaceSuffix { get; set; }
        public string ClassName { get; set; }
        public string PForm { get; set; }
        public string EForm { get; set; }
        public string QForm { get; set; }
        public string IsTask { get; set; }
        public string Reltable { get; set; }
        public string PkPropertyname { get; set; }

        public string Title { get; set; }
        public string Area { get; set; }
        public List<FieldSet> fieldSets { get; set; }
        public List<GridPanel> panels { get; set; }
        public List<GridPanel> AllGrids { get; set; }
        public TableLayoutForm tableLayouts { get; set; }
        public List<string> PanelNames { get; set; }
        public Dictionary<string, string> Expressions { get; set; }
        public Toolbar Toolbar { get; set; }
        public string TableName { get; set; }

        public int BodyCmpCount { get; set; }   //表体游离panel数量
        public string HasBlobdoc { get; set; }  //有金格控件
        public string HasEppocx { get; set; }   //有进度控件
        public string HasReport { get; set; }   //是否有报表
        public FieldSet FieldSetBlobdoc { get; set; }  //金格控件fieldset

        public List<NGPictureBox> PictureBoxs { get; set; }  //图片控件

        public string HasAsrGrid { get; set; }  //有附件单据体

        public GridPanel AsrGrid { get; set; }  //附件单据体

        public string HasWfGrid { get; set; }  //有审批单据体

        public GridPanel WfGrid { get; set; }  //审批单据体

        public void WriteEx(string sDirectory)
        {
            Common.FormTableName = TableName;
            Common.IsTask = IsTask;

            //this.NameSpace = NameSpacePrefix + ".Controller";
            sDirectory = AppDomain.CurrentDomain.BaseDirectory + "NG3Resource\\js\\eformJs\\";
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sDirectory)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(sDirectory));

            string sFile = sDirectory + this.ClassName + "Edit.js";
            System.IO.File.WriteAllText(sFile, this.TransformText(), Encoding.UTF8);
        }

    }
}
