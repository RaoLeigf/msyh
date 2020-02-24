#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformanceMstService
    * 命名空间：        GJX3.JX.Service.Interface
    * 文 件 名：        IPerformanceMstService.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;
using Enterprise3.WebApi.GJX3.JX.Model.Request;
using GJX3.JX.Model.Domain;
using GYS3.YS.Model.Domain;

namespace GJX3.JX.Service.Interface
{
	/// <summary>
	/// PerformanceMst服务组装层接口
	/// </summary>
    public partial interface IPerformanceMstService : IEntServiceBase<PerformanceMstModel>
    {
        #region IPerformanceMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="performanceMstEntity"></param>
        /// <param name="performanceDtlTextContEntities"></param>
        /// <param name="performanceDtlBuDtlEntities"></param>
        /// <param name="performanceDtlTarImplEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SavePerformanceMst(PerformanceMstModel performanceMstEntity, List<PerformanceDtlTextContModel> performanceDtlTextContEntities, List<PerformanceDtlBuDtlModel> performanceDtlBuDtlEntities, List<PerformanceDtlTarImplModel> performanceDtlTarImplEntities);

        /// <summary>
        /// 通过外键值获取PerformanceDtlEval明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<PerformanceDtlEvalModel> FindPerformanceDtlEvalByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取PerformanceDtlTextCont明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<PerformanceDtlTextContModel> FindPerformanceDtlTextContByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取PerformanceDtlBuDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<PerformanceDtlBuDtlModel> FindPerformanceDtlBuDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取PerformanceDtlTarImpl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<PerformanceDtlTarImplModel> FindPerformanceDtlTarImplByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ThirdAttachment明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ThirdAttachmentModel> FindThirdAttachmentByForeignKey<TValType>(TValType id);

        /// <summary>
        /// BudgetDtlPerformTargetModel转换成PerformanceDtlTarImplModel
        /// </summary>
        /// <param name="list">预算绩效目标实现集合</param>
        /// <returns></returns>
        IList<PerformanceDtlTarImplModel> ConvertData(IList<BudgetDtlPerformTargetModel> list);

        /// <summary>
        /// 考虑到存在指标类别、类型名称转代码的问题
        /// 合并绩效目标实现情况数据，从预算明细表获取表格模板数据，从绩效明细表获取自评完成值和自评得分
        /// </summary>
        /// <param name="budgetDtlPerformTarget">预算绩效目标实现集合</param>
        /// <param name="performanceDtlTarImpl">绩效目标实现情况集合</param>
        /// <returns></returns>
        List<PerformanceDtlTarImplModel> ConvertSaveData(IList<BudgetDtlPerformTargetModel> budgetDtlPerformTarget, IList<PerformanceDtlTarImplModel> performanceDtlTarImpl);

        /// <summary>
        /// 通过id获取主表信息
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        FindedResult<PerformanceMstModel> Find2<TValType>(TValType id);

        /// <summary>
        /// 保存第三方评价数据
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveThird(List<ThirdAttachmentModel> adddata, List<ThirdAttachmentModel> updatedata, List<string> deletedata);
        #endregion

        #region//vue绩效相关
        /// <summary>
        /// 根据绩效主键获取单个绩效的数据
        /// </summary>
        /// <param name="phid">绩效主键</param>
        /// <returns></returns>
        PerformanceAllData GetPerformanceMst(long phid);

        /// <summary>
        /// 保存绩效抽评的第三方评价
        /// </summary>
        /// <param name="performanceMst">绩效抽评主对象</param>
        /// <param name="thirdAttachments">第三方评价的集合</param>
        /// <returns></returns>
        SavedResult<long> SaveThird(PerformanceMstModel performanceMst, IList<ThirdAttachmentModel> thirdAttachments);

        /// <summary>
        /// 将预算表中的BudgetDtlPerformTargetModel集合组装成绩效的PerformanceDtlTarImplModel集合
        /// </summary>
        /// <param name="list">预算绩效集合</param>
        /// <param name="performanceMst">绩效主对象</param>
        /// <returns></returns>
        IList<PerformanceDtlTarImplModel> ConvertData2(IList<BudgetDtlPerformTargetModel> list, PerformanceMstModel performanceMst);
        #endregion
    }
}
