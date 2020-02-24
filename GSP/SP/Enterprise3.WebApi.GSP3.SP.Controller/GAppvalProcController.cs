#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Controller
    * 类 名 称：			GAppvalProcController
    * 文 件 名：			GAppvalProcController.cs
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
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GData3.Common.Model;
using GData3.Common.Utils.Filters;
using Newtonsoft.Json;
using GYS3.YS.Service.Interface;

namespace Enterprise3.WebApi.GSP3.SP.Controller
{
    /// <summary>
    /// GAppvalProc控制处理类
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class GAppvalProcController : ApiBase
    {
        IGAppvalProcService GAppvalProcService { get; set; }
        
        IQTSysSetService QTSysSetService { get; set; }

        IBudgetMstService BudgetMstService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GAppvalProcController()
	    {
	        GAppvalProcService = base.GetObject<IGAppvalProcService>("GSP3.SP.Service.GAppvalProc");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
        }


        /// <summary>
        /// 根据组织id，单据类型,审批类型获取所有的审批流程
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAppvalProc([FromUri]BillRequestModel billRequest) {

            if (billRequest == null || billRequest.Orgid == 0) {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(billRequest.BType)) {
                return DCHelper.ErrorMessage("单据类型为空！");
            }

            try
            {

                List<GAppvalProcModel> procModels = GAppvalProcService.GetAppvalProc(billRequest.Orgid, billRequest.BType, 0);
                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    Data = procModels
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 根据组织id，单据类型,审批类型获取所有的审批流程
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAppvalProcList([FromUri]BillRequestModel billRequest)
        {

            if (billRequest == null || billRequest.Orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(billRequest.BType))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }

            try
            {
                List<GAppvalProcModel> procModels = new List<GAppvalProcModel>();
                //先判断这个审批流的类型还在不在
                var oldset = this.QTSysSetService.Find(t => t.DicType == "splx" && t.Value == billRequest.BType).Data;
                if (oldset != null && oldset.Count > 0)
                {
                    //throw new Exception("审批流类型配置不能重复！");
                    List<long> bPhids = JsonConvert.DeserializeObject<List<long>>(billRequest.BPhIds);

                    //procModels = GAppvalProcService.GetAppvalProcList(billRequest.Orgid, billRequest.BType, 0, bPhids);
                    procModels = GAppvalProcService.GetAppvalProcList(billRequest.Orgid, billRequest.BType, oldset[0].PhId, bPhids);
                }
                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    Data = procModels
                });
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 新的获取多个单据对应申报部门公有的审批流程
        /// </summary>
        /// <param name="billRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostNewAppvalProcList([FromBody]BillRequestModel billRequest)
        {

            if (billRequest == null || string.IsNullOrEmpty(billRequest.OrgCode))
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (string.IsNullOrEmpty(billRequest.BType))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            try
            {
                //根据组织code获取组织id
                OrganizeModel organize = this.BudgetMstService.GetOrganizeByCode(billRequest.OrgCode);
                if(organize == null)
                {
                    return DCHelper.ErrorMessage("组织查询失败！");
                }
                List<GAppvalProcModel> procModels = GAppvalProcService.GetAppvalProc(organize.PhId, billRequest.BType, 0);
                if (procModels != null && procModels.Count == 1)
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        Status = "success",
                        Data = procModels
                    });
                }
                else
                {
                    return DCHelper.ErrorMessage("您所选的单据的审批流程不唯一，不能进行批量筛选！");
                }
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 新增审批类型
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostAddProcType([FromBody]ProcRequestModel requestModel)
        {
            if (requestModel == null || string.IsNullOrEmpty(requestModel.BillType))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (string.IsNullOrEmpty(requestModel.ApprovalTypeName))
            {
                return DCHelper.ErrorMessage("审批类型名称为空！");
            }

            try
            {
                QTSysSetModel sysSetModel = new QTSysSetModel();
                sysSetModel.TypeName = requestModel.ApprovalTypeName;
                sysSetModel.TypeCode = requestModel.ApprovalTypeCode;
                sysSetModel.Value = requestModel.BillType;
                sysSetModel.PersistentState = PersistentState.Added;

                SavedResult<Int64> savedResult = QTSysSetService.PostAddProcType(sysSetModel);
                if (savedResult != null && savedResult.SaveRows > 0)
                {
                    return DCHelper.SuccessMessage("保存成功！");
                }
                else
                {
                    return DCHelper.ErrorMessage("保存失败！");
                }
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 获取审批类型
        /// </summary>
        /// <param name="baseListModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProcTypes([FromUri]BaseListModel baseListModel) {
            
            try
            {
                List<QTSysSetModel> procTypes = QTSysSetService.GetProcTypes();
                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Data = procTypes
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 修改审批类型
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateProcType([FromBody]ProcRequestModel requestModel) {
            if (requestModel == null || requestModel.ApprovalTypeId == 0) {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }
            if (string.IsNullOrEmpty(requestModel.ApprovalTypeName)) {
                return DCHelper.ErrorMessage("审批类型名称为空！");
            }
            if (string.IsNullOrEmpty(requestModel.BillType)) {
                return DCHelper.ErrorMessage("单据类型为空！");
            }

            try
            {
                QTSysSetModel sysSetModel = new QTSysSetModel();
                sysSetModel.PhId = requestModel.ApprovalTypeId;
                sysSetModel.TypeName = requestModel.ApprovalTypeName;
                sysSetModel.Value = requestModel.BillType;
                sysSetModel.TypeCode = int.Parse(sysSetModel.Value).ToString();//与001，002等相对应，与添加顺序无关
                SavedResult<Int64> savedResult = QTSysSetService.PostUpdateProcType(sysSetModel);
                if (savedResult != null && savedResult.SaveRows > 0)
                {
                    return DCHelper.SuccessMessage("修改成功！");
                }
                else
                {
                    return DCHelper.ErrorMessage("修改失败！");
                }
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 批量删除审批类型
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDeleteProcType([FromBody]ProcRequestModel requestModel) {
            if (requestModel == null || requestModel.ApprovalTypeIds == null || requestModel.ApprovalTypeIds.Count == 0)
            {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }

            try
            {
                //判断审批流程是否被引用
                foreach (long id in requestModel.ApprovalTypeIds) {
                    bool symbol = GAppvalProcService.ProcIsUsed(id);
                    //bool symbol = GAppvalProcService.ProcIsUsed(requestModel.ApprovalTypeId);
                    if (symbol)
                    {
                        return DCHelper.ErrorMessage("删除失败,已有审批流程被引用！");
                    }
                    var procs = this.GAppvalProcService.Find(t => t.SPLXPhid == id).Data;
                    if (procs != null && procs.Count > 0)
                    {
                        throw new Exception("此类型下已存在审批流，不能删除此审批流类型！");
                    }
                }

                DeletedResult deletedResult = GAppvalProcService.PostDeleteProcTypes(requestModel.ApprovalTypeIds);
                if (deletedResult != null && deletedResult.DelRows > 0)
                {
                    return DCHelper.SuccessMessage("删除成功！");
                }
                else
                {
                    return DCHelper.ErrorMessage("删除失败！");
                }
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 新增审批流程
        /// </summary>
        /// <param name="infoBase"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostAddProc([FromBody]InfoBaseModel<List<GAppvalProcModel>> infoBase) {
            if (infoBase == null || infoBase.infoData == null || infoBase.infoData.Count == 0) {
                return DCHelper.ErrorMessage("审批流程对象为空！");
            }
            
            try
            {
                SavedResult<Int64> savedResult = GAppvalProcService.PostAddProcs(infoBase.infoData);
                if (savedResult != null && savedResult.SaveRows > 0)
                {
                    return DCHelper.SuccessMessage("新增审批流程成功！");
                }
                else
                {
                    return DCHelper.ErrorMessage("新增审批流程失败！");
                }
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 分页获取审批流程数据
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProcList([FromUri]ProcRequestModel requestModel) {
            if (requestModel == null || requestModel.Orgid == 0)
                return DCHelper.ErrorMessage("组织id为空！");
            if (requestModel.ApprovalTypeId == 0)
                return DCHelper.ErrorMessage("审批类型id为空！");
            if (string.IsNullOrEmpty(requestModel.BillType))
                return DCHelper.ErrorMessage("单据类型为空！");
            if (requestModel.PageIndex == 0 || requestModel.PageSize == 0)
                return DCHelper.ErrorMessage("分页参数不正确！");

            try
            {
                int count = 0;
                List<GAppvalProcModel> procModels = GAppvalProcService.GetProcList(requestModel.Orgid, requestModel.ApprovalTypeId, requestModel.BillType, requestModel.PageIndex, requestModel.PageSize,requestModel.QueryFilter,out count);

                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Data = procModels,
                    Total = count
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 获取审批流程明细
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProcDetail([FromUri]ProcRequestModel requestModel) {

            if (requestModel.ApprovalTypeId == 0)
            {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }
            if (string.IsNullOrEmpty(requestModel.BillType)) {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (string.IsNullOrEmpty(requestModel.ProcCode)) {
                return DCHelper.ErrorMessage("审批流程编码为空！");
            }
            if (requestModel.OrgIds == null || requestModel.OrgIds.Count == 0) {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            

            try
            {
                GAppvalProcModel procModel = GAppvalProcService.GetProcDetail(requestModel.ApprovalTypeId,requestModel.BillType, requestModel.ProcCode, requestModel.OrgIds);
                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Data = procModel
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 修改审批流程
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateProc([FromBody]ProcRequestModel requestModel) {
            if (requestModel == null || requestModel.ProcModels == null || requestModel.ProcModels.Count == 0)
            {
                return DCHelper.ErrorMessage("审批流程对象为空！");
            }
            if (requestModel.ApprovalTypeId == 0)
            {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }
            if (string.IsNullOrEmpty(requestModel.BillType))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (string.IsNullOrEmpty(requestModel.ProcCode))
            {
                return DCHelper.ErrorMessage("审批流程编码为空！");
            }
            if (requestModel.OrgIds == null || requestModel.OrgIds.Count == 0)
            {
                return DCHelper.ErrorMessage("启用组织id为空！");
            }

            try
            {
                //if (requestModel.Ucode != "Admin" && requestModel.ProcModels != null && requestModel.ProcModels.Count > 0)
                //{
                //    foreach(var proc in requestModel.ProcModels)
                //    {
                //        if(proc.IsSystem == (byte)1)
                //        {
                //            return DCHelper.ErrorMessage("普通用户没有权限修改内置流程！");
                //        }
                //    }
                //}
                SavedResult<Int64> savedResult = GAppvalProcService.PostUpdateProc(requestModel.ApprovalTypeId, requestModel.BillType, requestModel.ProcCode,requestModel.OrgIds, requestModel.ProcModels, requestModel.Ucode);
                if (savedResult != null && savedResult.SaveRows > 0)
                {
                    return DCHelper.SuccessMessage("修改成功！");
                }
                else {
                    return DCHelper.ErrorMessage("修改失败！");
                }
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 删除审批流程
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDeleteProc([FromBody]ProcRequestModel requestModel) {
            if (requestModel == null || requestModel.ApprovalTypeId == 0)
            {
                return DCHelper.ErrorMessage("审批类型id为空！");
            }
            if (string.IsNullOrEmpty(requestModel.BillType))
            {
                return DCHelper.ErrorMessage("单据类型为空！");
            }
            if (string.IsNullOrEmpty(requestModel.ProcCode))
            {
                return DCHelper.ErrorMessage("审批流程编码为空！");
            }
            if (requestModel.OrgIds == null || requestModel.OrgIds.Count == 0)
            {
                return DCHelper.ErrorMessage("启用组织id为空！");
            }
            try
            {
                //if (requestModel.Ucode != "Admin" && requestModel.ProcModels != null && requestModel.ProcModels.Count > 0)
                //{
                //    foreach (var proc in requestModel.ProcModels)
                //    {
                //        if (proc.IsSystem == (byte)1)
                //        {
                //            return DCHelper.ErrorMessage("普通用户没有权限修改内置流程！");
                //        }
                //    }
                //}
                GAppvalProcService.PostDeleteProc(requestModel.ApprovalTypeId, requestModel.BillType, requestModel.ProcCode, requestModel.OrgIds, requestModel.Ucode);
                return DCHelper.SuccessMessage("删除成功！");
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 批量删除审批流程
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDeleteProcs([FromBody]ProcRequestModel requestModel) {
            if (requestModel == null || requestModel.ProcModels == null || requestModel.ProcModels.Count == 0) {
                return DCHelper.ErrorMessage("请选择要删除的审批流程！");
            }
            if (requestModel.Ucode != "Admin" && requestModel.ProcModels != null && requestModel.ProcModels.Count > 0)
            {
                foreach (var proc in requestModel.ProcModels)
                {
                    if (proc.IsSystem == (byte)1)
                    {
                        return DCHelper.ErrorMessage("普通用户没有权限修改内置流程！");
                    }
                }
            }

            try
            {
                GAppvalProcService.PostDeleteProc(requestModel.ProcModels);
                return DCHelper.SuccessMessage("删除成功！");
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 更新启用组织
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateProcOrganize([FromBody]GAppvalProcModel procModel) {
            if (procModel == null || procModel.Organizes == null || procModel.Organizes.Count == 0) {
                return DCHelper.ErrorMessage("启用组织为空！");
            }
            if (procModel.NewOrganizes == null || procModel.NewOrganizes.Count == 0) {
                return DCHelper.ErrorMessage("更新启用组织之前的组织为空！");
            }

            try
            {
                int result = GAppvalProcService.PostUpdateProcOrganize(procModel);
                if (result > 0)
                {
                    return DCHelper.SuccessMessage("更新成功！");
                }
                else {
                    return DCHelper.ErrorMessage("更新失败！");
                }
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

    }
}

