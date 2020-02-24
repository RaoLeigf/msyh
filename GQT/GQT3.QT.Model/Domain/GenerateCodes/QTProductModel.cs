#region Summary
/**************************************************************************************
    * 类 名 称：        QTProductModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTProductModel.cs
    * 创建时间：        2018/12/12 
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
    /// QTProduct实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTProductModel : EntityBase<QTProductModel>
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
		/// 产品标识
		/// </summary>
		[DataMember]
		public virtual System.String ProductBZ
		{
			get;
			set;
		}

		/// <summary>
		/// 产品名称
		/// </summary>
		[DataMember]
		public virtual System.String ProductName
		{
			get;
			set;
		}

		/// <summary>
		/// 产品URL
		/// </summary>
		[DataMember]
		public virtual System.String ProductUrl
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
		/// 组织代码
		/// </summary>
		[DataMember]
		public virtual System.String ORGCODE
		{
			get;
			set;
		}

		/// <summary>
		/// 组织主键
		/// </summary>
		[DataMember]
		public virtual System.String ORGID
		{
			get;
			set;
		}

        /// <summary>
		/// 数据库类型
		/// </summary>
		[DataMember]
        public virtual System.String FSqlType
        {
            get;
            set;
        }

        /// <summary>
		/// 服务
		/// </summary>
		[DataMember]
        public virtual System.String FSqlServer
        {
            get;
            set;
        }

        /// <summary>
		/// 数据源
		/// </summary>
		[DataMember]
        public virtual System.String FSqlSource
        {
            get;
            set;
        }

        /// <summary>
		/// 数据库名称
		/// </summary>
		[DataMember]
        public virtual System.String FSqlDataName
        {
            get;
            set;
        }

        /// <summary>
		/// 数据库密码
		/// </summary>
		[DataMember]
        public virtual System.String FSqlDataPwd
        {
            get;
            set;
        }

        /// <summary>
		/// 操作员维护表
		/// </summary>
		[DataMember]
        public virtual System.String FSqlUserTable
        {
            get;
            set;
        }

        /// <summary>
		/// 操作员表里代码字段
		/// </summary>
		[DataMember]
        public virtual System.String FSqlUserTableCode
        {
            get;
            set;
        }

        /// <summary>
		/// 操作员表里密码字段
		/// </summary>
		[DataMember]
        public virtual System.String FSqlUserTablePwd
        {
            get;
            set;
        }

        /// <summary>
        /// DEFSTR1
        /// </summary>
        [DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// DEFSTR2
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// DEFSTR3
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}

		/// <summary>
		/// DEFINT1
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT1
		{
			get;
			set;
		}

		/// <summary>
		/// DEFINT2
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT2
		{
			get;
			set;
		}

		/// <summary>
		/// DEFINT3
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT3
		{
			get;
			set;
		}

	}

}