#region Summary
/**************************************************************************************
    * 类 名 称：        SubjectMstModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        SubjectMstModel.cs
    * 创建时间：        2018/11/26 
    * 作    者：        董泉伟    
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
    /// SubjectMst实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class SubjectMstModel : EntityBase<SubjectMstModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SubjectMstModel()
        {
            List<PropertyColumnMapperInfo> list = new List<PropertyColumnMapperInfo>();

            PropertyColumnMapperInfo info = new PropertyColumnMapperInfo();
            info.PropertyName = "Mstphid";
            info.ColumnName = "mst_phid";
            info.PropertyType = EnumPropertyType.Long;
            info.IsPrimary = false;
            list.Add(info);

            ForeignKeys = list;//设置外键字段属性
        }

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
        /// 主表主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 Mstphid
        {
            get;
            set;
        }

        /// <summary>
        /// 项目年度
        /// </summary>
        [DataMember]
        public virtual System.String FYear
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
        /// 申报单位
        /// </summary>
        [DataMember]
        public virtual System.String FDeclarationUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 申报单位名称
        /// </summary>
        [DataMember]
        public virtual System.String FDeclarationUnit_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 申报部门
        /// </summary>
        [DataMember]
        public virtual System.String FDeclarationDept
        {
            get;
            set;
        }

        /// <summary>
        /// 预算部门
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetDept
        {
            get;
            set;
        }

        /// <summary>
        /// 预算部门名称
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetDept_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 申报日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FDateofDeclaration
        {
            get;
            set;
        }

        /// <summary>
        /// 申报人
        /// </summary>
        [DataMember]
        public virtual System.String FDeclarer
        {
            get;
            set;
        }

        /// <summary>
        /// 项目金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FProjAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        [DataMember]
        public virtual System.String FApproveStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 结转标志
        /// </summary>
        [DataMember]
        public virtual System.Int32 FCarryOver
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember]
        public virtual System.String FBillNo
        {
            get;
            set;
        }

        /// <summary>
        /// 审批人
        /// </summary>
        [DataMember]
        public virtual System.Int64 FApprover
        {
            get;
            set;
        }

        /// <summary>
        /// 审批日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FApproveDate
        {
            get;
            set;
        }

        /// <summary>
        /// 科目编码
        /// </summary>
        [DataMember]
        public virtual System.String FSubjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 科目名称
        /// </summary>
        [DataMember]
        public virtual System.String FSubjectName
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
        public virtual System.String FFillDept_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 分支部门审批
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetApproveState
        {
            get;
            set;
        }

        /// <summary>
        /// 分支部门审批人
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetApprover
        {
            get;
            set;
        }

        /// <summary>
        /// 科目属性
        /// </summary>
        [DataMember]
        public virtual System.String FKMLB
        {
            get;
            set;
        }

        /// <summary>
		/// 调整金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountEdit
        {
            get;
            set;
        }

        /// <summary>
		/// 调整后金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FAmountAfterEdit
        {
            get;
            set;
        }
    }

}