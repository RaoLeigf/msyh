#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetProcessCtrlFacade
    * 命名空间：        GYS3.YS.Facade
    * 文 件 名：        BudgetProcessCtrlFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;
using SUP.Common.Base;

using GYS3.YS.Facade.Interface;
using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;
using GQT3.QT.Model.Domain;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// BudgetProcessCtrl业务组装处理类
	/// </summary>
    public partial class BudgetProcessCtrlFacade : EntFacadeBase<BudgetProcessCtrlModel>, IBudgetProcessCtrlFacade
    {
		#region 类变量及属性
		/// <summary>
        /// BudgetProcessCtrl业务逻辑处理对象
        /// </summary>
		IBudgetProcessCtrlRule BudgetProcessCtrlRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IBudgetProcessCtrlRule;
            }
        }
		#endregion

		#region 重载方法
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<BudgetProcessCtrlModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<BudgetProcessCtrlModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<BudgetProcessCtrlModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<BudgetProcessCtrlModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public PagedResult<BudgetProcessCtrlModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<BudgetProcessCtrlModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<BudgetProcessCtrlModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<BudgetProcessCtrlModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        #endregion

        #region 实现 IBudgetProcessCtrlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetProcessCtrlModel> ExampleMethod<BudgetProcessCtrlModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存进度控制数据
        /// </summary>
        /// <returns></returns>
        public SavedResult<Int64> UpdateBudgetProcess(List<OrganizeModel> allOrganizeList, List<BudgetProcessCtrlModel> existOrganizeList,OrganizeModel findOrgModel,string focode) {
            List<BudgetProcessCtrlModel> budgetProcessesList = new List<BudgetProcessCtrlModel>();
            for (int i = 0; i < allOrganizeList.Count; i++)
            {
                if (!existOrganizeList.Exists(t => t.FDeptCode == allOrganizeList[i].OCode))
                {
                    BudgetProcessCtrlModel budgetProcessCtrlModel = new BudgetProcessCtrlModel();
                    budgetProcessCtrlModel.PersistentState = PersistentState.Added;
                    budgetProcessCtrlModel.FOcode = focode;
                    budgetProcessCtrlModel.FOname = findOrgModel.OName;
                    budgetProcessCtrlModel.FDeptCode = allOrganizeList[i].OCode;
                    budgetProcessCtrlModel.FDeptName = allOrganizeList[i].OName;
                    budgetProcessCtrlModel.FProcessStatus = "1";
                    budgetProcessesList.Add(budgetProcessCtrlModel);
                }
            }
            SavedResult<Int64> savedResult = base.Save<Int64>(budgetProcessesList);
            return savedResult;
        }
        #endregion
    }
}

