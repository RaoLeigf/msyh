#region Summary
/**************************************************************************************
    * 类 名 称：        YsIncomeMstModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        YsIncomeMstModel.cs
    * 创建时间：        2019/12/31 
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

namespace GYS3.YS.Model.Domain
{
    /// <summary>
    /// YsIncomeMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class YsIncomeMstModel : EntityBase<YsIncomeMstModel>
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
		/// 组织id
		/// </summary>
		[DataMember]
		public virtual System.Int64 FOrgID
		{
			get;
			set;
		}

		/// <summary>
		/// 组织编码
		/// </summary>
		[DataMember]
		public virtual System.String FOrgcode
		{
			get;
			set;
		}

		/// <summary>
		/// 年度
		/// </summary>
		[DataMember]
		public virtual System.String FYear
		{
			get;
			set;
		}

		/// <summary>
		/// 审批状态（0-待送审，1-审批中，2-审批不通过，9-审批通过）
		/// </summary>
		[DataMember]
		public virtual System.Byte FApproval
		{
			get;
			set;
		}

		/// <summary>
		/// 是否生成预算
		/// </summary>
		[DataMember]
		public virtual System.Byte FIsbudget
		{
			get;
			set;
		}

		/// <summary>
		/// 生成预算人id
		/// </summary>
		[DataMember]
		public virtual System.Int64 FBudgeter
		{
			get;
			set;
		}

        /// <summary>
        /// 生成预算时间
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FBudgettime
        {
            get;
            set;
        }


        /// <summary>
        /// 是否上报
        /// </summary>
        [DataMember]
		public virtual System.Byte FIsreport
		{
			get;
			set;
		}

		/// <summary>
		/// 上报时间（送审时间）
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FReporttime
		{
			get;
			set;
		}

		/// <summary>
		/// 上报人id（送审人）
		/// </summary>
		[DataMember]
		public virtual System.Int64 FReporter
		{
			get;
			set;
		}

        /// <summary>
        /// 申报日期（及保存的时间）
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FDeclareTime
        {
            get;
            set;
        }

        /// <summary>
        /// 申报金额（保存的总金额）
        /// </summary>
        [DataMember]
        public virtual System.Decimal FDeclareAmount
        {
            get;
            set;
        }
    }

}