#region Summary
/**************************************************************************************
    * 类 名 称：        GAppvalProcCondsModel
    * 命名空间：        GSP3.SP.Model.Domain
    * 文 件 名：        GAppvalProcCondsModel.cs
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
    /// 审批流程条件表
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GAppvalProcCondsModel : EntityBase<GAppvalProcCondsModel>
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
		/// 序号
		/// </summary>
		[DataMember]
		public virtual System.Int32 FSeq
		{
			get;
			set;
		}

		/// <summary>
		/// 操作数1
		/// </summary>
		[DataMember]
		public virtual System.String FOperand1
		{
			get;
			set;
		}

		/// <summary>
		/// 操作数1类型
		/// </summary>
		[DataMember]
		public virtual System.String FOperand1Tp
		{
			get;
			set;
		}

		/// <summary>
		/// 运算符
		/// </summary>
		[DataMember]
		public virtual System.String FOperator
		{
			get;
			set;
		}

		/// <summary>
		/// 操作数2
		/// </summary>
		[DataMember]
		public virtual System.String FOperand2
		{
			get;
			set;
		}

		/// <summary>
		/// 连接符
		/// </summary>
		[DataMember]
		public virtual System.String FConnector
		{
			get;
			set;
		}

	}

}