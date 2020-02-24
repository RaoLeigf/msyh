using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using GData3.Common.Model;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
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
    public class BankAccountApiController : ApiBase
    {
        IBankAccountService BankAccountService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BankAccountApiController()
        {
            BankAccountService = base.GetObject<IBankAccountService>("GQT3.QT.Service.BankAccount");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetBankAccountList([FromUri]long OrgPhid, [FromUri]string selectStr)
        {

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<Int64>.Eq("OrgPhid", OrgPhid));

            var data = BankAccountService.Find(dicWhere).Data.ToList();
            if (data.Count > 0 && !string.IsNullOrEmpty(selectStr))
            {
                data = data.FindAll(t => t.FAccount.IndexOf(selectStr) > -1 || t.FBankname.IndexOf(selectStr) > -1);
            }
            if (data.Count > 0)
            {
                data = BankAccountService.GetOrgName(data, OrgPhid);
            }

            //return DataConverterHelper.EntityListToJson<BankAccountModel>(bankAccountsList, bankAccountsList.Count); 
            return DCHelper.ModelListToJson<BankAccountModel>(data, data.Count);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostSave([FromBody]InfoBaseModel<List<BankAccountModel>> bankAccount)
        {
            /*UnChanged = 0,
            Added = 1,
            Modified = 2,
            Deleted = 3*/
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            List<long> modifyPhids = new List<long>();
            foreach (BankAccountModel data in bankAccount.infoData)
            {
                if (data.PersistentState == PersistentState.Modified || data.PersistentState == PersistentState.Deleted)
                {
                    modifyPhids.Add(data.PhId);
                }
            }
            if (modifyPhids.Count > 0)
            {
                List<long> usePhids = BankAccountService.judgeIfUse(modifyPhids);
                if (usePhids.Count > 0)
                {
                    string Msg = "银行账户名称为";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", usePhids));
                    IList<BankAccountModel> bankAccounts = BankAccountService.Find(dic).Data;
                    foreach (BankAccountModel a in bankAccounts)
                    {
                        Msg = Msg + a.FBankname + ",";
                    }
                    var data2 = bankAccount.infoData.Find(x => x.PhId == usePhids[0]);//因为只能删除或修改所以单据的状态只能是一个
                    if (data2.PersistentState == PersistentState.Deleted)
                    {
                        Msg = Msg.Substring(0, Msg.Length - 1) + "已被引用不能删除";
                    }
                    else
                    {
                        Msg = Msg.Substring(0, Msg.Length - 1) + "已被引用不能修改";
                    }
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = Msg;
                    return DataConverterHelper.SerializeObject(savedresult);
                }
            }

            try
            {
                savedresult = BankAccountService.Save<Int64>(bankAccount.infoData, "");
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }

            return DataConverterHelper.SerializeObject(savedresult);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostDelete([FromBody]InfoBaseModel<List<long>> phids)
        {
            var deletedresult = new DeletedResult();
            if (phids.infoData.Count > 0)
            {
                List<long> usePhids = BankAccountService.judgeIfUse(phids.infoData);
                if (usePhids.Count > 0)
                {
                    string Msg = "银行账户名称为";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<List<long>>.In("PhId", usePhids));
                    IList<BankAccountModel> bankAccounts = BankAccountService.Find(dic).Data;
                    foreach (BankAccountModel bankAccount in bankAccounts)
                    {
                        Msg = Msg + bankAccount.FBankname + ",";
                    }
                    Msg = Msg.Substring(0, Msg.Length - 1) + "已被引用不能删除";
                    deletedresult.Status = ResponseStatus.Error;
                    deletedresult.Msg = Msg;
                }
                else
                {
                    for (var i = 0; i < phids.infoData.Count; i++)
                    {
                        deletedresult = BankAccountService.Delete<System.Int64>(phids.infoData[i]);
                        //BankAccountService.Find(t => phids.infoData.Contains(t.PhId));
                    }
                }
            }

            return DataConverterHelper.SerializeObject(deletedresult);
        }
        /// <summary>
        ///根据其列表获取银行名
        /// </summary>
        /// <param name="OrgPhid"></param>
        /// <param name="selectStr"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBankAccountareaList([FromUri]long OrgPhid, [FromUri]string selectStr)
        {

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<Int64>.Eq("OrgPhid", OrgPhid));

            var data = BankAccountService.Find(dicWhere).Data.ToList();
            List<string> list = null;
            if (data.Count > 0 && !string.IsNullOrEmpty(selectStr))
            {
                data = data.FindAll(t => t.FAccount.IndexOf(selectStr) > -1 || t.FBankname.IndexOf(selectStr) > -1);
                var dataresult = data.Select(m => m.BankName);
                list = dataresult.Distinct().ToList();
                //string[] area = (data.Select(m => m.BankName).ToString()).Split(',');
                //List<string> list = new List<string>();
                //foreach (var item in area)
                //{
                //    if (!list.Contains(item))
                //    {
                //        list.Add(item);
                //    }
                //}
            }
            if (list.Count > 0)
            {
                return DCHelper.ModelListToJson<string>(list, list.Count);

            }
            else
            {
                return DCHelper.ModelListToJson<string>(list, 0);

            }
        }
    }
}
