#region Summary
/**************************************************************************************
    * 类 名 称：        QTSysSetModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QTSysSetModel.cs
    * 创建时间：        2019/6/3 
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
    /// QTSysSet实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QTSysSetModel : EntityBase<QTSysSetModel>
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
		/// 字典类型代码("splx":审批类型的数据)
		/// </summary>
		[DataMember]
		public virtual System.String DicType
		{
			get;
			set;
		}

		/// <summary>
		/// 字典类型名称
		/// </summary>
		[DataMember]
		public virtual System.String DicName
		{
			get;
			set;
		}

		/// <summary>
		/// 编码
		/// </summary>
		[DataMember]
		public virtual System.String TypeCode
		{
			get;
			set;
		}

		/// <summary>
		/// 名称
		/// </summary>
		[DataMember]
		public virtual System.String TypeName
		{
			get;
			set;
		}

		/// <summary>
		/// 值
		/// </summary>
		[DataMember]
		public virtual System.String Value
		{
			get;
			set;
		}

		/// <summary>
		/// 是否启用
		/// </summary>
		[DataMember]
		public virtual System.Byte Isactive
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
		/// 排序
		/// </summary>
		[DataMember]
		public virtual System.Int32 Sortcode
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
		/// 是否系统内置
		/// </summary>
		[DataMember]
        public virtual System.Byte Issystem
        {
            get;
            set;
        }


        /// <summary>
        /// 审批过得数量
        /// </summary>
        [DataMember]
        public virtual System.Int32 YNum
        {
            get;
            set;
        }

        /// <summary>
        /// 审批没过得数量
        /// </summary>
        [DataMember]
        public virtual System.Int32 NNum
        {
            get;
            set;
        }

        /// <summary>
        /// 组织集合
        /// </summary>
        [DataMember]
        public virtual List<OrganizeModel> OrgList
        {
            get;
            set;
        }

        /// <summary>
        /// 组织主键集合
        /// </summary>
        [DataMember]
        public virtual List<long> PhidList
        {
            get;
            set;
        }
    }

}