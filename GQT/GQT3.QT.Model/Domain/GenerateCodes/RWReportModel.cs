#region Summary
/**************************************************************************************
    * 类 名 称：        RWReportModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        RWReportModel.cs
    * 创建时间：        2018/10/9 
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

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// RWReport实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class RWReportModel : EntityBase<RWReportModel>
    {
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
		/// 所属组织编号
		/// </summary>
		[DataMember]
		public virtual System.String RepCode
		{
			get;
			set;
		}

		/// <summary>
		/// 所属组织名称
		/// </summary>
		[DataMember]
		public virtual System.String RepName
		{
			get;
			set;
		}

		/// <summary>
		/// 类别
		/// </summary>
		[DataMember]
		public virtual System.Int16 RepFormat
		{
			get;
			set;
		}

		/// <summary>
		/// catalog_id
		/// </summary>
		[DataMember]
		public virtual System.Int64 CatalogId
		{
			get;
			set;
		}

		/// <summary>
		/// rep_classid
		/// </summary>
		[DataMember]
		public virtual System.Int64 RepClassid
		{
			get;
			set;
		}

		/// <summary>
		/// rep_type
		/// </summary>
		[DataMember]
		public virtual System.Int16 RepType
		{
			get;
			set;
		}

		/// <summary>
		/// rep_keys
		/// </summary>
		[DataMember]
		public virtual System.String RepKeys
		{
			get;
			set;
		}

		/// <summary>
		/// rep_summary
		/// </summary>
		[DataMember]
		public virtual System.String RepSummary
		{
			get;
			set;
		}

		/// <summary>
		/// 创建者
		/// </summary>
		[DataMember]
		public virtual System.String RepCreater
		{
			get;
			set;
		}

		/// <summary>
		/// 创建日期
		/// </summary>
		[DataMember]
		public virtual System.DateTime? RepCreatedate
		{
			get;
			set;
		}

		/// <summary>
		/// rep_updater
		/// </summary>
		[DataMember]
		public virtual System.String RepUpdater
		{
			get;
			set;
		}

		/// <summary>
		/// rep_updatedate
		/// </summary>
		[DataMember]
		public virtual System.DateTime? RepUpdatedate
		{
			get;
			set;
		}

		/// <summary>
		/// ocode
		/// </summary>
		[DataMember]
		public virtual System.String OCode
		{
			get;
			set;
		}

		/// <summary>
		/// rep_status
		/// </summary>
		[DataMember]
		public virtual System.Int16 RepStatus
		{
			get;
			set;
		}

		/// <summary>
		/// rep_src
		/// </summary>
		[DataMember]
		public virtual System.Int16 RepSrc
		{
			get;
			set;
		}

		/// <summary>
		/// externallink
		/// </summary>
		[DataMember]
		public virtual System.String ExternalLink
		{
			get;
			set;
		}

		/// <summary>
		/// needapp
		/// </summary>
		[DataMember]
		public virtual System.Byte NeedApp
		{
			get;
			set;
		}

		/// <summary>
		/// appman
		/// </summary>
		[DataMember]
		public virtual System.String AppMan
		{
			get;
			set;
		}

		/// <summary>
		/// appdate
		/// </summary>
		[DataMember]
		public virtual System.DateTime? AppDate
		{
			get;
			set;
		}

		/// <summary>
		/// 报表属性
		/// </summary>
		[DataMember]
		public virtual System.Int16 RepGenre
		{
			get;
			set;
		}

		/// <summary>
		/// is_template
		/// </summary>
		[DataMember]
		public virtual System.Int16 IsTemplate
		{
			get;
			set;
		}

		/// <summary>
		/// cycle_type
		/// </summary>
		[DataMember]
		public virtual System.Int16 CycleType
		{
			get;
			set;
		}

		/// <summary>
		/// fill_year
		/// </summary>
		[DataMember]
		public virtual System.String FillYear
		{
			get;
			set;
		}

		/// <summary>
		/// fill_month
		/// </summary>
		[DataMember]
		public virtual System.String FillMonth
		{
			get;
			set;
		}

		/// <summary>
		/// project_code
		/// </summary>
		[DataMember]
		public virtual System.String ProjectCode
		{
			get;
			set;
		}

		/// <summary>
		/// rep_tmpid
		/// </summary>
		[DataMember]
		public virtual System.Int64 RepTmpid
		{
			get;
			set;
		}

		/// <summary>
		/// rep_holder
		/// </summary>
		[DataMember]
		public virtual System.String rep_holder
		{
			get;
			set;
		}

		/// <summary>
		/// plan_status
		/// </summary>
		[DataMember]
		public virtual System.Int32 PlanStatus
		{
			get;
			set;
		}

		/// <summary>
		/// fill_day
		/// </summary>
		[DataMember]
		public virtual System.DateTime? FillDay
		{
			get;
			set;
		}

		/// <summary>
		/// rep_orderid
		/// </summary>
		[DataMember]
		public virtual System.String RepOrderid
		{
			get;
			set;
		}

		/// <summary>
		/// ismobile
		/// </summary>
		[DataMember]
		public virtual System.Int16 ismobile
		{
			get;
			set;
		}

	}

}