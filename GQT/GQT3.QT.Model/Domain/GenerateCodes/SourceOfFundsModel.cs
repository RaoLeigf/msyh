#region Summary
/**************************************************************************************
    * 类 名 称：        SourceOfFundsModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        SourceOfFundsModel.cs
    * 创建时间：        2018/9/3 
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
    /// SourceOfFunds实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class SourceOfFundsModel : EntityBase<SourceOfFundsModel>
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
		public virtual System.String DM
		{
			get;
			set;
		}

		/// <summary>
		/// 名称
		/// </summary>
		[DataMember]
		public virtual System.String MC
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
		/// 是否末级
		/// </summary>
		[DataMember]
        public virtual System.String isend
        {
            get;
            set;
        }

    }

}