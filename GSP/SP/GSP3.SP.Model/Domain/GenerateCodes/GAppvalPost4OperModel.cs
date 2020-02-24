#region Summary
/**************************************************************************************
    * 类 名 称：        GAppvalPost4OperModel
    * 命名空间：        GSP3.SP.Model.Domain
    * 文 件 名：        GAppvalPost4OperModel.cs
    * 创建时间：        2019/5/20 
    * 作    者：        李明    
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
    /// 审批岗位对应操作员表
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GAppvalPost4OperModel : EntityBase<GAppvalPost4OperModel>
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
		/// 岗位表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PostPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 操作员phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 OperatorPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 操作员代码
		/// </summary>
		[DataMember]
		public virtual System.String OperatorCode
		{
			get;
			set;
		}

		/// <summary>
		/// 顺序号
		/// </summary>
		[DataMember]
		public virtual System.Int32 FSeq
		{
			get;
			set;
		}

	}

}