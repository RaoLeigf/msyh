using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Events.Implementation
{
    /// <summary>
    /// PB表达式的类型
    /// </summary>
    public enum PbExpressionType
    {
        /// <summary>
        /// 单据合法性检测
        /// </summary>
        BillValidation = 0,
        /// <summary>
        /// 检查字段唯一性（表头）
        /// </summary>
        BillHeadUniqueCheck = 1,
        /// <summary>
        /// 计算定义公式的值并填写到指定列(加减乘除)
        /// </summary>
        ComputeFuncAndFillSpecificCol = 2,
        /// <summary>
        /// 计算定义SQL语句并填写到指定列
        /// </summary>
        ComputeSqlAndFillSpecificCol = 3,
        /// <summary>
        /// 计算定义SQL语句并填写到单据体
        /// </summary>
        ComputeSqlAndFillSpecificBodyCol = 4,
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        ExecuteSql = 5,
        /// <summary>
        /// 计算定义表达式的值并填写到指定列(只有表头列,可以用日期转换等函数)
        /// </summary>
        ComputeExpAndFillSpecificCol = 6,
        /// <summary>
        /// 指定列数据改变时发起工作流
        /// </summary>
        SpecificColChgAndStartWf = 7,
        /// <summary>
        /// 根据表达式动态显示单据体列信息
        /// </summary>
        ComputeExpAndDisBodyCol = 8,
        /// <summary>
        /// 检查字段唯一性(单据体)
        /// </summary>
        CheckBodyUnique = 11,

        /// <summary>
        /// 可见表达式
        /// </summary>
        IsVisible = 15,
        /// <summary>
        /// 必输表达式
        /// </summary>
        IsMustInput = 16,
        /// <summary>
        /// 保护表达式
        /// </summary>
        IsReadOnly = 17,

        Other,
    }
}
