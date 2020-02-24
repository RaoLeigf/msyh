using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Enterprise3.WebApi.GYS3.YS.Model;
using Enterprise3.WebApi.GYS3.YS.Model.Request;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GSP3.SP.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Extra;
using GYS3.YS.Service.Interface;
using Newtonsoft.Json;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Enterprise3.WebApi.GYS3.YS.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class ExpenseMstApiController: ApiBase
    {
        IExpenseMstService ExpenseMstService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        IQtAttachmentService QtAttachmentService { get; set; }

        IGAppvalProcService GAppvalProcService { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExpenseMstApiController()
        {
            ExpenseMstService = base.GetObject<IExpenseMstService>("GYS3.YS.Service.ExpenseMst");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");
            GAppvalProcService = base.GetObject<IGAppvalProcService>("GSP3.SP.Service.GAppvalProc");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetExpenseMstList([FromUri]ExpenseListRequestModel param)
        {
            //string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary

            //var userId = System.Web.HttpContext.Current.Request.Params["userId"];
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            /*var dicWhereDept = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDept)
                .Add(ORMRestrictions<string>.Eq("Dwdm", param.UserCode)).Add(ORMRestrictions<string>.Eq("Dylx", "97")); //闭区间
            var deptList = CorrespondenceSettingsService.Find(dicWhereDept);
            List<string> deptL = new List<string>();
            for (var i = 0; i < deptList.Data.Count; i++)
            {
                deptL.Add(deptList.Data[i].Dydm);
            }
            new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<IList<String>>.In("FBudgetDept", deptL));*/

            new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<string>.Eq("FDeclarationDept", param.FDeclarationDept));
            if (param.FApprovestatus.Count>0)
            {
                new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<List<string>>.In("FApprovestatus", param.FApprovestatus));
            }
            if (param.FStartdate!=null)
            {
                new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<System.DateTime?>.Ge("FDateofdeclaration", param.FStartdate));
            }
            if (param.FEnddate != null)
            {
                new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<System.DateTime?>.Le("FDateofdeclaration", param.FEnddate));
            }
            if (param.MinAmount != 0)
            {
                new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<System.Decimal>.Ge("FSurplusamount", param.MinAmount));
            }
            if (param.MaxAmount != 0)
            {
                new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<System.Decimal>.Le("FSurplusamount", param.MaxAmount));
            }
            if (!string.IsNullOrEmpty(param.searchValue))
            {
                var dic1 = new Dictionary<string, object>();
                var dic2 = new Dictionary<string, object>();
                new CreateCriteria(dic1)
                   .Add(ORMRestrictions<String>.Like("FPerformevaltype", param.searchValue));
                new CreateCriteria(dic2)
                  .Add(ORMRestrictions<String>.Like("FProjname", param.searchValue));
                new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions.Or(dic1, dic2));
            }
            //根据单据号进行排序
            var result = ExpenseMstService.LoadWithPage(param.PageIndex, param.PageSize, dicWhere, new string[] { "FPerformevaltype Desc" });

            if(param.ProcPhid != 0)
            {
                var expenseList = ExpenseMstService.Find(dicWhere, new string[] { "FPerformevaltype Desc" }).Data;
                if(expenseList != null && expenseList.Count > 0)
                {
                    List<string> orgList = expenseList.ToList().Select(t => t.FBudgetDept).Distinct().ToList();
                    if (orgList != null && orgList.Count > 0)
                    {
                        var procList = this.GAppvalProcService.Find(t => orgList.Contains(t.OrgCode)).Data;
                        if (procList != null && procList.Count > 0)
                        {
                            //可以选取相同审批流的打上标记
                            foreach (var res in expenseList)
                            {
                                if (res.FApprovestatus == "1" && procList.ToList().Find(t => t.OrgCode == res.FBudgetDept && t.PhId == param.ProcPhid) != null)
                                {
                                    res.BatchPracBz = 1;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            expenseList = expenseList.ToList().FindAll(t => t.BatchPracBz == 1);
                        }
                    }
                }

                result.Results = expenseList.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList();
                result.TotalItems = expenseList.Count;
            }

            return DCHelper.ModelListToJson<ExpenseMstModel>(result.Results, (Int32)result.TotalItems);
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetExpenseMstInfo([FromUri]long id)
        {
            ExpenseAllModel ExpenseAll = new ExpenseAllModel();
            ExpenseAll.ExpenseMst = ExpenseMstService.Find(id).Data;
            ExpenseAll.ExpenseDtls = ExpenseMstService.FindExpenseDtlByForeignKey(id).Data.ToList();
            ExpenseAll.ExpenseHxs= ExpenseMstService.FindExpenseHxByForeignKey(id).Data.ToList();
            //用款计划对应的附件
            var qtAttachments = this.QtAttachmentService.Find(t => t.RelPhid == id && t.BTable == "YS3_EXPENSEMST").Data;
            if(qtAttachments != null && qtAttachments.Count > 0)
            {
                ExpenseAll.QtAttachments = qtAttachments.ToList();
            }
            return DataConverterHelper.SerializeObject(ExpenseAll);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostSave([FromBody]ExpenseAllRequestModel ExpenseAllData)
        {
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                if (ExpenseAllData.ExpenseMst != null)
                {
                    if (ExpenseAllData.ExpenseMst.PhId == 0)
                    {
                        ExpenseAllData.ExpenseMst.PersistentState = PersistentState.Added;
                        //ExpenseAllData.ExpenseMst.FSurplusamount += decimal.Parse(ExpenseMstService.SumFSurplusamount(ExpenseAllData.ExpenseMst.FProjcode));
                    }
                    else
                    {
                        if (ExpenseAllData.ExpenseMst.PersistentState != PersistentState.Deleted)
                        {
                            ExpenseAllData.ExpenseMst.PersistentState = PersistentState.Modified;
                        }
                    }
                }
                if (ExpenseAllData.ExpenseDtls != null && ExpenseAllData.ExpenseDtls.Count > 0)
                {
                    foreach (var dtl in ExpenseAllData.ExpenseDtls)
                    {
                        if (dtl.PhId == 0)
                        {
                            dtl.PersistentState = PersistentState.Added;
                        }
                        else
                        {
                            if (dtl.PersistentState != PersistentState.Deleted)
                            {
                                dtl.PersistentState = PersistentState.Modified;
                            }
                        }
                    }
                }

                savedresult = ExpenseMstService.SaveExpenseMst(ExpenseAllData.ExpenseMst, ExpenseAllData.ExpenseDtls, ExpenseAllData.ExpenseMst.FProjAmount.ToString(), ExpenseAllData.beforeSum, ExpenseAllData.beforeFReturnamount, ExpenseAllData.Ifreturn);
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 用款计划保存附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSave2()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            List<QtAttachmentModel> oldattachmentModels = new List<QtAttachmentModel>();
            //具体数据对象
            long expensePhid = 0;
            //判断form表单类型是否正确
            if (!Request.Content.IsMimeMultipartContent())
            {
                var data1 = new
                {
                    Status = ResponseStatus.Error,
                    Msg = "请求数据不是multipart/form-data类型",
                    Data = ""
                };
                return DataConverterHelper.SerializeObject(data1);
            }
            //I6WebAppInfo i6AppInfo = (I6WebAppInfo)HttpContext.Current.Session["NGWebAppInfo"] ?? null;
            //获取AppInfo值 头部信息记录
            var base64EncodedBytes = Convert.FromBase64String(HttpContext.Current.Request.Headers.GetValues("AppInfo").First());
            var jsonText = Encoding.UTF8.GetString(base64EncodedBytes);
            var AppInfo = JsonConvert.DeserializeObject<AppInfoBase>(jsonText);


            //如果路径不存在,创建路径
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/ExpenseMst/");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = Path.Combine(root, date);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
            var contentsList = multipartMemoryStreamProvider.Contents;

            foreach (var content in contentsList)
            {
                //通过判断fileName是否为空,是否为文件
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    //处理文件名字符串
                    string fileName = content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    using (Stream stream = await content.ReadAsStreamAsync())
                    {
                        //文件如果大于100MB  提示不允许
                        if (stream.Length > 104857600)
                        {
                            return DCHelper.ErrorMessage("上传的文件不能大于100MB！");
                        }
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        stream.Seek(0, SeekOrigin.Begin);

                        //获取对应文件后缀名
                        string extension = Path.GetExtension(fileName);
                        //获取文件名
                        string b_name = Path.GetFileName(fileName);

                        //修改文件名
                        string newFileName = Guid.NewGuid().ToString("N") + extension;
                        string uploadPath = Path.Combine(filePath, newFileName);

                        //保存文件
                        MemoryStream ms = new MemoryStream(bytes);
                        FileStream fs = new FileStream(uploadPath, FileMode.Create);
                        ms.WriteTo(fs);
                        ms.Close();
                        fs.Close();

                        string b_urlpath = "/UpLoadFiles/ExpenseMst/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "YS3_EXPENSEMST";
                        attachmentModel.BType = extension;
                        attachmentModel.BUrlpath = b_urlpath;
                        attachmentModel.PersistentState = PersistentState.Added;
                        attachmentModels.Add(attachmentModel);
                    }
                }
                else
                {
                    //获取键值对值,并通过反射获取对象中的属性
                    string key = content.Headers.ContentDisposition.Name.Replace("\"", string.Empty);
                    string value = await content.ReadAsStringAsync();
                    //取项目主键
                    //projectPhid = long.Parse(value);
                    //取用款计划主键
                    if (key == "PhId")
                    {
                        expensePhid = long.Parse(value);
                    }
                    else if (key == "OldAttachments")
                    {
                        var value2 = JsonConvert.DeserializeObject<List<QtAttachmentModel>>(value);
                        oldattachmentModels = value2;
                    }
                    ////取用款计划主键
                    //expensePhid = long.Parse(value);
                }
            }

            if (expensePhid <= 0)
            {
                return DCHelper.ErrorMessage("用款计划保存附件失败！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                //原有的附件要删除
                IList<QtAttachmentModel> oldAttachments = new List<QtAttachmentModel>();
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "YS3_EXPENSEMST" && t.RelPhid == expensePhid).Data;
                if (oldAttachments != null && oldAttachments.Count > 0)
                {
                    foreach (var oldAtt in oldAttachments)
                    {
                        oldAtt.PersistentState = PersistentState.Deleted;
                    }
                    this.QtAttachmentService.Save<long>(oldAttachments, "");
                }
                if (attachmentModels != null && attachmentModels.Count > 0)
                {
                    foreach (var att in attachmentModels)
                    {
                        att.RelPhid = expensePhid;
                        att.BTable = "YS3_EXPENSEMST";
                        att.PersistentState = PersistentState.Added;
                    }
                    //savedResult = this.QtAttachmentService.Save<long>(attachmentModels, "");
                }
                if (oldattachmentModels != null && oldattachmentModels.Count > 0)
                {
                    foreach (var oldAtt in oldattachmentModels)
                    {
                        oldAtt.RelPhid = expensePhid;
                        oldAtt.BTable = "YS3_EXPENSEMST";
                        oldAtt.PersistentState = PersistentState.Added;
                        attachmentModels.Add(oldAtt);
                    }
                }
                if (attachmentModels != null && attachmentModels.Count > 0)
                {
                    savedResult = this.QtAttachmentService.Save<long>(attachmentModels, "");
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetDelete([FromUri]long id)
        {
            var deletedresult = ExpenseMstService.Delete2(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 删除额度返还单据（额度逆返还）
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetDeleteReturn([FromUri]long id)
        {
            var deletedresult = ExpenseMstService.DeleteReturn(id);

            return DataConverterHelper.SerializeObject(deletedresult);
        }

        /// <summary>
        /// 根据预算主键取各种信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetinfoByProjCode([FromUri]long YsPhid)
        {

            var result = ExpenseMstService.GetinfoByProjCode(YsPhid);

            return DataConverterHelper.SerializeObject(result);
        }
        /// <summary>
        /// 根据预算部门取项目支出预算申报总数、申报总额、有哪些项目及金额饼图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetinfoByDept([FromUri]BaseListModel param)
        {

            var result = ExpenseMstService.GetinfoByDept(param.orgCode, param.Year);

            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 保存额度核销数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostSaveHX([FromBody]BaseInfoModel<List<ExpenseHxModel>> param)
        {
            if(param.infoData == null)
            {
                return DCHelper.ErrorMessage("保存的数据不能为空！");
            }
            try
            {
                var addinfo = new List<ExpenseHxModel>();
                var updateinfo = new List<ExpenseHxModel>();
                var deleteinfo = new List<string>();
                foreach(ExpenseHxModel a in param.infoData)
                {
                    if (a.PhId == 0)
                    {
                        a.PersistentState = PersistentState.Added;
                        addinfo.Add(a);
                    }
                    else
                    {
                        if(a.PersistentState== PersistentState.Deleted)
                        {
                            deleteinfo.Add(a.PhId.ToString());
                        }
                        else
                        {
                            a.PersistentState = PersistentState.Modified;
                            updateinfo.Add(a);
                        }
                    }
                }
                CommonResult savedresult = new CommonResult();
                savedresult = ExpenseMstService.SaveHX(addinfo, updateinfo, deleteinfo);
                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 额度核销执行完毕确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveHXgo([FromBody]ExpenseAllRequestModel ExpenseAllData)
        {
            if(ExpenseAllData.ExpenseMst == null || ExpenseAllData.ExpenseMst.PhId == 0)
            {
                return DCHelper.ErrorMessage("参数传递有误！");
            }
            try
            {
                CommonResult savedresult = new CommonResult();
                savedresult = ExpenseMstService.SaveHXgo(ExpenseAllData.ExpenseMst.PhId, ExpenseAllData.ExpenseMst.FPlayamount, ExpenseAllData.ExpenseMst.FReturnamount, ExpenseAllData.ExpenseDtls);
                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }

        }

        /// <summary>
        /// 额度核销撤销
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string SaveHXreturn([FromBody]ExpenseAllRequestModel ExpenseAllData)
        {
            if (ExpenseAllData.ExpenseMst == null || ExpenseAllData.ExpenseMst.PhId == 0)
            {
                return DCHelper.ErrorMessage("参数传递有误！");
            }
            try
            {
                CommonResult savedresult = new CommonResult();
                savedresult = ExpenseMstService.SaveHXreturn(ExpenseAllData.ExpenseMst.PhId, ExpenseAllData.ExpenseMst.FPlayamount);
                return DataConverterHelper.SerializeObject(savedresult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据用款计划的主键获取相关数据集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetExpenseAllModel([FromUri]ExpenseAllRequestModel param)
        {
            if(param.ExpensePhId == 0)
            {
                return DCHelper.ErrorMessage("用款计划主键不能为空！");
            }
            try
            {
                var result = this.ExpenseMstService.GetExpenseAllModel(param.ExpensePhId);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "数据获取成功",
                    Data = result
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}
