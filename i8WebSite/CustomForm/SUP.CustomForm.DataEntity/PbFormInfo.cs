using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity
{
    public class PbFormInfo
    {
        private string title; //描述信息，用于在界面打开时，标题显示，生成代码后，注册menu信息时也需要使用
        private string billName;//表单名称，用于代码生成的类名

        private bool hasTab; //是否含有tab页

        private string listTable;//列表的表名       
        private string listSql;//列表获取的sql语句

        private string headerName;//表头标识

      
        private string masterTable;//主表
        private string masterSql;//主信息sql

        private Dictionary<string, DetailInfo> detail = new Dictionary<string, DetailInfo>();//明细表

        public PbFormInfo()
        {
 
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool HasTab
        {
            get { return hasTab; }
            set { hasTab = value; }
        }

        public string BillName
        {
            get { return billName; }
            set { billName = value; }
        }

        public string ListTable
        {
            get { return listTable; }
            set { listTable = value; }
        }

        public string ListSql
        {
            get { return listSql; }
            set { listSql = value; }
        }

        public string HeaderName
        {
            get { return headerName; }
            set { headerName = value; }
        }

        public string MasterTable
        {
            get { return masterTable; }
            set { masterTable = value; }
        }

        public string MasterSql
        {
            get { return masterSql; }
            set { masterSql = value; }
        }

        public Dictionary<string, DetailInfo> Detail
        {
            get { return detail; }
            set { detail = value; }
        }

    }
}
