using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB列信息
    /// </summary>
    public class PbColumnInfo : MetadataGod
    {
        private SqlDbType _columnDataType = SqlDbType.UniqueIdentifier;

        private string _columnName = string.Empty;

        private string _defaultValue = string.Empty;

        private int _textLen = 0;

        private int _precision = 0;

        private int _scale = 0;

        private bool _isPrimaryKey = false;

        private bool _isForeignKey = false;

        private string _tableName = string.Empty;



        /// <summary>
        /// 数据类型
        /// </summary>
        public SqlDbType ColumnDataType
        {
            get { return _columnDataType; }
            set { _columnDataType = value; }
        }



        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        /// <summary>
        /// 字段长度(针对字符类型)
        /// </summary>
        public int TextLen
        {
            get { return _textLen; }
            set { _textLen = value; }
        }

        /// <summary>
        /// 小数有效位数
        /// </summary>
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        /// <summary>
        /// 小数后的位数
        /// </summary>
        public int Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
            set { _isPrimaryKey = value; }
        }

        /// <summary>
        /// 是否关联的外键
        /// </summary>
        public bool IsForeignKey
        {
            get { return _isForeignKey; }
            set { _isForeignKey = value; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
    }
}
