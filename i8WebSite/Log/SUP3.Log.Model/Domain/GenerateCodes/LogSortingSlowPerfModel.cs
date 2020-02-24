#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingSlowPerfModel
    * 命名空间：        SUP3.Log.Model.Domain
    * 文 件 名：        LogSortingSlowPerfModel.cs
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
    /// LogSortingSlowPerf实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class LogSortingSlowPerfModel : EntityBase<LogSortingSlowPerfModel>
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
		/// session id
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
		/// URL
		/// </summary>
		[DataMember]
		public virtual System.String Url
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
		/// 持续时间
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
		/// 日志收集类型
		/// </summary>
		[DataMember]
		public virtual System.String CollectType
		{
			get;
			set;
		}

		/// <summary>
		/// 数据库名称
		/// </summary>
		[DataMember]
		public virtual System.String DbName
		{
			get;
			set;
		}

	}

}