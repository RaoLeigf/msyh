using Enterprise3.NHORM.Service;
using NG3.Addin.Model.Domain;
using NG3.Addin.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NG3.Addin.Model.Domain.BusinessModel;
using Enterprise3.Common.Model.Results;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using NG3.Addin.Facade.Interface;

namespace NG3.Addin.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicSettingService : EntServiceBase<AddinOperatorModel>, IBasicSettingService
    {

        IBasicSettingFacade BasicSettingFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentFacade as IBasicSettingFacade;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult DeleteAddinOperator(long id)
        {
            return BasicSettingFacade.DeleteAddinOperator(id);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<SupportVariableBizModel> GetBizVariables()
        {
            return BasicSettingFacade.GetBizVariables();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<ServiceUIParamBizModel> GetServiceRequestParameters()
        {
            return BasicSettingFacade.GetServiceRequestParameters();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<SupportVariableBizModel> GetSystemVariables()
        {
            return BasicSettingFacade.GetSystemVariables();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorModel"></param>
        /// <returns></returns>
        public SavedResult<long> SaveAddinOperator(IList<AddinOperatorModel> operatorModel)
        {
            return BasicSettingFacade.SaveAddinOperator(operatorModel);
        }
    }
}
