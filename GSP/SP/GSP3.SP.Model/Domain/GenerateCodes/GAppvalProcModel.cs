#region Summary
/**************************************************************************************
    * 类 名 称：        GAppvalProcModel
    * 命名空间：        GSP3.SP.Model.Domain
    * 文 件 名：        GAppvalProcModel.cs
    * 创建时间：        2019/5/20 
    * 作    者：        李明    
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

namespace GSP3.SP.Model.Domain
{
    /// <summary>
    /// 审批流程表
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class GAppvalProcModel : EntityBase<GAppvalProcModel>
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
		/// 流程代码
		/// </summary>
		[DataMember]
		public virtual System.String FCode
		{
			get;
			set;
		}

		/// <summary>
		/// 流程名称
		/// </summary>
		[DataMember]
		public virtual System.String FName
		{
			get;
			set;
		}

		/// <summary>
		/// 关联单据类型
		/// </summary>
		[DataMember]
		public virtual System.String FBilltype
		{
			get;
			set;
		}

		/// <summary>
		/// 是否启用
		/// </summary>
		[DataMember]
		public virtual System.Byte FEnable
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
        /// 审批类型id
        /// </summary>
        [DataMember]
        public virtual System.Int64 SPLXPhid
        {
            get;
            set;
        }

        /// <summary>
        /// 审批类型编码
        /// </summary>
        [DataMember]
        public virtual System.String SPLXCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否内置
        /// </summary>
        [DataMember]
        public virtual System.Byte IsSystem
        {
            get;
            set;
        }

    }

}