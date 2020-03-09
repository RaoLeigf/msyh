#region Summary
/**************************************************************************************
    * 类 名 称：        XmReportDtlModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        XmReportDtlModel.cs
    * 创建时间：        2020/1/17 
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

namespace GXM3.XM.Model.Domain
{
    /// <summary>
    /// XmReportDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class XmReportDtlModel : EntityBase<XmReportDtlModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public XmReportDtlModel()
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
		/// 对象的项目主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 XmPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 费用项目名称
		/// </summary>
		[DataMember]
		public virtual System.String XmName
		{
			get;
			set;
		}

		/// <summary>
		/// 数量
		/// </summary>
		[DataMember]
		public virtual System.Int32 FNum
		{
			get;
			set;
		}

		/// <summary>
		/// 金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FPrice
		{
			get;
			set;
		}

		/// <summary>
		/// 明细金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 是否为固定成本
		/// </summary>
		[DataMember]
		public virtual System.Int32 FIsCost
		{
			get;
			set;
		}

		/// <summary>
		/// 明细备注
		/// </summary>
		[DataMember]
		public virtual System.String FRemark
		{
			get;
			set;
		}

        /// <summary>
		/// 费用项目代码
		/// </summary>
		[DataMember]
        public virtual System.String CostitemCode
        {
            get;
            set;
        }

        /// <summary>
		/// 返回金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FReturnAmount
        {
            get;
            set;
        }
        /// <summary>
		/// 变量1
		/// </summary>
		[DataMember]
        public virtual System.Decimal FVariable1
        {
            get;
            set;
        }
        /// <summary>
		/// 单位1
		/// </summary>
		[DataMember]
        public virtual System.String FUnit1
        {
            get;
            set;
        }
        /// <summary>
		/// 变量2
		/// </summary>
		[DataMember]
        public virtual System.Decimal FVariable2
        {
            get;
            set;
        }
        /// <summary>
		/// 单位2
		/// </summary>
		[DataMember]
        public virtual System.String FUnit2
        {
            get;
            set;
        }
        /// <summary>
        /// 变量3
		/// </summary>
		[DataMember]
        public virtual System.Decimal FVariable3
        {
            get;
            set;
        }
        /// <summary>
		/// 单位3
		/// </summary>
		[DataMember]
        public virtual System.String FUnit3
        {
            get;
            set;
        }
    }

}