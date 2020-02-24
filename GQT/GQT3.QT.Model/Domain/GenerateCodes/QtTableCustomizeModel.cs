#region Summary
/**************************************************************************************
    * 类 名 称：        QtTableCustomizeModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtTableCustomizeModel.cs
    * 创建时间：        2019/11/26 
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
    /// QtTableCustomize实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtTableCustomizeModel : EntityBase<QtTableCustomizeModel>
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
		/// 用户id
		/// </summary>
		[DataMember]
		public virtual System.Int64 UserId
		{
			get;
			set;
		}

		/// <summary>
		/// 用户code
		/// </summary>
		[DataMember]
		public virtual System.String UserCode
		{
			get;
			set;
		}

		/// <summary>
		/// 表编码
		/// </summary>
		[DataMember]
		public virtual System.String TableCode
		{
			get;
			set;
		}

		/// <summary>
		/// 表名
		/// </summary>
		[DataMember]
		public virtual System.String TableName
		{
			get;
			set;
		}

		/// <summary>
		/// 列编码
		/// </summary>
		[DataMember]
		public virtual System.String ColumnCode
		{
			get;
			set;
		}

		/// <summary>
		/// 列名
		/// </summary>
		[DataMember]
		public virtual System.String ColumnName
		{
			get;
			set;
		}

		/// <summary>
		/// 是否显示(0-显示,1-不显示)
		/// </summary>
		[DataMember]
		public virtual System.Byte EnabledMark
		{
			get;
			set;
		}

		/// <summary>
		/// 列顺序
		/// </summary>
		[DataMember]
		public virtual System.Int32 ColumnSort
		{
			get;
			set;
		}

		/// <summary>
		/// 说明
		/// </summary>
		[DataMember]
		public virtual System.String Description
		{
			get;
			set;
		}

		/// <summary>
		/// 内容排序
		/// </summary>
		[DataMember]
		public virtual System.Int32 ContentSort
		{
			get;
			set;
		}


        /// <summary>
        /// 是否是数字(0-不是, 1-是)
        /// </summary>
        [DataMember]
        public virtual System.Byte IsNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 内容显示格式
        /// </summary>
        [DataMember]
        public virtual System.String Align
        {
            get;
            set;
        }

        /// <summary>
        /// 内容宽度
        /// </summary>
        [DataMember]
        public virtual System.Int32 Width
        {
            get;
            set;
        }

        /// <summary>
        /// 内容类型（0-公用，1-项目的，2-明细的）
        /// </summary>
        [DataMember]
        public virtual System.Int32 ContentType
        {
            get;
            set;
        }
    }

}