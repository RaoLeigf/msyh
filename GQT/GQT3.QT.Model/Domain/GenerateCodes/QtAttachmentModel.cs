#region Summary
/**************************************************************************************
    * 类 名 称：        QtAttachmentModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtAttachmentModel.cs
    * 创建时间：        2019/6/17 
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
    /// QtAttachment实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtAttachmentModel : EntityBase<QtAttachmentModel>
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
		/// 附件对应表名
		/// </summary>
		[DataMember]
		public virtual System.String BTable
		{
			get;
			set;
		}

		/// <summary>
		/// 附件对应的表主键
		/// </summary>
		[DataMember]
		public virtual System.Int64 RelPhid
		{
			get;
			set;
		}

		/// <summary>
		/// 附件名
		/// </summary>
		[DataMember]
		public virtual System.String BName
		{
			get;
			set;
		}

		/// <summary>
		/// 附件类型
		/// </summary>
		[DataMember]
		public virtual System.String BType
		{
			get;
			set;
		}

		/// <summary>
		/// 附件大小
		/// </summary>
		[DataMember]
		public virtual System.Decimal BSize
		{
			get;
			set;
		}

		/// <summary>
		/// 附件2进制
		/// </summary>
		[DataMember]
		public virtual System.Byte[] BFilebody
		{
			get;
			set;
		}

		/// <summary>
		/// 附件地址
		/// </summary>
		[DataMember]
		public virtual System.String BUrlpath
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String BRemark
		{
			get;
			set;
		}

	}

}