#region Summary
/**************************************************************************************
    * 类 名 称：        PerformEvalTypeModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        PerformEvalTypeModel.cs
    * 创建时间：        2018/10/16 
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
    /// PerformEvalType实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class PerformEvalTypeModel : EntityBase<PerformEvalTypeModel>
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
		/// 代码
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
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String FRemark
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
    }

}