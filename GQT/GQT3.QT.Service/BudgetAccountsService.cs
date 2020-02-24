#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetAccountsService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        BudgetAccountsService.cs
    * 创建时间：        2018/8/29 
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
using System.Web;
using System.IO;
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;
using Enterprise3.Common.Base.Criterion;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// BudgetAccounts服务组装处理类
	/// </summary>
    public partial class BudgetAccountsService : EntServiceBase<BudgetAccountsModel>, IBudgetAccountsService
    {
		#region 类变量及属性
		/// <summary>
        /// BudgetAccounts业务外观处理对象
        /// </summary>
		IBudgetAccountsFacade BudgetAccountsFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IBudgetAccountsFacade;
            }
        }
        #endregion

        #region 实现 IBudgetAccountsService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetAccountsModel> ExampleMethod<BudgetAccountsModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        public IList<BudgetAccountsModel> ExportData() {

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            IList<BudgetAccountsModel> budgetAccounts = base.ServiceHelper.LoadWithPageInfinity("GQT.QT.GetAllBudgetAccount", dicWhere).Results;
            return budgetAccounts;
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public SavedResult<Int64> ImportData(string filePath, string clear) {

            IList<BudgetAccountsModel> budgetAccountList = new List<BudgetAccountsModel>();

            if ("1".Equals(clear)) {
                try {
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    dicwhere.Add("1", "1");
                    base.Delete(dicwhere);
                }
                catch (Exception e) {
                    return null;
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try {
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = sheet.LastRowNum;
                    for (int i = 1; i <= rowCount; i++)
                    {
                        BudgetAccountsModel budgetAccounts = new BudgetAccountsModel();
                        IRow row = sheet.GetRow(i);
                        ICell cell1 = row.GetCell(0);
                        ICell cell2 = row.GetCell(1);
                        ICell cell3 = row.GetCell(2);
                        cell1.SetCellType(CellType.String);
                        cell2.SetCellType(CellType.String);
                        cell3.SetCellType(CellType.String);
                        string kmdm = cell1.StringCellValue;
                        string kmmc = cell2.StringCellValue;
                        string kmlb = (cell3.StringCellValue == "收入" ? "0" : "1");
                        budgetAccounts.KMDM = kmdm;
                        budgetAccounts.KMMC = kmmc;
                        budgetAccounts.KMLB = kmlb;
                        budgetAccounts.PersistentState = PersistentState.Added;
                        budgetAccountList.Add(budgetAccounts);
                    }
                }
                catch (Exception e) {
                    return null;
                }
            }

            return base.Save<Int64>(budgetAccountList,"");             
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public FindedResults<BudgetAccountsModel> ExecuteDataCheck(ref List<BudgetAccountsModel> budgetAccounts,string otype) {
            IList<string> kmdm = new List<string>();
            FindedResults<BudgetAccountsModel> results = new FindedResults<BudgetAccountsModel>();
            if (budgetAccounts == null)
            {
                results.Status = ResponseStatus.Error;
                results.Msg = "保存失败，数据异常！";
            }
            else {
                for (int i = 0; i < budgetAccounts.Count; i++) {
                    budgetAccounts[i].KMDM = budgetAccounts[i].KMDM.Replace(" ","");
                    budgetAccounts[i].KMMC = budgetAccounts[i].KMMC.Replace(" ", "");
                    budgetAccounts[i].KMLB = budgetAccounts[i].KMLB.Replace(" ", "");
                    kmdm.Add(budgetAccounts[i].KMDM);
                }
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<IList<string>>.In("KMDM", kmdm));
                results = base.Find(dicWhere);
                if (results != null && results.Data.Count > 0 && otype != "edit") {
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，科目代码重复！";
                }
            }
            return results; 
        }

        /// <summary>
        /// 根据code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public FindedResults<BudgetAccountsModel> IfLastStage(string code)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.NotEq("KMDM", code))
                .Add(ORMRestrictions<string>.LLike("KMDM", code));
            var findResult = base.Find(dicWhere);
            return findResult;
        }
        #endregion
    }
}

