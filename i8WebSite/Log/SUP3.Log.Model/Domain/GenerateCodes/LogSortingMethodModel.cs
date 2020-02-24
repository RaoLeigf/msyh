#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingMethodModel
    * 命名空间：        SUP3.Log.Model.Domain
    * 文 件 名：        LogSortingMethodModel.cs
    * 创建时间：        2017/11/4 
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

namespace SUP3.Log.Model.Domain
{
    /// <summary>
    /// LogSortingMethod实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class LogSortingMethodModel : EntityBase<LogSortingMethodModel>
    {
		/// <summary>
		/// PK
		/// </summary>
		[DataMember]
		public virtual System.Int64 Phid
		{
			get;
			set;
		}

		/// <summary>
		/// 类名
		/// </summary>
		[DataMember]
		public virtual System.String ClassName
		{
			get;
			set;
		}

		/// <summary>
		/// 方法名
		/// </summary>
		[DataMember]
		public virtual System.String MethodName
		{
			get;
			set;
		}

		/// <summary>
		/// 调用次数
		/// </summary>
		[DataMember]
		public virtual System.Int64 InvokeCount
		{
			get;
			set;
		}

		/// <summary>
		/// 总共执行时间
		/// </summary>
		[DataMember]
		public virtual System.Int64 TotalDuration
		{
			get;
			set;
		}

		/// <summary>
		/// 是大执行时间
		/// </summary>
		[DataMember]
		public virtual System.Int64 MaxDuration
		{
			get;
			set;
		}

		/// <summary>
		/// 最小执行时间
		/// </summary>
		[DataMember]
		public virtual System.Int64 MinDuration
		{
			get;
			set;
		}

	}

}