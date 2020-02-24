#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Facade
    * 类 名 称：			ProjectDtlPersonNameFacade
    * 文 件 名：			ProjectDtlPersonNameFacade.cs
    * 创建时间：			2020/1/14 
    * 作    者：			王冠冠    
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
	/// ProjectDtlPersonName业务组装处理类
	/// </summary>
    public partial class ProjectDtlPersonNameFacade : EntFacadeBase<ProjectDtlPersonNameModel>, IProjectDtlPersonNameFacade
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectDtlPersonName业务逻辑处理对象
        /// </summary>
		IProjectDtlPersonNameRule ProjectDtlPersonNameRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectDtlPersonNameRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<ProjectDtlPersonNameModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<ProjectDtlPersonNameModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<ProjectDtlPersonNameModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<ProjectDtlPersonNameModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        #endregion

        #region 实现 IProjectDtlPersonNameFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlPersonNameModel> ExampleMethod<ProjectDtlPersonNameModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存维护人员集合
        /// </summary>
        /// <param name="projectDtlPersonNames">人员集合</param>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        public SavedResult<long> SaveMSYHPersonNames(List<ProjectDtlPersonNameModel> projectDtlPersonNames, long phid)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            savedResult = this.ProjectDtlPersonNameRule.Save(projectDtlPersonNames, phid);
            return savedResult;
        }
        #endregion
    }
}

