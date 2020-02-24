using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;
using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Facade.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBasicSettingFacade : IEntFacadeBase<AddinOperatorModel>
    {
        /// <summary>
        /// 保存操作员
        /// </summary>
        /// <param name="operatorModel"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveAddinOperator(IList<AddinOperatorModel> operatorModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeletedResult DeleteAddinOperator(long id);


        /// <summary>
        /// 系统参数
        /// </summary>
        /// <returns></returns>
        IList<SupportVariableBizModel> GetSystemVariables();
        /// <summary>
        /// 业务参数
        /// </summary>
        /// <returns></returns>
        IList<SupportVariableBizModel> GetBizVariables();


        /// <summary>
        /// 服务的UI参数
        /// </summary>
        /// <returns></returns>
        IList<ServiceUIParamBizModel> GetServiceRequestParameters();
    }
}
