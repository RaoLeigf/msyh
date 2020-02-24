#region Summary
/**************************************************************************************
    * 类 名 称：        BankAccountModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        BankAccountModel.cs
    * 创建时间：        2019/5/28 
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
    /// BankAccount实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class BankAccountModel : EntityBase<BankAccountModel>
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
		/// 组织主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 OrgPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 组织编码
		/// </summary>
		[DataMember]
		public virtual System.String OrgCode
		{
			get;
			set;
		}

		/// <summary>
		/// 银行账户名称
		/// </summary>
		[DataMember]
		public virtual System.String FBankname
		{
			get;
			set;
		}

		/// <summary>
		/// 银行账号
		/// </summary>
		[DataMember]
		public virtual System.String FAccount
		{
			get;
			set;
		}

		/// <summary>
		/// 银行行号
		/// </summary>
		[DataMember]
		public virtual System.String FBankcode
		{
			get;
			set;
		}

		/// <summary>
		/// 描述
		/// </summary>
		[DataMember]
		public virtual System.String FDescribe
		{
			get;
			set;
		}

		/// <summary>
		/// 所在城市
		/// </summary>
		[DataMember]
		public virtual System.String FCity
		{
			get;
			set;
		}

		/// <summary>
		/// 是否启用
		/// </summary>
		[DataMember]
		public virtual System.Int32 FLifecycle
		{
			get;
			set;
		}

        /// <summary>
		/// 开户行
		/// </summary>
		[DataMember]
        public virtual System.String FOpenAccount
        {
            get;
            set;
        }

        /// <summary>
		/// 组织名称
		/// </summary>
		[DataMember]
        public virtual System.String OrgName
        {
            get;
            set;
        }
        /// <summary>
        /// 银行名称
        /// </summary>
        [DataMember]
        public virtual System.String BankName
        {
            get;
            set;
        }

    }

}