#region Summary
/**************************************************************************************
    * 类 名 称：        CorrespondenceSettingsFacade
    * 命名空间：        GQT3.QT.Facade
    * 文 件 名：        CorrespondenceSettingsFacade.cs
    * 创建时间：        2018/9/3 
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
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Facade
{
	/// <summary>
	/// CorrespondenceSettings业务组装处理类
	/// </summary>
    public partial class CorrespondenceSettingsFacade : EntFacadeBase<CorrespondenceSettingsModel>, ICorrespondenceSettingsFacade
    {
		#region 类变量及属性
		/// <summary>
        /// CorrespondenceSettings业务逻辑处理对象
        /// </summary>
		ICorrespondenceSettingsRule CorrespondenceSettingsRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as ICorrespondenceSettingsRule;
            }
        }

        /// <summary>
        /// User业务逻辑处理对象
        /// </summary>
		IUserRule UserRule { get; set; }
        /// <summary>
        /// Organization业务逻辑处理对象
        /// </summary>
        IOrganizationRule OrganizationRule { get; set; }
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
        public PagedResult<CorrespondenceSettingsModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<CorrespondenceSettingsModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<CorrespondenceSettingsModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<CorrespondenceSettingsModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
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
        public PagedResult<CorrespondenceSettingsModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<CorrespondenceSettingsModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<CorrespondenceSettingsModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<CorrespondenceSettingsModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        #endregion

        #region 实现 ICorrespondenceSettingsFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<CorrespondenceSettingsModel> ExampleMethod<CorrespondenceSettingsModel>(string param)
        //{
        //    //编写代码
        //}

       /// <summary>
       /// 根据当前usercode 获取所拥有部门信息
       /// </summary>
       /// <param name="usercode"></param>
       /// <returns></returns>
        public List<CorrespondenceSettingsModel> GetUserDepementList(string usercode)
        {
            var dic = new Dictionary<string, object>();

            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("Dylx", "97"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", usercode));


            #region 取最大的200，循环取LastPage
            List<CorrespondenceSettingsModel> Result = new List<CorrespondenceSettingsModel>();
            int pagesize = 100;
            int pageindex = 0;
            //去第一次200的数据
            var resultt = base.FacadeHelper.LoadWithPage(pageindex, pagesize, dic);

            long total = resultt.TotalItems;
            //IList<CorrespondenceSettingsModel> list1 = resultt.Results;
            Result.AddRange(resultt.Results.ToList());


            while (total > (pageindex + 1) * pagesize)
            {
                pageindex++;
                var resulttemp = base.FacadeHelper.LoadWithPage(pageindex, pagesize, dic);
                Result.AddRange(resulttemp.Results);

            }
            #endregion

            return Result;

        }

        #endregion
    }
}

