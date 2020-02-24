#region Summary
/**************************************************************************************
    * 类 名 称：        QTIndividualInfoModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTIndividualInfoModel.cs
    * 创建时间：        2019/5/14 
    * 作    者：        董泉伟    
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
    /// QTIndividualInfo实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTIndividualInfoModel : EntityBase<QTIndividualInfoModel>
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
		/// 自定义表单主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 IndividualinfoPhid
		{
			get;
			set;
		}

        /// <summary>
		/// 自定义表单主键
		/// </summary>
		[DataMember]
        public virtual System.String IndividualinfoPhid_EXName
        {
            get;
            set;
        }
        

        /// <summary>
        /// 自定义表单名称
        /// </summary>
        [DataMember]
		public virtual System.String IndividualinfoName
		{
			get;
			set;
		}

		/// <summary>
		/// 自定义表单类型
		/// </summary>
		[DataMember]
		public virtual System.String IndividualinfoBustype
		{
			get;
			set;
		}

		/// <summary>
		/// 自定义表单类型名称
		/// </summary>
		[DataMember]
		public virtual System.String IndividualinfoBustypeName
		{
			get;
			set;
		}

		/// <summary>
		/// 金额控制开始金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal IndividualinfoAmount1
		{
			get;
			set;
		}

		/// <summary>
		/// 金额控制结束金额
		/// </summary>
		[DataMember]
		public virtual System.Decimal IndividualinfoAmount2
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String BZ
		{
			get;
			set;
		}

        /// <summary>
        /// 预立项模板名称主键
        /// </summary>
        [DataMember]
		public virtual System.String YLXPhid
        {
			get;
			set;
		}

        /// <summary>
        /// 项目立项模板名称主键
        /// </summary>
        [DataMember]
		public virtual System.String XMLXPhid
        {
			get;
			set;
		}

        /// <summary>
        /// 年中调整模板名称主键
        /// </summary>
        [DataMember]
		public virtual System.String NZTXPhid
        {
			get;
			set;
		}

        /// <summary>
        /// 预立项模板名称
        /// </summary>
        [DataMember]
        public virtual System.String YLXPhid_EXName
        {
            get;
            set;
        }
        /// <summary>
        /// 项目立项模板名称
        /// </summary>
        [DataMember]
        public virtual System.String XMLXPhid_EXName
        {
            get;
            set;
        }
        /// <summary>
        /// 年中调整模板名称
        /// </summary>
        [DataMember]
        public virtual System.String NZTXPhid_EXName
        {
            get;
            set;
        }
        /// <summary>
        /// 备用
        /// </summary>
        [DataMember]
		public virtual System.String DEFSTR4
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR5
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR6
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR7
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR8
		{
			get;
			set;
		}

		/// <summary>
		/// 单位代码(以,分割)
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR9
		{
			get;
			set;
		}

        /// <summary>
        /// 单位名称(以,分割)
        /// </summary>
        [DataMember]
		public virtual System.String DEFSTR10
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT1
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT2
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT3
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Decimal DEFMoney1
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Decimal DEFMoney2
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.Decimal DEFMoney3
		{
			get;
			set;
		}

		/// <summary>
		/// 备用
		/// </summary>
		[DataMember]
		public virtual System.DateTime? BEGINFDATE
		{
			get;
			set;
		}

	}

}