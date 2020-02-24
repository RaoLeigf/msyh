#region Summary
/**************************************************************************************
    * 类 名 称：        UserOrgService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        UserOrgService.cs
    * 创建时间：        2018/9/19 
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
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;
using Enterprise3.Common.Base.Criterion;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service
{
	/// <summary>
	/// UserOrg服务组装处理类
	/// </summary>
    public partial class UserOrgService : EntServiceBase<UserOrganize2Model>, IUserOrgService
    {
		#region 类变量及属性
		/// <summary>
        /// UserOrg业务外观处理对象
        /// </summary>
		IUserOrgFacade UserOrgFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IUserOrgFacade;
            }
        }

        /// <summary>
        /// 报表RWReportFacade
        /// </summary>
		private IRWReportFacade RWReportFacade { get; set; }
        #endregion

        #region 实现 IUserOrgService 业务添加的成员

        /// <summary>
        /// 报表
        /// </summary>
        public PagedResult<RWReportModel> GetList(long userId) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("RepType", "2"))
                .Add(ORMRestrictions<string>.Eq("RepGenre", "1"))
                .Add(ORMRestrictions<string>.Eq("OCode", base.OrgID.ToString()));
            PagedResult<RWReportModel> result = RWReportFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetList", dicWhere);
            return result;
        }
        #endregion
    }
}

