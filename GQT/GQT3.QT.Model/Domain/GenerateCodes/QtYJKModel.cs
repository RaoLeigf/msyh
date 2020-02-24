#region Summary
/**************************************************************************************
    * 类 名 称：        QtYJKModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtYJKModel.cs
    * 创建时间：        2019/4/15 
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
    /// QtYJK实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtYJKModel : EntityBase<QtYJKModel>
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
		/// 年度
		/// </summary>
		[DataMember]
		public virtual System.String Year
		{
			get;
			set;
		}

		/// <summary>
		/// 意见
		/// </summary>
		[DataMember]
		public virtual System.String Text
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
		/// 使用次数
		/// </summary>
		[DataMember]
		public virtual System.Int32 Usenum
		{
			get;
			set;
		}

		/// <summary>
		/// 操作员代码
		/// </summary>
		[DataMember]
		public virtual System.String Usercode
		{
			get;
			set;
		}

		/// <summary>
		/// 部门
		/// </summary>
		[DataMember]
		public virtual System.String Dept
		{
			get;
			set;
		}

		/// <summary>
		/// 组织
		/// </summary>
		[DataMember]
		public virtual System.String Org
		{
			get;
			set;
		}

	}

}