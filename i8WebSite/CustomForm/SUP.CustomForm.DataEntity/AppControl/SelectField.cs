using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class SelectField:TextField
    {

        private string text;//显示字段,value是值字段
        private List<string> options;//数据       

        public SelectField()
        { 
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public List<string> Options
        {
            get { return options; }
            set { options = value; }
        }      



    }
}
