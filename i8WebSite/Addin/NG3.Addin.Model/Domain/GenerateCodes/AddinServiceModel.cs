#region Summary
/**************************************************************************************
    * 类 名 称：        AddinServiceModel
    * 命名空间：        NG3.Addin.Model.Domain
    * 文 件 名：        AddinServiceModel.cs
    * 创建时间：        2017/12/13 
    * 作    者：        洪鹏    
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

namespace NG3.Addin.Model.Domain
{
    /// <summary>
    /// AddinService实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class AddinServiceModel : EntityBase<AddinServiceModel>
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
		/// 服务名称
		/// </summary>
		[DataMember]
		public virtual System.String ServiceName
		{
			get;
			set;
		}

		/// <summary>
		/// 服务功能
		/// </summary>
		[DataMember]
		public virtual System.String ServiceFuncName
		{
			get;
			set;
		}

		/// <summary>
		/// 目标assembly
		/// </summary>
		[DataMember]
		public virtual System.String TargetAssemblyName
		{
			get;
			set;
		}

		/// <summary>
		/// 目标类名
		/// </summary>
		[DataMember]
		public virtual System.String TargetClassName
		{
			get;
			set;
		}

		/// <summary>
		/// 目标方法名
		/// </summary>
		[DataMember]
		public virtual System.String TargetMethodName
		{
			get;
			set;
		}

		/// <summary>
		/// 匹配服务的条件
		/// </summary>
		[DataMember]
		public virtual System.String MatchCondition
		{
			get;
			set;
		}

	}

}