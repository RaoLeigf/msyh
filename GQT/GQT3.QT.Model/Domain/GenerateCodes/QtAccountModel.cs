#region Summary
/**************************************************************************************
    * 类 名 称：        QtAccountModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtAccountModel.cs
    * 创建时间：        2019/9/18 
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
    /// QtAccount实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtAccountModel : EntityBase<QtAccountModel>
    {
		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.Int64 PhId
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String Dm
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String Mc
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public virtual System.String Bz
		{
			get;
			set;
		}

        /// <summary>
		/// 账套连接串
		/// </summary>
		[DataMember]
        public virtual System.String FConn
        {
            get;
            set;
        }

        /// <summary>
		/// 是否默认账套
		/// </summary>
		[DataMember]
        public virtual System.Byte IsDefault
        {
            get;
            set;
        }
    }

}