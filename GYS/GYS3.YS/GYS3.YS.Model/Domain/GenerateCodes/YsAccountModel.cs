#region Summary
/**************************************************************************************
    * 类 名 称：        YsAccountModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        YsAccountModel.cs
    * 创建时间：        2019/9/23 
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
    /// YsAccount实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class YsAccountModel : EntityBase<YsAccountModel>
    {
		///// <summary>
		///// 构造函数
		///// </summary>
		//public YsAccountModel()
		//{
		//	List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

		//	PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
		//	info.PropertyName = "PHIDMST";
		//	info.ColumnName = "PHID_MST";
		//	info.PropertyType = EnumPropertyType.Long;
		//	info.IsPrimary = false;
		//	list.Add(info);

		//	ForeignKeys = list;//设置外键字段属性
		//}

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
		/// 外键
		/// </summary>
		[DataMember]
		public virtual System.Int64 PHIDMST
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 PHIDSUBJECT
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String SUBJECTCODE
		{
			get;
			set;
		}

        /// <summary>
        /// 科目名称
        /// </summary>
        [DataMember]
        public virtual System.String SUBJECTNAME
        {
            get;
            set;
        }

        /// <summary>
        /// 组织id
        /// </summary>
        [DataMember]
		public virtual System.Int64 ORGID
		{
			get;
			set;
		}

		/// <summary>
		/// 组织code
		/// </summary>
		[DataMember]
		public virtual System.String ORGCODE
		{
			get;
			set;
		}

		/// <summary>
		/// 年份
		/// </summary>
		[DataMember]
		public virtual System.String UYEAR
		{
			get;
			set;
		}

		/// <summary>
		/// 上年决算
		/// </summary>
		[DataMember]
		public virtual System.Decimal FINALACCOUNTSTOTAL
		{
			get;
			set;
		}

		/// <summary>
		/// 本年预算
		/// </summary>
		[DataMember]
		public virtual System.Decimal BUDGETTOTAL
		{
			get;
			set;
		}

		/// <summary>
		/// 本年调整
		/// </summary>
		[DataMember]
		public virtual System.Decimal ADJUSTMENT
		{
			get;
			set;
		}

		/// <summary>
		/// 调整后金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal APPROVEDBUDGETTOTAL
		{
			get;
			set;
		}

		/// <summary>
		/// 本年决算
		/// </summary>
		[DataMember]
		public virtual System.Decimal ThisaccountsTotal
		{
			get;
			set;
		}

		/// <summary>
		/// 完成率（本年决算/调整后预算）
		/// </summary>
		[DataMember]
		public virtual System.Decimal COMPLETE
		{
			get;
			set;
		}

        /// <summary>
        /// 预算率 （调整后预算/年初预算）
        /// </summary>
        [DataMember]
        public virtual System.Decimal BudgetComplete
        {
            get;
            set;
        }

        /// <summary>
        /// 年初明细说明
        /// </summary>
        [DataMember]
		public virtual System.String DESCRIPTION
		{
			get;
			set;
		}

		/// <summary>
		/// 年初上报标志
		/// </summary>
		[DataMember]
		public virtual System.Int32 VERIFYSTART
		{
			get;
			set;
		}

        /// <summary>
        /// 年中上报标志
        /// </summary>
        [DataMember]
		public virtual System.Int32 VERIFYMIDDLE
		{
			get;
			set;
		}

        /// <summary>
        /// 年末上报标志
        /// </summary>
        [DataMember]
		public virtual System.Int32 VERIFYEND
		{
			get;
			set;
		}

		/// <summary>
		/// 年初上报时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? VERIFYSTARTTIME
		{
			get;
			set;
		}

		/// <summary>
		/// 年中上报时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? VERIFYMIDDLETIME
		{
			get;
			set;
		}

		/// <summary>
		/// 年末上报时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? VERIFYENDTIME
		{
			get;
			set;
		}

		/// <summary>
		/// 年中明细说明
		/// </summary>
		[DataMember]
		public virtual System.String DESCRIPTIONMIDDLE
		{
			get;
			set;
		}

		/// <summary>
		/// 年末上报说明
		/// </summary>
		[DataMember]
		public virtual System.String DESCRIPTIONEND
		{
			get;
			set;
		}

        /// <summary>
        /// 下级科目
        /// </summary>
        [DataMember]
        public virtual List<YsAccountModel> Childrens
        {
            get;
            set;
        }

        #region//非实体

        /// <summary>
        /// 年末上报标志
        /// </summary>
        [DataMember]
        public virtual System.Int32 Layers
        {
            get;
            set;
        }

        #endregion
    }

}