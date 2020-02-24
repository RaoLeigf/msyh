using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity
{
    //明细相关信息
    public  class DetailInfo
    {
        private string name;//标识明细，生成明细的取数方法时候用到
        private string tableName;        
        private string sql;
        private DetailInfo detail;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        public string Sql
        {
            get { return sql; }
            set { sql = value; }
        }
        public DetailInfo Detail
        {
            get { return detail; }
            set { detail = value; }
        }
    }
}
