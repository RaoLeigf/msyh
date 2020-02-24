#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPerformTargetFacade
    * 命名空间：        GXM3.XM.Facade
    * 文 件 名：        ProjectDtlPerformTargetFacade.cs
    * 创建时间：        2018/10/15 
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
	/// ProjectDtlPerformTarget业务组装处理类
	/// </summary>
    public partial class ProjectDtlPerformTargetFacade : EntFacadeBase<ProjectDtlPerformTargetModel>, IProjectDtlPerformTargetFacade
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectDtlPerformTarget业务逻辑处理对象
        /// </summary>
		IProjectDtlPerformTargetRule ProjectDtlPerformTargetRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectDtlPerformTargetRule;
            }
        }
	
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<ProjectDtlPerformTargetModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<ProjectDtlPerformTargetModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectDtlPerformTargetModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectDtlPerformTargetModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<ProjectDtlPerformTargetModel>(findedResults.Data, "FTargetClassCode", "FTargetClassCode_EXName", "GHPerformEvalTargetClass", "");
            helpdac.CodeToName<ProjectDtlPerformTargetModel>(findedResults.Data, "FTargetTypeCode", "FTargetTypeCode_EXName", "GHPerformEvalTargetType", "");
            #endregion

            return findedResults;
        }
        
        #endregion

		#region 实现 IProjectDtlPerformTargetFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlPerformTargetModel> ExampleMethod<ProjectDtlPerformTargetModel>(string param)
        //{
        //    //编写代码
        //}

        #endregion
    }
}

