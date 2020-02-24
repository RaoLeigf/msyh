#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetMstModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        BudgetMstModel.cs
    * 创建时间：        2018/8/30 
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

using GYS3.YS.Model.Enums;

namespace GYS3.YS.Model.Domain
{
    /// <summary>
    /// BudgetMst实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class BudgetMstModel : EntityBase<BudgetMstModel>
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
        /// 项目表主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 XmMstPhid
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
        /// 申报部门名称
        /// </summary>
        [DataMember]
        public virtual System.String FDeclarationDept_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目属性
        /// </summary>
        [DataMember]
        public virtual System.String FProjAttr
        {
            get;
            set;
        }

        /// <summary>
        /// 项目属性名称
        /// </summary>
        [DataMember]
        public virtual System.String FProjAttr_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 存续期限
        /// </summary>
        [DataMember]
        public virtual System.String FDuration
        {
            get;
            set;
        }

        /// <summary>
        /// 存续期限名称
        /// </summary>
        [DataMember]
        public virtual System.String FDuration_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 支出类别
        /// </summary>
        [DataMember]
        public virtual System.String FExpenseCategory
        {
            get;
            set;
        }

        /// <summary>
		/// 支出类别名称
		/// </summary>
		[DataMember]
        public virtual System.String FExpenseCategory_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目状态
        /// </summary>
        [DataMember]
        public virtual System.Int32 FProjStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FEndDate
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
        /// 是否绩效评价
        /// </summary>
        [DataMember]
        public virtual EnumYesNo FIfPerformanceAppraisal
        {
            get;
            set;
        }

        /// <summary>
        /// 是否重点评价
        /// </summary>
        [DataMember]
        public virtual EnumYesNo FIfKeyEvaluation
        {
            get;
            set;
        }

        /// <summary>
        /// 会议时间
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FMeetingTime
        {
            get;
            set;
        }

        /// <summary>
        /// 会议纪要编号
        /// </summary>
        [DataMember]
        public virtual System.String FMeetiingSummaryNo
        {
            get;
            set;
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        [DataMember]
        public virtual System.String FType
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
        /// 调整版本号
        /// </summary>
        [DataMember]
        public virtual System.String FVerNo
        {
            get;
            set;
        }

        /// <summary>
        /// 版本标识
        /// </summary>
        [DataMember]
        public virtual System.Int32 FLifeCycle
        {
            get;
            set;
        }

        /// <summary>
        /// 结转标志
        /// </summary>
        [DataMember]
        public virtual EnumCarryOver FCarryOver
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
		/// 审批人
		/// </summary>
		[DataMember]
        public virtual System.Int64 FApprover
        {
            get;
            set;
        }

        /// <summary>
		/// 审批人名称
		/// </summary>
		[DataMember]
        public virtual System.String FApprover_EXName
        {
            get;
            set;
        }
        /// <summary>
        /// 审批时间
        /// </summary>
        [DataMember]
        public virtual System.DateTime? FApproveDate
        {
            get;
            set;
        }
        /// <summary>
		/// 预算金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal FBudgetAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 单据调整判断
        /// </summary>
        [DataMember]
        public virtual System.String FMidYearChange
        {
            get;
            set;
        }


        /// <summary>
		/// 是否集中采购
		/// </summary>
		[DataMember]
        public virtual System.Int32 FIfPurchase
        {
            get;
            set;
        }

        /// <summary>
        /// 绩效项目类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FPerformType
        {
            get;
            set;
        }

        /// <summary>
        /// 绩效项目类型代码名称
        /// </summary>
        [DataMember]
        public virtual System.String FPerformType_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 绩效评价类型代码
        /// </summary>
        [DataMember]
        public virtual System.String FPerformEvalType
        {
            get;
            set;
        }

        /// <summary>
        /// 绩效评价类型代码名称
        /// </summary>
        [DataMember]
        public virtual System.String FPerformEvalType_EXName
        {
            get;
            set;
        }
        /// <summary>
        /// 生成到老G6H记录
        /// </summary>
        [DataMember]
        public virtual System.Int32 FSaveToOldG6h
        {
            get;
            set;
        }

        /// <summary>
		/// 下一审批岗位
		/// </summary>
		[DataMember]
        public virtual System.String FNextApprove
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许预备费抵扣
        /// </summary>
        [DataMember]
        public virtual System.String FBillNO
        {
            get;
            set;
        }

        /// <summary>
		/// 项目已使用金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal UseAmount
        {
            get;
            set;
        }

        /// <summary>
		/// 项目剩余金额
		/// </summary>
		[DataMember]
        public virtual System.Decimal RemainAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 自定义模板主键
        /// </summary>
        [DataMember]
        public virtual System.String FIndividualinfophid
        {
            get;
            set;
        }

        /// <summary>
        /// 执行年度
        /// </summary>
        [DataMember]
        public virtual System.String FGoYear
        {
            get;
            set;
        }

        /// <summary>
        /// 支出类别（1项目支出2基本支出）
        /// </summary>
        [DataMember]
        public virtual System.String FZcType
        {
            get;
            set;
        }

        /// <summary>
        /// 临时项目（1是2否）
        /// </summary>
        [DataMember]
        public virtual EnumYesNo FTemporary
        {
            get;
            set;
        }
        /// <summary>
        /// 项目级别
        /// </summary>
        [DataMember]
        public virtual System.String FLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 项目级别
        /// </summary>
        [DataMember]
        public virtual System.String FLevel_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 年中调整的类型（分4种：与年中调整的审批类型一一对应:01-年中新增，02-年中新增明细，03-年中只调整明细，04-年中调整新增明细）
        /// </summary>
        [DataMember]
        public virtual System.String FAdjustType
        {
            get;
            set;
        }

        #region 绩效list页面显示用  数据库不存在实体列
        /// <summary>
		/// 实际执行数
		/// </summary>
		[DataMember]
        public virtual System.Decimal FActualAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 结余金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FBalanceAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 执行率
        /// </summary>
        [DataMember]
        public virtual System.Decimal FImplRate
        {
            get;
            set;
        }
        #endregion

        #region//用款计划特有-wgg

        /// <summary>
        /// 剩余可编报数
        /// </summary>
        [DataMember]
        public virtual System.Decimal FSurplusAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 实际发生数
        /// </summary>
        [DataMember]
        public virtual System.Decimal FHappenAmount
        {
            get;
            set;
        }


        #endregion


        /// <summary>
        /// 账套
        /// </summary>
        [DataMember]
        public virtual System.String FAccount
        {
            get;
            set;
        }

        /// <summary>
		/// 账套
		/// </summary>
		[DataMember]
        public virtual System.String FAccount_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 虚拟字段 用于项目查询是区分是项目单据还是预算单据(xm为项目单据)
        /// </summary>
        [DataMember]
        public virtual System.String FBillType
        {
            get;
            set;
        }

        /// <summary>
		/// 年结标志
		/// </summary>
		[DataMember]
        public virtual System.String FReference
        {
            get;
            set;
        }
        /// <summary>
		/// 活动地点
		/// </summary>
		[DataMember]
        public virtual System.String FVenue
        {
            get;
            set;
        }
        /// <summary>
		/// 参加对象
		/// </summary>
		[DataMember]
        public virtual System.String FParticipants
        {
            get;
            set;
        }
        /// <summary>
        /// 配置附件数量
        /// </summary>
        /// 
		[DataMember]

        public virtual int UploadFileCount { get; set; }
        /// <summary>
        /// /
        /// </summary>
        [DataMember]

        public virtual List<GQT3.QT.Model.Domain.QtAttachmentModel>  list { get; set; }


        /// <summary>
		/// 绩效结论（虚拟）
		/// </summary>
		[DataMember]
        public virtual System.String JXResult
        {
            get;
            set;
        }

        /// <summary>
        /// 是否能批量审批的标志（1-是，0-否）
        /// </summary>
        [DataMember]
        public virtual System.Int64 BatchPracBz
        {
            get;
            set;
        }

        #region//名生银行相关字段
        /// <summary>
        /// 业务条线主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 FBusinessId
        {
            get;
            set;
        }
        /// <summary>
        /// 业务条线编码
        /// </summary>
        [DataMember]
        public virtual System.String FBusinessCode
        {
            get;
            set;
        }
        /// <summary>
        /// 是否申请补助
        /// </summary>
        [DataMember]
        public virtual System.Byte FIsApply
        {
            get;
            set;
        }
        /// <summary>
        /// 是否集中采购
        /// </summary>
        [DataMember]
        public virtual System.Byte FIsPurchase
        {
            get;
            set;
        }
        /// <summary>
        /// 是否必须签报列支
        /// </summary>
        [DataMember]
        public virtual System.Byte FIsReport
        {
            get;
            set;
        }
        /// <summary>
        /// 是否集体决议
        /// </summary>
        [DataMember]
        public virtual System.Byte FIsResolution
        {
            get;
            set;
        }
        /// <summary>
        /// 是否个人额度分摊
        /// </summary>
        [DataMember]
        public virtual System.Byte FIsShare
        {
            get;
            set;
        }

        /// <summary>
        /// 申报人主键
        /// </summary>
        [DataMember]
        public virtual System.Int64 FDeclarerId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [DataMember]
        public virtual System.Byte FDeleteMark
        {
            get;
            set;
        }
        #region//虚拟字段
        /// <summary>
        /// 业务条线名称
        /// </summary>
        [DataMember]
        public virtual System.String FBusinessName
        {
            get;
            set;
        }
        #endregion
        #endregion
    }

}