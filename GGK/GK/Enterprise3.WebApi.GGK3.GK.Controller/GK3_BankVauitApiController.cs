using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GSP3.SP.Model.Common;
using GData3.Common.Model;
using GGK3.GK.Model.Domain;
using GGK3.GK.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GGK3.GK.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public class GK3_BankVauitApiController : ApiBase
    {
        IGK3_BankVauitService GK3_BankVauitService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GK3_BankVauitApiController()
        {
            GK3_BankVauitService = base.GetObject<IGK3_BankVauitService>("GGK3.GK.Service.GK3_BankVauit");
        }
        /// <summary>
        /// 获取当前组织的所有银行名称
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public string GetBank(long orgid)
        {
            var dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
           new CreateCriteria(dic).Add(ORMRestrictions<Int64>.Eq("OrgId", orgid));
            var Query = GK3_BankVauitService.Find(dic).Data.ToList();
            return DataConverterHelper.SerializeObject(Query);
        }
        /// <summary>
        /// 银行信息存储
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostBank([FromBody]BaseSingleModel list)
        {
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            if (list!=null)
            {
                if (list.gklist.Count > 0)
                {

                    foreach (var item in list.gklist)
                    {
                        if (item.PhId == 0)
                        {
                            item.PersistentState = PersistentState.Added;
                            var dic = new Dictionary<string, object>();
                            new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                            var Query = GK3_BankVauitService.Find(dic).Data.ToList();
                            foreach (var items in Query)
                            {
                                if (items.Name!=item.Name)
                                {
                                    try
                                    {
                                        var Result = GK3_BankVauitService.Save<Int64>(item, "");
                                        savedresult = Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        savedresult.Status = ResponseStatus.Error;
                                        savedresult.Msg = ex.Message.ToString();
                                    }
                                }
                                else
                                {
                                    return DCHelper.ErrorMessage("" + item.Name + "" + "当前银行名已存在请选择其他！");
                                }
                            }
                           
                        }
                        else
                        {
                            item.PersistentState = PersistentState.Modified;
                            var dic = new Dictionary<string, object>();
                            new CreateCriteria(dic).Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                            var Query = GK3_BankVauitService.Find(dic).Data.ToList();
                            foreach (var items in Query)
                            {
                                if (items.Name!=item.Name)
                                {
                                    try
                                    {
                                        var Result = GK3_BankVauitService.Save<Int64>(item, "");
                                        savedresult = Result;
                                    }
                                    catch (Exception ex)
                                    {
                                        savedresult.Status = ResponseStatus.Error;
                                        savedresult.Msg = ex.Message.ToString();
                                    }
                                }
                                else
                                {
                                    return DCHelper.ErrorMessage(""+ item.Name+ ""+"当前银行名已存在请选择其他！");
                                }
                            }
                           
                        }
                    }

                }
                else
                {
                    return DCHelper.ErrorMessage("请求参数为空！");
                }
            }
            else
            {
                return DCHelper.ErrorMessage("请求参数为空！");
            }

            return DataConverterHelper.SerializeObject(savedresult);
        }
    }
}
