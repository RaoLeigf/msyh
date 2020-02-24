#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlFundApplFacade
    * 命名空间：        GXM3.XM.Facade
    * 文 件 名：        ProjectDtlFundApplFacade.cs
    * 创建时间：        2018/8/28 
    * 作    者：        李明    
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

using GXM3.XM.Facade.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade
{
	/// <summary>
	/// ProjectDtlFundAppl业务组装处理类
	/// </summary>
    public partial class ProjectDtlFundApplFacade : EntFacadeBase<ProjectDtlFundApplModel>, IProjectDtlFundApplFacade
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectDtlFundAppl业务逻辑处理对象
        /// </summary>
		IProjectDtlFundApplRule ProjectDtlFundApplRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectDtlFundApplRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<ProjectDtlFundApplModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<ProjectDtlFundApplModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectDtlFundApplModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectDtlFundApplModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<ProjectDtlFundApplModel>(findedResults.Data, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
            #endregion

            return findedResults;
        }

        #endregion

		#region 实现 IProjectDtlFundApplFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlFundApplModel> ExampleMethod<ProjectDtlFundApplModel>(string param)
        //{
        //    //编写代码
        //}


        #endregion
    }
}

