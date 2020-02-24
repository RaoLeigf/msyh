#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetDtlFundApplModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        BudgetDtlFundApplModel.cs
    * 创建时间：        2018/8/30 
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
    /// BudgetDtlFundAppl实体定义类 
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class BudgetDtlFundApplModel : EntityBase<BudgetDtlFundApplModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public BudgetDtlFundApplModel()
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
		/// 项目表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 XmPhid
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
        /// 金额
        /// </summary>
        [DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

	}

}