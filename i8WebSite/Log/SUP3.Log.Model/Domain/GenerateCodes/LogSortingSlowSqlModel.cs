#region Summary
/**************************************************************************************
    * 类 名 称：        LogSortingSlowSqlModel
    * 命名空间：        SUP3.Log.Model.Domain
    * 文 件 名：        LogSortingSlowSqlModel.cs
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
    /// LogSortingSlowSql实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class LogSortingSlowSqlModel : EntityBase<LogSortingSlowSqlModel>
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
		/// Session Id
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
		/// SQL文本
		/// </summary>
		[DataMember]
		public virtual System.String SqlText
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
		/// 收集类型
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