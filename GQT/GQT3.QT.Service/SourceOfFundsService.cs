#region Summary
/**************************************************************************************
    * 类 名 称：        SourceOfFundsService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        SourceOfFundsService.cs
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
	/// SourceOfFunds服务组装处理类
	/// </summary>
    public partial class SourceOfFundsService : EntServiceBase<SourceOfFundsModel>, ISourceOfFundsService
    {
		#region 类变量及属性
		/// <summary>
        /// SourceOfFunds业务外观处理对象
        /// </summary>
		ISourceOfFundsFacade SourceOfFundsFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as ISourceOfFundsFacade;
            }
        }
        #endregion

        #region 实现 ISourceOfFundsService 业务添加的成员
        /// <summary>
        /// 根据支出类别(项目类型)的code判断是否是末级
        /// </summary>
        /// <returns>返回Json串</returns>
        public FindedResults<SourceOfFundsModel> IfLastStage(string code)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.NotEq("DM", code))
                .Add(ORMRestrictions<string>.LLike("DM", code));
            var findResult = base.Find(dicWhere);
            return findResult;
        }

        #endregion
    }
}

