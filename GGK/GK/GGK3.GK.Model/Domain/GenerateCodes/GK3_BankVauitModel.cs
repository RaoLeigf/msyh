#region Summary
/**************************************************************************************
    * 类 名 称：        GK3_BankVauitModel
    * 命名空间：        GGK3.GK.Model.Domain
    * 文 件 名：        GK3_BankVauitModel.cs
    * 创建时间：        2019/11/18 
    * 作    者：        张宇    
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

namespace GGK3.GK.Model.Domain
{
    /// <summary>
    /// GK3_BankVauit实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GK3_BankVauitModel : EntityBase<GK3_BankVauitModel>
    {
		/// <summary>
		/// 主键ID
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 银行名称
		/// </summary>
		[DataMember]
		public virtual System.String Name
		{
			get;
			set;
		}

		/// <summary>
		/// 银行网络地址
		/// </summary>
		[DataMember]
		public virtual System.String Address
		{
			get;
			set;
		}

		/// <summary>
		/// 创建时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? Createtime
		{
			get;
			set;
		}

		/// <summary>
		/// 修改人
		/// </summary>
		[DataMember]
		public virtual System.Int64 Modifier
		{
			get;
			set;
		}

		/// <summary>
		/// 修改时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? Modifiertime
		{
			get;
			set;
		}
        /// <summary>
        /// 组织ID
        /// </summary>
        [DataMember]
        public virtual System.Int64 OrgId
        {
            get;
            set;
        }
    }

}