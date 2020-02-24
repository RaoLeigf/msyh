#region Summary
/**************************************************************************************
    * 类 名 称：        YsAccountMstModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        YsAccountMstModel.cs
    * 创建时间：        2019/9/23 
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
    /// YsAccountMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class YsAccountMstModel : EntityBase<YsAccountMstModel>
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
		/// 组织id
		/// </summary>
		[DataMember]
		public virtual System.Int64 Orgid
		{
			get;
			set;
		}

		/// <summary>
		/// 组织code
		/// </summary>
		[DataMember]
		public virtual System.String Orgcode
		{
			get;
			set;
		}

		/// <summary>
		/// 年份
		/// </summary>
		[DataMember]
		public virtual System.String Uyear
		{
			get;
			set;
		}

        /// <summary>
		/// 年初保存标志
		/// </summary>
		[DataMember]
        public virtual System.Int32 SaveStart
        {
            get;
            set;
        }

        /// <summary>
        /// 年中保存标志
        /// </summary>
        [DataMember]
        public virtual System.Int32 SaveMiddle
        {
            get;
            set;
        }

        /// <summary>
        /// 年末保存标志
        /// </summary>
        [DataMember]
        public virtual System.Int32 SaveEnd
        {
            get;
            set;
        }

        /// <summary>
        /// 年初上报标志
        /// </summary>
        [DataMember]
		public virtual System.Int32 VerifyStart
		{
			get;
			set;
		}

		/// <summary>
		/// 年初上报标志
		/// </summary>
		[DataMember]
		public virtual System.DateTime? VerifyStartTime
		{
			get;
			set;
		}

		/// <summary>
		/// 年中调整标志
		/// </summary>
		[DataMember]
		public virtual System.Int32 VerifyMiddle
		{
			get;
			set;
		}

		/// <summary>
		/// 年中调整时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? VerifyMiddleTime
		{
			get;
			set;
		}

		/// <summary>
		/// 年末决算标志
		/// </summary>
		[DataMember]
		public virtual System.Int32 VerifyEnd
		{
			get;
			set;
		}

		/// <summary>
		/// 年末决算时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? VerifyEndTime
		{
			get;
			set;
		}

		/// <summary>
		/// 年初说明书
		/// </summary>
		[DataMember]
		public virtual System.String DescriptionStart
		{
			get;
			set;
		}

		/// <summary>
		/// 年中说明书
		/// </summary>
		[DataMember]
		public virtual System.String DescriptionMiddle
		{
			get;
			set;
		}

		/// <summary>
		/// 年末说明书
		/// </summary>
		[DataMember]
		public virtual System.String DescriptionEnd
		{
			get;
			set;
		}

		/// <summary>
		/// 年初上报人姓名
		/// </summary>
		[DataMember]
		public virtual System.String StartReportMan
		{
			get;
			set;
		}

		/// <summary>
		/// 年初上报时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? StartReportDate
		{
			get;
			set;
		}

		/// <summary>
		/// 年中上报人名称
		/// </summary>
		[DataMember]
		public virtual System.String MiddleReportMan
		{
			get;
			set;
		}

		/// <summary>
		/// 年中上报时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? MiddleReportDate
		{
			get;
			set;
		}

		/// <summary>
		/// 年末上报人名称
		/// </summary>
		[DataMember]
		public virtual System.String EndReportMan
		{
			get;
			set;
		}

		/// <summary>
		/// 年末上报时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? EndReportDate
		{
			get;
			set;
		}

        /// <summary>
        /// 子表集合
        /// </summary>
        [DataMember]
        public virtual IList<YsAccountModel> YsAccounts
        {
            get;
            set;
        }
    }

}