#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetMstRule
    * 命名空间：        GYS3.YS.Rule
    * 文 件 名：        BudgetMstRule.cs
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
using Enterprise3.Common.Model.NHORM;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.NHORM.Rule;

using GYS3.YS.Rule.Interface;
using GYS3.YS.Dac.Interface;
using GYS3.YS.Model.Domain;
using System.Data;

namespace GYS3.YS.Rule
{
	/// <summary>
	/// BudgetMst业务逻辑处理类
	/// </summary>
    public partial class BudgetMstRule : EntRuleBase<BudgetMstModel>, IBudgetMstRule
    {
        #region 类变量及属性
		/// <summary>
        /// BudgetMst数据访问处理对象
        /// </summary>
        IBudgetMstDac BudgetMstDac
        {
            get
            {
                if (CurrentDac == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentDac as IBudgetMstDac;
            }
        }

        #endregion

        #region 数据校验
		/// <summary>
        /// 数据校验(重写方法)
        /// </summary>
        /// <param name="entities">实体集合</param>
		/// <param name="prompt">提示信息</param>
        /// <returns>处理成功返回True</returns>
        public override bool DataCheck(IList<BudgetMstModel> entities, out string prompt)
        {
			prompt = null;
            return true;
        }
        #endregion

        #region 数据处理
		/// <summary>
        /// 数据处理
        /// </summary>
        /// <para>
        /// 此方法必须进行重写,执行顺序在DataCheck方法之前。
        /// </para>
        /// <param name="entities">实体集合</param>
        /// <param name="masterId">主表关键字值</param>
        public override void DataHandling<Int64>(ref IList<BudgetMstModel> entities, Int64 masterId)
        {
            //参数idPropertyName：主键Id的属性名称，新增时赋主键值用，若是单主键，则无需传递idPropertyName，
            //参数noPropertyName：编码列的属性名称，新增时赋用户编码用，BillNoReqType不为空时，需赋值
            base.RuleHelper.DataHandlingPrimaryKey(ref entities, string.Empty, string.Empty);
 
        }
        #endregion

        #region 实现 IBudgetMstRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetMstModel> ExampleMethod<BudgetMstModel>(string param)
        //{
        //    //编写代码
        //}

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
        public int AddData(string userConn, string zbly_dm, List<string> valuesqlList, List<string> mstSqlList, List<string> dtlSqlList, List<DateTime?> DJRQList, string DEF_BZ1, List<DateTime?> DT1List, List<DateTime?> DT2List)
        {
            return BudgetMstDac.AddData(userConn, zbly_dm, valuesqlList, mstSqlList, dtlSqlList, DJRQList,DEF_BZ1, DT1List, DT2List);
        }

        /// <summary>
        /// 取最大ID值
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public int GetId(string userConn)
        {
            return BudgetMstDac.GetId(userConn);
        }

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public int AddYBF(string userConn, string code)
        {
            return BudgetMstDac.AddYBF(userConn, code);
        }

        /// <summary>
        /// 获取老g6h预算数据主表
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public DataTable GetOldMstList(string userConn)
        {
            return BudgetMstDac.GetOldMstList(userConn);
        }

        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public DataTable GetOldDtlList(string userConn)
        {
            return BudgetMstDac.GetOldDtlList(userConn);
        }

        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public DataTable GetOldTextList(string userConn)
        {
            return BudgetMstDac.GetOldTextList(userConn);
        }
        #endregion
    }
}
