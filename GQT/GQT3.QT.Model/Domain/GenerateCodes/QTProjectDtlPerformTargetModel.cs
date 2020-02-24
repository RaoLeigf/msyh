#region Summary
/**************************************************************************************
    * 类 名 称：        QTProjectDtlPerformTargetModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProjectDtlPerformTargetModel.cs
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
    /// QTProjectDtlPerformTarget实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProjectDtlPerformTargetModel : EntityBase<QTProjectDtlPerformTargetModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public QTProjectDtlPerformTargetModel()
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
		/// 指标代码
		/// </summary>
		[DataMember]
		public virtual System.String FTargetCode
		{
			get;
			set;
		}

		/// <summary>
		/// 指标名称
		/// </summary>
		[DataMember]
		public virtual System.String FTargetName
		{
			get;
			set;
		}

		/// <summary>
		/// 指标内容
		/// </summary>
		[DataMember]
		public virtual System.String FTargetContent
		{
			get;
			set;
		}

		/// <summary>
		/// 指标值
		/// </summary>
		[DataMember]
		public virtual System.String FTargetValue
		{
			get;
			set;
		}

		/// <summary>
		/// 指标权重
		/// </summary>
		[DataMember]
		public virtual System.Decimal FTargetWeight
		{
			get;
			set;
		}

		/// <summary>
		/// 指标描述
		/// </summary>
		[DataMember]
		public virtual System.String FTargetDescribe
		{
			get;
			set;
		}

		/// <summary>
		/// 指标类别代码
		/// </summary>
		[DataMember]
		public virtual System.String FTargetClassCode
		{
			get;
			set;
		}

		/// <summary>
		/// 指标类型代码
		/// </summary>
		[DataMember]
		public virtual System.String FTargetTypeCode
		{
			get;
			set;
		}

		/// <summary>
		/// 是否用户增加
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfCustom
		{
			get;
			set;
		}

	}

}