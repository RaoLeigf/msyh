using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Controls;
using SUP.CustomForm.DataEntity.Control;
using SUP.CustomForm.DataEntity.AppControl;

namespace SUP.CustomForm.DataEntity.Container
{
    public class TableLayoutForm : ExtContainer
    {
        
        private int columnsPerRow;
        private int formLabelWidth;
        private List<string> columnNames = new List<string>();  //所有列名

        //private List<PbBaseControlInfo> items = new List<PbBaseControlInfo>();  //这个属性基类有了，这里注释掉
        private List<ExtControlBase> allfields = new List<ExtControlBase>();
        private List<BaseField> fieldsapp = new List<BaseField>();
        private List<List<ExtControlBase>> fieldRows = new List<List<ExtControlBase>>();  //二维数组

        public TableLayoutForm()
        {
            this.IsAbsoluteLayout = true;
        }

        public List<string> ColumnNames
        {
            get { return columnNames; }
            set { columnNames = value; }
        }

        /// <summary>
        /// 每行分几列
        /// </summary>
        public int ColumnsPerRow
        {
            get { return columnsPerRow; }
            set { columnsPerRow = value; }
        }

        /// <summary>
        /// 每列占位数
        /// </summary>
        public int FormLabelWidth
        {
            get { return formLabelWidth; }
            set { formLabelWidth = value; }
        }

        ///// <summary>
        ///// 容器的子元素，pb类型
        ///// </summary>
        //public List<PbBaseControlInfo> Items
        //{
        //    get { return items; }
        //    set { items = value; }
        //}

        /// <summary>
        /// 容器的子元素，已转成ng3类型
        /// </summary>
        public List<ExtControlBase> AllFields
        {
            get { return allfields; }
            set { allfields = value; }
        }

        /// <summary>
        /// 容器的子元素，已转成ng3类型, App使用
        /// </summary>
        public List<BaseField> FieldsApp
        {
            get { return fieldsapp; }
            set { fieldsapp = value; }
        }

        public List<List<ExtControlBase>> FieldRows
        {
            get { return fieldRows; }
            set { fieldRows = value; }
        }

        //是否绝对布局
        public bool IsAbsoluteLayout { get; set; }

        //文档模板可选项
        public string Otid { get; set; }
    }
}
