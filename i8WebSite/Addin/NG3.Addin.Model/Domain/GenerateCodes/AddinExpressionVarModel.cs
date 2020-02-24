#region Summary
/**************************************************************************************
    * 类 名 称：        AddinExpressionVarModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        AddinExpressionVarModel.cs
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
    /// AddinExpressionVar实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class AddinExpressionVarModel : EntityBase<AddinExpressionVarModel>
    {

        /// <summary>
        /// 
        /// </summary>
        public AddinExpressionVarModel()
        {
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
        /// Phid
        /// </summary>
        [DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

		/// <summary>
		/// 主表Phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 变量名称
		/// </summary>
		[DataMember]
		public virtual System.String VarName
		{
			get;
			set;
		}

		/// <summary>
		/// 来源的数据源
		/// </summary>
		[DataMember]
		public virtual System.String FromDs
		{
			get;
			set;
		}

		/// <summary>
		/// 数据源中数据源类型
		/// </summary>
		[DataMember]
		public virtual EnumUIDataSourceType RowsType
		{
			get;
			set;
		}

		/// <summary>
		/// SQL类型
		/// </summary>
		[DataMember]
		public virtual EnumSqlOpType SqlOpType
		{
			get;
			set;
		}

		/// <summary>
		/// Sql文本
		/// </summary>
		[DataMember]
		public virtual System.String SqlText
		{
			get;
			set;
		}

	}

}