#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPurchaseDtlModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        ProjectDtlPurchaseDtlModel.cs
    * 创建时间：        2018/10/15 
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

using GXM3.XM.Model.Enums;

namespace GXM3.XM.Model.Domain
{
    /// <summary>
    /// ProjectDtlPurchaseDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectDtlPurchaseDtlModel : EntityBase<ProjectDtlPurchaseDtlModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectDtlPurchaseDtlModel()
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
        /// phid
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
        /// 采购内容
        /// </summary>
        [DataMember]
		public virtual System.String FContent
		{
			get;
			set;
		}

		/// <summary>
		/// 采购目录代码
		/// </summary>
		[DataMember]
		public virtual System.String FCatalogCode
		{
			get;
			set;
		}

		/// <summary>
		/// 采购目录代码名称
		/// </summary>
		[DataMember]
		public virtual System.String FCatalogCode_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 采购类型代码
		/// </summary>
		[DataMember]
		public virtual System.String FTypeCode
		{
			get;
			set;
		}

		/// <summary>
		/// 采购类型代码名称
		/// </summary>
		[DataMember]
		public virtual System.String FTypeCode_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 采购程序代码
		/// </summary>
		[DataMember]
		public virtual System.String FProcedureCode
		{
			get;
			set;
		}

		/// <summary>
		/// 采购程序代码名称
		/// </summary>
		[DataMember]
		public virtual System.String FProcedureCode_EXName
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
		/// 数量
		/// </summary>
		[DataMember]
		public virtual System.Decimal FQty
		{
			get;
			set;
		}

		/// <summary>
		/// 预计单价
		/// </summary>
		[DataMember]
		public virtual System.Decimal FPrice
		{
			get;
			set;
		}

		/// <summary>
		/// 总计金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 技术参数及配置标准
		/// </summary>
		[DataMember]
		public virtual System.String FSpecification
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String FRemark
		{
			get;
			set;
		}

		/// <summary>
		/// 预计采购时间
		/// </summary>
		[DataMember]
		public virtual System.String FEstimatedPurTime
		{
			get;
			set;
		}

		/// <summary>
		/// 是否绩效评价
		/// </summary>
		[DataMember]
		public virtual EnumYesNo FIfPerformanceAppraisal
		{
			get;
			set;
		}

        /// <summary>
        /// 新增的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurchaseDtlModel> ProjectDtlPurchaseDtlsAdd
        {
            get;
            set;
        }

        /// <summary>
        /// 修改的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurchaseDtlModel> ProjectDtlPurchaseDtlsMid
        {
            get;
            set;
        }

        /// <summary>
        /// 删除的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurchaseDtlModel> ProjectDtlPurchaseDtlsDel
        {
            get;
            set;
        }
    }

}