#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetProcessCtrlService
    * 命名空间：        GYS3.YS.Service
    * 文 件 名：        BudgetProcessCtrlService.cs
    * 创建时间：        2018/9/10 
    * 作    者：        夏华军    
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
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;

using GYS3.YS.Service.Interface;
using GYS3.YS.Facade.Interface;
using GQT3.QT.Facade.Interface;
using GYS3.YS.Model.Domain;
using GQT3.QT.Model.Domain;

namespace GYS3.YS.Service
{
	/// <summary>
	/// BudgetProcessCtrl服务组装处理类
	/// </summary>
    public partial class BudgetProcessCtrlService : EntServiceBase<BudgetProcessCtrlModel>, IBudgetProcessCtrlService
    {
		#region 类变量及属性
		/// <summary>
        /// BudgetProcessCtrl业务外观处理对象
        /// </summary>
		IBudgetProcessCtrlFacade BudgetProcessCtrlFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IBudgetProcessCtrlFacade;
            }
        }
        /// <summary>
        /// BudgetMstFacade业务外观处理对象
        /// </summary>
		private IBudgetMstFacade BudgetMstFacade { get; set; }

        /// <summary>
        /// OrganizationFacade业务外观处理对象
        /// </summary>
        private IOrganizationFacade OrganizationFacade { get; set; }

        /// <summary>
        /// UserOrgFacade
        /// </summary>
        private IUserOrgFacade UserOrgFacade { get; set; }
        #endregion

        #region 实现 IBudgetProcessCtrlService 业务添加的成员

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public PagedResult<BudgetProcessCtrlModel> GetBudgetProcessCtrlDistinctList(DataStoreParam storeParam, string query,string userId) {

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            /*if (query != null && query != "")
            {
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FOcode", query));
                new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FOname", query));
                new CreateCriteria(dicWhere).Add(ORMRestrictions.Or(dicWhere1, dicWhere2));
            }*/
            if (userId != null && userId != "") {
                //根据用户id查出所有组织
                long userid = Convert.ToInt64(userId);
                new CreateCriteria(dicWhere1).Add(ORMRestrictions<Int64>.Eq("UserId", userid));
                FindedResults<UserOrganize2Model> findedResult = UserOrgFacade.Find(dicWhere1);

                List<long> orgIds = new List<long>();
                for (int i = 0; i < findedResult.Data.Count; i++) {
                    orgIds.Add(findedResult.Data[i].OrgId);
                }
                //根据组织id，查出所有的组织code
                new CreateCriteria(dicWhere2)
                    .Add(ORMRestrictions<List<Int64>>.In("PhId", orgIds))
                    .Add(ORMRestrictions<string>.Eq("IfCorp", "Y"));
                FindedResults<OrganizeModel> findedResult2 = OrganizationFacade.Find(dicWhere2);

                List<string> orgCodes = new List<string>();
                for (int i = 0; i < findedResult2.Data.Count; i++) {
                    orgCodes.Add(findedResult2.Data[i].OCode);
                }

                new CreateCriteria(dicWhere).Add(ORMRestrictions<List<string>>.In("FOcode", orgCodes));
            }
            var result = base.ServiceHelper.LoadWithPageInfinity("GQT.QT.GetAllProcessSetting", dicWhere);
            IList<BudgetProcessCtrlModel> processCtrlModels = result.Results;
            List<BudgetProcessCtrlModel> distinctList = processCtrlModels.GroupBy(r => r.FOcode).Select(r => r.First()).ToList();
            result.Results = distinctList;
            result.TotalItems = distinctList.Count;
            return result;
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        public void GetBudgetProcessCtrlPorcessList(DataStoreParam storeParam, string focode,string FYear, out List<BudgetProcessCtrlModel> list) {
            Dictionary<string, object> where = new Dictionary<string, object>();
            Dictionary<string, object> where2 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            List<BudgetProcessCtrlModel> budgetProcessesList = new List<BudgetProcessCtrlModel>();
            if (focode != null && focode != "")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FOcode", focode)).Add(ORMRestrictions<string>.Eq("FYear", FYear));

                //根据组织找出所有的部门
                new CreateCriteria(where).Add(ORMRestrictions<string>.Eq("OCode", focode));
                FindedResults<OrganizeModel> findedResult = OrganizationFacade.Find(where);
                new CreateCriteria(where2).Add(ORMRestrictions<Int64>.Eq("ParentOrgId", findedResult.Data[0].PhId)).Add(ORMRestrictions<string>.Eq("IfCorp", "N"));
                FindedResults<OrganizeModel> findedResults2 = OrganizationFacade.Find(where2);
                //根据组织找出进度控制表中已经存在的部门
                FindedResults<BudgetProcessCtrlModel> findedResults3 = base.Find(dicWhere);
                List<OrganizeModel> allOrganizeList = findedResults2.Data.ToList();
                //为民生银行收入预算做准，加入当前组织的进度控制
                var orgSelf = this.OrganizationFacade.Find(t => t.PhId == findedResult.Data[0].PhId && t.IfCorp == "Y").Data;
                if(orgSelf != null && orgSelf.Count > 0)
                {
                    allOrganizeList.Add(orgSelf[0]);
                }


                List<BudgetProcessCtrlModel> existOrganizeList = findedResults3.Data.ToList();
                
                //if (allOrganizeList.Count != existOrganizeList.Count)
                //{
                    //BudgetProcessCtrlFacade.UpdateBudgetProcess(allOrganizeList, existOrganizeList,findedResult.Data[0],focode);
                    //budgetProcessesList = new List<BudgetProcessCtrlModel>();
                for (int i = 0; i < allOrganizeList.Count; i++)
                {
                    if (!existOrganizeList.Exists(t => t.FDeptCode == allOrganizeList[i].OCode))
                    {
                        BudgetProcessCtrlModel budgetProcessCtrlModel = new BudgetProcessCtrlModel();
                        budgetProcessCtrlModel.PersistentState = PersistentState.Added;
                        budgetProcessCtrlModel.FOcode = focode;
                        budgetProcessCtrlModel.FOname = findedResult.Data[0].OName;
                        budgetProcessCtrlModel.FDeptCode = allOrganizeList[i].OCode;
                        budgetProcessCtrlModel.FDeptName = allOrganizeList[i].OName;
                        budgetProcessCtrlModel.FProcessStatus = "1";
                        budgetProcessCtrlModel.FYear = FYear;
                        budgetProcessCtrlModel.StartDt =Convert.ToDateTime("1990-01-01");//时间初始化
                        budgetProcessCtrlModel.StartDt2 = Convert.ToDateTime("1990-01-01");//时间初始化
                        budgetProcessCtrlModel.EndDt = Convert.ToDateTime("1990-12-31");//时间初始化
                        budgetProcessCtrlModel.EndDt2 = Convert.ToDateTime("1990-12-31");//时间初始化

                        budgetProcessesList.Add(budgetProcessCtrlModel);
                    }
                }

                for (int i = 0; i < existOrganizeList.Count; i++) {
                    if (existOrganizeList[i].FDeptCode != null && existOrganizeList[i].FDeptCode != "") {
                        if (!allOrganizeList.Exists(t => t.OCode == existOrganizeList[i].FDeptCode))
                        {
                            Dictionary<string, object> dicWhere4 = new Dictionary<string, object>();
                            new CreateCriteria(dicWhere4)
                                .Add(ORMRestrictions<string>.Eq("FOcode", focode))
                                .Add(ORMRestrictions<string>.Eq("FDeptCode", existOrganizeList[i].FDeptCode));
                            base.Delete(dicWhere4);
                        }
                    }    
                }
                   
                    //SavedResult<Int64> savedResult = BudgetProcessCtrlFacade.Save<Int64>(budgetProcessesList);
                //}
                
            }

            list = budgetProcessesList;

            //var result = base.LoadWithPage(storeParam.PageIndex, storeParam.PageSize, dicWhere);
            //if (result.Results.Count == 1 && ("".Equals(result.Results[0].FDeptCode) || result.Results[0].FDeptCode == null))
            //{
            //    result = new PagedResult<BudgetProcessCtrlModel>();
            //}
            //return result;
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回</returns>
        public PagedResult<BudgetProcessCtrlModel> GetBudgetProcessCtrlPorcessList2(DataStoreParam storeParam, string focode,string FYear) {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (focode != null && focode != "")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FOcode", focode));
            }
            if (FYear != null && FYear != "")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            }
            var result = base.LoadWithPage(storeParam.PageIndex, storeParam.PageSize, dicWhere);
            if (result.Results.Count == 1 && ("".Equals(result.Results[0].FDeptCode) || result.Results[0].FDeptCode == null))
            {
                result = new PagedResult<BudgetProcessCtrlModel>();
            }
            return result;
        }

        /// <summary>
        /// 不分页取列表数据
        /// </summary>
        /// <returns>返回</returns>
        public List<BudgetProcessCtrlModel> GetBudgetProcessCtrlPorcessList3(string focode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            if (focode != null && focode != "")
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FOcode", focode));
            }
            return base.Find(dicWhere).Data.ToList();
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>List</returns>
        public IList<BudgetProcessCtrlModel> FindBudgetProcessCtrlModelByList(List<BudgetProcessCtrlModel> list) {
            List<Int64> ids = new List<long>();
            for (int i=0;i<list.Count;i++) {
                ids.Add(list[i].PhId);
            }
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<List<Int64>>.In("PhId", ids));
            FindedResults<BudgetProcessCtrlModel> results = base.ServiceHelper.Find<BudgetProcessCtrlModel>(dicWhere);
            return results.Data;
        }
        /// <summary>
        /// 根据预算单位和预算部门查找部门所在预算进度
        /// </summary>
        /// <param name="oCode"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public string FindBudgetProcessCtrl(string oCode,string deptCode,string FYear)
        {
            
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FOcode", oCode))
                     .Add(ORMRestrictions<string>.Eq("FDeptCode", deptCode)).Add(ORMRestrictions<string>.Eq("FYear", FYear));
            FindedResults<BudgetProcessCtrlModel> results = base.ServiceHelper.Find<BudgetProcessCtrlModel>(dicWhere);
            if(results.Data.Count() > 0)
            {
                return results.Data[0].FProcessStatus;
            }
            return "";
        }

        /// <summary>
        /// 根据预算部门查找部门所在预算进度
        /// </summary>
       
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public FindedResults<BudgetProcessCtrlModel> FindBudgetProcessCtrlByList( List<string> deptCode,string FYear)
        {

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<IList<String>>.In("FDeptCode", deptCode)).Add(ORMRestrictions<String>.Eq("FYear", FYear));

            FindedResults<BudgetProcessCtrlModel> results = base.ServiceHelper.Find<BudgetProcessCtrlModel>(dicWhere);
            return results;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public SavedResult<Int64> SaveProcessSetting(List<BudgetProcessCtrlModel> models,string symbol) {
            SavedResult<Int64> result = new SavedResult<Int64>();
            //进度控制是否做判断
            if (symbol == "1")
            {
                /*Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                PagedResult<BudgetProcessCtrlModel> pagedResult = base.ServiceHelper.LoadWithPageInfinity("GQT.QT.GetAllProcessSetting",dicWhere);
                List<BudgetProcessCtrlModel> processList = pagedResult.Results.ToList();*/
                if (models != null && models.Count>0)
                {
                    //for (int i = 0; i < models.Count; i++)
                    //{
                    //    BudgetProcessCtrlModel model = models[i];
                    //    BudgetProcessCtrlModel budget = processList.Find(t => t.PhId == model.PhId);
                    //    //保证不能设置之前的状态
                    //    if ("1".Equals(budget.FProcessStatus))
                    //    {
                    //        if (!("1".Equals(model.FProcessStatus) || "2".Equals(model.FProcessStatus))) {
                    //            result.Status = ResponseStatus.Error;
                    //            result.Msg = "设置失败！";
                    //            return result;
                    //        }
                    //    }
                    //    else if ("2".Equals(budget.FProcessStatus))
                    //    {
                    //        if (!("2".Equals(model.FProcessStatus) || "3".Equals(model.FProcessStatus)))
                    //        {
                    //            result.Status = ResponseStatus.Error;
                    //            result.Msg = "设置失败！";
                    //            return result;
                    //        }
                    //    }
                    //    else if ("3".Equals(budget.FProcessStatus))
                    //    {
                    //        if (!("3".Equals(model.FProcessStatus) || "4".Equals(model.FProcessStatus)))
                    //        {
                    //            result.Status = ResponseStatus.Error;
                    //            result.Msg = "设置失败！";
                    //            return result;
                    //        }
                    //    }
                    //    else if ("4".Equals(budget.FProcessStatus))
                    //    {
                    //        if (!"4".Equals(model.FProcessStatus))
                    //        {
                    //            result.Status = ResponseStatus.Error;
                    //            result.Msg = "设置失败！";
                    //            return result;
                    //        }
                    //    }
                    //}
                    foreach(var a in models)
                    {
                        result = BudgetProcessCtrlFacade.Save<Int64>(a, "");
                    }
                    //result = base.Save<Int64>(models, "");
                }
                else {
                    result.Status = ResponseStatus.Error;
                    result.Msg = "设置失败，数据异常！";
                    return result;
                }
            }
            else {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                PagedResult<BudgetProcessCtrlModel> pagedResult = base.ServiceHelper.LoadWithPageInfinity("GQT.QT.GetAllProcessSetting", dicWhere);
                List<BudgetProcessCtrlModel> processList = pagedResult.Results.ToList();
                if (models != null && models.Count > 0)
                {
                    for (int i = 0; i < models.Count; i++)
                    {
                        BudgetProcessCtrlModel model = models[i];
                        BudgetProcessCtrlModel budget = processList.Find(t => t.PhId == model.PhId);
                        //保证不能设置之前的状态
                        if ("1".Equals(budget.FProcessStatus))
                        {
                            if (!("1".Equals(model.FProcessStatus) || "2".Equals(model.FProcessStatus)))
                            {
                                result.Status = ResponseStatus.Error;
                                result.Msg = "设置失败！";
                                return result;
                            }
                        }
                        else if ("2".Equals(budget.FProcessStatus))
                        {
                            if (!("2".Equals(model.FProcessStatus) || "3".Equals(model.FProcessStatus)))
                            {
                                result.Status = ResponseStatus.Error;
                                result.Msg = "设置失败！";
                                return result;
                            }
                        }
                        else if ("3".Equals(budget.FProcessStatus))
                        {
                            if (!("3".Equals(model.FProcessStatus) || "4".Equals(model.FProcessStatus)))
                            {
                                result.Status = ResponseStatus.Error;
                                result.Msg = "设置失败！";
                                return result;
                            }
                        }
                        else if ("4".Equals(budget.FProcessStatus))
                        {
                            if (!"4".Equals(model.FProcessStatus))
                            {
                                result.Status = ResponseStatus.Error;
                                result.Msg = "设置失败！";
                                return result;
                            }
                        }
                        //判断当前部门下的所有项目是否已经审批完
                        Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere2)
                            .Add(ORMRestrictions<string>.Eq("FLifeCycle", "0"))
                            .Add(ORMRestrictions<string>.Eq("FBudgetDept", model.FDeptCode))
                            .Add(ORMRestrictions<List<string>>.In("FApproveStatus", new List<string> { "1","2"}));
                        PagedResult<BudgetMstModel> budgemstResult = BudgetMstFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetBudgetApproveStatus", dicWhere2);
                        if (budgemstResult.TotalItems > 0) {
                            result.Status = ResponseStatus.Error;
                            result.Msg = "设置失败，项目审批中！";
                            return result;
                        }
                    }
                    result = base.Save<Int64>(models, "");
                }
                else
                {
                    result.Status = ResponseStatus.Error;
                    result.Msg = "设置失败，数据异常！";
                    return result;
                }
            }
            return result;
        }
        #endregion
    }
}

