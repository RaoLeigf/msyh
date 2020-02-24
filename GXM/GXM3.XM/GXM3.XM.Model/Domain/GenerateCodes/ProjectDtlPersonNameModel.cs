#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPersonNameModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        ProjectDtlPersonNameModel.cs
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

namespace GXM3.XM.Model.Domain
{
    /// <summary>
    /// ProjectDtlPersonName实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectDtlPersonNameModel : EntityBase<ProjectDtlPersonNameModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public ProjectDtlPersonNameModel()
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

        #region//虚拟字段

        /// <summary>
        /// 用户名称
        /// </summary>
        [DataMember]
        public virtual System.String FUserName
        {
            get;
            set;
        }
        /// <summary>
        /// 组织名称
        /// </summary>
        [DataMember]
        public virtual System.String FOrgName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否关联人员
        /// </summary>
        [DataMember]
        public virtual System.Int32 IsRelation
        {
            get;
            set;
        }
        /// <summary>
        /// 是否固定(不能进行修改的)
        /// </summary>
        [DataMember]
        public virtual System.Int32 IsFix
        {
            get;
            set;
        }
        #endregion
    }

}