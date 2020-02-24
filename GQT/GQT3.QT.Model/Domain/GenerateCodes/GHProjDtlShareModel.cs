#region Summary
/**************************************************************************************
    * 类 名 称：        GHProjDtlShareModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        GHProjDtlShareModel.cs
    * 创建时间：        2018/9/11 
    * 作    者：        李明    
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
    /// GHProjDtlShare实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GHProjDtlShareModel : EntityBase<GHProjDtlShareModel>
    {
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
		/// 模块代码
		/// </summary>
		[DataMember]
		public virtual System.String MK
		{
			get;
			set;
		}

		/// <summary>
		/// 项目代码
		/// </summary>
		[DataMember]
		public virtual System.String XMDM
		{
			get;
			set;
		}

		/// <summary>
		/// 明细项目代码
		/// </summary>
		[DataMember]
		public virtual System.String DM
		{
			get;
			set;
		}

		/// <summary>
		/// 明细项目名称
		/// </summary>
		[DataMember]
		public virtual System.String MC
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR3
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR4
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR5
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR6
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR7
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR8
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR9
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR10
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR11
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR12
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR13
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR14
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR15
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR16
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR17
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR18
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR19
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String STR20
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY1
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY1
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY2
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY2
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY3
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY3
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY4
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY4
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY5
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY5
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY6
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY6
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY7
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY7
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY8
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY8
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY9
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY9
		{
			get;
			set;
		}

		/// <summary>
		/// MONEY10
		/// </summary>
		[DataMember]
		public virtual System.Decimal MONEY10
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT3
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT4
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT5
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT6
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT7
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT8
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT9
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 INT10
		{
			get;
			set;
		}

	}

}