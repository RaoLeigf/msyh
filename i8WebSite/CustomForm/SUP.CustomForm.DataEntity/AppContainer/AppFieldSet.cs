using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity.Control;
using NG3.Metadata.UI.PowserBuilder.Controls;

namespace SUP.CustomForm.DataEntity.AppContainer
{
    public class AppFieldSet
    {
        private string id = string.Empty;
        private string title = string.Empty;   
        private int x = -1;
        private int y = -1;
        private int height = -1;
        private int width = -1;

        private List<ExtControlBase> allfields = new List<ExtControlBase>();
        private List<PbBaseControlInfo> items = new List<PbBaseControlInfo>();

        public AppFieldSet()
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

        /// <summary>
        /// 容器的子元素
        /// </summary>
        public List<ExtControlBase> AllFields
        {
            get { return allfields; }
            set { allfields = value; }
        }

        /// <summary>
        /// 容器的子元素
        /// </summary>
        public List<PbBaseControlInfo> Items
        {
            get { return items; }
            set { items = value; }
        }

    }
}
