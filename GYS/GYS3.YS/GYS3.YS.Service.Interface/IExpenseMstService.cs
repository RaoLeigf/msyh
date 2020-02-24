#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseMstService
    * 命名空间：        GYS3.YS.Service.Interface
    * 文 件 名：        IExpenseMstService.cs
    * 创建时间：        2019/1/24 
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Extra;

namespace GYS3.YS.Service.Interface
{
	/// <summary>
	/// ExpenseMst服务组装层接口
	/// </summary>
    public partial interface IExpenseMstService : IEntServiceBase<ExpenseMstModel>
    {
        #region IExpenseMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="expenseMstEntity"></param>
        /// <param name="expenseDtlEntities"></param>
        /// <param name="NCmoney">年初预算金额</param>
        /// <param name="beforeSum">本单据初始预计支出金额</param>
        /// <param name="beforeFReturnamount">本单据初始预计返还金额</param>
        /// <param name="Ifreturn">是否额度返还</param>
        /// <returns></returns>
        SavedResult<Int64> SaveExpenseMst(ExpenseMstModel expenseMstEntity, List<ExpenseDtlModel> expenseDtlEntities, string NCmoney, string beforeSum, string beforeFReturnamount, string Ifreturn);

        /// <summary>
        /// 通过外键值获取ExpenseDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ExpenseDtlModel> FindExpenseDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取ExpenseHx明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<ExpenseHxModel> FindExpenseHxByForeignKey<Int64>(Int64 id);

        /// <summary>
        /// 根据项目代码取预计支出金额的和
        /// </summary>
        /// <param name="FProjCode"></param>
        /// <returns></returns>
        string SumFSurplusamount(string FProjCode);

        /// <summary>
        /// 额度逆返还
        /// </summary>
        /// <param name="id">额度返还的单据phid</param>
        /// <returns></returns>
        CommonResult DeleteReturn(long id);

        /// <summary>
        /// 删除正常单据 可编报数变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CommonResult Delete2(long id);

        /// <summary>
        /// 根据主表phid取明细剩余金额
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        string RestOfAmount(long id, string code);

        /*/// <summary>
        /// 项目支出预算情况查询
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedResult<ExpenseMstModel> GetXmZcYs(string userID, int pageIndex, int pageSize);*/

        /// <summary>
        /// 通过项目代码和操作员获取财务实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        string GetSJFSSbyCode(string userID, string code);

        /// 保存额度核销数据
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        CommonResult SaveHX(List<ExpenseHxModel> adddata, List<ExpenseHxModel> updatedata, List<string> deletedata);

        /// <summary>
        /// 额度核销执行完毕确认
        /// </summary>
        /// <param name="id"></param>
        /// <param name="FPlayamount"></param>
        /// <param name="FReturnamount"></param>
        /// <param name="dtls"></param>
        /// <returns></returns>
        CommonResult SaveHXgo(long id, Decimal FPlayamount, Decimal FReturnamount, List<ExpenseDtlModel> dtls);

        /// <summary>
        /// 额度核销撤销
        /// </summary>
        /// <param name="id"></param>
        /// <param name="FPlayamount"></param>
        /// <returns></returns>
        CommonResult SaveHXreturn(long id, Decimal FPlayamount);

        /// <summary>
        /// 根据项目代码获取额度核销总金额
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        decimal GetHXsumByCode(string code);

        /// <summary>
        /// 根据预算主键获取信息
        /// </summary>
        /// <param name="YsPhid"></param>
        /// <returns></returns>
        object GetinfoByProjCode(long YsPhid);

        /// <summary>
        /// 根据预算部门取项目支出预算申报总数、申报总额、有哪些项目及金额饼图
        /// </summary>
        /// <param name="Dept"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        object GetinfoByDept(string Dept, string Year);

        /// <summary>
        /// 根据用款计划的主键获取相关数据集合
        /// </summary>
        /// <param name="phid">主键</param>
        /// <returns></returns>
        ExpenseAllModel GetExpenseAllModel(long phid);
        #endregion
    }
}
