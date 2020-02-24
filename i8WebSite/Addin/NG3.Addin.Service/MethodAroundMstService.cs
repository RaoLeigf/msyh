#region Summary
/**************************************************************************************
    * 类 名 称：        MethodAroundMstService
    * 命名空间：        NG3.Addin.Service
    * 文 件 名：        MethodAroundMstService.cs
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
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using NG3.Addin.Service.Interface;
using NG3.Addin.Facade.Interface;
using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;

namespace NG3.Addin.Service
{
	/// <summary>
	/// MethodAroundMst服务组装处理类
	/// </summary>
    public partial class MethodAroundMstService : EntServiceBase<MethodAroundMstModel>, IMethodAroundMstService
    {
		#region 类变量及属性
		/// <summary>
        /// MethodAroundMst业务外观处理对象
        /// </summary>
		IMethodAroundMstFacade MethodAroundMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentFacade as IMethodAroundMstFacade;
            }
        }




        #endregion

        #region 实现 IMethodAroundMstService 业务添加的成员

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult DeleteMethodAround(long id)
        {
            return MethodAroundMstFacade.DeleteMethodAround(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinAssemblyModel> FindAddinAssemblyByMstPhid(long id)
        {
            return MethodAroundMstFacade.FindAddinAssemblyByMstPhid(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinExpressionModel> FindAddinExpressionByMstPhid(long id)
        {
            return MethodAroundMstFacade.FindAddinExpressionByMstPhid(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinExpressionVarModel> FindAddinExpressionVarByMstPhid(long id)
        {
            return MethodAroundMstFacade.FindAddinExpressionVarByMstPhid(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinSqlModel> FindAddinSqlByMstPhid(long id)
        {
            return MethodAroundMstFacade.FindAddinSqlByMstPhid(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FindedResult<MethodAroundMstModel> GetMethodAroundMst(long id)
        {
            return MethodAroundMstFacade.GetMethodAroundMst(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<SupportFunctionBizModel> GetSupportFunctions()
        {
            return MethodAroundMstFacade.GetSupportFunctions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mstModels"></param>
        /// <param name="sqlModels"></param>
        /// <param name="varModels"></param>
        /// <param name="expModels"></param>
        /// <param name="assemblyModels"></param>
        /// <returns></returns>
        public SavedResult<long> SaveMethodAround(List<MethodAroundMstModel> mstModels, List<AddinSqlModel> sqlModels, List<AddinExpressionVarModel> varModels, List<AddinExpressionModel> expModels, List<AddinAssemblyModel> assemblyModels)
        {
            return MethodAroundMstFacade.SaveMethodAround(mstModels, sqlModels, varModels, expModels, assemblyModels);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeployConfigure(long id)
        {
            return MethodAroundMstFacade.DeployConfigure(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UndeployConfigure(long id)
        {
            return MethodAroundMstFacade.UndeployConfigure(id);
        }

        /// <summary>
        /// 获取注入功能点列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedResult<MethodAroundMstModel> GetMethodAroundMstList(int pageIndex, int pageSize, Dictionary<string, object> dic)
        {
            return MethodAroundMstFacade.GetMethodAroundMstList(pageIndex,pageSize,dic);
        }

        #endregion
    }
}

