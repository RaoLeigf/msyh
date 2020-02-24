using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity.Individual
{
    public  class ExtContainer
    {
        private string id = string.Empty;
        private string title = string.Empty;
        private string buskey = string.Empty;
        private string region = string.Empty;


        private List<ExtControlInfoBase> items = new List<ExtControlInfoBase>();
        
        public ExtContainer()
        {
 
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string BusKey
        {
            get { return buskey; }
            set { buskey = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        /// <summary>
        /// 容器的子元素
        /// </summary>
        public List<ExtControlInfoBase> Items
        {
            get { return items; }
            set { items = value; }
        }

    }
    
    public class TableLayoutForm : ExtContainer
    {
        private int columnsPerRow;
      
        private List<ExtControlInfoBase> fields = new List<ExtControlInfoBase>();


        public TableLayoutForm()
        {
 
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
        /// 容器的子元素
        /// </summary>
        public List<ExtControlInfoBase> Fields
        {
            get { return fields; }
            set { fields = value; }
        }

    }

    public class FieldSet 
    {
        private string title = string.Empty;
        private int columnsPerRow;

        private List<ExtControlInfoBase> allfields = new List<ExtControlInfoBase>();

        public FieldSet()
        {
 
        }

        /// <summary>
        /// 分组框的标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
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
        /// 容器的子元素
        /// </summary>
        public List<ExtControlInfoBase> AllFields
        {
            get { return allfields; }
            set { allfields = value; }
        }
    }

    public class FieldSetForm : ExtContainer
    {
        private List<FieldSet> fieldSets = new List<FieldSet>();

        public List<FieldSet> FieldSets
        {
            get { return fieldSets; }
            set { fieldSets = value; }
        }
    }

    public class GridPanel : ExtContainer
    {

        private List<ExtGridColumnInfoBase> columns = new List<ExtGridColumnInfoBase>();
             
        public GridPanel()
        {
 
        }

        public List<ExtGridColumnInfoBase> Columns
        {
            get { return columns; }
            set { columns = value; }
        }
    }
    
}
