#region Summary
/**************************************************************************************
    * 类 名 称：        IGHSubjectFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        IGHSubjectFacade.cs
    * 创建时间：        2018/11/26 
    * 作    者：        董泉伟    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// GHSubject业务组装层接口
	/// </summary>
    public partial interface IGHSubjectFacade : IEntFacadeBase<GHSubjectModel>
    {
		#region IGHSubjectFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GHSubjectModel> ExampleMethod<GHSubjectModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gHSubjectEntity"></param>
		/// <param name="subjectMstEntities"></param>
		/// <param name="subjectMstBudgetDtlEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveGHSubject(GHSubjectModel gHSubjectEntity, List<SubjectMstModel> subjectMstEntities, List<SubjectMstBudgetDtlModel> subjectMstBudgetDtlEntities);

        PagedResult<GHSubjectModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts);

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        string AddData();

        /// <summary>
        /// 纳入预算同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        string AddDataSP();
        #endregion
    }
}
