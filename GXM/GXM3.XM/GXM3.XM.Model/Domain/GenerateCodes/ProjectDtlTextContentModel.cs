#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlTextContentModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        ProjectDtlTextContentModel.cs
    * 创建时间：        2018/9/4 
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

namespace GXM3.XM.Model.Domain
{
    /// <summary>
    /// ProjectDtlTextContent实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectDtlTextContentModel : EntityBase<ProjectDtlTextContentModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public ProjectDtlTextContentModel()
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
		/// 部门职能概述
		/// </summary>
		[DataMember]
		public virtual System.String FFunctionalOverview
		{
			get;
			set;
		}

		/// <summary>
		/// 项目概况
		/// </summary>
		[DataMember]
		public virtual System.String FProjOverview
		{
			get;
			set;
		}

		/// <summary>
		/// 立项依据
		/// </summary>
		[DataMember]
		public virtual System.String FProjBasis
		{
			get;
			set;
		}

		/// <summary>
		/// 可行性
		/// </summary>
		[DataMember]
		public virtual System.String FFeasibility
		{
			get;
			set;
		}

		/// <summary>
		/// 必要性
		/// </summary>
		[DataMember]
		public virtual System.String FNecessity
		{
			get;
			set;
		}

		/// <summary>
		/// 长期绩效目标
		/// </summary>
		[DataMember]
		public virtual System.String FLTPerformGoal
		{
			get;
			set;
		}

		/// <summary>
		/// 年度绩效目标
		/// </summary>
		[DataMember]
		public virtual System.String FAnnualPerformGoal
		{
			get;
			set;
		}

        /// <summary>
        /// 党组会议意见
        /// </summary>
        [DataMember]
        public virtual System.String FLeadingOpinions
        {
            get;
            set;
        }

        /// <summary>
        /// 主席办公意见
        /// </summary>
        [DataMember]
        public virtual System.String FChairmanOpinions
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual System.String FBz
        {
            get;
            set;
        }

        /// <summary>
        /// 部门领导意见
        /// </summary>
        [DataMember]
        public virtual System.String FDeptOpinions
        {
            get;
            set;
        }

        /// <summary>
        /// 部门分管领导意见
        /// </summary>
        [DataMember]
        public virtual System.String FDeptOpinions2
        {
            get;
            set;
        }

        /// <summary>
        /// 会议决议(0通过\1不通过)
        /// </summary>
        [DataMember]
        public virtual System.String FResolution
        {
            get;
            set;
        }

        /// <summary>
        /// 新增的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlTextContentModel> ProjectDtlTextContentsAdd
        {
            get;
            set;
        }

        /// <summary>
        /// 修改的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlTextContentModel> ProjectDtlTextContentsMid
        {
            get;
            set;
        }

        /// <summary>
        /// 删除的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlTextContentModel> ProjectDtlTextContentsDel
        {
            get;
            set;
        }

        #region//民生银行所需字段
        /// <summary>
        /// 会议日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FMeetingTime
        {
            get;
            set;
        }

        /// <summary>
        /// 会议纪要编号
        /// </summary>
        [DataMember]
        public virtual System.String FMeetiingSummaryNo
        {
            get;
            set;
        }

        /// <summary>
        /// 项目年度
        /// </summary>
        [DataMember]
        public virtual System.String FYear
        {
            get;
            set;
        }
        /// <summary>
        /// 申报单位（组织编码）
        /// </summary>
        [DataMember]
        public virtual System.String FDeclarationUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 申报人主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 FDeclarerId
        {
            get;
            set;
        }
        #endregion
    }

}