#region Summary
/**************************************************************************************
    * 类 名 称：        LogCfgModel
    * 命名空间：        SUP3.Log.Model.Domain
    * 文 件 名：        LogCfgModel.cs
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
    /// LogCfg实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class LogCfgModel : EntityBase<LogCfgModel>
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
		/// 业务模块
		/// </summary>
		[DataMember]
		public virtual System.String BizModule
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
		/// 登录名
		/// </summary>
		[DataMember]
		public virtual System.String LogId
		{
			get;
			set;
		}

		/// <summary>
		/// 是否打开日志
		/// </summary>
		[DataMember]
		public virtual System.Byte OpenFlag
		{
			get;
			set;
		}

	}

}