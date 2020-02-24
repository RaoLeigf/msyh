using Enterprise3.NHORM.Facade;
using NG3.Addin.Facade.Interface;
using NG3.Addin.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Addin.Model.Domain.BusinessModel;
using Enterprise3.Common.Model.Results;
using NG3.Addin.Rule.Interface;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using NG3.Addin.Core.Parameter;
using NG3.Addin.Core.Interceptor;

namespace NG3.Addin.Facade
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicSettingFacade : EntFacadeBase<AddinOperatorModel>, IBasicSettingFacade
    {

        private InterceptorExecutor executor;

        #region 类变量及属性
        /// <summary>
        /// AddinSql业务逻辑处理对象
        /// </summary>
        IAddinOperatorRule AddinOperatorRule
        {
            get
            {
                if (CurrentRule == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentRule as IAddinOperatorRule;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult DeleteAddinOperator(long id)
        {
            var result = AddinOperatorRule.Delete<Int64>(id);
            DeletedResult dresult = new DeletedResult();
            dresult.DelRows = result;
            dresult.Status = "success"; //返回状态

            return dresult;
        }

        #endregion

        #region 实现类

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<SupportVariableBizModel> GetBizVariables()
        {
            return ParameterManager.GetBizVariables();
        }

        /// <summary>
        /// 取出服务的参数列表
        /// </summary>
        /// <returns></returns>
        public IList<ServiceUIParamBizModel> GetServiceRequestParameters()
        {
            return executor.GetServiceRequestParameters();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<SupportVariableBizModel> GetSystemVariables()
        {
            return ParameterManager.GetSystemVariables();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorModel"></param>
        /// <returns></returns>
        public SavedResult<long> SaveAddinOperator(IList<AddinOperatorModel> operatorModel)
        {
            var result = AddinOperatorRule.Save<Int64>(operatorModel.ToList());
            executor.ReloadOperator();
            return result;
        }

        #endregion
    }
}