using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.AppControl
{
    public class BaseField
    {

        private string _id;//Ext.getCmp("id")
        private string _itemId;//元素id,  cmp.down("#itemid")       
        private string _xtype;//ui类型        
        private string _name;//字段名
        private string _value;//值
        private List<String> _expressions;  //表达式 
        
        private bool _mustInput = false;//是否必输项                
        private string _fieldLabel;//标签名
             

        public BaseField()
        {
 
        }


        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public string XType
        {
            get { return _xtype; }
            set { _xtype = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool MustInput
        {
            get { return _mustInput; }
            set { _mustInput = value; }
        }

        public string FieldLabel
        {
            get { return _fieldLabel; }
            set { _fieldLabel = value; }
        }

        public List<String> Expressions
        {
            get { return _expressions; }
            set { _expressions = value; }
        }

    }
}
