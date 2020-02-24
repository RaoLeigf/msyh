#region Summary
/**************************************************************************************
    * 类 名 称：        LogPerfModel
    * 命名空间：        SUP3.Log.Model.Domain
    * 文 件 名：        LogPerfModel.cs
    * 创建时间：        2017/10/9 
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
    /// LogPerf实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class LogPerfModel : EntityBase<LogPerfModel>
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
		/// 登录名
		/// </summary>
		[DataMember]
		public virtual System.String LogId
		{
			get;
			set;
		}

		/// <summary>
		/// Session ID
		/// </summary>
		[DataMember]
		public virtual System.String SessionId
		{
			get;
			set;
		}

		/// <summary>
		/// 创建时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? CreateDt
		{
			get;
			set;
		}

		/// <summary>
		/// 请求的URL
		/// </summary>
		[DataMember]
		public virtual System.String Url
		{
			get;
			set;
		}

		/// <summary>
		/// 方法名
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
		public virtual System.String Method
		{
			get;
			set;
		}

		/// <summary>
		/// 方法执行时间
		/// </summary>
		[DataMember]
		public virtual System.Int64 Duration
		{
			get;
			set;
		}

		/// <summary>
		/// 客户端IP
		/// </summary>
		[DataMember]
		public virtual System.String ClientIp
		{
			get;
			set;
		}

		/// <summary>
		/// 收集类型
		/// </summary>
		[DataMember]
		public virtual System.String CollectType
		{
			get;
			set;
		}

	}

}