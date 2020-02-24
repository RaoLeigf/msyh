#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetMstDac
    * 命名空间：        GYS3.YS.Dac.Interface
    * 文 件 名：        IBudgetMstDac.cs
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
using System.Data;
using System.Linq;
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Dac.Interface
{
	/// <summary>
	/// BudgetMst数据访问层接口
	/// </summary>
    public partial interface IBudgetMstDac : IEntDacBase<BudgetMstModel>
    {
        #region IBudgetMstDac 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetMstModel> ExampleMethod<BudgetMstModel>(string param)

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="zbly_dm"></param>
        /// <param name="valuesqlList"></param>
        /// <param name="mstSqlList"></param>
        /// <param name="dtlSqlList"></param>
        /// <param name="DJRQList"></param>
        /// <param name="DEF_BZ1"></param>
        /// <param name="DT1List"></param>
        /// <param name="DT2List"></param>
        /// <returns></returns>
        int AddData(string userConn, string zbly_dm, List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList, string DEF_BZ1, List<DateTime?> DT1List, List<DateTime?> DT2List);


        /// <summary>
        /// 取最大ID值
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        int GetId(string userConn);

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        int AddYBF(string userConn, string code);

        /// <summary>
        /// 获取老g6h预算数据主表
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        DataTable GetOldMstList(string userConn);

        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        DataTable GetOldDtlList(string userConn);

        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        DataTable GetOldTextList(string userConn);
        #endregion
    }
}

