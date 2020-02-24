#region Summary
/**************************************************************************************
    * 类 名 称：        QTProjectDtlTextContentModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProjectDtlTextContentModel.cs
    * 创建时间：        2019/9/4 
    * 作    者：        刘杭    
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

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// QTProjectDtlTextContent实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProjectDtlTextContentModel : EntityBase<QTProjectDtlTextContentModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public QTProjectDtlTextContentModel()
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
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 主表phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 部门职能概述
		/// </summary>
		[DataMember]
		public virtual System.String FFunctionalOverview
		{
			get;
			set;
		}

		/// <summary>
		/// 项目概况
		/// </summary>
		[DataMember]
		public virtual System.String FProjOverview
		{
			get;
			set;
		}

		/// <summary>
		/// 立项依据
		/// </summary>
		[DataMember]
		public virtual System.String FProjBasis
		{
			get;
			set;
		}

		/// <summary>
		/// 可行性
		/// </summary>
		[DataMember]
		public virtual System.String FFeasibility
		{
			get;
			set;
		}

		/// <summary>
		/// 必要性
		/// </summary>
		[DataMember]
		public virtual System.String FNecessity
		{
			get;
			set;
		}

		/// <summary>
		/// 长期绩效目标
		/// </summary>
		[DataMember]
		public virtual System.String FLTPerformGoal
		{
			get;
			set;
		}

		/// <summary>
		/// 年度绩效目标
		/// </summary>
		[DataMember]
		public virtual System.String FAnnualPerformGoal
		{
			get;
			set;
		}

	}

}