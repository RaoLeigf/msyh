#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlImplPlanModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        ProjectDtlImplPlanModel.cs
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
    /// ProjectDtlImplPlan实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectDtlImplPlanModel : EntityBase<ProjectDtlImplPlanModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public ProjectDtlImplPlanModel()
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
		/// 主表phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 实施内容
		/// </summary>
		[DataMember]
		public virtual System.String FImplContent
		{
			get;
			set;
		}

		/// <summary>
		/// 开始日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FStartDate
		{
			get;
			set;
		}

		/// <summary>
		/// 结束日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FEndDate
		{
			get;
			set;
		}

        /// <summary>
        /// 新增的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlImplPlanModel> ProjectDtlImplPlansAdd
        {
            get;
            set;
        }

        /// <summary>
        /// 修改的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlImplPlanModel> ProjectDtlImplPlansMid
        {
            get;
            set;
        }

        /// <summary>
        /// 删除的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlImplPlanModel> ProjectDtlImplPlansDel
        {
            get;
            set;
        }
    }

}