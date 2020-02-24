#region Summary
/**************************************************************************************
    * 类 名 称：        PaymentXmModel
    * 命名空间：        GBK3.BK.Model.Domain
    * 文 件 名：        PaymentXmModel.cs
    * 创建时间：        2019/5/23 
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

namespace GBK3.BK.Model.Domain
{
    /// <summary>
    /// PaymentXm实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PaymentXmModel : EntityBase<PaymentXmModel>
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
		/// 资金拨付主表
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 项目主表
		/// </summary>
		[DataMember]
		public virtual System.Int64 XmMstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 项目编码
		/// </summary>
		[DataMember]
		public virtual System.String XmProjcode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String XmProjname
		{
			get;
			set;
		}

		/// <summary>
		/// 合计申请金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmountTotal
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String FRemarks
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
        /// 是否作废
        /// </summary>
        [DataMember]
        public virtual System.Byte FDelete
        {
            get;
            set;
        }
    }

}