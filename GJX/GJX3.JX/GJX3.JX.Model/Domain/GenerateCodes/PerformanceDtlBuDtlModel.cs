#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceDtlBuDtlModel
    * 命名空间：        GJX3.JX.Model.Domain
    * 文 件 名：        PerformanceDtlBuDtlModel.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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

namespace GJX3.JX.Model.Domain
{
    /// <summary>
    /// PerformanceDtlBuDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PerformanceDtlBuDtlModel : EntityBase<PerformanceDtlBuDtlModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public PerformanceDtlBuDtlModel()
		{
			List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

			PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
			info.PropertyName = "MstPhid";
			info.ColumnName = "mst_phid";
			info.PropertyType = EnumPropertyType.Long;
			info.IsPrimary = false;
			list.Add(info);

			info = new PropertyColumnMapperInfo();
			info.PropertyName = "DelPhid";
			info.ColumnName = "del_phid";
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
		/// 项目明细主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 DelPhid
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
        /// 资金来源
        /// </summary>
        [DataMember]
		public virtual System.String FSourceOfFunds
		{
			get;
			set;
		}

        /// <summary>
        /// 资金来源名称
        /// </summary>
        [DataMember]
        public virtual System.String FSourceOfFunds_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 支出渠道名称
        /// </summary>
        [DataMember]
        public virtual System.String FExpensesChannel_EXName
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
		/// 实际执行数
		/// </summary>
		[DataMember]
		public virtual System.Decimal FActualAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 结余金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FBalanceAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 执行率
		/// </summary>
		[DataMember]
		public virtual System.Decimal FImplRate
		{
			get;
			set;
		}

	}

}