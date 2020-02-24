using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity.Individual
{
    [Serializable]
    public class ExtControlInfoBase
    {
        private string _id;
               
        private string _xtype;//ui类型
        private string _name;//字段名
        private bool _mustInput = false;//是否必输项
        private string _fieldLabel;//标签名
        private int _colspan = 1;//跨的列数
        private string _iconCls;//图标样式id
        private string _value; // 默认值

        public ExtControlInfoBase()
        {
 
        }

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string xtype
        {
            get { return _xtype; }
            set { _xtype = value; }
        }

        public string fieldLabel
        {
            get { return _fieldLabel; }
            set { _fieldLabel = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool mustInput
        {
            get { return _mustInput; }
            set { _mustInput = value; }
        }

        public int colspan
        {
            get { return _colspan; }
            set { _colspan = value; }
        }

        public string iconCls
        {
            get { return _iconCls; }
            set { _iconCls = value; }
        }

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string itemId { get; set; }

        public string FieldUIId { get; set; }//字段UI的id(表名.字段名),删除字段ui信息需要校验
    }


}
