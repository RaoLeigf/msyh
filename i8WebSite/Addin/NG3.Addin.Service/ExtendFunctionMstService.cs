#region Summary
/**************************************************************************************
    * 类 名 称：        ExtendFunctionMstService
    * 命名空间：        NG3.Addin.Service
    * 文 件 名：        ExtendFunctionMstService.cs
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
	/// ExtendFunctionMst服务组装处理类
	/// </summary>
    public partial class ExtendFunctionMstService : EntServiceBase<ExtendFunctionMstModel>, IExtendFunctionMstService
    {
		#region 类变量及属性
		/// <summary>
        /// ExtendFunctionMst业务外观处理对象
        /// </summary>
		IExtendFunctionMstFacade ExtendFunctionMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentFacade as IExtendFunctionMstFacade;
            }
        }

        #endregion

        #region 实现 IExtendFunctionMstService 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<ExtendFunctionMstModel> ExampleMethod<ExtendFunctionMstModel>(string param)
        //{
        //    //编写代码
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult DeleteExtendFunc(long id)
        {
            return ExtendFunctionMstFacade.DeleteExtendFunc(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinAssemblyModel> FindAddinAssemblyByMstPhid(long id)
        {
            return ExtendFunctionMstFacade.FindAddinAssemblyByMstPhid(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinSqlModel> FindAddinSqlByMstPhid(long id)
        {
            return ExtendFunctionMstFacade.FindAddinSqlByMstPhid(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<ExtendFuncUrlBizModel> GetUrl(long id)
        {
            return ExtendFunctionMstFacade.GetUrl(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mstModels"></param>
        /// <param name="urlModels"></param>
        /// <param name="sqlModels"></param>
        /// <param name="assemblyModels"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveExtendFunc(List<ExtendFunctionMstModel> mstModels, List<ExtendFuncUrlBizModel> urlModels, List<AddinSqlModel> sqlModels, List<AddinAssemblyModel> assemblyModels)
        {
            string url = string.Empty;

            if (urlModels.Count > 0)
            {
                url = urlModels[0].Url;
            }
            mstModels.ForEach(data => { data.Url = url; });



            return ExtendFunctionMstFacade.SaveExtendFunc(mstModels, sqlModels, assemblyModels);
        }

        #endregion
    }
}

