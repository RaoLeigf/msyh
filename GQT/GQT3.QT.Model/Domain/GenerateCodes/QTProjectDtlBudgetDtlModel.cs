#region Summary
/**************************************************************************************
    * 类 名 称：        QTProjectDtlBudgetDtlModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProjectDtlBudgetDtlModel.cs
    * 创建时间：        2019/9/4 
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

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// QTProjectDtlBudgetDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProjectDtlBudgetDtlModel : EntityBase<QTProjectDtlBudgetDtlModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public QTProjectDtlBudgetDtlModel()
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
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 主表phid
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
		public virtual System.String FDtlCode
		{
			get;
			set;
		}

		/// <summary>
		/// 明细项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

		/// <summary>
		/// 计量单位
		/// </summary>
		[DataMember]
		public virtual System.String FMeasUnit
		{
			get;
			set;
		}

		/// <summary>
		/// 天数
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
		public virtual System.Decimal FBudgetAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 资金来源
		/// </summary>
		[DataMember]
		public virtual System.String FSourceOfFunds
		{
			get;
			set;
		}

		/// <summary>
		/// 预算科目
		/// </summary>
		[DataMember]
		public virtual System.String FBudgetAccounts
		{
			get;
			set;
		}
        /// <summary>
		/// 预算科目
		/// </summary>
		[DataMember]
        public virtual System.String FBudgetAccounts_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 其他说明
        /// </summary>
        [DataMember]
		public virtual System.String FOtherInstructions
		{
			get;
			set;
		}

		/// <summary>
		/// 支付方式
		/// </summary>
		[DataMember]
		public virtual System.String FPaymentMethod
		{
			get;
			set;
		}

		/// <summary>
		/// 支出渠道
		/// </summary>
		[DataMember]
		public virtual System.String FExpensesChannel
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
		/// 年中调整判断
		/// </summary>
		[DataMember]
		public virtual System.String FMidEdit
		{
			get;
			set;
		}

		/// <summary>
		/// 是否集中采购
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIfPurchase
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
		public virtual System.Decimal FAmountEdit
		{
			get;
			set;
		}

		/// <summary>
		/// 调整后金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmountAfterEdit
		{
			get;
			set;
		}

		/// <summary>
		/// 支出功能分类科目
		/// </summary>
		[DataMember]
		public virtual System.String FQtZcgnfl
		{
			get;
			set;
		}

		/// <summary>
		/// 数量
		/// </summary>
		[DataMember]
		public virtual System.Decimal FNum
		{
			get;
			set;
		}
        /// <summary>
        /// 上年预算金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FLastYearAmount
        {
            get;
            set;
        }
    }

}