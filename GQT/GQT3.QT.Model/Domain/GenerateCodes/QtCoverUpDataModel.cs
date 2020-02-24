#region Summary
/**************************************************************************************
    * 类 名 称：        QtCoverUpDataModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtCoverUpDataModel.cs
    * 创建时间：        2019/10/29 
    * 作    者：        王冠冠    
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
    /// QtCoverUpData实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtCoverUpDataModel : EntityBase<QtCoverUpDataModel>
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
		/// 编码
		/// </summary>
		[DataMember]
		public virtual System.String EnCode
		{
			get;
			set;
		}

		/// <summary>
		/// 名称
		/// </summary>
		[DataMember]
		public virtual System.String Name
		{
			get;
			set;
		}

		/// <summary>
		/// 排序
		/// </summary>
		[DataMember]
		public virtual System.Int32 SortCode
		{
			get;
			set;
		}

		/// <summary>
		/// 是否启用标志
		/// </summary>
		[DataMember]
		public virtual System.Byte EnabledMark
		{
			get;
			set;
		}

		/// <summary>
		/// 备注
		/// </summary>
		[DataMember]
		public virtual System.String Description
		{
			get;
			set;
		}

		/// <summary>
		/// 是否内置标志
		/// </summary>
		[DataMember]
		public virtual System.Byte IsSystem
		{
			get;
			set;
		}

        /// <summary>
        /// 用来区分类型（同一类型用同一个数字：1-汇总，2-申报）
        /// </summary>
        [DataMember]
        public virtual System.Int32 TypeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 类型名称（目前只分汇总和申报）
        /// </summary>
        [DataMember]
        public virtual System.String TypeName
        {
            get;
            set;
        }
    }

}