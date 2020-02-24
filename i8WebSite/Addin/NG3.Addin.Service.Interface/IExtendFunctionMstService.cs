#region Summary
/**************************************************************************************
    * 类 名 称：        IExtendFunctionMstService
    * 命名空间：        NG3.Addin.Service.Interface
    * 文 件 名：        IExtendFunctionMstService.cs
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

namespace NG3.Addin.Service.Interface
{
	/// <summary>
	/// ExtendFunctionMst服务组装层接口
	/// </summary>
    public partial interface IExtendFunctionMstService : IEntServiceBase<ExtendFunctionMstModel>
    {
        #region IExtendFunctionMstService 业务添加的成员

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mstModels"></param>
        /// <param name="urlModels"></param>
        /// <param name="sqlModels"></param>
        /// <param name="assemblyModels"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveExtendFunc(List<ExtendFunctionMstModel> mstModels, List<ExtendFuncUrlBizModel> urlModels, List<AddinSqlModel> sqlModels, List<AddinAssemblyModel> assemblyModels);

    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeletedResult DeleteExtendFunc(Int64 id);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<ExtendFuncUrlBizModel> GetUrl(Int64 id);

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
        IList<AddinAssemblyModel> FindAddinAssemblyByMstPhid(Int64 id);

        #endregion
    }
}
