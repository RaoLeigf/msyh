using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SUP.Common.DataEntity
{
    public class CommonHelpEntity
    {

        public CommonHelpEntity()
        {
            this.FieldPropertyDic = new Dictionary<string, string>();
            this.FieldDic = new Dictionary<string, string>();
        }

        #region property

        /// <summary>
        /// 帮助类型，
        /// Mode=0：拼装sql语句方式,适用简单的帮助
        /// Mode=1 : 通过反射方式，获取具体业务实现的结果集，使用复杂，很难用sql语句组装出来的帮助
        /// </summary>
        public HelpMode Mode 
        {
            get; 
            set;
        }

        /// <summary>
        /// 帮助id
        /// </summary>
        public string HelpID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        //系统编码
        public string CodeField
        {
            get;
            set;
        }

        public string CodeProperty
        {
            get; 
            set;
        }

        //用户编码
        public string UserCodeField
        {
            get;
            set;
        }

        public string UserCodeProperty
        {
            get;
            set;
        }

        public string NameField
        {
            get;
            set;
        }

        public string NameProperty
        {
            get; 
            set;
        }

        public string AllField
        {
            get;
            set;
        }

        public string AllFieldWithTableName
        {
            get;
            set;
        }

        public string AllProperty
        {
            get; 
            set;
        }

        //字段和属性对应字典
        public Dictionary<string,string> FieldPropertyDic
        {
            get;
            set;
        }

        //字段（小写->大写）对应字典
        public Dictionary<string, string> FieldDic
        {
            get;
            set;
        }
        public string HeadText
        {
            get;
            set;
        }
        

        public string SqlFilter
        {
            get;
            set;
        }

        //带有@ocode@和@orgid@宏变量的sql条件
        public string SqlFilterWithMacro
        {
            get;
            set;
        }

        public string SortField
        {
            get;
            set;
        }

        //树形展示，在已经转换好的结果集，排序字段名可能已经变成属性名
        public string SortProperty
        {
            get;
            set;
        }

        public string Distinct
        {
            get;
            set;
        }

        public string Assembly
        {
            get;
            set;
        }

        public string ClassName
        {
            get;
            set;
        }

        public string GetListMethod
        {
            get;
            set;
        }

        public string GetNameMethod
        {
            get;
            set;
        }

        public string CodeToNameMethod
        {
            get;
            set;
        }

        public string ShowTree
        {
            get;
            set;
        }

        public string TreePid
        {
            get;
            set;
        }

        public string TreeChildId
        {
            get;
            set;
        }

        public bool OutDataSource
        {
            get;
            set;
        }

        public bool ExistQueryProperty
        {
            get;
            set;
        }
        
        //业务类型，控制信息权限用
        public Int64 BusPhid { get; set; }


        //明细表
        public string DetailTable
        {
            get;
            set;
        }

        //明细表的字段
        public string DetailTableFields
        {
            get;
            set;
        }

        //明细表的实体属性集
        public string DetailTablePropertys
        {
            get;
            set;
        }

        //明细表头信息
        public string DetailTableHeaders
        {
            get;
            set;
        }

        //主表的主键
        public string MasterTableKey
        {
            get;
            set;
        }

        //主表的主键
        public string MasterTableKeyProperty
        {
            get;
            set;
        }

        //明细表的外键，与主表的主键关联的字段
        public string MasterID
        {
            get;
            set;
        }


        //明细表的过滤条件
        public string DetailSqlFilter
        {
            get;
            set;
        }

        //拼音首字母字段
        public string PYField
        {
            get;
            set;
        }

        //supcan控件的查询区
        public string QueryTemplate
        {
            get;
            set;
        }

        //supcan控件的列表区
        public string ListTemplate
        {
            get;
            set;
        }

        //Json描述模板
        public string JsonTemplate
        {
            get;
            set;
        }
        
        /// <summary>
        /// 查询区
        /// </summary>
        public CommHelpQuery Query
        {
            get;
            set;
        }

        /// <summary>
        /// 列表格式
        /// </summary>
        public  XmlNode List
        {
            get;
            set;
        }


        public QueryPropertyItem[]  QueryPropertyItem
        {
            get;
            set;
        }

        #endregion


    }


    public enum HelpMode 
    {
        Default,
        GetHelpSql,//通过反射获得sql语句
        GetHelpResult//通过反射得到结果
    }

    public enum SelectMode
    {
        Single,
        Multi//多选 
    }

    public enum HelpType
    {
        NGCommonhelp,
        NGRichhelp,
        NGmshelp//明细表取数 
    }

    //查询属性
    public class QueryPropertyItem
    {
        public string code { get; set; }
        public string boxLabel{get;set;}
        public string name { get; set; }
        public string inputValue { get; set; }
        public bool @checked { get; set; }
    }
}
