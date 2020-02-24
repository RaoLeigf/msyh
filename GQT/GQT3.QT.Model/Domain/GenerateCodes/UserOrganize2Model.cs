#region Summary
/**************************************************************************************
    * 类 名 称：        UserOrgModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        UserOrgModel.cs
    * 创建时间：        2018/9/19 
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

using GQT3.QT.Model.Enums;

namespace GQT3.QT.Model.Domain
{
    /// <summary>
    /// UserOrg实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class UserOrganize2Model : EntityBase<UserOrganize2Model>
    {
		/// <summary>
		/// phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// userid
		/// </summary>
		[DataMember]
		public virtual System.Int64 UserId
		{
			get;
			set;
		}

		/// <summary>
		/// orgid
		/// </summary>
		[DataMember]
		public virtual System.Int64 OrgId
		{
			get;
			set;
		}

		/// <summary>
		/// 关联组织类型
		/// </summary>
		[DataMember]
		public virtual EnumOrgInnerType InnerType
		{
			get;
			set;
		}

	}

}