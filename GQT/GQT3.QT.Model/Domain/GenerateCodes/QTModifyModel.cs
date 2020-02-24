#region Summary
/**************************************************************************************
    * 类 名 称：        QTModifyModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTModifyModel.cs
    * 创建时间：        2019/5/20 
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
    /// QTModify实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTModifyModel : EntityBase<QTModifyModel>
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
		/// 用户编码
		/// </summary>
		[DataMember]
		public virtual System.String UserCode
		{
			get;
			set;
		}

		/// <summary>
		/// 用户名称
		/// </summary>
		[DataMember]
		public virtual System.String UserName
		{
			get;
			set;
		}

		/// <summary>
		/// 用户IP
		/// </summary>
		[DataMember]
		public virtual System.String IP
		{
			get;
			set;
		}

		/// <summary>
		/// 修改字段
		/// </summary>
		[DataMember]
		public virtual System.String ModifyField
		{
			get;
			set;
		}

		/// <summary>
		/// 修改前
		/// </summary>
		[DataMember]
		public virtual System.String BeforeValue
		{
			get;
			set;
		}

		/// <summary>
		/// 修改后
		/// </summary>
		[DataMember]
		public virtual System.String AfterValue
		{
			get;
			set;
		}

		/// <summary>
		/// 单据代码
		/// </summary>
		[DataMember]
		public virtual System.String FProjCode
		{
			get;
			set;
		}

		/// <summary>
		/// 单据名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjName
		{
			get;
			set;
		}

		/// <summary>
		/// 修改处tab名称
		/// </summary>
		[DataMember]
		public virtual System.String TabName
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
		/// DEFSTR4
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR4
		{
			get;
			set;
		}

		/// <summary>
		/// DEFSTR5
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR5
		{
			get;
			set;
		}

		/// <summary>
		/// DEFSTR6
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR6
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
		/// DEFNUM1
		/// </summary>
		[DataMember]
		public virtual System.Decimal DEFNUM1
		{
			get;
			set;
		}

		/// <summary>
		/// DEFNUM2
		/// </summary>
		[DataMember]
		public virtual System.Decimal DEFNUM2
		{
			get;
			set;
		}

	}

}