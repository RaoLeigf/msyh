#region Summary
/**************************************************************************************
    * 类 名 称：        QTIfApproveModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTIfApproveModel.cs
    * 创建时间：        2019/8/22 
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

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// QTIfApprove实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTIfApproveModel : EntityBase<QTIfApproveModel>
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
		/// 编码
		/// </summary>
		[DataMember]
		public virtual System.String FCode
		{
			get;
			set;
		}

		/// <summary>
		/// 名称
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

		/// <summary>
		/// 是否启用
		/// </summary>
		[DataMember]
		public virtual System.Byte FIfuse
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String Bz
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
        /// 组织集合
        /// </summary>
        [DataMember]
        public virtual List<OrganizeModel> OrgList
        {
            get;
            set;
        }

        /// <summary>
        /// 组织主键集合
        /// </summary>
        [DataMember]
        public virtual List<long> PhidList
        {
            get;
            set;
        }
    }

}