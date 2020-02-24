#region Summary
/**************************************************************************************
    * 类 名 称：        ExpenseCategoryService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        ExpenseCategoryService.cs
    * 创建时间：        2018/8/30 
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
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;
using Enterprise3.Common.Base.Criterion;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// ExpenseCategory服务组装处理类
	/// </summary>
    public partial class ExpenseCategoryService : EntServiceBase<ExpenseCategoryModel>, IExpenseCategoryService
    {
		#region 类变量及属性
		/// <summary>
        /// ExpenseCategory业务外观处理对象
        /// </summary>
		IExpenseCategoryFacade ExpenseCategoryFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IExpenseCategoryFacade;
            }
        }
        #endregion

        #region 实现 IExpenseCategoryService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ExpenseCategoryModel> ExampleMethod<ExpenseCategoryModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public FindedResults<ExpenseCategoryModel> ExecuteDataCheck(ref List<ExpenseCategoryModel> expenseCategories,string otype) {
            IList<string> dm = new List<string>();
            IList<string> mc = new List<string>();
            FindedResults<ExpenseCategoryModel> results = new FindedResults<ExpenseCategoryModel>();
            if (expenseCategories == null)
            {
                results.Status = ResponseStatus.Error;
                results.Msg = "保存失败，数据异常！";
            }
            else
            {
                for (int i = 0; i < expenseCategories.Count; i++)
                {
                    expenseCategories[i].Dm = expenseCategories[i].Dm.Replace(" ","");
                    expenseCategories[i].Mc = expenseCategories[i].Mc.Replace(" ","");
                    expenseCategories[i].Bz.Trim();
                    dm.Add(expenseCategories[i].Dm);
                    mc.Add(expenseCategories[i].Mc);
                }
                var dicWhere1 = new Dictionary<string, object>();
                var dicWhere2 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1).
                    Add(ORMRestrictions<IList<string>>.In("Dm", dm));
                new CreateCriteria(dicWhere2).
                    Add(ORMRestrictions<IList<string>>.In("Mc", mc));
                if (base.Find(dicWhere1) != null && base.Find(dicWhere1).Data.Count > 0 && otype != "edit") {
                    results = base.Find(dicWhere1);
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，代码重复！";
                }
                if (base.Find(dicWhere2) != null && base.Find(dicWhere2).Data.Count > 0) {
                    results = base.Find(dicWhere2);
                    results.Status = ResponseStatus.Error;
                    results.Msg = "保存失败，名称重复！";
                }
            }
            return results;
        }

        /// <summary>
        /// 根据支出类别(项目类型)的code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public FindedResults<ExpenseCategoryModel> IfLastStage(string code) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.NotEq("Dm", code))
                .Add(ORMRestrictions<string>.LLike("Dm",code));
            var findResult = base.Find(dicWhere);
            return findResult;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="expenseCategory"></param>
        /// <param name="edittype"></param>
        /// <returns></returns>
        public SavedResult<Int64> Save2(ExpenseCategoryModel expenseCategory, string edittype)
        {
            SavedResult<Int64> result = new SavedResult<Int64>();
            if (edittype == "edit")
            {
                ExpenseCategoryModel expenseCategory2 = base.Find(expenseCategory.PhId).Data;
                if (expenseCategory.Mc != expenseCategory2.Mc)
                {
                    var dicWhere = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere).
                        Add(ORMRestrictions<string>.Eq("Mc", expenseCategory.Mc));
                    if (base.Find(dicWhere).Data.Count > 0)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "保存失败，名称重复！";
                        return result;
                    }
                }
                expenseCategory2.Mc = expenseCategory.Mc;
                expenseCategory2.Bz = expenseCategory.Bz;
                expenseCategory2.PersistentState = PersistentState.Modified;
                result = base.Save<Int64>(expenseCategory2,"");

            }
            if (edittype == "add")
            {
                var dicWhere1 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere1).
                    Add(ORMRestrictions<string>.Eq("Dm", expenseCategory.Dm));
                if (base.Find(dicWhere1).Data.Count > 0)
                {
                    result.Status = ResponseStatus.Error;
                    result.Msg = "保存失败，代码重复！";
                    return result;
                }
                var dicWhere2 = new Dictionary<string, object>();
                new CreateCriteria(dicWhere2).
                    Add(ORMRestrictions<string>.Eq("Mc", expenseCategory.Mc));
                if (base.Find(dicWhere2).Data.Count > 0)
                {
                    result.Status = ResponseStatus.Error;
                    result.Msg = "保存失败，名称重复！";
                    return result;
                }
                expenseCategory.PersistentState = PersistentState.Added;
                result = base.Save<Int64>(expenseCategory,"");
            }
            return result;
        }

        #endregion
    }
}

