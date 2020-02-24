#region Summary
/**************************************************************************************
    * 类 名 称：        GAppvalProc4PostModel
    * 命名空间：        GSP3.SP.Model.Domain
    * 文 件 名：        GAppvalProc4PostModel.cs
    * 创建时间：        2019/5/21 
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

namespace GSP3.SP.Model.Domain
{
    /// <summary>
    /// 审批流程岗位对应表
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GAppvalProc4PostModel : EntityBase<GAppvalProc4PostModel>
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
		/// 流程表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 ProcPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 岗位表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PostPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 序号
		/// </summary>
		[DataMember]
		public virtual System.Int32 FSeq
		{
			get;
			set;
		}

		/// <summary>
		/// 会签模式
		/// </summary>
		[DataMember]
		public virtual System.Byte FMode
		{
			get;
			set;
		}

	}

}