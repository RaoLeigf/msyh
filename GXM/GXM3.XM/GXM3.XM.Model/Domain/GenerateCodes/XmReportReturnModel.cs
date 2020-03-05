#region Summary
/**************************************************************************************
    * 类 名 称：        XmReportDtlModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        XmReportDtlModel.cs
    * 创建时间：        2020/1/17 
    * 作    者：        王冠冠    
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

namespace GXM3.XM.Model.Domain
{
	/// <summary>
	/// XmReportReturn实体定义类
	/// </summary>
	[Serializable]
	[DataContract(Namespace = "")]
	public partial class XmReportReturnModel : EntityBase<XmReportReturnModel>
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public XmReportReturnModel()
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
		/// 报账日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FTime
		{
			get;
			set;
		}

		/// <summary>
		/// 关联报账单编码
		/// </summary>
		[DataMember]
		public virtual System.String FCode
		{
			get;
			set;
		}

		/// <summary>
		/// 关联报账单名称
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

		/// <summary>
		/// 报账金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}
	}

}