#region Summary
/**************************************************************************************
    * 类 名 称：        PerformanceDtlTarImplModel
    * 命名空间：        GJX3.JX.Model.Domain
    * 文 件 名：        PerformanceDtlTarImplModel.cs
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

namespace GJX3.JX.Model.Domain
{
    /// <summary>
    /// PerformanceDtlTarImpl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PerformanceDtlTarImplModel : EntityBase<PerformanceDtlTarImplModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public PerformanceDtlTarImplModel()
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
		/// 主表id
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 项目id
		/// </summary>
		[DataMember]
		public virtual System.Int64 XmPhid
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
		/// 指标类型代码
		/// </summary>
		[DataMember]
		public virtual System.String FTargetTypeCode
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

		/// <summary>
		/// 自评完成值
		/// </summary>
		[DataMember]
		public virtual System.String FCompletionValue
		{
			get;
			set;
		}

		/// <summary>
		/// 自评得分
		/// </summary>
		[DataMember]
		public virtual System.Decimal FScore
		{
			get;
			set;
		}

        /// <summary>
		/// 抽评完成值
		/// </summary>
		[DataMember]
        public virtual System.String FCheckCompletionValue
        {
            get;
            set;
        }

        /// <summary>
        /// 抽评得分
        /// </summary>
        [DataMember]
        public virtual System.Decimal FCheckScore
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方评价完成值
        /// </summary>
        [DataMember]
        public virtual System.String FThirdCheckCompletionValue
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方评价得分
        /// </summary>
        [DataMember]
        public virtual System.Decimal FThirdCheckScore
        {
            get;
            set;
        }

        #region//为前端显示做准备

        /// <summary>
        /// 指标类别名称
        /// </summary>
        [DataMember]
        public virtual System.String FTargetClassName
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

        #endregion

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