#region Summary
/**************************************************************************************
    * 类 名 称：        GHSubjectFacade
    * 命名空间：        GYS3.YS.Facade
    * 文 件 名：        GHSubjectFacade.cs
    * 创建时间：        2018/11/26 
    * 作    者：        董泉伟    
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
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GYS3.YS.Facade.Interface;
using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;

using GYS3.YS.Model.Enums;
using NG3.WorkFlow.Interfaces;
using Enterprise3.Common.Base.Criterion;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Helpers;

using NG3.WorkFlow.Rule;
using Newtonsoft.Json.Linq;
using SUP.Common.Base;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// GHSubject业务组装处理类
	/// </summary>
    public partial class GHSubjectFacade : EntFacadeBase<GHSubjectModel>, IGHSubjectFacade, IWorkFlowPlugin
    {
		#region 类变量及属性
		/// <summary>
        /// GHSubject业务逻辑处理对象
        /// </summary>
		IGHSubjectRule GHSubjectRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGHSubjectRule;
            }
        }
		/// <summary>
        /// SubjectMst业务逻辑处理对象
        /// </summary>
		ISubjectMstRule SubjectMstRule { get; set; }
		/// <summary>
        /// SubjectMstBudgetDtl业务逻辑处理对象
        /// </summary>
		ISubjectMstBudgetDtlRule SubjectMstBudgetDtlRule { get; set; }

        IBudgetMstRule BudgetMstRule { get; set; }

        IQtOrgDygxRule QtOrgDygxRule { get; set; }
        ICorrespondenceSettings2Rule CorrespondenceSettings2Rule { get; set; }
        #endregion

        #region 重载方法
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<GHSubjectModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<GHSubjectModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            pageResult = AddNextApproveName(pageResult, "GHSubject");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public PagedResult<GHSubjectModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<GHSubjectModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            pageResult = AddNextApproveName(pageResult, "GHSubject");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
			helpdac.CodeToName<GHSubjectModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			SubjectMstRule.RuleHelper.DeleteByForeignKey(id);
			SubjectMstBudgetDtlRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			SubjectMstRule.RuleHelper.DeleteByForeignKey(ids);
			SubjectMstBudgetDtlRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IGHSubjectFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<GHSubjectModel> ExampleMethod<GHSubjectModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="gHSubjectEntity"></param>
		/// <param name="subjectMstEntities"></param>
		/// <param name="subjectMstBudgetDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveGHSubject(GHSubjectModel gHSubjectEntity, List<SubjectMstModel> subjectMstEntities, List<SubjectMstBudgetDtlModel> subjectMstBudgetDtlEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(gHSubjectEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (subjectMstEntities.Count > 0)
				{
					SubjectMstRule.Save(subjectMstEntities, savedResult.KeyCodes[0]);
				}
				if (subjectMstBudgetDtlEntities.Count > 0)
				{
					SubjectMstBudgetDtlRule.Save(subjectMstBudgetDtlEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddData()
        {
           
            string result = "";
            
            List<string> FProjNameList = new List<string>();
            FProjNameList.Add("各部门收入预算汇总数据");
            FProjNameList.Add("各部门基本支出汇总数据");

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<List<string>>.In("FProjName", FProjNameList))
                .Add(ORMRestrictions<string>.Eq("FYear", ConfigHelper.GetString("DBG6H_Year")));

            IList<GHSubjectModel> SubjectList = GHSubjectRule.Find(dicWhere);
            List<string> OrgList = new List<string>();
            if (SubjectList.Count > 0)
            {
                for (var x = 0; x < SubjectList.Count; x++)
                {
                    if (!OrgList.Contains(SubjectList[x].FDeclarationUnit))
                    {
                        OrgList.Add(SubjectList[x].FDeclarationUnit);
                    }
                }

                for (var y = 0; y < OrgList.Count; y++)
                {
                    List<string> valuesqlList = new List<string>();
                    List<DateTime?> DJRQList = new List<DateTime?>();
                    List<string> mstSqlList = new List<string>();
                    List<string> MstCodeList = new List<string>();
                    List<string> dtlSqlList = new List<string>();
                    

                    Dictionary<string, object> conndic = new Dictionary<string, object>();
                    new CreateCriteria(conndic)
                        .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                        .Add(ORMRestrictions<string>.Eq("DefStr1", OrgList[y]));
                    IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
                    if (CorrespondenceSettings2s.Count > 0&& CorrespondenceSettings2s[0].DefStr2!=null)
                    {
                        string userConn = CorrespondenceSettings2s[0].DefStr2;
                        string zbly_dm = CorrespondenceSettings2s[0].DefStr3;
                        try
                        {
                            int ID = BudgetMstRule.GetId(userConn);
                            for (var i = 0; i < SubjectList.Count; i++)
                            {
                                if (OrgList[y] == SubjectList[i].FDeclarationUnit)
                                {
                                    GHSubjectModel Subject = SubjectList[i];
                                    string DWDM;

                                    IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.findByXmorg(Subject.FDeclarationUnit);
                                    if (OrgDygx.Count > 0)
                                    {
                                        // Subject.FDeclarationUnit = OrgDygx[0].Oldorg;
                                        DWDM = OrgDygx[0].Oldorg; 
                                    }
                                    else
                                    {
                                        DWDM = Subject.FDeclarationUnit;
                                    }


                                    DateTime? DJRQ = Subject.FDateofDeclaration;
                                    //string DWDM = Subject.FDeclarationUnit;
                                    //string ZY = Subject.FDeclarationDept;
                                    //string DEF_STR7 = Subject.FDeclarationDept;

                                    int year = int.Parse(Subject.FYear);
                                    IList<SubjectMstModel> MstList = SubjectMstRule.FindByForeignKey(SubjectList[i].PhId);
                                    if (MstList.Count > 0)
                                    {
                                        for (var j = 0; j < MstList.Count; j++)
                                        {
                                            SubjectMstModel Mst = MstList[j];
                                            if (Mst.FProjCode != "" && Mst.FProjCode != null)
                                            {
                                                string YSKM_DM = Mst.FSubjectCode;
                                                //decimal PAY_JE = Mst.FProjAmount;
                                                string ZY;
                                                string DEF_STR7;
                                                IList<QtOrgDygxModel> OrgZY = QtOrgDygxRule.findByXmorg(Subject.FDeclarationUnit);
                                                if (OrgZY.Count > 0)
                                                {
                                                    ZY = OrgZY[0].Oldbudget; 
                                                    DEF_STR7 = OrgZY[0].Oldbudget; 
                                                }
                                                else
                                                {
                                                    ZY = Mst.FFillDept;
                                                    DEF_STR7 = Mst.FFillDept;
                                                }




                                                Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                                                new CreateCriteria(dicWhere2)
                                                    .Add(ORMRestrictions<string>.Eq("FProjCode", Mst.FProjCode))
                                                    .Add(ORMRestrictions<Int64>.Eq("Mstphid", SubjectList[i].PhId));

                                                IList<SubjectMstBudgetDtlModel> DtlList = SubjectMstBudgetDtlRule.Find(dicWhere2);
                                                if (DtlList.Count > 0)
                                                {
                                                    string mstSql = "('" + "zc" + Mst.FProjCode + "','" + Mst.FProjName + "','zc')";
                                                    if (!MstCodeList.Contains(Mst.FProjCode))
                                                    {
                                                        MstCodeList.Add(Mst.FProjCode);
                                                        mstSqlList.Add(mstSql);
                                                    }
                                                    for (var a = 0; a < DtlList.Count; a++)
                                                    {
                                                        SubjectMstBudgetDtlModel Dtl = DtlList[a];
                                                        ID += 1;
                                                        decimal PAY_JE = Dtl.FAmount;
                                                        string DJH = ID.ToString();
                                                        string DEF_STR1 = "zc" + Mst.FProjCode;
                                                        string MXXM = "zc" + Dtl.FProjCode + "-" + (a + 1).ToString();
                                                        string DEF_STR8 = "";
                                                        if (Dtl.FExpensesChannel != null && Dtl.FExpensesChannel != "")
                                                        {
                                                            IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(Dtl.FExpensesChannel);
                                                            if (OrgZCQD.Count > 0)
                                                            {
                                                                //Dtl.FExpensesChannel = OrgZCQD[0].Oldorg;
                                                                DEF_STR8 = OrgZCQD[0].Oldorg;
                                                            }
                                                            else
                                                            {
                                                                DEF_STR8 = Dtl.FExpensesChannel;
                                                            }
                                                        }
                                                        //string DEF_STR8 = Dtl.FExpensesChannel;

                                                        string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "'," + PAY_JE + ",'" + ZY + "','" +
                                                            MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "'," + year + ",'" + DEF_STR1 + "'";
                                                        string dtlSql = "('jj','" + "zc" + Mst.FProjCode + "','" + MXXM + "','" + Dtl.FDtlName + "','zc')";
                                                        valuesqlList.Add(valuesql);
                                                        DJRQList.Add(DJRQ);
                                                        dtlSqlList.Add(dtlSql);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            GHSubjectRule.AddData(userConn, zbly_dm, valuesqlList, mstSqlList, dtlSqlList, DJRQList);
                        }
                        catch (Exception e)
                        {
                            result = result + OrgList[y] + ",";
                        }
                    }
                    else
                    {
                        result = result + OrgList[y] + ",";
                    }
                    
                }
            }

            if (result != "")
            {
                result = result.Substring(0, result.Length - 1);
                result = result + "同步失败";
            }
            return result;
        }

        /// <summary>
        /// 纳入预算同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddDataSP()
        {

            string result = "";
            
            List<string> FProjNameList = new List<string>();
            FProjNameList.Add("各部门收入预算汇总数据");
            FProjNameList.Add("各部门基本支出汇总数据");

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<List<string>>.In("FProjName", FProjNameList))
                .Add(ORMRestrictions<string>.Eq("FYear", ConfigHelper.GetString("DBG6H_Year")));

            IList<GHSubjectModel> SubjectList = GHSubjectRule.Find(dicWhere);
            List<string> OrgList = new List<string>();
            if (SubjectList.Count > 0)
            {
                for (var x = 0; x < SubjectList.Count; x++)
                {
                    if (!OrgList.Contains(SubjectList[x].FDeclarationUnit))
                    {
                        OrgList.Add(SubjectList[x].FDeclarationUnit);
                    }
                }
                for (var y = 0; y < OrgList.Count; y++)
                {
                    List<string> valuesqlList = new List<string>();
                    List<DateTime?> DJRQList = new List<DateTime?>();
                    List<string> mstSqlList = new List<string>();
                    List<string> MstCodeList = new List<string>();
                    List<string> dtlSqlList = new List<string>();
                    Dictionary<string, object> conndic = new Dictionary<string, object>();
                    new CreateCriteria(conndic)
                        .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                        .Add(ORMRestrictions<string>.Eq("DefStr1", OrgList[y]));
                    IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
                    if (CorrespondenceSettings2s.Count > 0&& CorrespondenceSettings2s[0].DefStr2!=null)
                    {
                        string userConn = CorrespondenceSettings2s[0].DefStr2;
                        string zbly_dm = CorrespondenceSettings2s[0].DefStr3;
                        try
                        {
                            int ID = BudgetMstRule.GetId(userConn);
                            for (var i = 0; i < SubjectList.Count; i++)
                            {
                                if (OrgList[y] == SubjectList[i].FDeclarationUnit)
                                {
                                    GHSubjectModel Subject = SubjectList[i];
                                    string DWDM;

                                    IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.findByXmorg(Subject.FDeclarationUnit);
                                    if (OrgDygx.Count > 0)
                                    {
                                        //Subject.FDeclarationUnit = OrgDygx[0].Oldorg;
                                        DWDM = OrgDygx[0].Oldorg;
                                    }
                                    else
                                    {
                                        DWDM = Subject.FDeclarationUnit;
                                    }

                                    DateTime? DJRQ = Subject.FDateofDeclaration;
                                    //string DWDM = Subject.FDeclarationUnit;
                                    //string ZY = Subject.FDeclarationDept;
                                    //string DEF_STR7 = Subject.FDeclarationDept;

                                    int year = int.Parse(Subject.FYear);
                                    IList<SubjectMstModel> MstList = SubjectMstRule.FindByForeignKey(SubjectList[i].PhId);
                                    if (MstList.Count > 0)
                                    {
                                        for (var j = 0; j < MstList.Count; j++)
                                        {
                                            SubjectMstModel Mst = MstList[j];
                                            if (Mst.FProjCode != "" && Mst.FProjCode != null)
                                            {
                                                string YSKM_DM = Mst.FSubjectCode;
                                                //decimal PAY_JE = Mst.FProjAmount;
                                                string ZY;
                                                string DEF_STR7;
                                                IList<QtOrgDygxModel> OrgZY = QtOrgDygxRule.findByXmorg(Subject.FDeclarationUnit);
                                                if (OrgZY.Count > 0)
                                                {
                                                    ZY = OrgZY[0].Oldbudget; 
                                                    DEF_STR7 = OrgZY[0].Oldbudget; 
                                                }
                                                else
                                                {
                                                    ZY = Mst.FFillDept;
                                                    DEF_STR7 = Mst.FFillDept;
                                                }


                                                Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                                                new CreateCriteria(dicWhere2)
                                                    .Add(ORMRestrictions<string>.Eq("FProjCode", Mst.FProjCode))
                                                    .Add(ORMRestrictions<Int64>.Eq("Mstphid", SubjectList[i].PhId));

                                                IList<SubjectMstBudgetDtlModel> DtlList = SubjectMstBudgetDtlRule.Find(dicWhere2);
                                                if (DtlList.Count > 0)
                                                {
                                                    string mstSql = "('" + "zc" + Mst.FProjCode + "','" + Mst.FProjName + "','zc')";
                                                    if (!MstCodeList.Contains(Mst.FProjCode))
                                                    {
                                                        MstCodeList.Add(Mst.FProjCode);
                                                        mstSqlList.Add(mstSql);
                                                    }
                                                    for (var a = 0; a < DtlList.Count; a++)
                                                    {
                                                        SubjectMstBudgetDtlModel Dtl = DtlList[a];
                                                        ID += 1;
                                                        decimal PAY_JE = Dtl.FAmount;
                                                        string DJH = ID.ToString();
                                                        string DEF_STR1 = "zc" + Mst.FProjCode;
                                                        string MXXM = "zc" + Dtl.FProjCode + "-" + (a + 1).ToString();
                                                        string DEF_STR8 = "";
                                                        if (Dtl.FExpensesChannel != null && Dtl.FExpensesChannel != "")
                                                        {
                                                            IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(Dtl.FExpensesChannel);
                                                            if (OrgZCQD.Count > 0)
                                                            {
                                                                // Dtl.FExpensesChannel = OrgZCQD[0].Oldorg;
                                                                DEF_STR8 = OrgZCQD[0].Oldorg;
                                                            }
                                                            else
                                                            {
                                                                DEF_STR8 = Dtl.FExpensesChannel;
                                                            }
                                                        }
                                                        //string DEF_STR8 = Dtl.FExpensesChannel;

                                                        string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "'," + PAY_JE + ",'" + ZY + "','" +
                                                            MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "'," + year + ",'" + DEF_STR1 + "'";
                                                        string dtlSql = "('jj','" + "zc" + Mst.FProjCode + "','" + MXXM + "','" + Dtl.FDtlName + "','zc')";
                                                        valuesqlList.Add(valuesql);
                                                        DJRQList.Add(DJRQ);
                                                        dtlSqlList.Add(dtlSql);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            GHSubjectRule.AddDataSP(userConn, zbly_dm, valuesqlList, mstSqlList, dtlSqlList, DJRQList);
                        }
                        catch (Exception e)
                        {
                            result = result + OrgList[y] + ",";
                        }
                    }
                    else
                    {
                        result = result + OrgList[y] + ",";
                    }
                }
            }
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1);
                result = result + "同步失败";
            }
            return result;
        }

        #endregion

        #region 工作流接口
        /// <summary>
        /// 流程发起时调用（一般用于修改表单状态为送审中、或是维护表单已挂工作流）
        /// </summary>
        /// <param name="ec"></param>
        public void FlowStart(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);

            //更改状态为：审批中
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.IsPending).ToString();
           // mst.Data.FApproveDate = DateTime.Today;
           // mst.Data.FApprover = base.UserID;
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 在审批任务执行前调用，在这里判断是否允许执行审批操作（现在流程中没有判断杜绝多个审批节点执行，所以单据状态为已审批也允许再次审批）
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        ///  审批组件任务办理时调用（现在流程中没有判断杜绝多个审批节点执行，所以如果单据已审批就修改审批人、审批时间）
        /// </summary>
        /// <param name="ec"></param>
        public void Approve(WorkFlowExecutionContext ec)
        {

            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);

            //更改状态为：审批中
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
            mst.Data.FApproveDate = DateTime.Today;
            mst.Data.FApprover = base.UserID;
            CurrentRule.Update<Int64>(mst.Data);
            //用 FlowEnd(), 在流程结束时进行操作(approve 方法 在进行审批节点后就会调用,可能存在多个审批节点)
            //long phid = Convert.ToInt64(ec.BillInfo.PK1);
            //var mst = base.Find(phid);
            ////更新状态为已审批
            //if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            //{
            //    //审批完成时,项目状态为预立项,则项目状态改为立项,审批状态改为未审批
            //    if (mst.Data.FProjStatus == 1)
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            //        mst.Data.FProjStatus = 2;
            //    }
            //    else
            //    {
            //        mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
            //    }

            //    mst.Data.FApproveDate = DateTime.Now;
            //    mst.Data.FApprover = base.UserID;
            //    CurrentRule.Update<Int64>(mst.Data);
            //}

        }

        /// <summary>
        /// 审批节点回退前判断是否允许取消审批
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckCancelApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        /// 审批节点回退时调用进行单据取消审批操作
        /// </summary>
        /// <param name="ec"></param>
        public void CancelApprove(WorkFlowExecutionContext ec)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 流程被终止时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowAbort(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为待上报
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            mst.Data.FApprover = 0;
            //mst.Data.FProjStatus = 1;//项目立项审批驳回-->从预立项从新修改，发起；状态默认预立项；
            mst.Data.FApproveDate = new Nullable<DateTime>();
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 流程结束时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowEnd(WorkFlowExecutionContext ec)
        {

            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为已审批
            if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            {
               
                
                mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
                

                mst.Data.FApproveDate = DateTime.Today;
                mst.Data.FApprover = base.UserID;
                CurrentRule.Update<Int64>(mst.Data);
            }
            //调整单据审批结束将原调整中改为调整完成
            if (mst.Data.FType == "tz")
            {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("FDeclarationDept", mst.Data.FDeclarationDept))
                    .Add(ORMRestrictions<string>.Eq("FProjAttr", mst.Data.FProjAttr))
                    .Add(ORMRestrictions<string>.Eq("FApproveStatus", "6"));
                IList<GHSubjectModel> models = GHSubjectRule.Find(dicWhere);
                if (models.Count > 0)
                {
                    foreach (GHSubjectModel a in models)
                    {
                        a.FApproveStatus = "7";
                        //a.PersistentState = PersistentState.Modified;
                        CurrentRule.Update<Int64>(a);
                    }
                    //GHSubjectRule.Save(models, "");
                    //CurrentRule.Update<Int64>(models);
                }

            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 新增、编辑\审核类组件任务执行时调用,方法参数中包含组件id
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="ec"></param>
        public void EditUserTaskComplete(string compId, WorkFlowExecutionContext ec)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据转pdf所需的套打模块、及数据，用于APP端
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public BizToPdfEntity GetBizToPdfEntity(WorkFlowExecutionContext executionContext)
        {
            return null;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据附用（用于App端）
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public List<BizAttachment> GetBizAttachment(WorkFlowExecutionContext executionContext)
        {
            return new List<BizAttachment>();
        }

        /// <summary>
        /// app办理时如果单据有修改则调用该方法判断是否允许保存修改
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public ApproveValidResult CheckBizSaveByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return ApproveValidResult.DefaultValue;
        }

        /// <summary>
        /// app端办理时如果修改了单据内容则调用该方法进行单据保存。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public bool SaveBizDataByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return true;
           // throw new NotImplementedException();
        }
        #endregion

        #region 下一审批人
        /// <summary>
        /// 获取审批中的单据id
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public List<string> GetApproveList(PagedResult<GHSubjectModel> pageResult)
        {
            var idList = new List<string>();
            foreach (var item in pageResult.Results)
            {
                if (item.FApproveStatus == "2")
                {
                    idList.Add(item.PhId.ToString());
                }
            }
            return idList;
        }

        /// <summary>
        /// 增加单据中下一审批节点名称
        /// </summary>
        /// <param name="pageResult"></param>
        /// <param name="BizType"> 审批流业务类型</param>
        /// <returns></returns>
        public PagedResult<GHSubjectModel> AddNextApproveName(PagedResult<GHSubjectModel> pageResult, string BizType)
        {
            var approveListId = GetApproveList(pageResult);
            if (approveListId.Count == 0)
            {
                foreach (var item in pageResult.Results)
                {
                    item.FNextApprove = "无";
                }
                return pageResult;
            }
            var Next_approve = WorkFlowHelper.GetFlowInfoByBizList(BizType, approveListId);

            foreach (var item in pageResult.Results)
            {
                if (item.FApproveStatus == "2")
                {
                    for (var i = 0; i < Next_approve.Count; i++)
                    {
                        if (Next_approve[i]["pk1"].ToString() == item.PhId.ToString() && Next_approve[i]["flow_status_name"].ToString() == "运行中")
                        {
                            item.FNextApprove = Next_approve[i]["curnodes"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    item.FNextApprove = "无";
                }
            }

            return pageResult;
        }
        #endregion
    }
}

