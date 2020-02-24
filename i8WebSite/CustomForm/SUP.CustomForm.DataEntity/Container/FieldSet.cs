using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Controls;
using SUP.CustomForm.DataEntity.Control;

namespace SUP.CustomForm.DataEntity.Container
{
    public class FieldSet
    {
        private string id = string.Empty;
        private string title = string.Empty;
        private int columnsPerRow;        
        private int x = -1;
        private int y = -1;
        private int height = -1;
        private int width = -1;
        private int minheight = -1;
        private string region = string.Empty;
        private bool collapsible = false;  //是否允许面板展开收缩
        private bool collapsed = false;    //设置面板在第一次渲染时是否处于收缩状态（true为收缩）
        private bool border = true;  //fieldset边框是否存在

        private List<ExtControlBase> allfields = new List<ExtControlBase>();
        private List<PbBaseControlInfo> items = new List<PbBaseControlInfo>();
        private List<GridPanel> panels = new List<GridPanel>();
        private List<List<ExtControlBase>> fieldRows = new List<List<ExtControlBase>>();  //二维数组

        public FieldSet()
        {

        }        

        public string ID
        {
            get { return id; }
            set { id = value; }
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

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int MinHeight
        {
            get { return minheight; }
            set { minheight = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public bool Collapsible
        {
            get { return collapsible; }
            set { collapsible = value; }
        }

        public bool Collapsed
        {
            get { return collapsed; }
            set { collapsed = value; }
        }

        public bool Border
        {
            get { return border; }
            set { border = value; }
        }

        /// <summary>
        /// 容器的子元素，已转成ng3类型
        /// </summary>
        public List<ExtControlBase> AllFields
        {
            get { return allfields; }
            set { allfields = value; }
        }

        /// <summary>
        /// 容器的子元素，pb类型
        /// </summary>
        public List<PbBaseControlInfo> Items
        {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// 容器里的panel
        /// </summary>
        public List<GridPanel> Panels
        {
            get { return panels; }
            set { panels = value; }
        }

        public List<List<ExtControlBase>> FieldRows
        {
            get { return fieldRows; }
            set { fieldRows = value; }
        }
    }
}
