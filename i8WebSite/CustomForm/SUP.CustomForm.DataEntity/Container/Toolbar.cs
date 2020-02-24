using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Container
{
    public class Toolbar
    {
        private List<string> _LNgButtons = new List<string>();
        private List<string> _RNgButtons = new List<string>();


        public Toolbar()
        {
            
        }

        //布局
        public string Region { get; set; }      

        //权限id
        public string RightName { get; set; }

        //左边的按钮
        public List<string> LNgButtons
        {
            get { return _LNgButtons; }
            set { _LNgButtons = value; }
        }

        //右边的按钮
        public List<string> RNgButtons 
        {
            get { return _RNgButtons; }
            set { _RNgButtons = value; }
        }   
    }
}
