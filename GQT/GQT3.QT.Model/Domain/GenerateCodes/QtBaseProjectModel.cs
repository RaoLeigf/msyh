#region Summary
/**************************************************************************************
    * 类 名 称：        QtBaseProjectModel
    * 命名空间：        GQT3.QT.Model.Domain
    * 文 件 名：        QtBaseProjectModel.cs
    * 创建时间：        2018/11/23 
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
    /// QtBaseProject实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class QtBaseProjectModel : EntityBase<QtBaseProjectModel>
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
		/// 组织phid
		/// </summary>
		[DataMember]
		public virtual System.Int64 Fphid
		{
			get;
			set;
		}

		/// <summary>
		/// 预算科目代码
		/// </summary>
		[DataMember]
		public virtual System.String FKmdm
		{
			get;
			set;
		}

		/// <summary>
		/// 科目名称
		/// </summary>
		[DataMember]
		public virtual System.String Fkmmc
		{
			get;
			set;
		}

		/// <summary>
		/// 科目类别
		/// </summary>
		[DataMember]
		public virtual System.String FKMLB
		{
			get;
			set;
		}

		/// <summary>
		/// 项目编码
		/// </summary>
		[DataMember]
		public virtual System.String FProjCode
		{
			get;
			set;
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		[DataMember]
		public virtual System.String FProjName
		{
			get;
			set;
		}

		/// <summary>
		/// 填报部门
		/// </summary>
		[DataMember]
		public virtual System.String FFillDept
		{
			get;
			set;
		}

        /// <summary>
		/// 填报部门名称
		/// </summary>
		[DataMember]
        public virtual System.String FFillDept_Name
        {
            get;
            set;
        }

        

        /// <summary>
        /// 单位代码
        /// </summary>
        [DataMember]
		public virtual System.String FDwdm
		{
			get;
			set;
		}

		/// <summary>
		/// 单位名称
		/// </summary>
		[DataMember]
		public virtual System.String FDwmc
		{
			get;
			set;
		}

        /// <summary>
		/// 填报进度
		/// </summary>
		[DataMember]
        public virtual System.String Tbjd
        {
            get;
            set;
        }

        /// <summary>
		/// 单据类型(c:年初；z:年中)
		/// </summary>
		[DataMember]
        public virtual System.String FType
        {
            get;
            set;
        }

        /// <summary>
		/// 年度
		/// </summary>
		[DataMember]
        public virtual System.String FYear
        {
            get;
            set;
        }
    }

}