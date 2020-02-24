#region Summary
/**************************************************************************************
    * 类 名 称：        IMethodAroundMstFacade
    * 命名空间：        NG3.Addin.Facade.Interface
    * 文 件 名：        IMethodAroundMstFacade.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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

using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;

namespace NG3.Addin.Facade.Interface
{
	/// <summary>
	/// MethodAroundMst业务组装层接口
	/// </summary>
    public partial interface IMethodAroundMstFacade : IEntFacadeBase<MethodAroundMstModel>
    {
        #region IMethodAroundMstFacade 业务添加的成员

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mstModels"></param>
        /// <param name="sqlModels"></param>
        /// <param name="varModels"></param>
        /// <param name="expModels"></param>
        /// <param name="assemblyModels"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveMethodAround(List<MethodAroundMstModel> mstModels, List<AddinSqlModel> sqlModels, List<AddinExpressionVarModel> varModels, List<AddinExpressionModel> expModels, List<AddinAssemblyModel> assemblyModels);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeletedResult DeleteMethodAround(Int64 id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FindedResult<MethodAroundMstModel> GetMethodAroundMst(Int64 id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<AddinSqlModel> FindAddinSqlByMstPhid(Int64 id);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<AddinExpressionModel> FindAddinExpressionByMstPhid(Int64 id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<AddinExpressionVarModel> FindAddinExpressionVarByMstPhid(Int64 id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<AddinAssemblyModel> FindAddinAssemblyByMstPhid(Int64 id);

        /// <summary>
        /// 当前系统支持的函数
        /// </summary>
        /// <returns></returns>
        IList<SupportFunctionBizModel> GetSupportFunctions();

        /// <summary>
        /// 发布配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeployConfigure(Int64 id);

        /// <summary>
        /// 取消发布配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool UndeployConfigure(Int64 id);

        PagedResult<MethodAroundMstModel> GetMethodAroundMstList(int pageIndex, int pageSize, Dictionary<string, object> dic);
        #endregion
    }
}
