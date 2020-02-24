#region Summary
/**************************************************************************************
    * 类 名 称：        LogLogsModel
    * 命名空间：        SUP3.Log.Model.Domain
    * 文 件 名：        LogLogsModel.cs
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
    /// LogLogs实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class LogLogsModel : EntityBase<LogLogsModel>
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
		/// 类名
		/// </summary>
		[DataMember]
		public virtual System.String ClassName
		{
			get;
			set;
		}

		/// <summary>
		/// 日志级别
		/// </summary>
		[DataMember]
		public virtual System.Int32 LogLevel
		{
			get;
			set;
		}

		/// <summary>
		/// 业务类型
		/// </summary>
		[DataMember]
		public virtual System.String BizModule
		{
			get;
			set;
		}

		/// <summary>
		/// 日志信息
		/// </summary>
		[DataMember]
		public virtual System.String LogInfo
		{
			get;
			set;
		}

		/// <summary>
		/// 错误信息
		/// </summary>
		[DataMember]
		public virtual System.String ErrInfo
		{
			get;
			set;
		}

	}

}