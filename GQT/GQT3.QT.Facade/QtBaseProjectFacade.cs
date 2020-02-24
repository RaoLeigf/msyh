#region Summary
/**************************************************************************************
    * 类 名 称：        QtBaseProjectFacade
    * 命名空间：        GQT3.QT.Facade
    * 文 件 名：        QtBaseProjectFacade.cs
    * 创建时间：        2018/11/23 
    * 作    者：        刘杭    
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

using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade
{
	/// <summary>
	/// QtBaseProject业务组装处理类
	/// </summary>
    public partial class QtBaseProjectFacade : EntFacadeBase<QtBaseProjectModel>, IQtBaseProjectFacade
    {
		#region 类变量及属性
		/// <summary>
        /// QtBaseProject业务逻辑处理对象
        /// </summary>
		IQtBaseProjectRule QtBaseProjectRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IQtBaseProjectRule;
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
        public PagedResult<QtBaseProjectModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<QtBaseProjectModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "FFillDept", "FFillDept_Name", "ys_orglist", "");
            helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "FKmdm", "Fkmmc", "GHBudgetAccounts", "");
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
        public PagedResult<QtBaseProjectModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<QtBaseProjectModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "FFillDept", "FFillDept_Name", "ys_orglist", "");
            helpdac.CodeToName<QtBaseProjectModel>(pageResult.Results, "FKmdm", "Fkmmc", "GHBudgetAccounts", "");
            #endregion

            return pageResult;
        }

        #endregion

		#region 实现 IQtBaseProjectFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<QtBaseProjectModel> ExampleMethod<QtBaseProjectModel>(string param)
        //{
        //    //编写代码
        //}


        #endregion
    }
}

