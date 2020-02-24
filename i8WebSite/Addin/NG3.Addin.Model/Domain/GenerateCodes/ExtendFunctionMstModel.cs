#region Summary
/**************************************************************************************
    * 类 名 称：        ExtendFunctionMstModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        ExtendFunctionMstModel.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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

using NG3.Addin.Model.Enums;

namespace NG3.Addin.Model.Domain
{
    /// <summary>
    /// ExtendFunctionMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ExtendFunctionMstModel : EntityBase<ExtendFunctionMstModel>
    {
		/// <summary>
		/// Phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

		/// <summary>
		/// 功能名称
		/// </summary>
		[DataMember]
		public virtual System.String FuncName
		{
			get;
			set;
		}

		/// <summary>
		/// 目标controller
		/// </summary>
		[DataMember]
		public virtual System.String TargetController
		{
			get;
			set;
		}

		/// <summary>
		/// 功能扩展类型
		/// </summary>
		[DataMember]
		public virtual EnumAddinType FuncType
		{
			get;
			set;
		}

		/// <summary>
		/// 重新定位的URL
		/// </summary>
		[DataMember]
		public virtual System.String Url
		{
			get;
			set;
		}

	}

}