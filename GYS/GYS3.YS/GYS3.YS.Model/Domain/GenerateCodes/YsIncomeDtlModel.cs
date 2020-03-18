#region Summary
/**************************************************************************************
    * 类 名 称：        YsIncomeDtlModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        YsIncomeDtlModel.cs
    * 创建时间：        2019/12/31 
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
    /// YsIncomeDtl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class YsIncomeDtlModel : EntityBase<YsIncomeDtlModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public YsIncomeDtlModel()
		{
			List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

			PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
			info.PropertyName = "MstPhid";
			info.ColumnName = "Mst_Phid";
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
		/// 科目编码
		/// </summary>
		[DataMember]
		public virtual System.String FSubjectCode
		{
			get;
			set;
		}

		/// <summary>
		/// 科目名称
		/// </summary>
		[DataMember]
		public virtual System.String FSubjectname
		{
			get;
			set;
		}

		/// <summary>
		/// 分项目主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 FSubitemid
		{
			get;
			set;
		}

		/// <summary>
		/// 分项目编码
		/// </summary>
		[DataMember]
		public virtual System.String FSubitemCode
		{
			get;
			set;
		}

		/// <summary>
		/// 预算金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FBudgetamount
		{
			get;
			set;
		}

		/// <summary>
		/// 收入归属部门id(预算部门)
		/// </summary>
		[DataMember]
		public virtual System.Int64 FBudgetid
		{
			get;
			set;
		}

        /// <summary>
        /// 收入归属部门编码
        /// </summary>
        [DataMember]
		public virtual System.String FBudgetcode
		{
			get;
			set;
		}

		/// <summary>
		/// 编制依据
		/// </summary>
		[DataMember]
		public virtual System.String FFormation
		{
			get;
			set;
		}

		/// <summary>
		/// 申报进度（与进度控制相对应）
		/// </summary>
		[DataMember]
		public virtual System.String FProcessStatus
		{
			get;
			set;
		}

        
        /// <summary>
        /// 分项目名称
        /// </summary>
        [DataMember]
        public virtual System.String FSubitemName
        {
            get;
            set;
        }
        #region//虚字段
        /// <summary>
        /// 收入归属部门名称
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetName
        {
            get;
            set;
        }

        /// <summary>
        /// 收入归属部门名称
        /// </summary>
        [DataMember]
        public virtual System.Int32 IsLast
        {
            get;
            set;
        }

		/// <summary>
		/// 是否修改
		/// </summary>
		[DataMember]
		public virtual bool edit
		{
			get;
			set;
		}
		#endregion
	}

}