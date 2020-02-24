#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectMstModel
    * 命名空间：        GXM3.XM.Model.Domain
    * 文 件 名：        ProjectMstModel.cs
    * 创建时间：        2018/9/4 
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

using GXM3.XM.Model.Enums;

namespace GXM3.XM.Model.Domain
{
    /// <summary>
    /// ProjectMst实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectMstApproveModel : EntityBase<ProjectMstApproveModel>
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
        /// piid
        /// </summary>
        [DataMember]
		public virtual System.String Instcode
        {
			get;
			set;
		}

        /// <summary>
        /// piid
        /// </summary>
        [DataMember]
        public virtual System.String Bizid
        {
            get;
            set;
        }

        /// <summary>
        /// piid
        /// </summary>
        [DataMember]
        public virtual System.String Pk1
        {
            get;
            set;
        }

        /// <summary>
        /// 日期
        /// </summary>
        [DataMember]
		public virtual System.DateTime? Startdt
        {
			get;
			set;
		}

		
    }

}