#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetDtlPerformTargetModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        BudgetDtlPerformTargetModel.cs
    * 创建时间：        2018/10/16 
    * 作    者：        夏华军    
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
    /// BudgetDtlPerformTarget实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class BudgetDtlPerformTargetModel : EntityBase<BudgetDtlPerformTargetModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public BudgetDtlPerformTargetModel()
		{
			List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

			PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
			info.PropertyName = "MstPhId";
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
		/// 主表id
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhId
		{
			get;
			set;
		}

		/// <summary>
		/// 项目phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 XmPhId
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
		public virtual System.Int32 FIfCustom
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