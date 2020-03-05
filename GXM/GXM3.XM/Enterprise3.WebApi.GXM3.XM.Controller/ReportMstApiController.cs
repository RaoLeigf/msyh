using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Enterprise3.WebApi.GXM3.XM.Model.Common;
using Enterprise3.WebApi.GXM3.XM.Model.Request;
using GData3.Common.Utils.Filters;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GXM3.XM.Model.Domain;
using GXM3.XM.Service.Interface;
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

namespace Enterprise3.WebApi.GXM3.XM.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class ReportMstApiController : ApiBase
    {
        IXmReportMstService XmReportMstService;
        IProjectMstService ProjectMstService;
        IQTSysSetService QTSysSetService;
        IQtAttachmentService QtAttachmentService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportMstApiController()
        {
            XmReportMstService = base.GetObject<IXmReportMstService>("GXM3.XM.Service.XmReportMst");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");
        }

        /// <summary>
        /// 点击生成项目草案获取该单据对应的签报单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetMSYHProjectReport([FromUri]BaseListModel param)
        {
            if (param.FPhid == 0)
            {
                return DCHelper.ErrorMessage("单据主键不能为空！");
            }
            try
            {
                var result = this.XmReportMstService.GetMSYHProjectReport(param.FPhid);
                var data = new
                {
                    Status = ResponseStatus.Success,
                    Msg = "获取成功！",
                    Data = result
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 取签报单列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetXmReportMstList([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.OrgCode))
            {
                return DCHelper.ErrorMessage("组织code为空！");
            }
            if (param.UserId == 0)
            {
                return DCHelper.ErrorMessage("操作员ID为空！");
            }
            if (string.IsNullOrEmpty(param.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            var projectMsts = this.ProjectMstService.Find(t => t.FDeclarationUnit == param.OrgCode && t.FDeclarerId == param.UserId && t.FApproveStatus == "3" && t.FYear == param.Year).Data;
            var result = new List<XmReportMstModel>();
            if (projectMsts.Count > 0)
            {
                var XmPhids = projectMsts.Select(x => x.PhId).ToList();
                result = XmReportMstService.Find(x => x.FDeclarerId == param.UserId && XmPhids.Contains(x.XmPhid)).Data.ToList();
                var projectMst = new ProjectMstModel();
                foreach (var mst in result)
                {
                    //虚拟字段
                    projectMst = projectMsts.ToList().Find(x => x.PhId == mst.XmPhid);
                    mst.FProjCode = projectMst.FProjCode;
                    mst.FProjName = projectMst.FProjName;
                    mst.FProjStatus = projectMst.FProjStatus;
                }
            }
            return DCHelper.ModelListToJson<XmReportMstModel>(result, (Int32)result.Count);
        }


        /// <summary>
        /// 保存签报单的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSave([FromBody]XmReportSaveModel data)
        {
            XmReportMstModel XmReportMst = new XmReportMstModel();
            if (data.XmReportMst != null)
            {
                XmReportMst = data.XmReportMst;
            }
            List<XmReportDtlModel> XmReportDtls = new List<XmReportDtlModel>();
            if (data.XmReportDtls != null)
            {
                XmReportDtls = data.XmReportDtls;
            }
            if (XmReportMst.PhId == 0)
            {
                XmReportMst.PersistentState = PersistentState.Added;
                foreach (var dtl in XmReportDtls)
                {
                    dtl.PersistentState = PersistentState.Added;
                }
            }
            else
            {
                XmReportMst.PersistentState = PersistentState.Modified;
            }
            SavedResult<Int64> result = XmReportMstService.SaveXmReportMst(XmReportMst, XmReportDtls);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 取签报单单据详情
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetXmReport([FromUri]BaseListModel param)
        {
            if (param.FPhid == 0)
            {
                return DCHelper.ErrorMessage("请先选择项目！");
            }
            if (param.OrgId == 0)
            {
                return DCHelper.ErrorMessage("组织code为空！");
            }
            var XmReportMst = XmReportMstService.Find(param.FPhid).Data;
            if (XmReportMst.XmPhid != 0)
            {
                var projectMst = ProjectMstService.Find(XmReportMst.XmPhid).Data;
                XmReportMst.FProjCode = projectMst.FProjCode;
                XmReportMst.FProjName = projectMst.FProjName;
                XmReportMst.FProjStatus = projectMst.FProjStatus;
            }
            if (!string.IsNullOrEmpty(XmReportMst.FBusinessCode))
            {
                var syssets = QTSysSetService.Find(x => x.DicType == "Business" && x.Orgid == param.OrgId && x.TypeCode == XmReportMst.FBusinessCode).Data.ToList();
                if (syssets != null && syssets.Count > 0)
                {
                    XmReportMst.FBusinessName = syssets[0].TypeName;
                }
            }
            //操作员代码转名称
            /*if (XmReportMst.FDeclarerId!=0)
            {
                FDeclarerId
            }*/
            var dtls = XmReportMstService.FindXmReportDtlByForeignKey(param.FPhid).Data;
            if (dtls != null && dtls.Count > 0)
            {
                var sysset2s = QTSysSetService.Find(x => x.DicType == "Costitem" && x.Orgid == param.OrgId).Data.ToList();
                if (sysset2s != null && sysset2s.Count > 0)
                {
                    foreach (var dtl in dtls)
                    {
                        var sysset2 = new QTSysSetModel();
                        //费用项目代码转名称
                        if (!string.IsNullOrEmpty(dtl.CostitemCode))
                        {
                            sysset2 = sysset2s.Find(x => x.TypeCode == dtl.CostitemCode);
                            if (sysset2 != null)
                            {
                                dtl.XmName = sysset2.TypeName;
                            }
                        }
                    }
                }
            }
            var Returns = XmReportMstService.FindXmReportReturnByForeignKey(param.FPhid).Data;
            var result = new XmReportSaveModel();
            result.XmReportMst = XmReportMst;
            result.XmReportDtls = dtls.ToList();
            result.XmReportReturns = Returns.ToList();
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Data = result
            };

            return DataConverterHelper.SerializeObject(data);

        }

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PostSaveXmReportMst()
        {
            List<QtAttachmentModel> attachmentModels = new List<QtAttachmentModel>();
            List<QtAttachmentModel> oldattachmentModels = new List<QtAttachmentModel>();
            //具体数据对象
            long projectPhid = 0;
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
            var root = System.Web.Hosting.HostingEnvironment.MapPath("~/UpLoadFiles/XmReportMst/");
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

                        string b_urlpath = "/UpLoadFiles/XmReportMst/" + date + "/" + newFileName;

                        QtAttachmentModel attachmentModel = new QtAttachmentModel();
                        attachmentModel.BName = b_name;
                        attachmentModel.BSize = decimal.Round((decimal)stream.Length / 1024, 2);
                        attachmentModel.BTable = "XM3_ReportMst";
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
                    if (key == "PhId")
                    {
                        projectPhid = long.Parse(value);
                    }
                    else if (key == "OldAttachments")
                    {
                        var value2 = JsonConvert.DeserializeObject<List<QtAttachmentModel>>(value);
                        oldattachmentModels = value2;
                    }
                }
            }
            if (projectPhid <= 0)
            {
                return DCHelper.ErrorMessage("签报主表保存有误！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                //原有的附件要删除
                IList<QtAttachmentModel> oldAttachments = new List<QtAttachmentModel>();
                oldAttachments = this.QtAttachmentService.Find(t => t.BTable == "XM3_ReportMst" && t.RelPhid == projectPhid).Data;
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
                        att.RelPhid = projectPhid;
                        att.BTable = "XM3_ReportMst";
                        att.PersistentState = PersistentState.Added;
                    }
                }
                if (oldattachmentModels != null && oldattachmentModels.Count > 0)
                {
                    foreach (var oldAtt in oldattachmentModels)
                    {
                        oldAtt.RelPhid = projectPhid;
                        oldAtt.BTable = "XM3_ReportMst";
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
        /// 获取该组织该人员进行签报时能取的费用说明
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetinfoForQB([FromUri]Model.Request.BaseListModel param)
        {
            if (param.OrgId == 0)
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            if (param.FPhid == 0)//项目单据的主键 不是签报单据的主键
            {
                return DCHelper.ErrorMessage("请先选择项目！");
            }
            var syssets = QTSysSetService.Find(x => x.DicType == "Costitem" && x.Orgid == param.OrgId).Data.ToList();
            var mst = ProjectMstService.Find(param.FPhid).Data;
            var dtls = ProjectMstService.FindProjectDtlBudgetDtlByForeignKey(param.FPhid).Data.ToList();
            if (dtls != null && dtls.Count > 0)
            {
                for (var i = 0; i < dtls.Count; i++)
                {
                    var set = new QTSysSetModel();
                    set.TypeName = dtls[i].FName;
                    syssets.Add(set);
                }
            }
            var XmReportMsts = XmReportMstService.Find(x => x.XmPhid == param.FPhid).Data.ToList();
            var FApproveAmount = XmReportMsts.Sum(x => x.FApproveAmount);
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Costitem = syssets,//费用说明
                FSurplusAmount = mst.FProjAmount - FApproveAmount//剩余预算额度
            };
            return DataConverterHelper.SerializeObject(data);
        }
    }
}