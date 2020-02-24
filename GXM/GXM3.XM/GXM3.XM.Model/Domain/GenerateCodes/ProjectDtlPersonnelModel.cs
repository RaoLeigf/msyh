#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPersonnelModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        ProjectDtlPersonnelModel.cs
    * 创建时间：        2020/1/6 
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
    /// ProjectDtlPersonnel实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectDtlPersonnelModel : EntityBase<ProjectDtlPersonnelModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public ProjectDtlPersonnelModel()
		{
			List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

			PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
			info.PropertyName = "MstPhid";
			info.ColumnName = "Mst_Phid";
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
		/// 人员编码（身份证号）
		/// </summary>
		[DataMember]
		public virtual System.String FUsercode
		{
			get;
			set;
		}

		/// <summary>
		/// 人员名称
		/// </summary>
		[DataMember]
		public virtual System.String FUsename
		{
			get;
			set;
		}

		/// <summary>
		/// 分摊金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 排序
		/// </summary>
		[DataMember]
		public virtual System.Int32 Sortcode
		{
			get;
			set;
		}

		/// <summary>
		/// 预留字段1
		/// </summary>
		[DataMember]
		public virtual System.String DefStr1
		{
			get;
			set;
		}

		/// <summary>
		/// 预留字段2
		/// </summary>
		[DataMember]
		public virtual System.Int32 DefInt1
		{
			get;
			set;
		}

	}

}