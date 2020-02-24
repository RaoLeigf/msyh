#region Summary
/**************************************************************************************
    * 类 名 称：        QTProductUserModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProductUserModel.cs
    * 创建时间：        2018/12/12 
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
    /// QTProductUser实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProductUserModel : EntityBase<QTProductUserModel>
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
		/// 产品主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 ProductPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 产品标识
		/// </summary>
		[DataMember]
		public virtual System.String ProductBZ
		{
			get;
			set;
		}

		/// <summary>
		/// 操作员代码
		/// </summary>
		[DataMember]
		public virtual System.String ProductUserCode
		{
			get;
			set;
		}

		/// <summary>
		/// 操作员名称
		/// </summary>
		[DataMember]
		public virtual System.String ProductUserName
		{
			get;
			set;
		}

		/// <summary>
		/// 操作员密码
		/// </summary>
		[DataMember]
		public virtual System.String ProductUserPwd
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String BZ
		{
			get;
			set;
		}

		/// <summary>
		/// ProductUserORGCODE
		/// </summary>
		[DataMember]
		public virtual System.String ProductUserORGCODE
		{
			get;
			set;
		}

		/// <summary>
		/// ProductUserORGID
		/// </summary>
		[DataMember]
		public virtual System.String ProductUserORGID
		{
			get;
			set;
		}

		/// <summary>
		/// DEFSTR1
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// DEFSTR2
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// DEFSTR3
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}

		/// <summary>
		/// DEFINT1
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT1
		{
			get;
			set;
		}

		/// <summary>
		/// DEFINT2
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT2
		{
			get;
			set;
		}

		/// <summary>
		/// DEFINT3
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT3
		{
			get;
			set;
		}

	}

}