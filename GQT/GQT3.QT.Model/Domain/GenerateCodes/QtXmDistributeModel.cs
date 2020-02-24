#region Summary
/**************************************************************************************
    * 类 名 称：        QtXmDistributeModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtXmDistributeModel.cs
    * 创建时间：        2020/1/6 
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
    /// QtXmDistribute实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtXmDistributeModel : EntityBase<QtXmDistributeModel>
    {
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
		/// 项目代码
		/// </summary>
		[DataMember]
		public virtual System.String FProjcode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjname
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
		/// 组织代码
		/// </summary>
		[DataMember]
		public virtual System.String Orgcode
		{
			get;
			set;
		}

		/// <summary>
		/// 是否被引用(0未被引用 1已被引用)
		/// </summary>
		[DataMember]
		public virtual System.Byte Isactive
		{
			get;
			set;
		}

		/// <summary>
		/// 分发组织id
		/// </summary>
		[DataMember]
		public virtual System.Int64 Distributeorgid
		{
			get;
			set;
		}

		/// <summary>
		/// 分发操作员id
		/// </summary>
		[DataMember]
		public virtual System.Int64 Distributeuserid
		{
			get;
			set;
		}
        /// <summary>
		/// 业务条线代码
		/// </summary>
		[DataMember]
        public virtual string FBusiness
        {
            get;
            set;
        }

        /// <summary>
		/// 业务条线名称
		/// </summary>
		[DataMember]
        public virtual string FBusiness_EXName
        {
            get;
            set;
        }
    }

}