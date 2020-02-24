#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Service
    * 类 名 称：			GAppvalPostService
    * 文 件 名：			GAppvalPostService.cs
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
using System.Linq;
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GSP3.SP.Service.Interface;
using GSP3.SP.Facade.Interface;
using GSP3.SP.Model.Domain;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using GSP3.SP.Model.Extra;
using GSP3.SP.Model.Enums;
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;

namespace GSP3.SP.Service
{
	/// <summary>
	/// GAppvalPost服务组装处理类
	/// </summary>
    public partial class GAppvalPostService : EntServiceBase<GAppvalPostModel>, IGAppvalPostService
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalPost业务外观处理对象
        /// </summary>
		IGAppvalPostFacade GAppvalPostFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGAppvalPostFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IGAppvalPost4OperFacade GAppvalPost4OperFacade { get; set; }

        private IGAppvalRecordFacade GAppvalRecordFacade { get; set; }

        private IUserFacade UserFacade { get; set; }

        private ICorrespondenceSettingsFacade CorrespondenceSettingsFacade { get; set; }

        private IGAppvalProc4PostFacade GAppvalProc4PostFacade { get; set; }

        /// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
        private IOrganizationFacade OrganizationFacade { get; set; }
        #endregion

        #region 实现 IGAppvalPostService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="gAppvalPostEntity"></param>
        /// <param name="gAppvalPost4OperEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveGAppvalPost(GAppvalPostModel gAppvalPostEntity, List<GAppvalPost4OperModel> gAppvalPost4OperEntities)
        {
			return GAppvalPostFacade.SaveGAppvalPost(gAppvalPostEntity, gAppvalPost4OperEntities);
        }

        /// <summary>
        /// 通过外键值获取GAppvalPost4Oper明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<GAppvalPost4OperModel> FindGAppvalPost4OperByForeignKey<TValType>(TValType id)
        {
            return GAppvalPost4OperFacade.FindByForeignKey(id);
        }


        /// <summary>
        /// 送审的时候，根据审批流程id，获取第一个审批岗位的审批人
        /// </summary>
        /// <param name="phid">审批流程id</param>
        /// <returns></returns>
        public GAppvalPostModel GetFirstStepOperator(long phid) {

            GAppvalPostModel postModel = null;

            //根据审批流程的id查找审批岗位
            List<GAppvalPostModel> postModels = GAppvalPostFacade.FindAppvalPostByProcID(phid);

            if (postModels == null || postModels.Count == 0) {
                postModel = new GAppvalPostModel();
                postModel.Operators = new List<GAppvalPost4OperModel>();
                return postModel;
            }
            //找到第一个岗位
            postModel = postModels.OrderBy(t => t.Seq).ToList()[0];
            //根据岗位id查找所有的操作员
            List<GAppvalPost4OperModel> operatorModels = GAppvalPost4OperFacade.GetOperatorsByPostID(postModel.PhId);
            postModel.Operators = operatorModels;

            return postModel;
        }

        /// <summary>
        /// 根据审批流程id,当前岗位的id,查找下一岗位的审批人
        /// </summary>
        /// <param name="proc_phid">审批流程id</param>
        /// <param name="post_phid">审批岗位id</param>
        /// <param name="refbillPhid">关联单据主键</param>
        /// <param name="fBilltype">关联单据类型</param>
        /// <returns></returns>
        public GAppvalPostModel GetNextStepOperator(long proc_phid, long post_phid, long refbillPhid, string fBilltype) {
            GAppvalPostModel postModel = null;

            //根据审批流程的id查找审批岗位
            List<GAppvalPostModel> postModels = GAppvalPostFacade.FindAppvalPostByProcID(proc_phid);

            if (postModels == null || postModels.Count == 0)
            {
                postModel = new GAppvalPostModel();
                postModel.Operators = new List<GAppvalPost4OperModel>();
                return postModel;
            }

            //当前审批岗位
            GAppvalPostModel presentPost = postModels.Find(t => t.PhId == post_phid);
            //当前岗位的审批记录
            List<GAppvalRecordModel> recordModels = GAppvalRecordFacade.Find(t => t.RefbillPhid == refbillPhid && t.FBilltype == fBilltype && t.PostPhid == presentPost.PhId).Data.ToList();
            //当前岗位审批未审批的审批记录
            List<GAppvalRecordModel> waitRecords = recordModels.FindAll(t => t.FApproval == (byte)Approval.Wait);
            if (presentPost.FMode == (Byte)ModeType.Yes && waitRecords.Count != 1)
            {
                postModel = new GAppvalPostModel();
                postModel.Operators = new List<GAppvalPost4OperModel>();
                return postModel;
            }

            //找到下一个岗位
            int seq = postModels.Find(t => t.PhId == post_phid).Seq;
            GAppvalPostModel nextAppvalPost = postModels.OrderBy(t => t.Seq).ToList().Find(t => t.Seq > seq);
            if (nextAppvalPost == null) {
                postModel = new GAppvalPostModel();
                postModel.Operators = new List<GAppvalPost4OperModel>();
                return postModel;
            }
            postModel = nextAppvalPost;

            //根据岗位id查找所有的操作员
            List<GAppvalPost4OperModel> operatorModels = GAppvalPost4OperFacade.GetOperatorsByPostID(postModel.PhId);
            postModel.Operators = operatorModels;

            return postModel;
        }

        /// <summary>
        /// 根据审批流程id获取审批岗位
        /// </summary>
        /// <param name="proc_phid">审批流程id</param>
        /// <returns></returns>
        public List<GAppvalPostModel> GetApprovalPost(long proc_phid) {
            if (proc_phid == 0) {
                return new List<GAppvalPostModel>();
            }

            return GAppvalPostFacade.FindAppvalPostByProcID(proc_phid);
        }

        /// <summary>
        /// 回退时,获取之前的所有岗位,包括发起人(岗位id为0)
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        public List<GAppvalPostModel> GetBackApprovalPost(GAppvalRecordModel recordModel) {
            List<GAppvalPostModel> postModels = new List<GAppvalPostModel>();

            if (recordModel == null || recordModel.ProcPhid == 0 || recordModel.PostPhid == 0)
                return postModels;

            postModels.Add(new GAppvalPostModel {
                PhId = 0,
                FName = "发起人"
            });

            //当前审批流程的所有审批岗位
            List<GAppvalPostModel> allPosts = GAppvalPostFacade.FindAppvalPostByProcID(recordModel.ProcPhid).OrderBy(t => t.Seq).ToList();
            //当前岗位
            GAppvalPostModel presentPost = allPosts.Find(t => t.PhId == recordModel.PostPhid);
            //当前岗位之前的岗位
            List<GAppvalPostModel> beforePosts = allPosts.FindAll(t => t.Seq < presentPost.Seq);

            postModels.AddRange(beforePosts);

            foreach (GAppvalPostModel postModel in postModels) {
                if (postModel.PhId == 0)
                {
                    if (recordModel.RefbillPhid == 0)
                    {
                        postModel.Operators = null;
                    }
                    else
                    {
                        try
                        {
                            List<GAppvalPost4OperModel> operModels = new List<GAppvalPost4OperModel>();
                            GAppvalRecordModel model = GAppvalRecordFacade.Find(t => t.RefbillPhid == recordModel.RefbillPhid && t.PostPhid == 0).Data[0];
                            User2Model user = UserFacade.Find(model.OperaPhid).Data;
                            operModels.Add(new GAppvalPost4OperModel
                            {
                                PostPhid = 0,
                                OperatorPhid = user.PhId,
                                OperatorName = user.UserName
                            });
                            postModel.Operators = operModels;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("查询发起人失败！");
                        }
                    }
                }
                else
                {
                    try
                    {

                        postModel.Operators = GAppvalPost4OperFacade.GetOperatorsByPostID(postModel.PhId);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("根据岗位id查找操作失败！");
                    }
                }
            }


            return postModels;
        }

        /// <summary>
        /// 根据流程id获取整个流程的岗位人员
        /// </summary>
        /// <param name="refbillPhid">单据主键</param>
        /// <param name="procPhid">流程id</param>
        /// <returns></returns>
        public List<GAppvalPostModel> GetBackApprovalPost2(long refbillPhid, long procPhid)
        {
            List<GAppvalPostModel> postModels = new List<GAppvalPostModel>();

            postModels.Add(new GAppvalPostModel
            {
                PhId = 0,
                FName = "发起人"
            });

            //当前审批流程的所有审批岗位
            List<GAppvalPostModel> allPosts = GAppvalPostFacade.FindAppvalPostByProcID(procPhid).OrderBy(t => t.Seq).ToList();

            postModels.AddRange(allPosts);

            foreach (GAppvalPostModel postModel in postModels)
            {
                if (postModel.PhId == 0)
                {
                    if (refbillPhid == 0)
                    {
                        postModel.Operators = null;
                    }
                    else
                    {
                        try
                        {
                            List<GAppvalPost4OperModel> operModels = new List<GAppvalPost4OperModel>();
                            GAppvalRecordModel model = GAppvalRecordFacade.Find(t => t.RefbillPhid == refbillPhid && t.PostPhid == 0 && t.FBilltype ==BillType.BeginProject).Data.ToList().OrderByDescending(t=>t.FSendDate).ToList()[0];
                            User2Model user = UserFacade.Find(model.OperaPhid).Data;
                            operModels.Add(new GAppvalPost4OperModel
                            {
                                PostPhid = 0,
                                OperatorPhid = user.PhId,
                                OperatorName = user.UserName
                            });
                            postModel.Operators = operModels;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("查询发起人失败！");
                        }
                    }
                }
                else
                {
                    try
                    {

                        postModel.Operators = GAppvalPost4OperFacade.GetOperatorsByPostID(postModel.PhId);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("根据岗位id查找操作失败！");
                    }
                }
            }
            return postModels;
        }

        /// <summary>
        /// 根据审批岗位获取审批人(包括岗位id为0的发起人)
        /// </summary>
        /// <param name="recordModel"></param>
        /// <returns></returns>
        public List<GAppvalPost4OperModel> GetOperators(GAppvalRecordModel recordModel) {

            List<GAppvalPost4OperModel> operModels = new List<GAppvalPost4OperModel>();

            if (recordModel == null)
                return operModels;


            if (recordModel.PostPhid == 0)
            {
                if (recordModel.RefbillPhid == 0)
                {
                    return operModels;
                }
                else {
                    try
                    {
                        GAppvalRecordModel model = GAppvalRecordFacade.Find(t => t.RefbillPhid == recordModel.RefbillPhid && t.PostPhid == 0).Data[0];
                        User2Model user = UserFacade.Find(model.OperaPhid).Data;
                        operModels.Add(new GAppvalPost4OperModel {
                            PostPhid = 0,
                            OperatorPhid = user.PhId,
                            OperatorName = user.UserName
                        });
                    }
                    catch (Exception e) {
                        throw new Exception("查询发起人失败！");
                    }
                }
            }
            else {
                try
                {
                    operModels = GAppvalPost4OperFacade.GetOperatorsByPostID(recordModel.PostPhid);
                }
                catch (Exception e) {
                    throw new Exception("根据岗位id查找操作失败！");
                }
            }

            return operModels;
        }

        /// <summary>
        /// 查看单个岗位的信息
        /// </summary>
        /// <param name="phid">岗位的phid</param>
        /// <returns></returns>
        public GAppvalPostAndOpersModel GetAppvalPostOpers(long phid)
        {
            GAppvalPostAndOpersModel gAppvalPostAndOpers = new GAppvalPostAndOpersModel();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<long>.Eq("PhId", phid));
            var result = this.GAppvalPostFacade.Find(dic).Data;
            if(result.Count > 0)
            {
                gAppvalPostAndOpers.GAppvalPost = result[0];
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("PostPhid", phid));
                var res = this.GAppvalPost4OperFacade.Find(dic).Data;
                if(res.Count > 0)
                {
                    var codeList = res.ToList().Select(t => t.OperatorCode).ToList();
                    dic.Clear();
                    new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<string>>.In("Dwdm", codeList))
                            .Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                            .Add(ORMRestrictions<int>.Eq("DefInt1", 1));
                    //根据操作员编码查找对应组织部门信息
                    var opers = this.CorrespondenceSettingsFacade.Find(dic).Data;//操作员编码：Dwdm;操作员姓名：DefStr1;部门代码：DefStr3;组织代码：Dydm
                    if (opers.Count > 0)
                    {
                        foreach (CorrespondenceSettingsModel data in opers)
                        {
                            if (!string.IsNullOrEmpty(data.Dydm))
                            {
                                data.DefStr4 = OrganizationFacade.FindMcByDm(data.Dydm);//DefStr4:组织名称
                            }
                            if (!string.IsNullOrEmpty(data.DefStr3))
                            {
                                data.DefStr5 = OrganizationFacade.FindMcByDm(data.DefStr3);//DefStr5:部门名称
                            }
                        }
                        foreach(GAppvalPost4OperModel gAppvalPost4Oper in res)
                        {
                            var corr = opers.Where(t => t.Dwdm == gAppvalPost4Oper.OperatorCode).ToList();
                            if(corr.Count > 0)
                            {
                                //将组织部门名存入到对象
                                gAppvalPost4Oper.OrgCode = corr[0].Dydm;
                                gAppvalPost4Oper.OrgName = corr[0].DefStr4;
                                gAppvalPost4Oper.DepCode = corr[0].DefStr3;
                                gAppvalPost4Oper.DepName = corr[0].DefStr5;
                                gAppvalPost4Oper.OperatorName = corr[0].DefStr1;
                            }

                        }
                    }
                    gAppvalPostAndOpers.GAppvalPost4Opers = res;
                }
            }
            return gAppvalPostAndOpers;
        }

        /// <summary>
        /// 获取岗位列表
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="orgId">组织id</param>
        /// <param name="uCode">用户编码</param>
        /// <param name="searchOrgid">搜索的组织</param>
        /// <param name="enableMark">是否启用</param>
        /// <param name="PostName">搜索字段</param>
        /// <returns></returns>
        public List<GAppvalPostAndOpersModel> GetAppvalPostOpersList(int PageIndex, int PageSize, long orgId, string uCode, List<long> searchOrgid, string enableMark, string PostName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<GAppvalPostAndOpersModel> gAppvalPostAndOpers = new List<GAppvalPostAndOpersModel>();

            if (!string.IsNullOrEmpty(PostName))
            {
                Dictionary<string, object> dicName = new Dictionary<string, object>();
                new CreateCriteria(dicName)
                    .Add(ORMRestrictions<string>.Like("FCode", PostName));
                Dictionary<string, object> dicCode = new Dictionary<string, object>();
                new CreateCriteria(dicCode)
                    .Add(ORMRestrictions<string>.Like("FName", PostName));
                new CreateCriteria(dic)
                    .Add(ORMRestrictions.Or(dicCode, dicName));
            }
            if (uCode != "Admin")
            {
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("OrgPhid", orgId));
            }
            else
            {
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Ge("PhId", (long)0));
            }
            if (searchOrgid != null && searchOrgid.Count > 0)
            {
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("OrgPhid", searchOrgid));
            }
            if (!string.IsNullOrEmpty(enableMark))
            {
                if(enableMark == "1")
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Byte>.Eq("FEnable", (byte)0));
                }
                else if (enableMark == "2")
                {
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<Byte>.Eq("FEnable", (byte)1));
                }
            }
            var result = this.GAppvalPostFacade.LoadWithPage(PageIndex, PageSize, dic, new string[] { "IsSystem desc"," FCode Asc" });
            if(result.Results.Count > 0)
            {
                foreach(var gAppvalPost in result.Results)
                {
                    GAppvalPostAndOpersModel gAppval = new GAppvalPostAndOpersModel();
                    gAppval.GAppvalPost = gAppvalPost;
                    dic.Clear();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<long>.Eq("PostPhid", gAppvalPost.PhId));
                    var res = this.GAppvalPost4OperFacade.Find(dic).Data;
                    if (res.Count > 0)
                    {
                        var codeList = res.ToList().Select(t => t.OperatorCode).ToList();
                        dic.Clear();
                        new CreateCriteria(dic)
                            .Add(ORMRestrictions<List<string>>.In("Dwdm", codeList))
                            .Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                            .Add(ORMRestrictions<int>.Eq("DefInt1", 1));
                        //根据操作员编码查找对应组织部门信息
                        var opers = this.CorrespondenceSettingsFacade.Find(dic).Data;//操作员编码：Dwdm;操作员姓名：DefStr1;部门代码：DefStr3;组织代码：Dydm
                        if (opers.Count > 0)
                        {
                            Dictionary<string, object> dic2 = new Dictionary<string, object>();
                            Dictionary<string, object> dic3 = new Dictionary<string, object>();
                            var depList = opers.ToList().Select(t => t.Dydm).ToList();
                            var orgList = opers.ToList().Select(t => t.DefStr3).ToList();
                            new CreateCriteria(dic2)
                                .Add(ORMRestrictions<List<string>>.In("OCode", depList));
                            new CreateCriteria(dic3)
                                .Add(ORMRestrictions<List<string>>.In("OCode", orgList));
                            dic.Clear();
                            new CreateCriteria(dic).Add(ORMRestrictions.Or(dic2, dic3));
                            var orgs = this.OrganizationFacade.Find(dic).Data;

                            foreach (CorrespondenceSettingsModel data in opers)
                            {                                
                                if(orgs.Where(t => t.OCode == data.Dydm).ToList().Count > 0)
                                    data.DefStr4 = orgs.Where(t=>t.OCode == data.Dydm).ToList()[0].OName;//DefStr4:组织名称
                                if (orgs.Where(t => t.OCode == data.DefStr3).ToList().Count > 0)
                                    data.DefStr5 = orgs.Where(t => t.OCode == data.DefStr3).ToList()[0].OName;//DefStr5:部门名称
                            }


                            foreach (GAppvalPost4OperModel gAppvalPost4Oper in res)
                            {
                                var corr = opers.Where(t => t.Dwdm == gAppvalPost4Oper.OperatorCode).ToList();
                                if (corr.Count > 0)
                                {
                                    //将组织部门名存入到对象
                                    gAppvalPost4Oper.OrgCode = corr[0].Dydm;
                                    gAppvalPost4Oper.OrgName = corr[0].DefStr4;
                                    gAppvalPost4Oper.DepCode = corr[0].DefStr3;
                                    gAppvalPost4Oper.DepName = corr[0].DefStr5;
                                    gAppvalPost4Oper.OperatorName = corr[0].DefStr1;
                                }

                            }
                        }
                        gAppval.GAppvalPost4Opers = res;
                    }

                    gAppvalPostAndOpers.Add(gAppval);
                }
            }
            return gAppvalPostAndOpers;
        }

        /// <summary>
        /// 根据组织获取岗位（2019.10.17改成岗位根据操作员对应组织权限取）
        /// </summary>
        /// <param name="userId">操作员id</param>
        /// <returns></returns>
        public IList<GAppvalPostModel> GetAppvalPostList(long userId)
        {
            List<Int64> orgs = OrganizationFacade.GetSBUnit(userId).ToList().Select(x=>x.PhId).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("OrgPhid", orgs))
                .Add(ORMRestrictions<byte>.Eq("FEnable", (byte)EnumYesNo.Yes));
            var result = this.GAppvalPostFacade.Find(dic,new string[] { "IsSystem desc", "FCode Asc" }).Data;
            return result;
        }


        /// <summary>
        /// 根据岗位主键集合删除岗位与岗位对应操作员信息
        /// </summary>
        /// <param name="phidList">岗位主键集合</param>
        /// <param name="uCode">用户账号</param>
        /// <returns></returns>
        public DeletedResult DeletedPostOpers(List<long> phidList, string uCode)
        {
            DeletedResult deletedResult = new DeletedResult();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("PostPhid", phidList));
            var record = this.GAppvalRecordFacade.Find(dic).Data;
            if(record.Count > 0)
            {
                throw new Exception("此岗位已被审批使用，无法进行删除！");
            }
            dic.Clear();
            new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PostPhid", phidList));
            var procPost = this.GAppvalProc4PostFacade.Find(dic).Data;
            if (procPost.Count > 0)
            {
                throw new Exception("此岗位已被流程使用，无法进行删除！");
            }
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("PhId", phidList))
                .Add(ORMRestrictions<byte>.Eq("IsSystem", (byte)1));
            var sysPosts = this.GAppvalPostFacade.Find(dic).Data;
            if(sysPosts.Count > 0 && uCode != "Admin")
            {
                throw new Exception("内置岗位普通操作员不能进行删除！");
            }
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("PhId", phidList));
            var posts = this.GAppvalPostFacade.Find(dic).Data;
            if(posts.Count > 0)
            {
                deletedResult = this.GAppvalPostFacade.Delete(dic);
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PostPhid", phidList));
                var opers = this.GAppvalPost4OperFacade.Find(dic).Data;
                if(opers.Count > 0)
                {
                    deletedResult = this.GAppvalPost4OperFacade.Delete(dic);
                }
            }
            return deletedResult;
        }

        /// <summary>
        /// 新增岗位与操作员
        /// </summary>
        /// <param name="gAppvalPostAndOpers">岗位与操作员对象</param>
        /// <returns></returns>
        public SavedResult<long> SavedSignle(GAppvalPostAndOpersModel gAppvalPostAndOpers)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if(gAppvalPostAndOpers != null)
            {
                GAppvalPostModel gAppvalPost  = gAppvalPostAndOpers.GAppvalPost;
                IList<GAppvalPost4OperModel> gAppvalOpers = gAppvalPostAndOpers.GAppvalPost4Opers;
                if(gAppvalPost.OrgPhid == 0)
                {
                    throw new Exception("组织参数传递有误！");
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<string>.Eq("FCode", gAppvalPost.FCode))
                    .Add(ORMRestrictions<long>.Eq("OrgPhid", gAppvalPost.OrgPhid));
                var result = this.GAppvalPostFacade.Find(dic).Data;
                if(result.Count > 0)
                {
                    throw new Exception("岗位编码重复，请重新填写！");
                }
                gAppvalPost.PersistentState = PersistentState.Added;
                savedResult = this.GAppvalPostFacade.Save<long>(gAppvalPost);
                if(savedResult.KeyCodes.Count > 0)
                {
                    var postPhid = savedResult.KeyCodes[0];
                    foreach(var oper in gAppvalOpers)
                    {
                        oper.PostPhid = postPhid;
                        oper.PersistentState = PersistentState.Added;
                    }
                    savedResult = this.GAppvalPost4OperFacade.Save<long>(gAppvalOpers);
                }
                return savedResult;
            }
            else
            {
                throw new Exception("对象传递有误！");
            }

        }

        /// <summary>
        /// 修改岗位与操作员信息
        /// </summary>
        /// <param name="gAppvalPostAndOpers">岗位以及操作员对象</param>
        /// <returns></returns>
        public SavedResult<long> UpdateSignle(GAppvalPostAndOpersModel gAppvalPostAndOpers)
        {
            GAppvalPostModel gAppvalPost = gAppvalPostAndOpers.GAppvalPost;
            IList<GAppvalPost4OperModel> gAppvalOpers = gAppvalPostAndOpers.GAppvalPost4Opers;
            SavedResult<long> savedResult = new SavedResult<long>();
            if(gAppvalPost.PhId == 0 || gAppvalPost.OrgPhid == 0)
            {
                throw new Exception("所传的岗位信息有误！");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<long>.Eq("PostPhid", gAppvalPost.PhId));
            var record = this.GAppvalRecordFacade.Find(dic).Data;
            //if (record.Count > 0)
            //{
            //    throw new Exception("此岗位已被使用，无法进行修改！");
            //}
            dic.Clear();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<string>.Eq("FCode", gAppvalPost.FCode))
                .Add(ORMRestrictions<long>.Eq("OrgPhid", gAppvalPost.OrgPhid))
                .Add(ORMRestrictions<long>.NotEq("PhId", gAppvalPost.PhId));
            var result = this.GAppvalPostFacade.Find(dic).Data;
            if (result.Count > 0)
            {
                throw new Exception("岗位编码重复，请重新填写！");
            }
            gAppvalPost.PersistentState = PersistentState.Modified;
            savedResult = this.GAppvalPostFacade.Save<long>(gAppvalPost);
            if (savedResult.KeyCodes.Count > 0)
            {
                dic.Clear();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<long>.Eq("PostPhid", gAppvalPost.PhId));
                var operList = this.GAppvalPost4OperFacade.Find(dic).Data;
                if(operList.Count > 0)
                {
                    var delete = this.GAppvalPost4OperFacade.Delete(dic);
                }
                var postPhid = savedResult.KeyCodes[0];
                foreach (var oper in gAppvalOpers)
                {
                    oper.PostPhid = postPhid;
                    oper.PersistentState = PersistentState.Added;
                }
                savedResult = this.GAppvalPost4OperFacade.Save<long>(gAppvalOpers);
            }
            return savedResult;
        }
        #endregion
    }
}

