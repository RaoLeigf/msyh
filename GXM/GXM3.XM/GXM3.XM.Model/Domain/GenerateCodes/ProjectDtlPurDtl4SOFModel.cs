#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPurDtl4SOFModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        ProjectDtlPurDtl4SOFModel.cs
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

namespace GXM3.XM.Model.Domain
{
    /// <summary>
    /// ProjectDtlPurDtl4SOF实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectDtlPurDtl4SOFModel : EntityBase<ProjectDtlPurDtl4SOFModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectDtlPurDtl4SOFModel()
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
        /// 金额
        /// </summary>
        [DataMember]
		public virtual System.Decimal FAmount
		{
			get;
			set;
		}

        /// <summary>
        /// 新增的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurDtl4SOFModel> ProjectDtlPurDtl4SOFsAdd
        {
            get;
            set;
        }

        /// <summary>
        /// 修改的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurDtl4SOFModel> ProjectDtlPurDtl4SOFsMid
        {
            get;
            set;
        }

        /// <summary>
        /// 删除的列表
        /// </summary>
        [DataMember]
        public virtual List<ProjectDtlPurDtl4SOFModel> ProjectDtlPurDtl4SOFsDel
        {
            get;
            set;
        }
    }

}