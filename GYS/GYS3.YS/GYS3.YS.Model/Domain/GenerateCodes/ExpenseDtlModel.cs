#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseDtlModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        ExpenseDtlModel.cs
    * 创建时间：        2019/1/24 
    * 作    者：        董泉伟    
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
    /// ExpenseDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ExpenseDtlModel : EntityBase<ExpenseDtlModel>
    {
        /// <summary>
		/// 构造函数 
		/// </summary>
		public ExpenseDtlModel()
        {
            List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

            PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
            info.PropertyName = "MstPhid";
            info.ColumnName = "mst_phid";
            info.PropertyType = EnumPropertyType.Long;
            info.IsPrimary = false;
            list.Add(info);

            ForeignKeys = list;//设置外键字段属性
        }

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
		/// 明细项目代码
		/// </summary>
		[DataMember]
		public virtual System.String FDtlcode
		{
			get;
			set;
		}

        /// <summary>
        /// 明细费用名称
        /// </summary>
        [DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

        /// <summary>
		/// 明细项目名称
		/// </summary>
		[DataMember]
        public virtual System.String FDtlName
        {
            get;
            set;
        }
        

        /// <summary>
        /// 计量单位
        /// </summary>
        [DataMember]
		public virtual System.String FMeasunit
		{
			get;
			set;
		}

		/// <summary>
		/// 数量
		/// </summary>
		[DataMember]
		public virtual System.Decimal FQty
		{
			get;
			set;
		}

		/// <summary>
		/// 单价
		/// </summary>
		[DataMember]
		public virtual System.Decimal FPrice
		{
			get;
			set;
		}

		/// <summary>
		/// 金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 预算金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FBudgetamount
		{
			get;
			set;
		}

		/// <summary>
		/// 资金来源
		/// </summary>
		[DataMember]
		public virtual System.String FSourceoffunds
		{
			get;
			set;
		}

		/// <summary>
		/// 资金来源名称
		/// </summary>
		[DataMember]
		public virtual System.String FSourceoffunds_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 预算科目
		/// </summary>
		[DataMember]
		public virtual System.String FBudgetaccounts
		{
			get;
			set;
		}

		/// <summary>
		/// 预算科目名称
		/// </summary>
		[DataMember]
		public virtual System.String FBudgetaccounts_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 其他说明
		/// </summary>
		[DataMember]
		public virtual System.String FOtherinstructions
		{
			get;
			set;
		}

		/// <summary>
		/// 支付方式
		/// </summary>
		[DataMember]
		public virtual System.String FPaymentmethod
		{
			get;
			set;
		}

		/// <summary>
		/// 支付方式名称
		/// </summary>
		[DataMember]
		public virtual System.String FPaymentmethod_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 支出渠道
		/// </summary>
		[DataMember]
		public virtual System.String FExpenseschannel
		{
			get;
			set;
		}

		/// <summary>
		/// 支出渠道名称
		/// </summary>
		[DataMember]
		public virtual System.String FExpenseschannel_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 反馈意见
		/// </summary>
		[DataMember]
		public virtual System.String FFeedback
		{
			get;
			set;
		}

		/// <summary>
		/// 调整判断
		/// </summary>
		[DataMember]
		public virtual System.String FMidedit
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfpurchase
		{
			get;
			set;
		}

		/// <summary>
		/// 人数
		/// </summary>
		[DataMember]
		public virtual System.Decimal FQty2
		{
			get;
			set;
		}

		/// <summary>
		/// 调整金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmountedit
		{
			get;
			set;
		}

		/// <summary>
		/// 修改后金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmountafteredit
		{
			get;
			set;
		}

		/// <summary>
		/// 返还金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FReturnamount
		{
			get;
			set;
		}

	}

}