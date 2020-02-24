#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetAccountsModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        BudgetAccountsModel.cs
    * 创建时间：        2018/8/29 
    * 作    者：        夏华军    
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

using GQT3.QT.Model.Enums;

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// BudgetAccounts实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class BudgetAccountsModel : EntityBase<BudgetAccountsModel>
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
		/// 科目代码
		/// </summary>
		[DataMember]
		public virtual System.String KMDM
		{
			get;
			set;
		}

		/// <summary>
		/// 科目名称
		/// </summary>
		[DataMember]
		public virtual System.String KMMC
		{
			get;
			set;
		}

		/// <summary>
		/// 科目类别
		/// </summary>
		[DataMember]
		public virtual System.String KMLB
		{
			get;
			set;
		}

		/// <summary>
		/// 科目性质
		/// </summary>
		[DataMember]
		public virtual System.String KMXZ
		{
			get;
			set;
		}

		/// <summary>
		/// 余额方向
		/// </summary>
		[DataMember]
		public virtual System.String YEFX
		{
			get;
			set;
		}

		/// <summary>
		/// 实付
		/// </summary>
		[DataMember]
		public virtual System.Byte SF
		{
			get;
			set;
		}

		/// <summary>
		/// 助记码
		/// </summary>
		[DataMember]
		public virtual System.String JM
		{
			get;
			set;
		}

		/// <summary>
		/// 部门
		/// </summary>
		[DataMember]
		public virtual System.Byte BM
		{
			get;
			set;
		}

		/// <summary>
		/// 货币
		/// </summary>
		[DataMember]
		public virtual System.String HB
		{
			get;
			set;
		}

		/// <summary>
		/// 对应总预算账套预算支出科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// 对应国库支付执行机构账套预算支出-财政直接支付科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// 对应国库支付执行机构账套预算支出-单位零余额账户额度科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}

		/// <summary>
		/// 对应国库支付执行机构账套预算支出-小额现金额度科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR4
		{
			get;
			set;
		}

		/// <summary>
		/// 拨款凭证预算内科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR5
		{
			get;
			set;
		}

		/// <summary>
		/// 拨款凭证预算外科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR6
		{
			get;
			set;
		}

		/// <summary>
		/// 拨款凭证其他科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR7
		{
			get;
			set;
		}

		/// <summary>
		/// 拨款凭证基金科目
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR8
		{
			get;
			set;
		}

		/// <summary>
		/// 宁波高新发送银行标志
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR9
		{
			get;
			set;
		}

		/// <summary>
		/// DEF_STR10
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR10
		{
			get;
			set;
		}

		/// <summary>
		/// Ocode
		/// </summary>
		[DataMember]
		public virtual System.String Ocode
		{
			get;
			set;
		}

		/// <summary>
		/// Orgid
		/// </summary>
		[DataMember]
		public virtual System.Int64 Orgid
		{
			get;
			set;
		}

	}

}