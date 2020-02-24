#region Summary
/**************************************************************************************
    * 类 名 称：        QTProjectDtlFundApplModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProjectDtlFundApplModel.cs
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
    /// QTProjectDtlFundAppl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProjectDtlFundApplModel : EntityBase<QTProjectDtlFundApplModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public QTProjectDtlFundApplModel()
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
		/// 资金来源
		/// </summary>
		[DataMember]
		public virtual System.String FSourceOfFunds
		{
			get;
			set;
		}

		/// <summary>
		/// 金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

	}

}