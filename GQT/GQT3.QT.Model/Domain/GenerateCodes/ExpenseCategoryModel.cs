#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseCategoryModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        ExpenseCategoryModel.cs
    * 创建时间：        2018/8/30 
    * 作    者：        夏华军    
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
    /// ExpenseCategory实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class ExpenseCategoryModel : EntityBase<ExpenseCategoryModel>
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
		public virtual System.String Dm
		{
			get;
			set;
		}

		/// <summary>
		/// 名称
		/// </summary>
		[DataMember]
		public virtual System.String Mc
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String Bz
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