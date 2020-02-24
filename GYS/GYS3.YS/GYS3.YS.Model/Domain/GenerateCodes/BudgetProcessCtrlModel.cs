#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetProcessCtrlModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        BudgetProcessCtrlModel.cs
    * 创建时间：        2018/9/10 
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

namespace GYS3.YS.Model.Domain
{
    /// <summary>
    /// BudgetProcessCtrl实体定义类
    /// </summary>
    [Serializable]
	[DataContract(Namespace = "")]
    public partial class BudgetProcessCtrlModel : EntityBase<BudgetProcessCtrlModel>
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
		/// 组织代码
		/// </summary>
		[DataMember]
		public virtual System.String FOcode
		{
			get;
			set;
		}

		/// <summary>
		/// 组织名称
		/// </summary>
		[DataMember]
		public virtual System.String FOname
		{
			get;
			set;
		}

		/// <summary>
		/// 部门代码
		/// </summary>
		[DataMember]
		public virtual System.String FDeptCode
		{
			get;
			set;
		}

		/// <summary>
		/// 部门名称
		/// </summary>
		[DataMember]
		public virtual System.String FDeptName
		{
			get;
			set;
		}

		/// <summary>
		/// 按部门设置
		/// </summary>
		[DataMember]
		public virtual System.Int32 FSetByDept
		{
			get;
			set;
		}

		/// <summary>
		/// 进度状态（1-年初申报，2-初报完成，3-年中调整，4-调整完成）
		/// </summary>
		[DataMember]
		public virtual System.String FProcessStatus
		{
			get;
			set;
		}

        /// <summary>
		/// 年初申报阶段起始时间
		/// </summary>
		[DataMember]
        public virtual System.DateTime StartDt
        {
            get;
            set;
        }

        /// <summary>
		/// 年初申报阶段截止时间
		/// </summary>
		[DataMember]
        public virtual System.DateTime EndDt
        {
            get;
            set;
        }

        /// <summary>
		/// 年初申报阶段起始时间
		/// </summary>
		[DataMember]
        public virtual System.DateTime StartDt2
        {
            get;
            set;
        }

        /// <summary>
		/// 年初申报阶段截止时间
		/// </summary>
		[DataMember]
        public virtual System.DateTime EndDt2
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

        /// <summary>
		/// 备用1
		/// </summary>
		[DataMember]
        public virtual System.String FDefstr1
        {
            get;
            set;
        }

        /// <summary>
		/// 备用2
		/// </summary>
		[DataMember]
        public virtual System.String FDefstr2
        {
            get;
            set;
        }

    }

}