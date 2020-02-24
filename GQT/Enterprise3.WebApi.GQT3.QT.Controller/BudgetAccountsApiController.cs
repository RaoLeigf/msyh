using Enterprise3.Common.Base.Criterion;
using Enterprise3.WebApi.ApiControllerBase;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class BudgetAccountsApiController : ApiBase
    {
        IBudgetAccountsService BudgetAccountsService { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        public BudgetAccountsApiController()
        {
            BudgetAccountsService = base.GetObject<IBudgetAccountsService>("GQT3.QT.Service.BudgetAccounts");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBudgetAccountsList()
        {

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<Int64>.NotEq("PhId", 0));

            var result = BudgetAccountsService.Find(dicWhere).Data;
            //return DataConverterHelper.EntityListToJson<BankAccountModel>(bankAccountsList, bankAccountsList.Count);
            return DCHelper.ModelListToJson<BudgetAccountsModel>(result, result.Count);
        }
    }
}
