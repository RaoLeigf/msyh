using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Report.Func.Core.Entity
{
    public class FuncParameter
    {
        private string name = "";
        private int index = 0;
        private string paramType = null; //参数类型
        private string property = "";
        private string displayProperty = "";
        private string paramValue = "";

        private string funcName;

        private string editType; //主要是为了支持下拉
        private string droplistCode; //下拉列表的名称

        //参数名称
        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        public int Index
        {
            set { index = value; }
            get { return index; }
        }

        public string ParamType
        {
            set { paramType = value; }
            get { return paramType; }
        }
        public string Property
        {
            set { property = value; }
            get { return property; }
        }

        public string DisplayProperty
        {
            set { displayProperty = value; }
            get { return displayProperty; }
        }

        public string Value
        {
            set { paramValue = value; }
            get { return paramValue; }
        }

        //函数名
        public string FuncName
        {
            get
            {
                return funcName;
            }

            set
            {
                funcName = value;
            }
        }

        public string EditType
        {
            get
            {
                return editType;
            }

            set
            {
                editType = value;
            }
        }

        public string DroplistCode
        {
            get
            {
                return droplistCode;
            }

            set
            {
                droplistCode = value;
            }
        }
    }
}
