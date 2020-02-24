#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPerformTargetModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        ProjectDtlPerformTargetModel.cs
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
    /// ProjectDtlPerformTarget实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectDtlPerformTargetModel : EntityBase<ProjectDtlPerformTargetModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectDtlPerformTargetModel()
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
		/// 指标代码
		/// </summary>
		[DataMember]
		public virtual System.String FTargetCode
		{
			get;
			set;
		}

		/// <summary>
		/// 指标名称
		/// </summary>
		[DataMember]
		public virtual System.String FTargetName
		{
			get;
			set;
		}

		/// <summary>
		/// 指标内容
		/// </summary>
		[DataMember]
		public virtual System.String FTargetContent
		{
			get;
			set;
		}

		/// <summary>
		/// 指标值
		/// </summary>
		[DataMember]
		public virtual System.String FTargetValue
		{
			get;
			set;
		}

		/// <summary>
		/// 指标权重
		/// </summary>
		[DataMember]
		public virtual System.Decimal FTargetWeight
		{
			get;
			set;
		}

		/// <summary>
		/// 指标描述
		/// </summary>
		[DataMember]
		public virtual System.String FTargetDescribe
		{
			get;
			set;
		}

		/// <summary>
		/// 指标类别代码
		/// </summary>
		[DataMember]
		public virtual System.String FTargetClassCode
		{
			get;
			set;
		}

        /// <summary>
		/// 指标类别代码名称
		/// </summary>
		[DataMember]
        public virtual System.String FTargetClassCode_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
		public virtual System.String FTargetTypeCode
		{
			get;
			set;
		}

        /// <summary>
        /// 指标类型代码名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否用户增加
        /// </summary>
        [DataMember]
		public virtual EnumYesNo FIfCustom
		{
			get;
			set;
		}

        /// <summary>
        /// 新增的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPerformTargetModel> ProjectDtlPerformTargetsAdd
        {
            get;
            set;
        }

        /// <summary>
        /// 修改的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPerformTargetModel> ProjectDtlPerformTargetsMid
        {
            get;
            set;
        }

        /// <summary>
        /// 删除的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPerformTargetModel> ProjectDtlPerformTargetsDel
        {
            get;
            set;
        }

        #region//多层指标类别
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypePerantCode
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode1
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName1
        {
            get;
            set;
        }
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode2
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName2
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode3
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName3
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode4
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName4
        {
            get;
            set;
        }
        /// <summary>
        /// 指标类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeCode5
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetTypeName5
        {
            get;
            set;
        }

        /// <summary>
        /// 指标类型的层级
        /// </summary>
        [DataMember]
        public virtual System.Int32 TypeCount
        {
            get;
            set;
        }
        #endregion
    }

}