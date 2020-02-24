#region Summary
/**************************************************************************************
    * 类 名 称：        XmReportMstModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        XmReportMstModel.cs
    * 创建时间：        2020/1/17 
    * 作    者：        王冠冠    
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

namespace GXM3.XM.Model.Domain
{
    /// <summary>
    /// XmReportMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class XmReportMstModel : EntityBase<XmReportMstModel>
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
		/// 签报单名称
		/// </summary>
		[DataMember]
		public virtual System.String FTitle
		{
			get;
			set;
		}


        /// <summary>
        /// 签报单编码
        /// </summary>
        [DataMember]
        public virtual System.String FCode
        {
            get;
            set;
        }

        /// <summary>
        /// 关联项目名称
        /// </summary>
        [DataMember]
		public virtual System.Int64 XmPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 业务条线编码
		/// </summary>
		[DataMember]
		public virtual System.String FBusinessCode
		{
			get;
			set;
		}

		/// <summary>
		/// 签报理由
		/// </summary>
		[DataMember]
		public virtual System.String FReason
		{
			get;
			set;
		}

		/// <summary>
		/// 申请时间（签报时间）
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FTime
		{
			get;
			set;
		}

		/// <summary>
		/// 签报人
		/// </summary>
		[DataMember]
		public virtual System.Int64 FDeclarerId
		{
			get;
			set;
		}

		/// <summary>
		/// 联系电话
		/// </summary>
		[DataMember]
		public virtual System.String FPhone
		{
			get;
			set;
		}

		/// <summary>
		/// 项目总金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FSumAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 剩余预算金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FSurplusAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 批示意见
		/// </summary>
		[DataMember]
		public virtual System.String FOpinion
		{
			get;
			set;
		}

		/// <summary>
		/// 审批流程查看
		/// </summary>
		[DataMember]
		public virtual System.String FUrl
		{
			get;
			set;
		}

		/// <summary>
		/// 签报状态（0-待送审，1-审批中，9-审批通过）
		/// </summary>
		[DataMember]
		public virtual System.Byte FApprove
		{
			get;
			set;
		}

		/// <summary>
		/// 审批通过的金额（及签报通过的金额）
		/// </summary>
		[DataMember]
		public virtual System.Decimal FApproveAmount
		{
			get;
			set;
		}

        /// <summary>
        /// 是否从生成草案而产生的签报单（0-否，1-是）
        /// </summary>
        [DataMember]
        public virtual System.Int32 FIsDraft
        {
            get;
            set;
        }

        #region//虚拟字段

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember]
        public virtual System.String FProjCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember]
        public virtual System.String FProjName
        {
            get;
            set;
        }
        /// <summary>
        /// 业务条线名称
        /// </summary>
        [DataMember]
        public virtual System.String FBusinessName
        {
            get;
            set;
        }

        /// <summary>
        /// 明细表集合
        /// </summary>
        [DataMember]
        public IList<XmReportDtlModel> XmReportDtls
        {
            get;
            set;
        }

        /// <summary>
        /// 项目状态
        /// </summary>
        [DataMember]
        public virtual System.Int32 FProjStatus
        {
            get;
            set;
        }
        #endregion
    }

}