﻿using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.ApiControllerBase.Models;
using Enterprise3.WebApi.GXM3.XM.Model.Common;
using Enterprise3.WebApi.GXM3.XM.Model.Request;
using Enterprise3.WebApi.GXM3.XM.Model.Response;
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
    public class ReportMstApiController: ApiBase
    {
        IXmReportMstService XmReportMstService;
        IProjectMstService ProjectMstService;
        IQTSysSetService QTSysSetService;
        IQtAttachmentService QtAttachmentService;
        ICorrespondenceSettingsService CorrespondenceSettingsService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportMstApiController()
        {
            XmReportMstService = base.GetObject<IXmReportMstService>("GXM3.XM.Service.XmReportMst");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            QtAttachmentService = base.GetObject<IQtAttachmentService>("GQT3.QT.Service.QtAttachment");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
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
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Data = result
            };

            return DataConverterHelper.SerializeObject(data);
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
            //FSurplusAmount剩余预算金额
            
            /*if (Msts != null && Msts.Count > 0)
            {
                List<XmReportMstModel> approveMsts = Msts.FindAll(x => x.FApprove == 9);
                if (approveMsts != null && approveMsts.Count > 0)
                {
                    XmReportMst.FSurplusAmount = XmReportMst.FSumAmount - approveMsts.Sum(x => x.FApproveAmount);
                }
            }*/
            if (XmReportMst.PhId == 0)
            {
                List<XmReportMstModel> Msts = XmReportMstService.Find(x => x.XmPhid == XmReportMst.XmPhid).Data.ToList();
                Msts.OrderByDescending(x => x.FCode);
                if (Msts == null || Msts.Count == 0)
                {
                    XmReportMst.FCode = XmReportMst.FProjCode + "0001";
                }
                else
                {
                    var maxFCode = Msts[0].FCode;
                    XmReportMst.FCode = maxFCode.Substring(0, maxFCode.Length - 4)+
                        (long.Parse(maxFCode.Substring(maxFCode.Length - 4, 4))+1).ToString();
                }

                
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
            SavedResult<Int64> result=XmReportMstService.SaveXmReportMst(XmReportMst, XmReportDtls);
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
            if (param.OrgId==0)
            {
                return DCHelper.ErrorMessage("组织code为空！");
            }
            var XmReportMst = XmReportMstService.Find(param.FPhid).Data;
            
            if (XmReportMst.XmPhid != 0) {
                var projectMst = ProjectMstService.Find(XmReportMst.XmPhid).Data;
                XmReportMst.FProjCode = projectMst.FProjCode;
                XmReportMst.FProjName = projectMst.FProjName;
                XmReportMst.FProjStatus = projectMst.FProjStatus;
            }
            if (!string.IsNullOrEmpty(XmReportMst.FBusinessCode))
            {
                var syssets = QTSysSetService.Find(x => x.DicType == "Business" && x.Orgid == param.OrgId && x.TypeCode== XmReportMst.FBusinessCode).Data.ToList();
                if(syssets!=null && syssets.Count > 0)
                {
                    XmReportMst.FBusinessName = syssets[0].TypeName;
                }
            }
            //操作员代码转名称
            if (XmReportMst.FDeclarerId!=0)
            {
                XmReportMst.FDeclarerName = CorrespondenceSettingsService.GetUserById(XmReportMst.FDeclarerId).UserName;
            }
            var dtls = XmReportMstService.FindXmReportDtlByForeignKey(param.FPhid).Data;
            if(dtls!=null && dtls.Count > 0)
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
            //返回对象增加附件
            if (result.XmReportMst != null)
            {
                result.Attachments = QtAttachmentService.Find(t => t.BTable == "XM3_ReportMst" && t.RelPhid == result.XmReportMst.PhId).Data.ToList();
            }
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
            var FIsDraft = 0;//该单据是否存在从生成草案而产生的签报单
            decimal FApproveAmount = 0;
            if (XmReportMsts != null)
            {
                FApproveAmount = XmReportMsts.Sum(x => x.FApproveAmount);
                var BA_Mst = XmReportMsts.Find(x => x.FIsDraft == 1);
                if (BA_Mst != null)
                {
                    FIsDraft = 1;
                }
            }
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Costitem = syssets,//费用说明
                FSurplusAmount = mst.FProjAmount- FApproveAmount,//剩余预算额度
                FIsDraft= FIsDraft//等于1是已经生成过预算草案
            };
            return DataConverterHelper.SerializeObject(data);
        }

        /// <summary>
        /// 保存签报单额度返还的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveReturn([FromBody]XmReportSaveModel data)
        {
            if(data.XmReportMst==null && data.XmReportMst.PhId == 0)
            {
                return DCHelper.ErrorMessage("单据主键为空！");
            }
            if (data.XmReportReturns == null)
            {
                return DCHelper.ErrorMessage("额度返还为空！");
            }
            XmReportMstModel XmReportMst = XmReportMstService.Find(data.XmReportMst.PhId).Data;
            foreach(var XmReportReturn in data.XmReportReturns)
            {
                if (XmReportReturn.PhId == 0)
                {
                    XmReportReturn.MstPhid = data.XmReportMst.PhId;
                    XmReportReturn.PersistentState = PersistentState.Added;
                }
                else
                {
                    XmReportReturn.PersistentState = PersistentState.Modified;
                }
            }
            var result=XmReportMstService.SaveReturn(data.XmReportReturns);
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 返还额度分配的保存
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostSaveReturnAmount([FromBody]XmReportSaveModel data)
        {
            //FApproveAmount审批通过的金额（及签报通过的金额）
            //FSurplusAmount剩余预算金额
            if (data.XmReportMst != null)
            {
                if (data.XmReportMst.PhId != 0)
                {
                    XmReportMstModel XmReportMst = XmReportMstService.Find(data.XmReportMst.PhId).Data;
                    Decimal FApproveAmount = 0;//该单据的审批通过金额
                    Decimal FSumAmount = XmReportMst.FSumAmount;
                    Decimal FSurplusAmount = 0;
                    List<XmReportDtlModel> XmReportDtls = XmReportMstService.FindXmReportDtlByForeignKey(data.XmReportMst.PhId).Data.ToList();
                    foreach(var dtl in XmReportDtls)
                    {
                        //前端传过来的明细数据
                        var postDtl = data.XmReportDtls.Find(x => x.PhId == dtl.PhId);
                        if (postDtl != null)
                        {
                            dtl.FAmount = postDtl.FAmount;
                            dtl.FReturnAmount= postDtl.FReturnAmount;
                        }
                        dtl.PersistentState = PersistentState.Modified;
                        FApproveAmount += dtl.FAmount - dtl.FReturnAmount;
                    }
                    XmReportMst.FApproveAmount = FApproveAmount;
                    //取该项目的所有相关签报单(不包括当前单据)
                    List<XmReportMstModel> Msts = XmReportMstService.Find(x => x.XmPhid == XmReportMst.XmPhid && x.PhId!= data.XmReportMst.PhId).Data.ToList();
                    //取已审批的所有相关签报单
                    List<XmReportMstModel> ApproveMsts = new List<XmReportMstModel>();
                    if (Msts != null)
                    {
                        ApproveMsts = Msts.FindAll(x => x.FApprove == 9);
                        if(ApproveMsts!=null && ApproveMsts.Count > 0)
                        {
                            for (var i = 0; i < ApproveMsts.Count; i++)
                            {
                                FApproveAmount += ApproveMsts[i].FApproveAmount;//总已审批金额
                            }
                        }
                    }
                    if (XmReportMst.XmPhid != 0)
                    {
                        //重新取单据的预算金额
                        var xm3_Mst = ProjectMstService.Find(XmReportMst.XmPhid).Data;
                        if (xm3_Mst != null)
                        {
                            FSumAmount = xm3_Mst.FProjAmount;
                        }
                    }
                    FSurplusAmount = FSumAmount - FApproveAmount;

                    foreach(var a in Msts)
                    {
                        a.FSumAmount = FSumAmount;
                        a.FSurplusAmount = FSurplusAmount;
                        a.PersistentState = PersistentState.Modified;
                    }
                    XmReportMst.PersistentState= PersistentState.Modified;
                    Msts.Add(XmReportMst);
                    string result = XmReportMstService.SaveReturnAmount(Msts, XmReportDtls);
                    return result;
                }
                else
                {
                    return DCHelper.ErrorMessage("单据主键为空！");
                }
            }
            else
            {
                return DCHelper.ErrorMessage("单据主表信息为空！");
            }
            
        }

        /// <summary>
        /// 删除签报数据
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDelete([FromBody]BaseListModel paramters)
        {
            if (paramters.FPhids == null && paramters.FPhids.Count== 0)
            {
                return DCHelper.ErrorMessage("未选中任何单据！");
            }
            List<XmReportMstModel> XmReportMsts = XmReportMstService.Find(x=>paramters.FPhids.Contains(x.PhId)).Data.ToList();
            var ApproveMsts = XmReportMsts.FindAll(x => x.FApprove == 1 || x.FApprove == 9);
            if(ApproveMsts!=null && ApproveMsts.Count > 0)
            {
                return DCHelper.ErrorMessage("存在单据处于审批中或者已审批！");
            }
            foreach(var Mst in XmReportMsts)
            {
                Mst.PersistentState = PersistentState.Deleted;
            }
            
            var result = XmReportMstService.Save<Int64>(XmReportMsts,"");
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 取签报单列表数据(给网报)
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetXmReportListToWB([FromUri]string OrgCode = "", [FromUri]string UserCode = "")
        {
            var projectMsts=new List<ProjectMstModel>();
            if (!string.IsNullOrEmpty(UserCode))
            {
                var User = CorrespondenceSettingsService.GetUserByCode(UserCode);
                if (User == null)
                {
                    return DCHelper.ErrorMessage("该操作员不存在！");
                }
                if (!string.IsNullOrEmpty(OrgCode))
                {
                    projectMsts = this.ProjectMstService.Find(t => t.FDeclarationUnit == OrgCode &&t.FDeclarerId== User.PhId && t.FApproveStatus == "3").Data.ToList();
                }
                else
                {
                    projectMsts = this.ProjectMstService.Find(t => t.FDeclarerId == User.PhId && t.FApproveStatus == "3").Data.ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(OrgCode))
                {
                    projectMsts = this.ProjectMstService.Find(t => t.FDeclarationUnit == OrgCode && t.FApproveStatus == "3").Data.ToList();
                }
                else
                {
                    return DCHelper.ErrorMessage("请传操作员代码或组织代码！");
                }
            }


            var result = new List<XmReportToWBModel>();
            if (projectMsts.Count > 0)
            {
                var XmPhids = projectMsts.Select(x => x.PhId).ToList();
                List<XmReportMstModel> xmReportMsts= XmReportMstService.Find(x => XmPhids.Contains(x.XmPhid)).Data.ToList();
                
                var projectMst = new ProjectMstModel();
                var Orgs = CorrespondenceSettingsService.GetOrgListByCode(projectMsts.Where(x => !string.IsNullOrEmpty(x.FBudgetDept)).Select(x => x.FBudgetDept).Distinct().ToList()).ToList();
                if (xmReportMsts!=null && xmReportMsts.Count > 0)
                {
                    var xmReportDtls = XmReportMstService.FindXmReportDtlsByForeignKeys(xmReportMsts.Select(x => x.PhId).ToList()).Data.ToList();
                    for (var i=0;i< xmReportMsts.Count; i++)
                    {
                        var dataToWB = new XmReportToWBModel();
                        projectMst = projectMsts.ToList().Find(x => x.PhId == xmReportMsts[i].XmPhid);
                        dataToWB.PhId = xmReportMsts[i].PhId;
                        dataToWB.FTitle = xmReportMsts[i].FTitle;
                        dataToWB.FCode = xmReportMsts[i].FCode;
                        dataToWB.FProjName = projectMst.FProjName;
                        dataToWB.FBudgetDept = projectMst.FBudgetDept;
                        dataToWB.FBudgetDept_EXName = Orgs.Find(x => x.OCode == projectMst.FBudgetDept).OName;
                        dataToWB.FAmount = xmReportMsts[i].FAmount;
                        var dtls = xmReportDtls.FindAll(x => x.MstPhid == xmReportMsts[i].PhId);
                        dataToWB.xmReportDtls = dtls.Select(x=>new XmReportDtlToWBModel
                        {
                            PhId=x.PhId,
                            CostitemCode=x.CostitemCode,
                            XmName=x.XmName,
                            FPrice=x.FPrice,
                            FAmount=x.FAmount,
                            FIsCost=x.FIsCost,
                            FReturnAmount=x.FReturnAmount,
                            FVariable1=x.FVariable1,
                            FUnit1=x.FUnit1,
                            FVariable2=x.FVariable2,
                            FUnit2=x.FUnit2,
                            FVariable3=x.FVariable3,
                            FUnit3=x.FUnit3

                        }).ToList();
                        dataToWB.FixedAmount = dtls.Where(x => x.FIsCost == 1).Sum(x => x.FAmount);
                        dataToWB.VariableAmount = dtls.Where(x => x.FIsCost == 0).Sum(x => x.FAmount);
                        result.Add(dataToWB);
                    }
                }
            }
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Data = result
            };

            return DataConverterHelper.SerializeObject(data);
        }


        /// <summary>
        /// 完整组织列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetOrgToWB()
        {
            var result = CorrespondenceSettingsService.GetALLOrgList().
                Select(x=>
                new {
                    x.OCode,
                    x.OName
                }
                );
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Data = result
            };

            return DataConverterHelper.SerializeObject(data);
        }

        /// <summary>
        /// 取支出预算列表数据(给网报)
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetProjectListToWB([FromUri]string OrgCode = "", [FromUri]string UserCode = "")
        {
            var projectMsts = new List<ProjectMstModel>();
            if (!string.IsNullOrEmpty(UserCode))
            {
                var User = CorrespondenceSettingsService.GetUserByCode(UserCode);
                if (User == null)
                {
                    return DCHelper.ErrorMessage("该操作员不存在！");
                }
                if (!string.IsNullOrEmpty(OrgCode))
                {
                    projectMsts = this.ProjectMstService.Find(t => t.FDeclarationUnit == OrgCode && t.FDeclarerId == User.PhId && t.FApproveStatus == "3").Data.ToList();
                }
                else
                {
                    projectMsts = this.ProjectMstService.Find(t => t.FDeclarerId == User.PhId && t.FApproveStatus == "3").Data.ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(OrgCode))
                {
                    projectMsts = this.ProjectMstService.Find(t => t.FDeclarationUnit == OrgCode && t.FApproveStatus == "3").Data.ToList();
                }
                else
                {
                    return DCHelper.ErrorMessage("请传操作员代码或组织代码！");
                }
            }
            var result = new object();
            if (projectMsts.Count > 0)
            {
                var phids = projectMsts.Select(x => x.PhId).ToList();
                var Orgs = CorrespondenceSettingsService.GetOrgListByCode(projectMsts.Where(x => !string.IsNullOrEmpty(x.FBudgetDept)).Select(x => x.FBudgetDept).Distinct().ToList()).ToList();
                var xmDtls = ProjectMstService.FindProjectDtlBudgetDtlsByForeignKeys(phids).Data.ToList();
                result = projectMsts.Select(x =>
                 new {
                     x.PhId,
                     x.FProjCode,
                     x.FProjName,
                     x.FBudgetDept,
                     FBudgetDept_EXName= Orgs.Find(a => a.OCode == x.FBudgetDept).OName,
                     x.FProjAmount,
                     xmDtls = xmDtls.FindAll(y => y.MstPhid == x.PhId).
                      Select(t => new
                      {
                        t.FDtlCode,
                        t.FName,
                        t.FAmount,
                        t.FAmountEdit,
                        t.FAmountAfterEdit
                      })
                  });
                //var Orgs = CorrespondenceSettingsService.GetOrgListByCode(projectMsts.Where(x => !string.IsNullOrEmpty(x.FBudgetDept)).Select(x => x.FBudgetDept).Distinct().ToList()).ToList();
                
                
            }
            var data = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功！",
                Data = result
            };

            return DataConverterHelper.SerializeObject(data);
        }

    }
}