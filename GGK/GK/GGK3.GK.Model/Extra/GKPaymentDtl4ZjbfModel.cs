#region Summary
/**************************************************************************************
    * 类 名 称：        GKPaymentDtlModel
    * 命名空间：        GGK3.GK.Model.Domain
    * 文 件 名：        GKPaymentDtlModel.cs
    * 创建时间：        2019/5/23 
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
using Newtonsoft.Json;

namespace GGK3.GK.Model.Domain
{
    /// <summary>
    /// 资金拨付支付单
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GKPaymentDtl4ZjbfModel: EntityBase<GKPaymentDtl4ZjbfModel>
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
		/// 主表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 组织主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 OrgPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 组织编码
		/// </summary>
		[DataMember]
		public virtual System.String OrgCode
		{
			get;
			set;
		}

        /// <summary>
        /// 关联业务单主键,对应 bk3_payment_dtl.mst_phid
        /// </summary>
        [DataMember]
		public virtual System.Int64 RefbillPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 关联业务单号
		/// </summary>
		[DataMember]
		public virtual System.String RefbillCode
		{
			get;
			set;
		}

        /// <summary>
        /// 关联业务单明细表主键, 对应 bk3_payment_dtl.phid
        /// </summary>
        [DataMember]
		public virtual System.Int64 RefbillDtlPhid
		{
			get;
			set;
		}

        /// <summary>
        /// 关联业务单明细表主键2,对应 bk3_payment_dtl.budgetdtl_phid
        /// </summary>
        [DataMember]
		public virtual System.Int64 RefbillDtlPhid2
		{
			get;
			set;
		}

		/// <summary>
		/// 付款金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 币种
		/// </summary>
		[DataMember]
		public virtual System.String FCurrency
		{
			get;
			set;
		}

		/// <summary>
		/// 付款方账户名称
		/// </summary>
		[DataMember]
		public virtual System.String FPayAcntname
		{
			get;
			set;
		}

		/// <summary>
		/// 付款方银行账号
		/// </summary>
		[DataMember]
		public virtual System.String FPayAcnt
		{
			get;
			set;
		}

		/// <summary>
		/// 付款方银行行号
		/// </summary>
		[DataMember]
		public virtual System.String FPayBankcode
		{
			get;
			set;
		}

		/// <summary>
		/// 收款方账户名称
		/// </summary>
		[DataMember]
		public virtual System.String FRecAcntname
		{
			get;
			set;
		}

		/// <summary>
		/// 收款方银行账号
		/// </summary>
		[DataMember]
		public virtual System.String FRecAcnt
		{
			get;
			set;
		}

		/// <summary>
		/// 收款方银行行号
		/// </summary>
		[DataMember]
		public virtual System.String FRecBankcode
		{
			get;
			set;
		}

		/// <summary>
		/// 收款方城市名
		/// </summary>
		[DataMember]
		public virtual System.String FRecCityname
		{
			get;
			set;
		}

		/// <summary>
		/// 是否同城
		/// </summary>
		[DataMember]
		public virtual System.Byte FSamecity
		{
			get;
			set;
		}

		/// <summary>
		/// 是否同行
		/// </summary>
		[DataMember]
		public virtual System.Byte FSamebank
		{
			get;
			set;
		}

		/// <summary>
		/// 是否加急
		/// </summary>
		[DataMember]
		public virtual System.Byte FIsurgent
		{
			get;
			set;
		}

		/// <summary>
		/// 对公对私
		/// </summary>
		[DataMember]
		public virtual System.Byte FCorp
		{
			get;
			set;
		}

		/// <summary>
		/// 用途
		/// </summary>
		[DataMember]
		public virtual System.String FUsage
		{
			get;
			set;
		}

		/// <summary>
		/// 附言
		/// </summary>
		[DataMember]
		public virtual System.String FPostscript
		{
			get;
			set;
		}

		/// <summary>
		/// 摘要
		/// </summary>
		[DataMember]
		public virtual System.String FExplation
		{
			get;
			set;
		}

		/// <summary>
		/// 描述
		/// </summary>
		[DataMember]
		public virtual System.String FDescribe
		{
			get;
			set;
		}

		/// <summary>
		/// 提交支付时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FSubmitdate
		{
			get;
			set;
		}

		/// <summary>
		/// 支付指令序号
		/// </summary>
		[DataMember]
		public virtual System.String FSeqno
		{
			get;
			set;
		}

		/// <summary>
		/// 银行交易流水号
		/// </summary>
		[DataMember]
		public virtual System.String FBkSn
		{
			get;
			set;
		}

		/// <summary>
		/// 支付返回结果
		/// </summary>
		[DataMember]
		public virtual System.String FResult
		{
			get;
			set;
		}

		/// <summary>
		/// 支付返回结果描述
		/// </summary>
		[DataMember]
		public virtual System.String FResultmsg
		{
			get;
			set;
		}

		/// <summary>
		/// 支付状态
		/// </summary>
		[DataMember]
		public virtual System.Byte FState
		{
			get;
			set;
		}

		/// <summary>
		/// 重新支付关联单号
		/// </summary>
		[DataMember]
		public virtual System.String FNewCode
		{
			get;
			set;
		}

        /// <summary>
        /// 拨付项目代码
        /// </summary>
        [DataMember]
        public virtual System.String XmProjcode
        {
            get;
            set;
        }

        /// <summary>
        /// 拨付项目名称
        /// </summary>
        [DataMember]
        public virtual System.String XmProjname
        {
            get;
            set;
        }

        /// <summary>
        /// 拨付明细项目名称
        /// </summary>
        [DataMember]
        public virtual System.String BudgetdtlName
        {
            get;
            set;
        }

        /// <summary>
        /// 补助单位/部门phid
        /// </summary>
        [DataMember]
        public virtual System.Int64 FDepartmentphid
        {
            get;
            set;
        }

        /// <summary>
        /// 补助单位/部门代码
        /// </summary>
        [DataMember]
        public virtual System.String FDepartmentcode
        {
            get;
            set;
        }
        
        /// <summary>
        /// 补助单位/部门名称
        /// </summary>
        [DataMember]
        public virtual System.String FDepartmentname
        {
            get;
            set;
        }

        /// <summary>
        /// 补助部门上级组织phid
        /// </summary>
        [DataMember]
        public virtual System.Int64 FDepartmentParentphid
        {
            get;
            set;
        }

        /// <summary>
        /// 补助部门上级组织代码
        /// </summary>
        [DataMember]
        public virtual System.String FDepartmentParentcode
        {
            get;
            set;
        }

        /// <summary>
        /// 补助部门上级组织名称
        /// </summary>
        [DataMember]
        public virtual System.String FDepartmentParentname
        {
            get;
            set;
        }

        /// <summary>
        /// 预算科目代码
        /// </summary>
        [DataMember]
        public virtual System.String QtKmdm
        {
            get;
            set;
        }

        /// <summary>
        /// 预算科目名称
        /// </summary>
        [DataMember]
        public virtual System.String QtKmmc
        {
            get;
            set;
        }

        /// <summary>
        /// 付款方所在城市名
        /// </summary>
        [DataMember]
        public virtual System.String FPayCityname
        {
            get;
            set;
        }

        /// <summary>
        /// 付款方银行名称
        /// </summary>
        [DataMember]
        public virtual System.String FPayBankname
        {
            get;
            set;
        }

        /// <summary>
        /// 收款方银行名称
        /// </summary>
        [DataMember]
        public virtual System.String FRecBankname
        {
            get;
            set;
        }

        /// <summary>
        /// 银行档案phid
        /// </summary>
        [DataMember]
        public virtual System.Int64 BankPhid
        {
            get;
            set;
        }

        /// <summary>
		/// 原支付单主表phid
		/// </summary>
		[DataMember]
        public virtual System.Int64 OldMstPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 原支付单明细表phid
        /// </summary>
        [DataMember]
        public virtual System.Int64 OldDtlPhid
        {
            get;
            set;
        }
                
        /// <summary>
        /// 支付明细序号
        /// </summary>
        [DataMember]
        public virtual System.String FIseqno
        {
            get;
            set;
        }

        /// <summary>
        /// 重新支付关联单号数组,多个用逗号分隔
        /// </summary>
        [DataMember]
        public virtual System.String FNewCodes
        {
            get;
            set;
        }

        /// <summary>
        /// 重新支付关联单号主表数组,多个用逗号分隔
        /// </summary>
        [DataMember]
        public virtual System.String FNewCodesMstPhid
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
        #region Json序列化要忽略的列增加 JsonIgnore 属性
        /// <summary>
        /// 外键
        /// </summary>
        [DataMember, JsonIgnore]
        override public IList<PropertyColumnMapperInfo> ForeignKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public IList<PropertyColumnMapperInfo> BusinessPrimaryKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public object _OldIdValue_ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public Dictionary<string, object> ExtendObjects { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember, JsonIgnore]
        override public Dictionary<string, byte[]> PropertyBytes { get; set; }

        #endregion
    }

}