#region Summary
/**************************************************************************************
    * 类 名 称：        QtZcgnflService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        QtZcgnflService.cs
    * 创建时间：        2019/1/23 
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

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtZcgnfl服务组装处理类
	/// </summary>
    public partial class QtZcgnflService : EntServiceBase<QtZcgnflModel>, IQtZcgnflService
    {
		#region 类变量及属性
		/// <summary>
        /// QtZcgnfl业务外观处理对象
        /// </summary>
		IQtZcgnflFacade QtZcgnflFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtZcgnflFacade;
            }
        }
        #endregion

        #region 实现 IQtZcgnflService 业务添加的成员
        /// <summary>
        /// 根据code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public FindedResults<QtZcgnflModel> IfLastStage(string code)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.NotEq("KMDM", code))
                .Add(ORMRestrictions<string>.LLike("KMDM", code));
            var findResult = base.Find(dicWhere);
            return findResult;
        }

        /// <summary>
        /// 根据组织获取支出功能分类列表
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<QtZcgnflModel> GetZcgnfls(string orgId, string orgCode)
        {
            IList<QtZcgnflModel> zcgnfls = new List<QtZcgnflModel>();
            zcgnfls = this.QtZcgnflFacade.Find(t => t.Orgid == long.Parse(orgId) && t.Ocode == orgCode).Data.OrderBy(t => t.KMDM).ToList();
            return zcgnfls;
        }

        #endregion
    }
}

