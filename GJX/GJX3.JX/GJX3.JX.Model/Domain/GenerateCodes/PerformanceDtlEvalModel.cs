#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceDtlEvalModel
    * 命名空间：        GJX3.JX.Model.Domain
    * 文 件 名：        PerformanceDtlEvalModel.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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

namespace GJX3.JX.Model.Domain
{
    /// <summary>
    /// PerformanceDtlEval实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PerformanceDtlEvalModel : EntityBase<PerformanceDtlEvalModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public PerformanceDtlEvalModel()
		{
			List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

			PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
			info.PropertyName = "MstPhid";
			info.ColumnName = "mst_phid";
			info.PropertyType = EnumPropertyType.Long;
			info.IsPrimary = false;
			list.Add(info);

			ForeignKeys = list;//设置外键字段属性
		}

		/// <summary>
		/// 主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 主表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 评价名称 
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

		/// <summary>
		/// 评价内容
		/// </summary>
		[DataMember]
		public virtual System.String FContent
		{
			get;
			set;
		}

		/// <summary>
		/// 权重
		/// </summary>
		[DataMember]
		public virtual System.Decimal FWeight
		{
			get;
			set;
		}

		/// <summary>
		/// 自评得分
		/// </summary>
		[DataMember]
		public virtual System.Decimal FScore
		{
			get;
			set;
		}

	}

}