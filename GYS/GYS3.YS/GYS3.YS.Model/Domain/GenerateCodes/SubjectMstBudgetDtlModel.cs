#region Summary
/**************************************************************************************
    * 类 名 称：        SubjectMstBudgetDtlModel
    * 命名空间：        GYS3.YS.Model.Domain
    * 文 件 名：        SubjectMstBudgetDtlModel.cs
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
    /// SubjectMstBudgetDtl实体定义类
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public partial class SubjectMstBudgetDtlModel : EntityBase<SubjectMstBudgetDtlModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SubjectMstBudgetDtlModel()
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
        /// 项目名称
        /// </summary>
        [DataMember]
        public virtual System.String FName
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
        /// 明细项目代码
        /// </summary>
        [DataMember]
        public virtual System.String FDtlCode
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        [DataMember]
        public virtual System.String FMeasUnit
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public virtual System.Decimal Fqty
        {
            get;
            set;
        }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember]
        public virtual System.Decimal FPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public virtual System.Decimal FAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 支出渠道
        /// </summary>
        [DataMember]
        public virtual System.String FExpensesChannel
        {
            get;
            set;
        }

        /// <summary>
		/// 支出渠道名称
		/// </summary>
		[DataMember]
        public virtual System.String FExpensesChannel_EXName
        {
            get;
            set;
        }

        /// <summary>
        /// 测算过程及其他需要说明的事项
        /// </summary>
        [DataMember]
        public virtual System.String FOtherInstructions
        {
            get;
            set;
        }

        /// <summary>
        /// 明细项目名称
        /// </summary>
        [DataMember]
        public virtual System.String FDtlName
        {
            get;
            set;
        }


        /// <summary>
        /// 分部门填报
        /// </summary>
        [DataMember]
        public virtual System.String FBudgetset
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