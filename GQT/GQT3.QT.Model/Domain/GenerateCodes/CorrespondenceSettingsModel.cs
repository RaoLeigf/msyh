#region Summary
/**************************************************************************************
    * 类 名 称：        CorrespondenceSettingsModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        CorrespondenceSettingsModel.cs
    * 创建时间：        2018/9/3 
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
    /// CorrespondenceSettings实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class CorrespondenceSettingsModel : EntityBase<CorrespondenceSettingsModel>
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
		/// 单位代码
		/// </summary>
		[DataMember]
		public virtual System.String Dwdm
		{
			get;
			set;
		}

		/// <summary>
		/// 对应代码
		/// </summary>
		[DataMember]
		public virtual System.String Dydm
		{
			get;
			set;
		}

		/// <summary>
		/// 对应类型
		/// </summary>
		[DataMember]
		public virtual System.String Dylx
		{
			get;
			set;
		}

		/// <summary>
		/// DefStr1
		/// </summary>
		[DataMember]
		public virtual System.String DefStr1
		{
			get;
			set;
		}

		/// <summary>
		/// DefStr2
		/// </summary>
		[DataMember]
		public virtual System.String DefStr2
		{
			get;
			set;
		}

		/// <summary>
		/// DefStr3
		/// </summary>
		[DataMember]
		public virtual System.String DefStr3
		{
			get;
			set;
		}

		/// <summary>
		/// DefStr4
		/// </summary>
		[DataMember]
		public virtual System.String DefStr4
		{
			get;
			set;
		}

		/// <summary>
		/// DefStr5
		/// </summary>
		[DataMember]
		public virtual System.String DefStr5
		{
			get;
			set;
		}

		/// <summary>
		/// DefStr6
		/// </summary>
		[DataMember]
		public virtual System.String DefStr6
		{
			get;
			set;
		}

		/// <summary>
		/// DefInt1
		/// </summary>
		[DataMember]
		public virtual System.Int32 DefInt1
		{
			get;
			set;
		}

		/// <summary>
		/// DefInt2
		/// </summary>
		[DataMember]
		public virtual System.Int32 DefInt2
		{
			get;
			set;
		}

		/// <summary>
		/// DefNum1
		/// </summary>
		[DataMember]
		public virtual System.String DefNum1
		{
			get;
			set;
		}

		/// <summary>
		/// DefNum2
		/// </summary>
		[DataMember]
		public virtual System.String DefNum2
		{
			get;
			set;
		}

	}

}