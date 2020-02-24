#region Summary
/**************************************************************************************
    * 类 名 称：        JxTrackingModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        JxTrackingModel.cs
    * 创建时间：        2019/10/17 
    * 作    者：        刘杭    
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
    /// JxTracking实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class JxTrackingModel : EntityBase<JxTrackingModel>
    {
		/// <summary>
		/// 构造函数
		/// </summary>
		public JxTrackingModel()
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
		/// 外键（预算主表时间）
		/// </summary>
		[DataMember]
		public virtual System.Int64 MstPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 上报时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FTime
		{
			get;
			set;
		}

		/// <summary>
		/// 文案
		/// </summary>
		[DataMember]
		public virtual System.String FText
		{
			get;
			set;
		}

		/// <summary>
		/// 申报组织
		/// </summary>
		[DataMember]
		public virtual System.String FDeclarationUnit
		{
			get;
			set;
		}

        /// <summary>
		/// 申报组织名称
		/// </summary>
		[DataMember]
        public virtual System.String FDeclarationUnit_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember]
		public virtual System.String FProjCode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjName
		{
			get;
			set;
		}

		/// <summary>
		/// 项目金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FProjAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 执行金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FActualAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 结余金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal FBalanceAmount
		{
			get;
			set;
		}

		/// <summary>
		/// 执行率
		/// </summary>
		[DataMember]
		public virtual System.Decimal FImplRate
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}

	}

}