#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Controller
    * 类 名 称：			GAppvalPostController
    * 文 件 名：			GAppvalPostController.cs
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
using Enterprise3.WebApi.GSP3.SP.Model.Common;
using Enterprise3.WebApi.GSP3.SP.Model.Request;
using GSP3.SP.Model.Extra;
using GData3.Common.Utils.Filters;
using GSP3.SP.Model.Enums;
using System.Linq;

namespace Enterprise3.WebApi.GSP3.SP.Controller
{
    /// <summary>
    /// GAppvalPost控制处理类
    /// </summary>
    [MethodExceptionFilter, SyncUserDbFilter]
    public class GAppvalPostController : ApiBase
    {
        IGAppvalPostService GAppvalPostService { get; set; }

        IGAppvalProcService GAppvalProcService { get; set; }

        IGAppvalRecordService GAppvalRecordService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GAppvalPostController()
	    {
	        GAppvalPostService = base.GetObject<IGAppvalPostService>("GSP3.SP.Service.GAppvalPost");
            GAppvalProcService = base.GetObject<IGAppvalProcService>("GSP3.SP.Service.GAppvalProc");
        }

        /// <summary>
        /// 送审的时候，根据审批流程id，获取第一个审批岗位的审批人
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetFirstStepOperator([FromUri]GAppvalProcModel procModel)
        {
            if (procModel == null || procModel.PhId == 0) {
                return DCHelper.ErrorMessage("单据ID为空！");
            }

            try
            {
                GAppvalPostModel postModel = GAppvalPostService.GetFirstStepOperator(procModel.PhId);

                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Data = postModel
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 根据审批流程id，审批岗位id查询审批流程数据和下一审批岗位的审批人
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAppvalProcAndOperator([FromUri]GAppvalRecordModel recordModel) {

            if (recordModel == null || recordModel.ProcPhid == 0) {
                return DCHelper.ErrorMessage("审批流程id为空！");
            }
            if (recordModel.PostPhid == 0) {
                return DCHelper.ErrorMessage("审批岗位id为空！");
            }

            try
            {
                //根据审批流程id查找审批流程
                GAppvalProcModel procModel = GAppvalProcService.FindSingle(recordModel.ProcPhid);
                //根据审批流程id,当前岗位的id,查找下一岗位的审批人
                GAppvalPostModel postModel = GAppvalPostService.GetNextStepOperator(procModel.PhId,recordModel.PostPhid, recordModel.RefbillPhid, recordModel.FBilltype);

                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Process = procModel,
                    AppvalPost = postModel
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 根据审批流程id获取审批岗位
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetApprovalPost([FromUri]GAppvalRecordModel recordModel) {
            if (recordModel == null || recordModel.ProcPhid == 0) {
                return DCHelper.ErrorMessage("流程id为空！");
            }

            try
            {
                List<GAppvalPostModel> postModels = GAppvalPostService.GetApprovalPost(recordModel.ProcPhid);
                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Data = postModels
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 回退时,获取之前的所有岗位,包括发起人(岗位id为0)
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetBackApprovalPost([FromUri]GAppvalRecordModel recordModel) {
            if (recordModel == null || recordModel.ProcPhid == 0) {
                return DCHelper.ErrorMessage("审批流程id为空！");
            }
            if (recordModel.PostPhid == 0) {
                return DCHelper.ErrorMessage("审批岗位id为空！");
            }

            try
            {
                List<GAppvalPostModel> postModels = GAppvalPostService.GetBackApprovalPost(recordModel);
                if(postModels != null && postModels.Count > 0)
                {
                    foreach (var post in postModels)
                    {
                        post.IsSpanBack = 0;
                    }
                }
                //如果是跨审批流回退，打上标记
                if(recordModel.IsSpanBack == 1)
                {
                    IList<GAppvalRecordModel> recordList = new List<GAppvalRecordModel>();
                    recordList = this.GAppvalRecordService.Find(t => t.RefbillPhid == recordModel.RefbillPhid && t.FBilltype == BillType.BeginProject).Data;
                    if(recordList != null && recordList.Count > 0)
                    {
                        var procPhid = recordList.ToList().OrderByDescending(t => t.FSendDate).ToList()[0].ProcPhid;
                        List<GAppvalPostModel> postModels2 = GAppvalPostService.GetBackApprovalPost2(recordModel.RefbillPhid, procPhid);
                        if (postModels2 != null && postModels2.Count > 0)
                        {
                            foreach (var post in postModels2)
                            {
                                post.IsSpanBack = 1;
                                post.BackProcPhid = procPhid;
                            }
                        }
                        postModels2.AddRange(postModels);
                        postModels = postModels2;
                    }

                }
                return DataConverterHelper.SerializeObject(new {
                    Status = "success",
                    Data = postModels
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// 根据审批岗位获取审批人(包括岗位id为0的发起人)
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetOperators([FromUri]GAppvalRecordModel recordModel) {
            if (recordModel == null) {
                return DCHelper.ErrorMessage("请求参数为空！");
            }

            try
            {

                List<GAppvalPost4OperModel> operModels = GAppvalPostService.GetOperators(recordModel);

                return DataConverterHelper.SerializeObject(new
                {
                    Status = "success",
                    Data = operModels
                });
            }
            catch (Exception e) {
                return DCHelper.ErrorMessage(e.Message);
            }

        }

        /// <summary>
        /// 查看单个岗位的信息
        /// </summary>
        /// <param name="gAppvalPost"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAppvalPostOpers([FromUri]GAppvalPostModel gAppvalPost)
        {
            if(gAppvalPost.PhId <= 0)
            {
                throw new Exception("岗位主键参数传递不正确！");
            }
            try
            {
                var result = this.GAppvalPostService.GetAppvalPostOpers(gAppvalPost.PhId);
                return DataConverterHelper.SerializeObject(result);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取岗位列表
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAppvalPostOpersList([FromUri]PostRequestModel paramters)
        {
            //string userType = NG3.AppInfoBase.UserType;
            //return "";
            if(string.IsNullOrEmpty(paramters.Ucode))
            {
                throw new Exception("用户账号传递不正确！");
            }
            if (paramters.Orgid == 0)
            {
                throw new Exception("组织账号传递不正确！");
            }
            try
            {
                var result = this.GAppvalPostService.GetAppvalPostOpersList(paramters.PageIndex, paramters.PageSize, paramters.Orgid, paramters.Ucode, paramters.SearchOrgid, paramters.EnableMark, paramters.PostName);
                return DCHelper.ModelListToJson(result, result.Count, paramters.PageIndex, paramters.PageSize);
                //return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据组织获取岗位
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public string GetAppvalPostList([FromUri]BaseListModel paramters)
        {
            if (paramters.Uid == 0)
            {
                throw new Exception("操作员账号传递不正确！");
            }
            try
            {
                var result = this.GAppvalPostService.GetAppvalPostList(paramters.Uid);
                return DataConverterHelper.SerializeObject(result);
            }
            catch(Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据岗位主键集合删除
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDelete([FromBody]PostRequestModel paramters)
        {
            List<long> phidList = paramters.PostPhidList;
            if(phidList.Count > 0)
            {
                foreach(var phid in phidList)
                {
                    if(phid == 0)
                    {
                        //throw new Exception("所传的主键参数集合有误！");
                        return DCHelper.ErrorMessage("所传的主键参数集合有误！");
                    }
                }
            }
            try
            {
                var result = this.GAppvalPostService.DeletedPostOpers(phidList, paramters.Ucode);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 新增岗位与操作员
        /// </summary>
        /// <param name="gAppvalPostAndOpers"></param>
        /// <returns></returns>
        public string PostAdd([FromBody]GAppvalPostAndOpersModel gAppvalPostAndOpers)
        {
            if(gAppvalPostAndOpers == null || gAppvalPostAndOpers.GAppvalPost == null || gAppvalPostAndOpers.GAppvalPost4Opers == null)
            {
                return DCHelper.ErrorMessage("新增参数不能为空！");
            }
            try
            {
                if(gAppvalPostAndOpers.Ucode == "Admin")
                {
                    gAppvalPostAndOpers.GAppvalPost.IsSystem = (byte)1;
                }
                else
                {
                    gAppvalPostAndOpers.GAppvalPost.IsSystem = (byte)0;
                }
                var result = this.GAppvalPostService.SavedSignle(gAppvalPostAndOpers);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 修改岗位与操作员信息
        /// </summary>
        /// <param name="gAppvalPostAndOpers"></param>
        /// <returns></returns>
        public string PostUpdate([FromBody]GAppvalPostAndOpersModel gAppvalPostAndOpers)
        {
            if(gAppvalPostAndOpers == null || gAppvalPostAndOpers.GAppvalPost == null || gAppvalPostAndOpers.GAppvalPost4Opers == null)
            {
                //throw new Exception("对象参数传递有误！");
                return DCHelper.ErrorMessage("对象参数传递有误！");
            }
            try
            {
                if(gAppvalPostAndOpers.Ucode != "Admin" && gAppvalPostAndOpers.GAppvalPost.IsSystem == (byte)1)
                {
                    return DCHelper.ErrorMessage("内置数据不允许普通操作员进行修改！");
                }
                if(gAppvalPostAndOpers.Ucode == "Admin")
                {
                    gAppvalPostAndOpers.GAppvalPost.IsSystem = (byte)1;
                }
                var result = this.GAppvalPostService.UpdateSignle(gAppvalPostAndOpers);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
    }
}

