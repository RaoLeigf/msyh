#region Summary
/**************************************************************************************
    * 类 名 称：        QTEditMemoModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTEditMemoModel.cs
    * 创建时间：        2019/5/29 
    * 作    者：        董泉伟    
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
    /// QTEditMemo实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTEditMemoModel : EntityBase<QTEditMemoModel>
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
		/// 批注单据主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 Memophid
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
		/// 批注时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? MemoTime
		{
			get;
			set;
		}

		/// <summary>
		/// 用户ip
		/// </summary>
		[DataMember]
		public virtual System.String IP
		{
			get;
			set;
		}

		/// <summary>
		/// 批注字段代码
		/// </summary>
		[DataMember]
		public virtual System.String MemoCode
		{
			get;
			set;
		}

		/// <summary>
		/// 批注字段名称
		/// </summary>
		[DataMember]
		public virtual System.String MenoName
		{
			get;
			set;
		}

		/// <summary>
		/// 批注区域
		/// </summary>
		[DataMember]
		public virtual System.String MemoArea
		{
			get;
			set;
		}

		/// <summary>
		/// 批注前代码
		/// </summary>
		[DataMember]
		public virtual System.String BeforeCode
		{
			get;
			set;
		}

		/// <summary>
		/// 批注前名称
		/// </summary>
		[DataMember]
		public virtual System.String BeforeName
		{
			get;
			set;
		}

		/// <summary>
		/// 批注后代码
		/// </summary>
		[DataMember]
		public virtual System.String AfterCode
		{
			get;
			set;
		}

		/// <summary>
		/// 批注后名称
		/// </summary>
		[DataMember]
		public virtual System.String AfterName
		{
			get;
			set;
		}

		/// <summary>
		/// 是否引用
		/// </summary>
		[DataMember]
		public virtual System.String IfChoose
		{
			get;
			set;
		}

		/// <summary>
		/// 项目代码
		/// </summary>
		[DataMember]
		public virtual System.String FProjCode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjName
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String TabName
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR4
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR5
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR6
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 DEFINT1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 DEFINT2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Decimal DEFNUM1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Decimal DEFNUM2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.DateTime? DEFDate1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.DateTime? DEFDate2
		{
			get;
			set;
		}

	}

}