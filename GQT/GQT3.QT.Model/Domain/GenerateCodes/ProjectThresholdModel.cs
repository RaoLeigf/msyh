#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectThresholdModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        ProjectThresholdModel.cs
    * 创建时间：        2018/10/17 
    * 作    者：        李长敏琛    
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
    /// ProjectThreshold实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ProjectThresholdModel : EntityBase<ProjectThresholdModel>
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
		/// 组织phid
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
		/// 阈值
		/// </summary>
		[DataMember]
		public virtual System.String FThreshold
		{
			get;
			set;
		}
        /// <summary>
        /// 项目类型代码 (用，分割)
        /// </summary>
        public virtual System.String ProjTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 项目类型代码名称 (用，分割)
        /// </summary>
        public virtual System.String ProjTypeName
        {
            get;
            set;
        }

    }

}