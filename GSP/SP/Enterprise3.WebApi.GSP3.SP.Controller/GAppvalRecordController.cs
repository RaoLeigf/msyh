#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Controller
    * 类 名 称：			GAppvalRecordController
    * 文 件 名：			GAppvalRecordController.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using Enterprise3.Common.Model.Results;
using GSP3.SP.Service.Interface;
using GSP3.SP.Model.Domain;
using Enterprise3.WebApi.ApiControllerBase;
using System.Web.Http;
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using Enterprise3.WebApi.GSP3.SP.Model.Common;
using Enterprise3.WebApi.GSP3.SP.Model.Response;
using Enterprise3.WebApi.GBK3.BK.Model.Request;
using GBK3.BK.Model.Enums;
using GQT3.QT.Service.Interface;
using GQT3.QT.Model.Domain;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Enterprise3.WebApi.ApiControllerBase.Models;
using System.IO;
using System.Linq;
using Spring.Data.Common;
using GData3.Common.Utils.Filters;
using GSP3.SP.Model.Enums;

namespace Enterprise3.WebApi.GSP3.SP.Controller
{
    /// <summary>
    /// GAppvalRecord控制处理类
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class GAppvalRecordController : ApiBase
    {
        IGAppvalRecordService GAppvalRecordService { get; set; }


        IQTSysSetService QTSysSetService { get; set; }

        IOrgRelatitem2Service OrgRelatitem2Service { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GAppvalRecordController()
	    {
	        GAppvalRecordService = base.GetObject<IGAppvalRecordService>("GSP3.SP.Service.GAppvalRecord");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            OrgRelatitem2Service = base.GetObject<IOrgRelatitem2Service>("GQT3.QT.Service.OrgRelatitem2");
        }

        /// <summary>
        /// 分页获取待我审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUnDoRecordList([FromBody]BillRequestModel billRequest) {
            if (billRequest == null || billRequest.Uid == 0) {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            //if (string.IsNullOrEmpty(billRequest.OrgCode)) {
            //    return DCHelper.ErrorMessage("组织编码为空！");
            //}
            if (billRequest.Orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(billRequest.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (billRequest.PageIndex == 0 || billRequest.PageSize == 0) {
                return DCHelper.ErrorMessage("分页参数不正确！");
            }
            if (string.IsNullOrEmpty(billRequest.BType)) {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (billRequest.Splx_Phid == 0) {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }

            try
            {
                int total = 0;
                //若是初次加载，则获取所有组织的审批信息
                if(billRequest.IsFirst == 1)
                {
                    //RELATID = 'lg' AND PARENTORG IS null
                    var orgRelatitems = this.OrgRelatitem2Service.Find(t => t.RelatId == "lg" && t.ParentOrgId == 0).Data;
                    if(orgRelatitems != null && orgRelatitems.Count == 1)
                    {
                        billRequest.Orgid = orgRelatitems[0].OrgId;
                    }
                }
                List<AppvalRecordVo> recordVos = this.GAppvalRecordService.GetUnDoRecordList(billRequest, out total);
                return DataConverterHelper.SerializeObject(new {
                    Status= "success",
                    Data = recordVos,
                    Total = total
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 分页获取待我审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUnDoRecordList2([FromBody]BillRequestModel billRequest)
        {
            if (billRequest == null || billRequest.Uid == 0)
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            //if (string.IsNullOrEmpty(billRequest.OrgCode)) {
            //    return DCHelper.ErrorMessage("组织编码为空！");
            //}
            if (billRequest.Orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(billRequest.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (billRequest.PageIndex == 0 || billRequest.PageSize == 0)
            {
                return DCHelper.ErrorMessage("分页参数不正确！");
            }
            if (string.IsNullOrEmpty(billRequest.BType))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (billRequest.Splx_Phid == 0)
            {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }

            try
            {
                int total = 0;
                //若是初次加载，则获取所有组织的审批信息
                if (billRequest.IsFirst == 1)
                {
                    //RELATID = 'lg' AND PARENTORG IS null
                    var orgRelatitems = this.OrgRelatitem2Service.Find(t => t.RelatId == "lg" && t.ParentOrgId == 0).Data;
                    if (orgRelatitems != null && orgRelatitems.Count == 1)
                    {
                        billRequest.Orgid = orgRelatitems[0].OrgId;
                    }
                }
                List<AppvalRecordVo> recordVos = this.GAppvalRecordService.GetUnDoRecordList(billRequest, out total);
                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    Data = recordVos,
                    Total = total
                });
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 分页获取已审批单据数据
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDoneRecordList([FromBody]BillRequestModel billRequest) {
            if (billRequest == null || billRequest.Uid == 0)
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            /*if (string.IsNullOrEmpty(billRequest.OrgCode))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }*/
            if (billRequest.Orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(billRequest.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (billRequest.PageIndex == 0 || billRequest.PageSize == 0)
            {
                return DCHelper.ErrorMessage("分页参数不正确！");
            }
            if (string.IsNullOrEmpty(billRequest.BType))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (billRequest.Splx_Phid == 0)
            {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }
            try
            {
                int total = 0;
                //若是初次加载，则获取所有组织的审批信息
                if (billRequest.IsFirst == 1)
                {
                    //RELATID = 'lg' AND PARENTORG IS null
                    var orgRelatitems = this.OrgRelatitem2Service.Find(t => t.RelatId == "lg" && t.ParentOrgId == 0).Data;
                    if (orgRelatitems != null && orgRelatitems.Count == 1)
                    {
                        billRequest.Orgid = orgRelatitems[0].OrgId;
                    }
                }
                List<AppvalRecordVo> recordVos = GAppvalRecordService.GetDoneRecordList(billRequest, out total);
                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    Data = recordVos,
                    Total = total
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 获取所有审批种类对应的审批单据的数量
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetRecordListNum([FromUri]BillRequestModel billRequest)
        {
            if (billRequest == null || billRequest.Uid == 0)
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            /*if (string.IsNullOrEmpty(billRequest.OrgCode))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }*/
            if (billRequest.Orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(billRequest.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }

            try
            {
                //获取审批所有类型
                List<QTSysSetModel> procTypes = QTSysSetService.GetProcTypes();
                if(procTypes != null && procTypes.Count > 0)
                {
                    //若是初次加载，则获取所有组织的审批信息
                    if (billRequest.IsFirst == 1)
                    {
                        //RELATID = 'lg' AND PARENTORG IS null
                        var orgRelatitems = this.OrgRelatitem2Service.Find(t => t.RelatId == "lg" && t.ParentOrgId == 0).Data;
                        if (orgRelatitems != null && orgRelatitems.Count == 1)
                        {
                            billRequest.Orgid = orgRelatitems[0].OrgId;
                        }
                    }
                    foreach (var sysSet in procTypes)
                    {
                        
                        billRequest.BType = sysSet.Value;
                        billRequest.Splx_Phid = sysSet.PhId;
                        int total = 0;
                        List<AppvalRecordVo> recordVos = GAppvalRecordService.GetDoneRecordList(billRequest, out total);
                        int total2 = 0;
                        List<AppvalRecordVo> recordVos2 = GAppvalRecordService.GetUnDoRecordList(billRequest, out total2);

                        sysSet.YNum = total;
                        sysSet.NNum = total2;
                        
                    }
                }           
                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    Data = procTypes
                });
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 审批流查看
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAppvalRecord([FromUri]GAppvalRecordModel recordModel) {
            if (recordModel == null || recordModel.RefbillPhid == 0) {
                return DCHelper.ErrorMessage("申请单据ID为空！");
            }
            if (string.IsNullOrEmpty(recordModel.FBilltype)) {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (recordModel.ProcPhid == 0) {
                return DCHelper.ErrorMessage("审批流程id为空！");
            }

            try
            {
                List<GAppvalRecordModel> recordModels = GAppvalRecordService.GetAppvalRecord(recordModel.RefbillPhid, recordModel.ProcPhid, recordModel.FBilltype);
                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Data = recordModels
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }


        /// <summary>
        /// 审批流查看（不需要流程主键）
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAppvalRecordList([FromUri]GAppvalRecordModel recordModel)
        {
            if (recordModel == null || recordModel.RefbillPhid == 0)
            {
                return DCHelper.ErrorMessage("申请单据ID为空！");
            }
            if (string.IsNullOrEmpty(recordModel.FBilltype))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            try
            {
                List<GAppvalRecordModel>  recordModels = GAppvalRecordService.GetAppvalRecordList(recordModel.RefbillPhid, recordModel.FBilltype);
                if(recordModels.Count > 0)
                {
                    recordModels[0].SortNum = 0;
                    for(int j =1;j<recordModels.Count;j++)
                    {
                        recordModels[j].SortNum = j;
                        if(recordModels[j].PostPhid == recordModels[j - 1].PostPhid && recordModels[j].PostPhid!=0)
                        {
                            recordModels[j].SortNum = recordModels[j - 1].SortNum;
                        }
                    }

                    for (int i = 0; i < recordModels.Count; i++)
                    {
                        recordModels[i].SameNum = recordModels.FindAll(t => t.SortNum == i).Count;
                    }
                }
                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    Data = recordModels
                });
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }



        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostApprovalRecord([FromBody]GAppvalRecordModel recordModel) {

            if (recordModel == null) {
                return DCHelper.ErrorMessage("审批数据为空！");
            }
            if (recordModel.ProcPhid == 0)
            {
                return DCHelper.ErrorMessage("审批流程id为空！");
            }
            if (recordModel.PostPhid == 0) {
                return DCHelper.ErrorMessage("审批岗位id为空！");
            }
            if (string.IsNullOrEmpty(recordModel.FBilltype)) {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (recordModel.RefbillPhid == 0) {
                return DCHelper.ErrorMessage("关联单据为空！");
            }
            if (recordModel.PhId == 0) {
                return DCHelper.ErrorMessage("审批记录的id为空！");
            }

            try
            {
                GAppvalRecordService.PostApprovalRecord(recordModel);

                return DCHelper.SuccessMessage("审批成功！");
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 审批(带附件)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostApprovalRecordList()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            try
            {
                GAppvalRecordModel recordModel = new GAppvalRecordModel();
                recordModel.PersistentState = PersistentState.Added;
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
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/ApprovalRecord/");
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

                            string b_urlpath = "/UpLoadFiles/ApprovalRecord/" + date + "/" + newFileName;

                            QtAttachmentModel attachmentModel = new QtAttachmentModel();
                            attachmentModel.BName = b_name;
                            attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                            attachmentModel.BTable = "SP3_APPVAL_RECORD";
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
                        var item = typeof(GAppvalRecordModel).GetProperty(key);
                        if (item != null)
                        {
                            //获取数据的类型
                            var propertyType = item.PropertyType;
                            object v;
                            if (key == "NextOperators")
                            {
                                var value2 = JsonConvert.DeserializeObject<List<long>>(value);
                                v = Convert.ChangeType(value2, propertyType);
                            }
                            else if (key == "RecordPhids")
                            {
                                var value3 = JsonConvert.DeserializeObject<List<long>>(value);
                                v = Convert.ChangeType(value3, propertyType);
                            }
                            else
                            {
                                //转换数据的类型
                                v = Convert.ChangeType(value, propertyType);
                            }
                            
                            item.SetValue(recordModel, v);
                        }
                    }
                }

                if (AppInfo != null)
                {
                    MultiDelegatingDbProvider.CurrentDbProviderName = AppInfo.DbName;
                }

                if (recordModel == null)
                {
                    return DCHelper.ErrorMessage("审批数据为空！");
                }
                if (recordModel.ProcPhid == 0)
                {
                    return DCHelper.ErrorMessage("审批流程id为空！");
                }
                if (recordModel.PostPhid == 0)
                {
                    return DCHelper.ErrorMessage("审批岗位id为空！");
                }
                if (string.IsNullOrEmpty(recordModel.FBilltype))
                {
                    return DCHelper.ErrorMessage("单据类型为空！");
                }
                if (recordModel.RefbillPhid == 0)
                {
                    return DCHelper.ErrorMessage("关联单据为空！");
                }
                if (recordModel.PhId == 0)
                {
                    return DCHelper.ErrorMessage("审批记录的id为空！");
                }
                //批量审批
                if(recordModel.RecordPhids != null && recordModel.RecordPhids.Count > 0)
                {
                    var records = this.GAppvalRecordService.Find(t => recordModel.RecordPhids.Contains(t.PhId)).Data;
                    if(records != null && records.Count > 0)
                    {
                        //对批量审批的数据进行判别
                        foreach (var record in records)
                        {
                            if (!record.ProcPhid.Equals(recordModel.ProcPhid) || !record.PostPhid.Equals(recordModel.PostPhid))
                            {
                                return DCHelper.ErrorMessage("只有审批流与审批岗位相同的审批记录可以批量审批！");
                            }
                            if(record.FApproval != (byte)Approval.Wait)
                            {
                                return DCHelper.ErrorMessage("只有待审批的记录审批！");
                            }
                        }
                        //进行批量审批
                        foreach (var record in records)
                        {
                            GAppvalRecordModel pAppvalRecordModel = new GAppvalRecordModel();
                            pAppvalRecordModel = recordModel;
                            pAppvalRecordModel.PhId = record.PhId;
                            pAppvalRecordModel.RefbillPhid = record.RefbillPhid;
                            GAppvalRecordService.PostApprovalRecordList(pAppvalRecordModel, attachmentModels);
                        }
                    }
                }
                else
                {
                    GAppvalRecordService.PostApprovalRecordList(recordModel, attachmentModels);
                }               
                return DCHelper.SuccessMessage("审批成功！");
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 生成支付单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostAddPayMent([FromBody]GAppvalRecordModel recordModel) {

            if (recordModel == null || recordModel.RefbillPhid == 0) {
                return DCHelper.ErrorMessage("单据id为空！");
            }

            try
            {
                GAppvalRecordService.PostAddPayMent(recordModel);

                return DCHelper.SuccessMessage("生成支付单成功！");
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }


        /// <summary>
        /// 生成多条支付单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostAddPayMents([FromBody]GAppvalRecordModel recordModel)
        {

            if (recordModel == null || recordModel.RefbillPhidList.Count <= 0)
            {
                return DCHelper.ErrorMessage("单据id为空！");
            }
            foreach(var phid in recordModel.RefbillPhidList)
            {
                if(phid == 0)
                {
                    return DCHelper.ErrorMessage("单据id为空！");
                }
            }
            try
            {
                GAppvalRecordService.PostAddPayMents(recordModel);

                return DCHelper.SuccessMessage("生成支付单成功！");
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }


        /// <summary>
        /// 单据送审（增加审批记录）
        /// </summary>
        /// <param name="gAppval">审批发起记录</param>
        /// <returns></returns>
        [HttpPost]
        public string PostAddAppvalRecord([FromBody]GAppvalRecordModel gAppval)
        {
            try
            {
                SavedResult<long> result = new SavedResult<long>();
                if (gAppval == null)
                {
                    return DCHelper.ErrorMessage("审批数据为空！");
                }
                if (gAppval.ProcPhid == 0)
                {
                    return DCHelper.ErrorMessage("审批流程id为空！");
                }
                if (gAppval.PostPhid == 0)
                {
                    return DCHelper.ErrorMessage("审批岗位id为空！");
                }
                if (string.IsNullOrEmpty(gAppval.FBilltype))
                {
                    return DCHelper.ErrorMessage("单据类型为空！");
                }
                if(gAppval.NextOperators == null || gAppval.NextOperators.Count <= 0)
                {
                    return DCHelper.ErrorMessage("单据的下一审批人不能为空！");
                }

                result = this.GAppvalRecordService.AddAppvalRecord(gAppval);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 单据送审（增加审批记录以及附件）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostAddAppvalRecords()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            try
            {
                GAppvalRecordModel recordModel = new GAppvalRecordModel();
                recordModel.PersistentState = PersistentState.Added;
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
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/ApprovalRecord/");
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

                            string b_urlpath = "/UpLoadFiles/ApprovalRecord/" + date + "/" + newFileName;

                            QtAttachmentModel attachmentModel = new QtAttachmentModel();
                            attachmentModel.BName = b_name;
                            attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                            attachmentModel.BTable = "SP3_APPVAL_RECORD";
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
                        var item = typeof(GAppvalRecordModel).GetProperty(key);
                        if (item != null && value != "")
                        {
                            //获取数据的类型
                            var propertyType = item.PropertyType;
                            object v;
                            if (key == "NextOperators" || key == "RefbillPhidList")
                            {
                                var value2 = JsonConvert.DeserializeObject<List<long>>(value);
                                v = Convert.ChangeType(value2, propertyType);
                            }
                            else
                            {
                                //转换数据的类型
                                v = Convert.ChangeType(value, propertyType);
                            }

                            item.SetValue(recordModel, v);
                        }
                    }
                }

                if (AppInfo != null)
                {
                    MultiDelegatingDbProvider.CurrentDbProviderName = AppInfo.DbName;
                }

                SavedResult<long> result = new SavedResult<long>();
                if (recordModel == null)
                {
                    return DCHelper.ErrorMessage("审批数据为空！");
                }
                if (recordModel.ProcPhid == 0)
                {
                    return DCHelper.ErrorMessage("审批流程id为空！");
                }
                if (recordModel.PostPhid == 0)
                {
                    return DCHelper.ErrorMessage("审批岗位id为空！");
                }
                if (string.IsNullOrEmpty(recordModel.FBilltype))
                {
                    return DCHelper.ErrorMessage("单据类型为空！");
                }
                if (recordModel.NextOperators == null || recordModel.NextOperators.Count <= 0)
                {
                    return DCHelper.ErrorMessage("单据的下一审批人不能为空！");
                }
                recordModel.QtAttachments = attachmentModels;
                result = this.GAppvalRecordService.AddAppvalRecord(recordModel);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }


        /// <summary>
        /// 根据流程获取所有岗位以及操作员名字
        /// </summary>
        /// <param name="procRequest">流程对象</param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllPostsAndOpersByProc([FromUri]ProcRequestModel procRequest)
        {
            try
            {
                if(procRequest.ProcId == 0)
                {
                    throw new Exception("流程主键不能为空！");
                }
                var result = this.GAppvalRecordService.GetAllPostsAndOpersByProc(procRequest.ProcId);
                return DataConverterHelper.SerializeObject(result);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 取消送审
        /// </summary>
        /// <param name="gAppval"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostCancelAppvalRecord([FromBody]GAppvalRecordModel gAppval)
        {
            try
            {
                if (string.IsNullOrEmpty(gAppval.FBilltype))
                {
                    return DCHelper.ErrorMessage("单据类型不能为空！");
                }
                if(gAppval.RefbillPhidList == null || gAppval.RefbillPhidList.Count < 1)
                {
                    return DCHelper.ErrorMessage("单据不能为空！");
                }
                SavedResult<long> savedResult = new SavedResult<long>();
                savedResult = this.GAppvalRecordService.PostCancelAppvalRecord(gAppval);
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        [HttpGet]
        public string Getceshi([FromUri]ProcRequestModel procRequest)
        {
            try
            {
                DateTime currentTicks = DateTime.Now;
                DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                long currentMillis = (long)(currentTicks - dtFrom).TotalMilliseconds;
                return currentMillis.ToString();
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}

