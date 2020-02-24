#region Summary
/**************************************************************************************
    * 类 名 称：        GAppvalRecordModel
    * 命名空间：        GSP3.SP.Model.Domain
    * 文 件 名：        GAppvalRecordModel.cs
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
    /// 审批流记录表
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GAppvalRecordModel : EntityBase<GAppvalRecordModel>
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
		/// 关联单据主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 RefbillPhid
		{
			get;
			set;
		}

        /// <summary>
        /// 关联单据类型
        /// </summary>
        [DataMember]
        public virtual System.String FBilltype
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
		/// 审批操作员主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 OperaPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 审批操作员代码
		/// </summary>
		[DataMember]
		public virtual System.String OperatorCode
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
		/// 送审时间(送审到下一个环节的时间)
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FSendDate
		{
			get;
			set;
		}

		/// <summary>
		/// 审批时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FDate
		{
			get;
			set;
		}

        /// <summary>
        /// 审批记录的审批状态(0- 未审批 1-待审批 2- 未通过 9-审批通过)
        /// </summary>
        [DataMember]
		public virtual System.Byte FApproval
		{
			get;
			set;
		}

		/// <summary>
		/// 审批意见
		/// </summary>
		[DataMember]
		public virtual System.String FOpinion
		{
			get;
			set;
		}

	}

}