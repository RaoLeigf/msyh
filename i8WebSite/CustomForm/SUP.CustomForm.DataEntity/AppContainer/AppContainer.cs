using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Controls;

namespace SUP.CustomForm.DataEntity.AppContainer
{
    public class AppContainer
    {
        private string id = string.Empty;
        private string title = string.Empty;
        private string buskey = string.Empty;
        private string type = string.Empty; //可以为FieldSet  / ngformpanel
        private int x = -1;
        private int y = -1;
        private int height = -1;
        private int width = -1;

        private List<PbBaseControlInfo> items = new List<PbBaseControlInfo>();

        public AppContainer()
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

        public string Type
        {
            get { return type; }
            set { type = value; }
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
