#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Controller
    * 类 名 称：			PaymentMstController
    * 文 件 名：			PaymentMstController.cs
    * 创建时间：			2019/5/15 
    * 作    者：			吾丰明    
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

using GBK3.BK.Service.Interface;
using Enterprise3.WebApi.ApiControllerBase;
using System.Web.Http;
using GBK3.BK.Model.Domain;
using Enterprise3.WebApi.GBK3.BK.Model.Request;
using System.Linq;
using Enterprise3.Common.Base.Criterion;
using GBK3.BK.Model.Extend;
using GBK3.BK.Model.Enums;
using GYS3.YS.Service.Interface;
using System.Threading.Tasks;
using GQT3.QT.Model.Domain;
using System.Net.Http;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Enterprise3.WebApi.ApiControllerBase.Models;
using System.IO;
using Spring.Data.Common;
using GQT3.QT.Service.Interface;
using GData3.Common.Utils.Filters;
using GYS3.YS.Model.Domain;
using GSP3.SP.Service.Interface;
using GSP3.SP.Model.Enums;

namespace Enterprise3.WebApi.GBK3.BK.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class PaymentMstApiController : ApiBase
    {
        IPaymentMstService PaymentMstService { get; set; }

        IBudgetMstService BudgetMstService { get; set; }

        IQtAttachmentService QtAttachmentService { get; set; }

        IGAppvalProcService GAppvalProcService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PaymentMstApiController()
	    {
	        PaymentMstService = base.GetObject<IPaymentMstService>("GBK3.BK.Service.PaymentMst");

            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");

            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");

            GAppvalProcService = base.GetObject<IGAppvalProcService>("GSP3.SP.Service.GAppvalProc");
        }

        /// <summary>
        /// 获取资金拨付列表
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPaymentMstList([FromUri]PaymentModel paramters)
        {
            try
            {
                PaymentMstModel payment = new PaymentMstModel();
                payment.FOrgphid = paramters.FOrgphid;
                payment.FYear = paramters.FYear;
                payment.FDepphid = paramters.FDepphid;
                payment.FName = paramters.FName;
                payment.StartDate = paramters.StartDate;
                payment.EndDate = paramters.EndDate;
                payment.MinAmount = paramters.MinAmount;
                payment.MaxAmount = paramters.MaxAmount;
                payment.ApprovalBzs = paramters.ApprovalBzs;
                payment.PayBzs = paramters.PayBzs;
                if (payment.FOrgphid < 1)
                {
                    throw new Exception("组织信息传递不正确！");
                }
                if (payment.FDepphid < 1)
                {
                    throw new Exception("部门信息传递不正确！");
                }               
                var result = this.PaymentMstService.GetPaymentMstList(paramters.PageIndex, payment, paramters.PageSize);
                var count = result.Count;
                var pageResult = result.Skip((paramters.PageIndex - 1) * paramters.PageSize).Take(paramters.PageSize).ToList();
                return DCHelper.ModelListToJson(pageResult, count);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 点击申请单显示详情
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPaymentMst([FromUri]BaseListModel paramters)
        {
            try
            {
                if (string.IsNullOrEmpty(paramters.fPhId))
                {
                    throw new Exception("申请单主键不能为空！");
                }
                var result = this.PaymentMstService.GetPaymentMst(paramters.fPhId);
                if(result !=null && result.PaymentXmDtl != null && result.PaymentXmDtl.Count > 0)
                {
                    foreach(var paymentxm in result.PaymentXmDtl)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        var res = this.PaymentMstService.GetSummary(paymentxm.PaymentXm.XmMstPhid.ToString(), paymentxm.PaymentXm.MstPhid);
                        decimal used = 0;
                        if (res != null && res.Count > 0)
                        {
                            foreach (var xm in res)
                            {
                                used = used + (decimal)xm.Value;
                                if (xm.Key == "Use")
                                {
                                    paymentxm.PaymentXm.Use = (decimal)xm.Value;
                                }
                                if (xm.Key == "Frozen")
                                {
                                    paymentxm.PaymentXm.Frozen = (decimal)xm.Value;
                                }
                            }
                        }
                        var total = this.BudgetMstService.GetDxbzDtl(paymentxm.PaymentXm.XmMstPhid, long.Parse(paramters.orgid));
                        Type type = total.GetType();
                        decimal sum = (decimal)type.GetProperty("FAmount").GetValue(total);
                        decimal surplus = sum - used;
                        paymentxm.PaymentXm.Sum = sum;
                        paymentxm.PaymentXm.Surplus = surplus;
                    }
                }
                return DataConverterHelper.SerializeObject(result);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 删除多条申请单
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDelete([FromBody]BaseListModel paramters)
        {
            try
            {
                List<long> fCodes = new List<long>();
                if (paramters.fPhIdList.Count() < 1)
                {
                    return DCHelper.ErrorMessage("删除的申请单编码列表不能为空！");
                }
                else
                {                    
                    for(int i = 0; i < paramters.fPhIdList.Count(); i++)
                    {
                        fCodes.Add(long.Parse(paramters.fPhIdList[i]));
                    }
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<long>>.In("PhId", fCodes));
                    var result = this.PaymentMstService.Find(dic).Data;
                    if(result.Count > 0)
                    {
                        string str = "";
                        foreach(var payment in result)
                        {
                            if(payment.FApproval!=0 && payment.FApproval != 2)
                            {
                                str = str + payment.FCode + ",";
                            }
                        }
                        if(str.Length > 0)
                        {
                            return DCHelper.ErrorMessage(str + "这些单据无法进行删除操作！");
                        }
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("删除的申请单不存在！");
                    }
                }
                var deleteResult = this.PaymentMstService.DeleteSignle(fCodes);
                if(deleteResult > 0)
                {
                    var data = new
                    {
                        Status = ResponseStatus.Success,
                        Msg = "删除成功！",
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else
                {
                    var data = new
                    {
                        Status = ResponseStatus.Error,
                        Msg = "删除失败！",
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        
        /// <summary>
        /// 新增申请单
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostAdd([FromBody]BaseInfoModel<PaymentMstAndXmModel> paramters)
        {
            try
            {
                if(paramters.infoData != null)
                {
                    PaymentMstAndXmModel paymentMstAndXm = paramters.infoData;
                    var result = this.PaymentMstService.AddSignle(paymentMstAndXm);
                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {
                    throw new Exception("参数传递有误！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 新增申请单（有附件的）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostAdd2()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            try
            {
                PaymentMstAndXmModel paymentMstAndXm = new PaymentMstAndXmModel();
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
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/BKPayment/");
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

                            string b_urlpath = "/UpLoadFiles/BKPayment/" + date + "/" + newFileName;

                            //如果上传文件为封面图片,将图片信息设置到新闻对象中
                            //if (".jpg".CompareTo(extension) == 0 || ".gif".CompareTo(extension) == 0 || ".png".CompareTo(extension) == 0 || ".jpeg".CompareTo(extension) == 0 || ".bmp".CompareTo(extension) == 0)
                            //{
                            //    newsModel.AttachmentName = b_name;
                            //    newsModel.AttachmentSize = decimal.Round((decimal)stream.Length / 1024, 2);
                            //    newsModel.Picpath = b_urlpath;

                            //}
                            //else {
                            QtAttachmentModel attachmentModel = new QtAttachmentModel();
                            attachmentModel.BName = b_name;
                            attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                            attachmentModel.BTable = "BK3_PAYMENT_MST";
                            attachmentModel.BType = extension;
                            attachmentModel.BUrlpath = b_urlpath;
                            attachmentModel.PersistentState = PersistentState.Added;
                            attachmentModels.Add(attachmentModel);
                            //}
                        }
                    }
                    else
                    {
                        //获取键值对值,并通过反射获取对象中的属性
                        string key = content.Headers.ContentDisposition.Name.Replace("\"", string.Empty);
                        string value = await content.ReadAsStringAsync();
                        var item = typeof(PaymentMstAndXmModel).GetProperty(key);
                        if (item != null)
                        {
                            //获取数据的类型
                            var propertyType = item.PropertyType;
                            //转换数据的类型
                            object v = Convert.ChangeType(value, propertyType);
                            item.SetValue(paymentMstAndXm, v);
                        }
                    }
                }

                if (AppInfo != null)
                {
                    MultiDelegatingDbProvider.CurrentDbProviderName = AppInfo.DbName;
                }
                if (paymentMstAndXm != null)
                {
                    var result = this.PaymentMstService.AddSignle(paymentMstAndXm);
                    if(result.KeyCodes.Count > 0)
                    {
                        long phid = result.KeyCodes[0];
                        if(attachmentModels.Count > 0)
                        {
                            foreach (var attach in attachmentModels)
                            {
                                attach.RelPhid = phid;
                            }
                        }
                        this.QtAttachmentService.Save<Int64>(attachmentModels, "");
                    }
                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {
                    throw new Exception("参数传递有误！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }



        /// <summary>
        /// 修改申请单
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdate([FromBody]BaseInfoModel<PaymentMstAndXmModel> paramters)
        {
            try
            {
                var paymentMstXm = paramters.infoData;
                if (paymentMstXm != null && paymentMstXm.PaymentMst != null)
                {
                    byte flam = paymentMstXm.PaymentMst.FApproval;
                    if(flam == (byte)ApprovalType.not || flam == (byte)ApprovalType.no)
                    {
                        var flam2 = paymentMstXm.PaymentMst.FDelete;
                        if(flam2 == (byte)DeleteType.Yes)
                        {
                            return DCHelper.ErrorMessage("作废的单据不允许进行修改！");
                        }
                        var result = this.PaymentMstService.UpdateSignle(paymentMstXm);
                        return DataConverterHelper.SerializeObject(result);
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("所选申请单不能进行修改操作！");
                    }
                }
                else
                {
                    return DCHelper.ErrorMessage("所选申请单不存在！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据主表主键查找明细表数据
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public string GetPaymentDtlsByMstPhid([FromUri]BaseListModel paramters)
        {
            try
            {
                if(paramters.mstPhid < 1)
                {
                    throw new Exception("参数传递不正确！");
                }
                var result = this.PaymentMstService.GetPaymentDtlsByMstPhid(paramters.mstPhid);
                return DataConverterHelper.SerializeObject(result);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据项目主键获取该项目预算总额，已使用，冻结与剩余金额
        /// </summary>
        /// <param name="xmPhid">项目主键</param>
        /// <param name="phid">单据主键</param>
        /// <param name="payment"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAmountOfMoney([FromUri]string xmPhid, [FromUri]long phid,[FromUri]PaymentModel payment)
        {
            try
            {
                if (string.IsNullOrEmpty(xmPhid))
                {
                    //throw new Exception("项目主键不能为空！");
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    List<BudgetMstModel> budgetMsts = BudgetMstService.GetYsList(payment.FOrgphid, payment.FDepphid, payment.FYear);
                    List<long> xmPhidList = budgetMsts.Select(x => x.PhId).ToList();
                    var result = this.PaymentMstService.GetSummary2(xmPhidList);
                    decimal used = 0;
                    if (result.Count > 0)
                    {
                        foreach (var res in result)
                        {
                            used = used + (decimal)res.Value;
                        }
                    }
                    decimal sum = 0;
                    foreach (long a in xmPhidList)
                    {
                        var total = this.BudgetMstService.GetDxbzDtl(a, payment.FOrgphid);
                        Type type = total.GetType();
                        sum += (decimal)type.GetProperty("FAmount").GetValue(total);
                    }
                    decimal surplus = sum - used;
                    result.Add("Sum", sum);
                    result.Add("Surplus", surplus);
                    return DataConverterHelper.SerializeObject(result);
                }
                else
                {

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    var result = this.PaymentMstService.GetSummary(xmPhid, phid);
                    decimal used = 0;
                    if (result.Count > 0)
                    {
                        foreach (var res in result)
                        {
                            used = used + (decimal)res.Value;
                        }
                    }
                    var total = this.BudgetMstService.GetDxbzDtl(long.Parse(xmPhid), payment.FOrgphid);
                    Type type = total.GetType();
                    decimal sum = (decimal)type.GetProperty("FAmount").GetValue(total);
                    decimal surplus = sum - used;
                    result.Add("Sum", sum);
                    result.Add("Surplus", surplus);
                    return DataConverterHelper.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region
        [HttpPost]
        public async Task<string> PostAddfj()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            try
            {
                PaymentMstAndXmModel paymentMstAndXm = new PaymentMstAndXmModel();
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
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/BKPayment/");
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

                            string b_urlpath = "/UpLoadFiles/BKPayment/" + date + "/" + newFileName;

                            //如果上传文件为封面图片,将图片信息设置到新闻对象中
                            //if (".jpg".CompareTo(extension) == 0 || ".gif".CompareTo(extension) == 0 || ".png".CompareTo(extension) == 0 || ".jpeg".CompareTo(extension) == 0 || ".bmp".CompareTo(extension) == 0)
                            //{
                            //    newsModel.AttachmentName = b_name;
                            //    newsModel.AttachmentSize = decimal.Round((decimal)stream.Length / 1024, 2);
                            //    newsModel.Picpath = b_urlpath;

                            //}
                            //else {
                            QtAttachmentModel attachmentModel = new QtAttachmentModel();
                            attachmentModel.BName = b_name;
                            attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                            attachmentModel.BTable = "BK3_PAYMENT_MST";
                            attachmentModel.BType = extension;
                            attachmentModel.BUrlpath = b_urlpath;
                            attachmentModel.PersistentState = PersistentState.Added;
                            attachmentModels.Add(attachmentModel);
                            //}
                        }
                    }
                    else
                    {
                        //获取键值对值,并通过反射获取对象中的属性
                        string key = content.Headers.ContentDisposition.Name.Replace("\"", string.Empty);
                        string value = await content.ReadAsStringAsync();
                        var item = typeof(PaymentMstAndXmModel).GetProperty(key);
                        if (item != null)
                        {
                            //获取数据的类型
                            var propertyType = item.PropertyType;
                            //转换数据的类型
                            object v = Convert.ChangeType(value, propertyType);
                            item.SetValue(paymentMstAndXm, v);
                        }
                    }
                }

                if (AppInfo != null)
                {
                    MultiDelegatingDbProvider.CurrentDbProviderName = AppInfo.DbName;
                }

                //SavedResult<Int64> savedResult = SysNewsService.SaveSysNews(newsModel, attachmentModels);
                //if (savedResult != null && savedResult.KeyCodes.Count > 0)
                //{
                //    return DCHelper.SuccessMessage("新增成功！");
                //    return DataConverterHelper.SerializeObject(savedResult);
                //}
                //else
                //{
                //    return DCHelper.ErrorMessage("新增失败！");
                //}
                return DCHelper.ErrorMessage("ceshiceshi!");
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 上传附件（单据的每个项目上传一次）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostUploadFile()
        {
            try
            {
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
                //web api 获取项目根目录下指定的文件下
                var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/Payment/");
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string resourcePath = Path.Combine(root, date);
                if (!Directory.Exists(resourcePath))
                {
                    Directory.CreateDirectory(resourcePath);
                }
                var multipartMemoryStreamProvider = await Request.Content.ReadAsMultipartAsync();
                var contentsList = multipartMemoryStreamProvider.Contents;

                //获取表单数据
                Dictionary<string, string> dic = new Dictionary<string, string>();
                //List<FileParameter> paraList = new List<FileParameter>();

                List<string> files = new List<string>();
                //原始文件名
                List<string> oldnames = new List<string>();
                List<QtAttachmentModel> list = new List<QtAttachmentModel>();

                foreach (var content in contentsList)
                {
                    //通过判断fileName是否为空，判断是否为文件类型
                    if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                    {
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
                            string uploadPath = Path.Combine(resourcePath, newFileName);

                            //保存文件
                            MemoryStream ms = new MemoryStream(bytes);
                            FileStream fs = new FileStream(uploadPath, FileMode.Create);
                            ms.WriteTo(fs);
                            ms.Close();
                            fs.Close();

                            string b_urlpath = "/UpLoadFiles/Payment/" + date + "/" + newFileName;

                            QtAttachmentModel model = new QtAttachmentModel();
                            model.BName = b_name;
                            model.BType = extension;
                            model.BSize = decimal.Round((decimal)stream.Length / (1024), 2); //保留2位小数
                            //model.BFilebody = bytes;
                            model.BUrlpath = b_urlpath;
                            model.BTable = "BK3_PAYMENT_XM";
                            model.BRemark = "";

                            list.Add(model);


                            //返回文件相对路径
                            files.Add(b_urlpath);
                            oldnames.Add(b_name);
                        }
                    }
                    else
                    {
                        string val = await content.ReadAsStringAsync();
                        dic.Add(content.Headers.ContentDisposition.Name.ToString(), val);
                    }
                }
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "附件上传成功。",
                    Data = list,
                    Attachment = files,
                    Oldnames = oldnames
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        [HttpPost]
        public string PostUpdatePaymentPay([FromBody]PaymentModel paramters)
        {
            try
            {
                this.PaymentMstService.UpdatePaymentPay(paramters.mstPhid, int.Parse(paramters.PayBz));
                return DCHelper.SuccessMessage("修改成功！");
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 新的获取资金拨付列表
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPaymentList([FromUri]PaymentModel paramters)
        {
            try
            {
                PaymentMstModel payment = new PaymentMstModel();
                payment.FOrgphid = paramters.FOrgphid;
                payment.FYear = paramters.FYear;
                payment.FDepphid = paramters.FDepphid;
                payment.FName = paramters.FName;
                payment.StartDate = paramters.StartDate;
                payment.EndDate = paramters.EndDate;
                payment.MinAmount = paramters.MinAmount;
                payment.MaxAmount = paramters.MaxAmount;
                payment.ApprovalBzs = paramters.ApprovalBzs;
                payment.PayBzs = paramters.PayBzs;
                payment.UserId = paramters.uid;
                if (payment.FOrgphid < 1)
                {
                    throw new Exception("组织信息传递不正确！");
                }
                if (payment.FDepphid < 1)
                {
                    throw new Exception("部门信息传递不正确！");
                }
                var result = this.PaymentMstService.GetPaymentList(payment);
                //取可选相同审批流是数据集合
                if(result != null && result.Count > 0)
                {
                    if(paramters.ProcPhid != 0)
                    {
                        List<string> orgList = result.ToList().Select(t => t.FBudcode).Distinct().ToList();
                        if(orgList != null && orgList.Count > 0)
                        {
                            var procList = this.GAppvalProcService.Find(t => orgList.Contains(t.OrgCode)).Data;
                            if (procList != null && procList.Count > 0)
                            {
                                //可以选取相同审批流的打上标记
                                foreach(var res in result)
                                {
                                    if(res.FApproval  == (byte)ApprovalType.not && procList.ToList().Find(t=>t.OrgCode == res.FBudcode && t.PhId == paramters.ProcPhid) != null)
                                    {
                                        res.BatchPracBz = 1;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                result = result.ToList().FindAll(t => t.BatchPracBz == 1);
                            }
                        }

                    }
                }
                var count = result.Count;
                var pageResult = result.Skip((paramters.PageIndex - 1) * paramters.PageSize).Take(paramters.PageSize).ToList();
                return DCHelper.ModelListToJson(pageResult, count);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 修改作废状态
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostCancetPaymentList([FromBody]BaseListModel paramters)
        {
            try
            {
                if (paramters.fPhIdList == null || paramters.fPhIdList.Count() < 1)
                {
                    return DCHelper.ErrorMessage("传递的单据集合有误！");
                }
                List<long> fCodes = new List<long>();
                for (int i = 0; i < paramters.fPhIdList.Count(); i++)
                {
                    fCodes.Add(long.Parse(paramters.fPhIdList[i]));
                }
                var result = this.PaymentMstService.PostCancetPaymentList(fCodes);
                return DataConverterHelper.SerializeObject(result);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}

