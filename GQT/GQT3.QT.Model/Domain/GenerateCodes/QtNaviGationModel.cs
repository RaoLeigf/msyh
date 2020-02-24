#region Summary
/**************************************************************************************
    * 类 名 称：        QtNaviGationModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtNaviGationModel.cs
    * 创建时间：        2019/11/14 
    * 作    者：        张宇    
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
    /// QtNaviGation实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtNaviGationModel : EntityBase<QtNaviGationModel>
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
		/// 按钮code
		/// </summary>
		[DataMember]
		public virtual System.String Buttoncode
		{
			get;
			set;
		}

		/// <summary>
		/// 排序值
		/// </summary>
		[DataMember]
		public virtual System.Int32 Sortvalue
		{
			get;
			set;
		}
        /// <summary>
		/// 是否隐藏
		/// </summary>
		[DataMember]
        public virtual System.Int32 Invisible
        {
            get;
            set;
        }

        /// <summary>
        /// 操作员
        /// </summary>
        [DataMember]
		public virtual System.Int64 Operator
		{
			get;
			set;
		}

		/// <summary>
		/// 创建时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? Createtime
		{
			get;
			set;
		}

		/// <summary>
		/// 修改人
		/// </summary>
		[DataMember]
		public virtual System.Int64 Modifier
		{
			get;
			set;
		}

		/// <summary>
		/// 修改时间
		/// </summary>
		[DataMember]
		public virtual System.DateTime? Modifiertime
		{
			get;
			set;
		}
        /// <summary>
		/// Name
		/// </summary>
		[DataMember]
        public virtual System.String Name
        {
            get;
            set;
        }
        /// <summary>
		/// Menu
		/// </summary>
		[DataMember]
        public virtual System.String Menu
        {
            get;
            set;
        }
        /// <summary>
		/// Srcs
		/// </summary>
		[DataMember]
        public virtual System.String Srcs
        {
            get;
            set;
        }
    }

}