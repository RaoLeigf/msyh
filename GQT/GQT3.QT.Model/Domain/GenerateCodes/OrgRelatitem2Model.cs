#region Summary
/**************************************************************************************
    * 类 名 称：        OrgRelatitemModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        OrgRelatitemModel.cs
    * 创建时间：        2018/9/20 
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
    /// OrgRelatitem实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class OrgRelatitem2Model : EntityBase<OrgRelatitem2Model>
    {
		/// <summary>
		/// 物理主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 关系代码
		/// </summary>
		[DataMember]
		public virtual System.String RelatId
		{
			get;
			set;
		}

		/// <summary>
		/// 组织代码
		/// </summary>
		[DataMember]
		public virtual System.String OCode
		{
			get;
			set;
		}

		/// <summary>
		/// 上级组织代码
		/// </summary>
		[DataMember]
		public virtual System.String ParentOrg
		{
			get;
			set;
		}

		/// <summary>
		/// 关联号
		/// </summary>
		[DataMember]
		public virtual System.String RelaIndex
		{
			get;
			set;
		}

		/// <summary>
		/// 关系序号
		/// </summary>
		[DataMember]
		public virtual System.String RelId
		{
			get;
			set;
		}

		/// <summary>
		/// 订货单位
		/// </summary>
		[DataMember]
		public virtual System.String OrderType
		{
			get;
			set;
		}

		/// <summary>
		/// 组织主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 OrgId
		{
			get;
			set;
		}

		/// <summary>
		/// 上级组织主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 ParentOrgId
		{
			get;
			set;
		}

		/// <summary>
		/// 主表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 MainPhId
		{
			get;
			set;
		}

		/// <summary>
		/// 组织属性
		/// </summary>
		[DataMember]
		public virtual System.String AttrCode
		{
			get;
			set;
		}

	}

}