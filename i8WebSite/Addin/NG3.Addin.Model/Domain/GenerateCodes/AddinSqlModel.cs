#region Summary
/**************************************************************************************
    * 类 名 称：        AddinSqlModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        AddinSqlModel.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.Model.Enums;

using NG3.Addin.Model.Enums;

namespace NG3.Addin.Model.Domain
{
    /// <summary>
    /// AddinSql实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class AddinSqlModel : EntityBase<AddinSqlModel>
    {

        /// <summary>
        /// 
        /// </summary>
        public AddinSqlModel()
        {
            //IList<PropertyColumnMapperInfo> fks = new List<PropertyColumnMapperInfo>();
            //fks.Add(new PropertyColumnMapperInfo
            //{
            //    PropertyName = "MstPhid",
            //    ColumnName = "mstphid",
            //    PropertyType = EnumPropertyType.Long,
            //    IsPrimary = false
            //});
            //ForeignKeys = fks;

            ForeignKeys = new List<PropertyColumnMapperInfo>
            {
                new PropertyColumnMapperInfo
                {
                    PropertyName = "MstPhid",
                    ColumnName = "mstphid",
                    PropertyType = EnumPropertyType.Long,
                    IsPrimary = false
                }

            };
        }
        /// <summary>
        /// phid
        /// </summary>
        [DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

		/// <summary>
		/// 主表PHID
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 来自的数据源
		/// </summary>
		[DataMember]
		public virtual System.String FromDs
		{
			get;
			set;
		}

		/// <summary>
		/// 数据源类型
		/// </summary>
		[DataMember]
		public virtual EnumUIDataSourceType RowsType
		{
			get;
			set;
		}

		/// <summary>
		/// Sql类型
		/// </summary>
		[DataMember]
		public virtual EnumSqlOpType SqlType
		{
			get;
			set;
		}

		/// <summary>
		/// 保存到指定表
		/// </summary>
		[DataMember]
		public virtual System.String ToTable
		{
			get;
			set;
		}

		/// <summary>
		/// 用来存入主表的主键列
		/// </summary>
		[DataMember]
		public virtual System.String ToTableKey
		{
			get;
			set;
		}

		/// <summary>
		/// SQL文本
		/// </summary>
		[DataMember]
		public virtual System.String SqlText
		{
			get;
			set;
		}

        /// <summary>
        /// 插件的类别，主要是功能扩展与功能环绕两种
        /// </summary>
        [DataMember]
        public virtual EnumCatalog SqlCatalog
        {
            get;
            set;
        }

    }

}