#region Summary
/**************************************************************************************
    * 类 名 称：        QTControlSetModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTControlSetModel.cs
    * 创建时间：        2019/4/3 
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
    /// QTControlSet实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTControlSetModel : EntityBase<QTControlSetModel>
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
		/// 控制对象
		/// </summary>
		[DataMember]
		public virtual System.String ControlObject
		{
			get;
			set;
		}

		/// <summary>
		/// 控制组织名称
		/// </summary>
		[DataMember]
		public virtual System.String ControlOrgName
		{
			get;
			set;
		}

		/// <summary>
		/// 是否控制
		/// </summary>
		[DataMember]
		public virtual System.String ControlOrNot
		{
			get;
			set;
		}

		/// <summary>
		/// 功能点标志
		/// </summary>
		[DataMember]
		public virtual System.String BZ
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR3
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR4
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR5
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR6
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR7
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR8
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR9
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String DEFSTR10
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT1
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT2
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int32 DEFINT3
		{
			get;
			set;
		}

        /// <summary>
        /// 启用日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? BEGINFDATE
        {
            get;
            set;
        }

    }

}