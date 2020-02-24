#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetDtlPersonNameModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        BudgetDtlPersonNameModel.cs
    * 创建时间：        2020/1/14 
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
    /// BudgetDtlPersonName实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class BudgetDtlPersonNameModel : EntityBase<BudgetDtlPersonNameModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public BudgetDtlPersonNameModel()
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
		/// 预算主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

        /// <summary>
        /// 对应项目人员追加名单的主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 XmPhId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
		public virtual System.Int64 FUserId
		{
			get;
			set;
		}

		/// <summary>
		/// 用户编码
		/// </summary>
		[DataMember]
		public virtual System.String FUserCode
		{
			get;
			set;
		}

		/// <summary>
		/// 组织id
		/// </summary>
		[DataMember]
		public virtual System.Int64 FOrgId
		{
			get;
			set;
		}

		/// <summary>
		/// 组织编码
		/// </summary>
		[DataMember]
		public virtual System.String FOrgCode
		{
			get;
			set;
		}

		/// <summary>
		/// 排序
		/// </summary>
		[DataMember]
		public virtual System.Int32 SortCode
		{
			get;
			set;
		}

	}

}