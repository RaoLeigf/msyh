using Newtonsoft.Json;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
using NG3.Web.Mvc;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using SUP.Common.Facade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SUP.Common.Controller
{
    public class IndividualFormController : AFController
    {
        private IIndividualFormFacade proxy;
        public IndividualFormController()
        {
            proxy = AopObjectProxy.GetObject<IIndividualFormFacade>(new IndividualFormFacade());
        }


        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index()
        {
            this.GridStateIDs = new string[] { "SupIndividualFormListgrid" };
            return View("IndividualFormList");
        }

        public string Save(string data)
        {

            try
            {
                DataTable dt = DataConverterHelper.ToDataTable(data, "select * from fg_individualform");
                int iret = proxy.Save(dt);

                ResponseResult result = new ResponseResult();
                if (iret > 0)
                {
                    result.Status = ResponseStatus.Success;
                }
                else
                {
                    result.Status = ResponseStatus.Error;
                }

                string response = JsonConvert.SerializeObject(result);
                return response;
            }
            catch (Exception ex)
            {
                ResponseResult result = new ResponseResult();
                result.Status = ResponseStatus.Error;
                result.Msg = ex.Message;
                string response = JsonConvert.SerializeObject(result);
                return response;
                //return "{Status : \"error\",Msg:'" + ex.Message + "'}";              
            }
        }

        public string GetIndividualFormList(string bustype)
        {
            DataTable dt = proxy.GetIndividualFormList(bustype);

            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);


            return json;
        }

    }
}
