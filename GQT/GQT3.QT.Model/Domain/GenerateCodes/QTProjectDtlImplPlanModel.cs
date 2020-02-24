#region Summary
/**************************************************************************************
    * 类 名 称：        QTProjectDtlImplPlanModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProjectDtlImplPlanModel.cs
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
    /// QTProjectDtlImplPlan实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProjectDtlImplPlanModel : EntityBase<QTProjectDtlImplPlanModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public QTProjectDtlImplPlanModel()
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
		/// 实施内容
		/// </summary>
		[DataMember]
		public virtual System.String FImplContent
		{
			get;
			set;
		}

		/// <summary>
		/// 开始日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FStartDate
		{
			get;
			set;
		}

		/// <summary>
		/// 结束日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FEndDate
		{
			get;
			set;
		}

	}

}