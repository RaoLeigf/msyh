#region Summary
/**************************************************************************************
    * 类 名 称：        QTProjectDtlPurchaseDtlModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProjectDtlPurchaseDtlModel.cs
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
    /// QTProjectDtlPurchaseDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProjectDtlPurchaseDtlModel : EntityBase<QTProjectDtlPurchaseDtlModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public QTProjectDtlPurchaseDtlModel()
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
		/// 明细项目代码
		/// </summary>
		[DataMember]
		public virtual System.String FDtlCode
		{
			get;
			set;
		}

		/// <summary>
		/// 采购内容
		/// </summary>
		[DataMember]
		public virtual System.String FContent
		{
			get;
			set;
		}

		/// <summary>
		/// 采购目录代码
		/// </summary>
		[DataMember]
		public virtual System.String FCatalogCode
		{
			get;
			set;
		}

		/// <summary>
		/// 采购类型代码
		/// </summary>
		[DataMember]
		public virtual System.String FTypeCode
		{
			get;
			set;
		}

		/// <summary>
		/// 采购程序代码
		/// </summary>
		[DataMember]
		public virtual System.String FProcedureCode
		{
			get;
			set;
		}

		/// <summary>
		/// 计量单位
		/// </summary>
		[DataMember]
		public virtual System.String FMeasUnit
		{
			get;
			set;
		}

		/// <summary>
		/// 数量
		/// </summary>
		[DataMember]
		public virtual System.Decimal FQty
		{
			get;
			set;
		}

		/// <summary>
		/// 预计单价
		/// </summary>
		[DataMember]
		public virtual System.Decimal FPrice
		{
			get;
			set;
		}

		/// <summary>
		/// 总计金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 技术参数及配置标准
		/// </summary>
		[DataMember]
		public virtual System.String FSpecification
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String FRemark
		{
			get;
			set;
		}

		/// <summary>
		/// 预计采购时间
		/// </summary>
		[DataMember]
		public virtual System.String FEstimatedPurTime
		{
			get;
			set;
		}

		/// <summary>
		/// 是否绩效评价
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfPerformanceAppraisal
		{
			get;
			set;
		}

		/// <summary>
		/// 明细项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

	}

}