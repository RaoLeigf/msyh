using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUP.CustomForm.DataEntity.Control;

namespace SUP.CustomForm.DataEntity.Container
{
    public class GridPanel : ExtContainer
    {
        private string tablename;//表名    
        private string titlename; //标题名
        private string sql;//sql语句
        private bool isInTab = false;
        private string subtotal = string.Empty;  //ini中的分组小计串
        private string levelsum;
        private string collapse;
        private Dictionary<string, string> sumdic = new Dictionary<string, string>();  //合计列
        private Dictionary<string, string> subdic = new Dictionary<string, string>();  //小计列
        private Dictionary<string, string> groupfield = new Dictionary<string, string>();  //分组小计的分组列
        private Dictionary<string, string> groupcolsdic = new Dictionary<string, string>();  //多表头标签
        private List<string> columnNames = new List<string>();  //所有列名
        private List<ExtGridColumn> columns = new List<ExtGridColumn>();

        public GridPanel()
        {

        }

        public List<string> ColumnNames
        {
            get { return columnNames; }
            set { columnNames = value; }
        }

        public List<ExtGridColumn> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public string Subtotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }

        public string LevelSum
        {
            get { return levelsum; }
            set { levelsum = value; }
        }

        public Dictionary<string, string> Sumdic
        {
            get { return sumdic; }
            set { sumdic = value; }
        }

        public Dictionary<string, string> Subdic
        {
            get { return subdic; }
            set { subdic = value; }
        }

        public Dictionary<string, string> Groupfield
        {
            get { return groupfield; }
            set { groupfield = value; }
        }

        public Dictionary<string, string> Groupcolsdic
        {
            get { return groupcolsdic; }
            set { groupcolsdic = value; }
        }

        public bool IsInTab
        {
            get { return isInTab; }
            set { isInTab = value; }
        }

        public string TableName
        {
            get { return tablename; }
            set { tablename = value; }
        }

        public string TitleName
        {
            get { return titlename; }
            set { titlename = value; }
        }

        public string Sql
        {
            get { return sql; }
            set { sql = value; }
        }

        public string Collapse
        {
            get { return collapse; }
            set { collapse = value; }
        }

    }
}
