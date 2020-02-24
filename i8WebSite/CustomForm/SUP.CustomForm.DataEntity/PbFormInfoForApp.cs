using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity
{
    /// <summary>
    /// App所需信息定义
    /// </summary>
    public  class PbFormInfoForApp
    {

        private string title; //描述信息，用于在界面打开时，标题显示，生成代码后，注册menu信息时也需要使用
        private string billName;//表单名称，用于代码生成的类名
               

        private string listTable;//列表的表名       
        private string listSql;//列表获取的sql语句

        private string headerName;//表头标识


        private string masterTable;//主表
        private string masterSql;//主信息sql

        public PbFormInfoForApp()
        { 
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
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

    }
}
