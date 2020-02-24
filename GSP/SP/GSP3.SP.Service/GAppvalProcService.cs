#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Service
    * 类 名 称：			GAppvalProcService
    * 文 件 名：			GAppvalProcService.cs
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
using Enterprise3.Common.Base.Criterion;
using GQT3.QT.Facade.Interface;
using SUP.Common.Base;
using GSP3.SP.Model.Extra;
using GQT3.QT.Model.Domain;
using GData3.Common.Utils;
using GSP3.SP.Model.Enums;

namespace GSP3.SP.Service
{
	/// <summary>
	/// GAppvalProc服务组装处理类
	/// </summary>
    public partial class GAppvalProcService : EntServiceBase<GAppvalProcModel>, IGAppvalProcService
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalProc业务外观处理对象
        /// </summary>
		IGAppvalProcFacade GAppvalProcFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IGAppvalProcFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IGAppvalProcCondsFacade GAppvalProcCondsFacade { get; set; }

        private IGAppvalProc4PostFacade GAppvalProc4PostFacade { get; set; }

        private IQTSysSetFacade QTSysSetFacade { get; set; }

        private IOrganizationFacade OrganizationFacade { get; set; }

        private IGAppvalPostFacade GAppvalPostFacade { get; set; }

        private IGAppvalRecordFacade GAppvalRecordFacade { get; set; }
        #endregion

        #region 实现 IGAppvalProcService 业务添加的成员

        /// <summary>
        /// 查找审批流程
        /// </summary>
        /// <param name="phid">审批流程phid</param>
        /// <returns></returns>
        public GAppvalProcModel FindSingle(long phid) {
            return GAppvalProcFacade.Find(phid).Data;
        }

        /// <summary>
        /// 根据组织id，单据类型，审批类型获取所有的审批流程
        /// </summary>
        /// <param name="orgid">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        public List<GAppvalProcModel> GetAppvalProc(long orgid, string bType, long splx_phid) {

            List<GAppvalProcModel> procModels = new List<GAppvalProcModel>();

            if (orgid == 0 || string.IsNullOrEmpty(bType)) {
                return procModels;
            }

            List<long> orgids = new List<long>();
            orgids.Add(orgid);
            procModels = GAppvalProcFacade.GetAppvalProc(orgids, bType, splx_phid);

            return procModels;
        }

        /// <summary>
        /// 根据组织id，单据类型，审批类型,单据主键获取所有的符合条件的审批流程
        /// </summary>
        /// <param name="orgid">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <param name="bPhIds">主键结合</param>
        /// <returns></returns>
        public List<GAppvalProcModel> GetAppvalProcList(long orgid, string bType, long splx_phid, List<long> bPhIds)
        {
            List<GAppvalProcModel> procModels = new List<GAppvalProcModel>();
            if (orgid == 0 || string.IsNullOrEmpty(bType))
            {
                return procModels;
            }
            List<long> orgids = new List<long>();
            orgids.Add(orgid);
            procModels = GAppvalProcFacade.GetAppvalProcList(orgids, bType, splx_phid, bPhIds);
            return procModels;
        }

        /// <summary>
        /// 判断审批流程是否被引用
        /// </summary>
        /// <param name="proc_phid">审批类型id</param>
        /// <returns></returns>
        public bool ProcIsUsed(long proc_phid) {
            bool symbol = false;
            if (proc_phid == 0)
                return symbol;

            symbol = GAppvalProcFacade.ProcIsUsed(proc_phid);

            return symbol;
        }

        /// <summary>
        /// 删除审批类型
        /// </summary>
        /// <param name="proc_phid">审批类型id</param>
        /// <returns></returns>
        public DeletedResult PostDeleteProcType(long proc_phid) {
            if (proc_phid == 0)
                return null;

            IList<GAppvalProcModel> procModels = GAppvalProcFacade.Find(t => t.SPLXPhid == proc_phid).Data;

            //批量删除审批流程
            GAppvalProcFacade.DeleteAppvalProc(procModels);
            
            DeletedResult deletedResult = QTSysSetFacade.Delete(proc_phid);

            return deletedResult;
        }

        /// <summary>
        /// 批量删除审批类型
        /// </summary>
        /// <param name="ids">审批类型id集合</param>
        /// <returns></returns>
        public DeletedResult PostDeleteProcTypes(List<long> ids) {
            if (ids == null || ids.Count == 0)
                return null;

            foreach (long proc_phid in ids) {
                IList<GAppvalProcModel> procModels = GAppvalProcFacade.Find(t => t.SPLXPhid == proc_phid).Data;

                //批量删除审批流程
                GAppvalProcFacade.DeleteAppvalProc(procModels);
            }

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<List<long>>.In("PhId", ids));
            DeletedResult deletedResult = QTSysSetFacade.Delete(dicWhere);

            return deletedResult;
        }

        /// <summary>
        /// 新增审批流程
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        public SavedResult<Int64> PostAddProc(GAppvalProcModel procModel) {

            //保存审批流程
            SavedResult<Int64> savedResult = GAppvalProcFacade.SaveAppvalProc(procModel);

            return savedResult;
        }

        /// <summary>
        /// 新增审批流程
        /// </summary>
        /// <param name="procModels"></param>
        /// <returns></returns>
        public SavedResult<Int64> PostAddProcs(List<GAppvalProcModel> procModels) {

            SavedResult<Int64> savedResult = null;

            foreach (GAppvalProcModel procModel in procModels) {
                //保存审批流程
                savedResult = GAppvalProcFacade.SaveAppvalProc(procModel);
            }

            return savedResult;
        }

        /// <summary>
        /// 分页获取审批流程数据
        /// </summary>
        /// <param name="orgid">组织id</param>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="queryStr">流程编码或流程名称的查询条件</param>
        /// <param name="count">总记录条数</param>
        /// <returns></returns>
        public List<GAppvalProcModel> GetProcList(long orgid, long approvalTypeId, string bType, int pageIndex, int pageSize, string queryStr,out int count)
        {
            List<GAppvalProcModel> procModels = new List<GAppvalProcModel>();
            if (orgid == 0 || approvalTypeId == 0 || string.IsNullOrEmpty(bType)) {
                count = 0;
                return procModels;
            }

            List<long> orgids = new List<long>();
            orgids.Add(orgid);
            //取当前组织的所有下级组织部门
            List<OrganizeModel> organizeModels = OrganizationFacade.GetAllChildList(orgid);
            if (organizeModels != null && organizeModels.Count > 0) {
                orgids.AddRange(organizeModels.Select(t => t.PhId));
            }

            //根据组织id，单据类型，审批类型获取所有的审批流程
            procModels = GAppvalProcFacade.GetAppvalProc(orgids, bType,approvalTypeId);

            //同一个审批流程的，不同启用组织的数据合并到同一个审批流程中
            Dictionary<string, GAppvalProcModel> map = new Dictionary<string, GAppvalProcModel>();
            foreach (GAppvalProcModel model in procModels) {
                if (map.ContainsKey(model.FCode))
                {
                    Organize organize = new Organize();
                    organize.OrgId = model.OrgPhid;
                    organize.OrgCode = model.OrgCode;
                    organize.OrgName = model.OrgName;

                    GAppvalProcModel procModel = map[model.FCode];
                    procModel.Organizes.Add(organize);
                }
                else {
                    List<Organize> organizes = new List<Organize>();
                    Organize organize = new Organize();
                    organize.OrgId = model.OrgPhid;
                    organize.OrgCode = model.OrgCode;
                    organize.OrgName = model.OrgName;
                    organizes.Add(organize);

                    model.Organizes = organizes;
                    map.Add(model.FCode,model);
                }
            }
            procModels = map.Values.ToList();

            //筛选数据
            if (!string.IsNullOrEmpty(queryStr)) {
                procModels = procModels.FindAll(t => (t.FCode != null && t.FCode.IndexOf(queryStr) > -1) || (t.FName != null && t.FName.IndexOf(queryStr) > -1));
            }
            count = procModels.Count;

            //分页
            procModels = procModels.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return procModels;
        }

        /// <summary>
        /// 获取审批流程明细
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="proc_code">审批流程编码</param>
        /// <param name="orgids">组织id集合</param>
        /// <returns></returns>
        public GAppvalProcModel GetProcDetail(long approvalTypeId, string bType, string proc_code,List<long> orgids) {
            
            //获取审批流程
            List<GAppvalProcModel> procModels = GAppvalProcFacade.GetAppvalProc(orgids, bType, approvalTypeId);
            if (procModels == null || procModels.Count == 0)
            {
                throw new Exception("审批流程为空！");
            }
            else {
                procModels = procModels.FindAll(t => t.FCode == proc_code);
            }

            List<Organize> organizes = new List<Organize>();
            foreach (GAppvalProcModel model in procModels)
            {
                Organize organize = new Organize();
                organize.OrgId = model.OrgPhid;
                organize.OrgCode = model.OrgCode;
                organize.OrgName = model.OrgName;
                organizes.Add(organize);
            }
            GAppvalProcModel procModel = procModels[0];
            procModel.Organizes = organizes;

            //获取审批流程对应的审批岗位
            List<GAppvalPostModel> postModels = GAppvalPostFacade.FindAppvalPostByProcID(procModel.PhId);
            procModel.PostModels = postModels;

            //获取审批流程的启用条件
            FindedResults<GAppvalProcCondsModel> findedResults = GAppvalProcCondsFacade.Find(t => t.ProcPhid == procModel.PhId);
            if (findedResults != null && findedResults.Data != null && findedResults.Data.Count > 0) {
                List<GAppvalProcCondsModel> procCondsModels = findedResults.Data.ToList();
                procModel.CondsModels = procCondsModels;
            }

            return procModel;
        }

        /// <summary>
        /// 修改审批流程
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="proc_code">审批流程编码</param>
        /// <param name="orgids">组织id集合</param>
        /// <param name="procModels">新增集合</param>
        /// <param name="uCode">用户账号</param>
        /// <returns></returns>
        public SavedResult<Int64> PostUpdateProc(long approvalTypeId, string bType, string proc_code, List<long> orgids, List<GAppvalProcModel> procModels, string uCode)
        {
            SavedResult<Int64> savedResult = null;

            //获取审批流程
            List<GAppvalProcModel> existProcModels = GAppvalProcFacade.GetAppvalProc(orgids, bType, approvalTypeId);

            if (existProcModels == null || existProcModels.Count == 0)
            {
                throw new Exception("审批流程为空！");
            }
            else {
                existProcModels = existProcModels.FindAll(t => t.FCode == proc_code);
            }

            //判断审批流程是否被引用
            foreach (GAppvalProcModel model in existProcModels) {
                IList<GAppvalRecordModel> models = GAppvalRecordFacade.Find(t => t.ProcPhid == model.PhId).Data;
                if (models != null && models.Count > 0) {
                    foreach(var mo in models)
                    {
                        throw new Exception("修改失败,流程为'" + model.FName + "'的审批流存在审批中的单据，无法修改！请审批完成后再来修改！！");
                        //if (mo.FApproval == (byte)Approval.Wait)
                        //{
                        //    throw new Exception("修改失败,流程为'" +  model.FName + "'的审批流存在审批中的单据，无法修改！请审批完成后再来修改！！");
                        //}
                    }
                    
                }
                if (uCode != "Admin" && model.IsSystem == (byte)1)
                {
                    throw new Exception("修改失败,流程为'" + model.FName + "'为内置流程，不允许普通操作员进行修改！");
                }
            }

            //如果审批流程没有被引用，更新操作为：删除审批流程，重新保存
            //批量删除审批流程
            GAppvalProcFacade.DeleteAppvalProc(existProcModels);
            
            foreach (GAppvalProcModel procModel in procModels)
            {
                if(uCode == "Admin")
                {
                    procModel.IsSystem = (byte)1;
                }
                //保存审批流程
                savedResult = GAppvalProcFacade.SaveAppvalProc(procModel);
            }

            return savedResult;
        }

        /// <summary>
        /// 删除审批流程
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="proc_code">审批流程编码</param>
        /// <param name="orgids">组织id集合</param>
        /// <param name="uCode">用户账号</param>
        public void PostDeleteProc(long approvalTypeId, string bType, string proc_code, List<long> orgids, string uCode) {
            //获取审批流程
            List<GAppvalProcModel> existProcModels = GAppvalProcFacade.GetAppvalProc(orgids, bType, approvalTypeId);

            if (existProcModels != null)
            {
                existProcModels = existProcModels.FindAll(t => t.FCode == proc_code);
            }
            else {
                throw new Exception("审批流程为空！");
            }

            if (existProcModels.Count == 0)
            {
                throw new Exception("审批流程为空！");
            }

            //判断审批流程是否被引用
            foreach (GAppvalProcModel model in existProcModels)
            {
                IList<GAppvalRecordModel> models = GAppvalRecordFacade.Find(t => t.ProcPhid == model.PhId).Data;
                if (models != null && models.Count > 0)
                {
                    throw new Exception("删除失败,流程为'" + model.FName + "'已经被引用！");
                }
                if (uCode != "Admin" && model.IsSystem == (byte)1)
                {
                    throw new Exception("删除失败,流程为'" + model.FName + "'为内置流程，不允许普通操作员进行删除！");
                }
            }

            //批量删除审批流程
            GAppvalProcFacade.DeleteAppvalProc(existProcModels);
        }

        /// <summary>
        /// 批量删除审批流程
        /// </summary>
        /// <param name="procModels"></param>
        public void PostDeleteProc(List<GAppvalProcModel> procModels) {
            if (procModels == null || procModels.Count == 0) {
                throw new Exception("审批流程为空！");
            }

            foreach (GAppvalProcModel model in procModels) {
                List<long> orgids = model.Organizes.Select(t => t.OrgId).ToList();
                //获取审批流程
                List<GAppvalProcModel> existProcModels = GAppvalProcFacade.GetAppvalProc(orgids, model.FBilltype, model.SPLXPhid);

                if (existProcModels != null)
                {
                    existProcModels = existProcModels.FindAll(t => t.FCode == model.FCode);
                }
                else
                {
                    throw new Exception("审批流程为空！");
                }

                if (existProcModels.Count == 0)
                {
                    throw new Exception("审批流程为空！");
                }

                //判断审批流程是否被引用
                foreach (GAppvalProcModel proc in existProcModels)
                {
                    IList<GAppvalRecordModel> models = GAppvalRecordFacade.Find(t => t.ProcPhid == proc.PhId).Data;
                    if (models != null && models.Count > 0)
                    {
                        throw new Exception("修改失败,流程为'" + proc.FName + "'已经被引用！");
                    }
                }

                //批量删除审批流程
                GAppvalProcFacade.DeleteAppvalProc(existProcModels);
            }
        }

        /// <summary>
        /// 更新启用组织
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        public int PostUpdateProcOrganize(GAppvalProcModel procModel) {
            int result = 0;

            List<long> orgids = procModel.Organizes.Select(t => t.OrgId).ToList();
            //获取审批流程
            List<GAppvalProcModel> existProcModels = GAppvalProcFacade.GetAppvalProc(orgids, procModel.FBilltype, procModel.SPLXPhid);

            if (existProcModels != null)
            {
                existProcModels = existProcModels.FindAll(t => t.FCode == procModel.FCode);
            }
            else
            {
                throw new Exception("审批流程为空！");
            }

            List<long> deleteOrgs = new List<long>();
            List<Organize> addOrgs = new List<Organize>();

            foreach (Organize model in procModel.NewOrganizes)
            {
                if (!procModel.Organizes.Exists(t => t.OrgId == model.OrgId)) {
                    addOrgs.Add(model);
                }
            }

            foreach (Organize model in procModel.Organizes)
            {
                if (!procModel.NewOrganizes.Exists(t => t.OrgId == model.OrgId))
                {
                    deleteOrgs.Add(model.OrgId);
                }
            }
            
            //获取审批流程和审批岗位的对应关系
            List<GAppvalProc4PostModel> proc4PostModels = GAppvalProc4PostFacade.Find(t => t.ProcPhid == procModel.PhId).Data.ToList();
            //获取审批流程的启用条件
            List<GAppvalProcCondsModel> procCondsModels = null;
            FindedResults<GAppvalProcCondsModel> findedResults = GAppvalProcCondsFacade.Find(t => t.ProcPhid == procModel.PhId);
            if (findedResults != null && findedResults.Data != null && findedResults.Data.Count > 0)
            {
                procCondsModels = findedResults.Data.ToList();
            }
            else {
                procCondsModels = new List<GAppvalProcCondsModel>();
            }

            //新增审批流程
            foreach (Organize model in addOrgs) {
                /*
                 * 这里审批流程添加启用组织，虽然审批流程，审批流程和岗位的对应关系，审批流程条件这些数据都是一样的
                 * 但是也不能直接将查询得到的数据赋值给审批流程对象去保存
                 * 因为循环过程中，每一个保存的审批流程对象，审批流程和岗位的对应关系对象，审批流程条件对象实际指向的都是内存中同一个内存地址，也就是同一个对象
                 * 又因为事务没有提交，循环保存的时候，同一个对象被多次保存，生成不同的主键，会导致报错，
                 * 所有要通过工具类复制一个对象
                 */

                GAppvalProcModel existProcModel = existProcModels[0];
                GAppvalProcModel gAppval = CommonUtils.TransReflection<GAppvalProcModel, GAppvalProcModel>(existProcModel);
                gAppval.OrgPhid = model.OrgId;
                gAppval.OrgCode = model.OrgCode;

                List<GAppvalProc4PostModel> proc4Posts = new List<GAppvalProc4PostModel>();
                foreach (GAppvalProc4PostModel proc4Post in proc4PostModels) {
                    GAppvalProc4PostModel gAppvalProc = CommonUtils.TransReflection<GAppvalProc4PostModel, GAppvalProc4PostModel>(proc4Post);
                    proc4Posts.Add(gAppvalProc);
                }

                List<GAppvalProcCondsModel> condsModels = new List<GAppvalProcCondsModel>();
                foreach (GAppvalProcCondsModel conds in procCondsModels) {
                    GAppvalProcCondsModel procCondsModel = CommonUtils.TransReflection<GAppvalProcCondsModel, GAppvalProcCondsModel>(conds);
                    condsModels.Add(procCondsModel);
                }
                gAppval.Proc4PostModels = proc4Posts;
                gAppval.CondsModels = condsModels;

                //保存审批流程
                SavedResult<Int64> savedResult = GAppvalProcFacade.SaveAppvalProc(gAppval);
                result += savedResult.SaveRows;
            }

            //删除审批流程
            List<GAppvalProcModel> deleteProcs = existProcModels.FindAll(t => deleteOrgs.Contains(t.OrgPhid));
            result += GAppvalProcFacade.DeleteAppvalProc(deleteProcs);


            return result;
        }
        #endregion
    }
}

