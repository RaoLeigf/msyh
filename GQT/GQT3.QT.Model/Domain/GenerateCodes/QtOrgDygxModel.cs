#region Summary
/**************************************************************************************
    * 类 名 称：        QtOrgDygxModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtOrgDygxModel.cs
    * 创建时间：        2019/2/14 
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
    /// QtOrgDygx实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtOrgDygxModel : EntityBase<QtOrgDygxModel>
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
		/// 项目库组织代码
		/// </summary>
		[DataMember]
		public virtual System.String Xmorg
		{
			get;
			set;
		}

		/// <summary>
		/// 项目库组织代码名称
		/// </summary>
		[DataMember]
		public virtual System.String Xmorg_EXName
		{
			get;
			set;
		}

		/// <summary>
		/// 老g6h组织代码
		/// </summary>
		[DataMember]
		public virtual System.String Oldorg
		{
			get;
			set;
		}

        /// <summary>
		/// 老g6h部门代码
		/// </summary>
		[DataMember]
        public virtual System.String Oldbudget
        {
            get;
            set;
        }


        /// <summary>
        /// 组织 部门区分（Y组织，N部门）
        /// </summary>
        [DataMember]
        public virtual System.String IfCorp
        {
            get;
            set;
        }

        /// <summary>
		/// 部门归属组织
		/// </summary>
		[DataMember]
        public virtual System.Int64 ParentOrgId
        {
            get;
            set;
        }

    }

}