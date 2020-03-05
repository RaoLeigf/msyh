using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GQT3.QT.Model;
using Enterprise3.WebApi.GQT3.QT.Model.Request;
using Enterprise3.WebApi.GQT3.QT.Model.Response.common;
using GData3.Common.Model;
using GQT3.QT.Model.Domain;
using GQT3.QT.Model.Extra;
using GQT3.QT.Service;
using GQT3.QT.Service.Interface;
using GSP3.SP.Model.Domain;
using GSP3.SP.Model.Enums;
using GSP3.SP.Service.Interface;
using GXM3.XM.Model.Domain;
using GXM3.XM.Service.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Enterprise3.WebApi.GQT3.QT.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [MethodExceptionFilter]
    public class QTSysSetApiController : ApiBase
    {
        #region 外部接口

        IQTSysSetService QTSysSetService { get; set; }

        IBudgetAccountsService BudgetAccountsService { get; set; }

        ISourceOfFundsService SourceOfFundsService { get; set; }

        IExpenseCategoryService ExpenseCategoryService { get; set; }

        ICorrespondenceSettings2Service CorrespondenceSettings2Service { get; set; }

        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }

        IPerformEvalTargetTypeService PerformEvalTargetTypeService { get; set; }

        IPerformEvalTargetService PerformEvalTargetService { get; set; }

        IPerformEvalTargetClassService PerformEvalTargetClassService { get; set; }

        IPerformEvalTypeService PerformEvalTypeService { get; set; }

        IProcurementCatalogService ProcurementCatalogService { get; set; }

        IProcurementProceduresService ProcurementProceduresService { get; set; }

        IProcurementTypeService ProcurementTypeService { get; set; }

        IQtZcgnflService QtZcgnflService { get; set; }

        IQTIfApproveService QTIfApproveService { get; set; }

        IGAppvalRecordService GAppvalRecordService { get; set; }

        IProjectMstService ProjectMstService { get; set; }

        IBudgetMstService BudgetMstService { get; set; }

        IExpenseMstService ExpenseMstService { get; set; }

        IQtXmDistributeService QtXmDistributeService { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public QTSysSetApiController()
        {
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            BudgetAccountsService = base.GetObject<IBudgetAccountsService>("GQT3.QT.Service.BudgetAccounts");
            SourceOfFundsService = base.GetObject<ISourceOfFundsService>("GQT3.QT.Service.SourceOfFunds");
            ExpenseCategoryService = base.GetObject<IExpenseCategoryService>("GQT3.QT.Service.ExpenseCategory");
            CorrespondenceSettings2Service = base.GetObject<ICorrespondenceSettings2Service>("GQT3.QT.Service.CorrespondenceSettings2");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            PerformEvalTargetTypeService = base.GetObject<IPerformEvalTargetTypeService>("GQT3.QT.Service.PerformEvalTargetType");
            PerformEvalTargetService = base.GetObject<IPerformEvalTargetService>("GQT3.QT.Service.PerformEvalTarget");
            PerformEvalTargetClassService = base.GetObject<IPerformEvalTargetClassService>("GQT3.QT.Service.PerformEvalTargetClass");
            PerformEvalTypeService = base.GetObject<IPerformEvalTypeService>("GQT3.QT.Service.PerformEvalType");
            ProcurementCatalogService = base.GetObject<IProcurementCatalogService>("GQT3.QT.Service.ProcurementCatalog");
            ProcurementProceduresService = base.GetObject<IProcurementProceduresService>("GQT3.QT.Service.ProcurementProcedures");
            ProcurementTypeService = base.GetObject<IProcurementTypeService>("GQT3.QT.Service.ProcurementType");
            QtZcgnflService = base.GetObject<IQtZcgnflService>("GQT3.QT.Service.QtZcgnfl");
            QTIfApproveService = base.GetObject<IQTIfApproveService>("GQT3.QT.Service.QTIfApprove");
            GAppvalRecordService = base.GetObject<IGAppvalRecordService>("GSP3.SP.Service.GAppvalRecord");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
            BudgetMstService = base.GetObject<IBudgetMstService>("GYS3.YS.Service.BudgetMst");
            ExpenseMstService = base.GetObject<IExpenseMstService>("GYS3.YS.Service.ExpenseMst");
            QtXmDistributeService = base.GetObject<IQtXmDistributeService>("GQT3.QT.Service.QtXmDistribute");
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostSave([FromBody]InfoBaseModel<List<QTSysSetModel>> SysSet)
        {
            if (SysSet.infoData == null || SysSet.infoData.Count <= 0)
            {
                return DCHelper.ErrorMessage("数据传递不正确！");
            }
            /*UnChanged = 0,
            Added = 1,
            Modified = 2,
            Deleted = 3*/
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            List<QTSysSetModel> resultSysSet = new List<QTSysSetModel>();

            //获取所有组织集合
            List<OrganizeModel> allOrgs = new List<OrganizeModel>();
            allOrgs = this.QTSysSetService.GetAllOrgs();

            //支付方式特殊处理
            if (SysSet.infoData[0].DicType == "PayMethod")//资金拨付的支付方式
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "PayMethod"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }

                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "PayMethod";
                                    b.DicName = "支付方式";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("支付方式编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的支付方式编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("支付方式编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的支付方式编码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "DxbzCode")//对下补助代码
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "DxbzCode"));

                var DxbzCodes = QTSysSetService.Find(dicWhere, new string[] { "Value Asc" }).Data;//所有对下补助的集合(数据库)
                var DxbzValues = DxbzCodes.Select(x => x.Value).Distinct().ToList();//所有对下补助值的集合(数据库)
                if (user.UserNo == "Admin")
                {
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        if (DxbzValues.Contains(set.Value))
                        {
                            DxbzValues.Remove(set.Value);
                            var DxbzCodesByValue = DxbzCodes.ToList().FindAll(x => x.Value == set.Value);//数据库

                            if (set.OrgList != null && set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    //如果存在就删除，最后剩下的是要删除的
                                    var DxbzCodesByValueOrg = DxbzCodesByValue.FindAll(x => x.Orgid == org.PhId);
                                    if (DxbzCodesByValueOrg.Count > 0)
                                    {
                                        DxbzCodesByValue.Remove(DxbzCodesByValueOrg[0]);
                                    }
                                    else
                                    {
                                        QTSysSetModel b = new QTSysSetModel();
                                        b.DicType = "DxbzCode";
                                        b.DicName = "对下补助代码维护";
                                        b.TypeName = "对下补助代码";
                                        b.Orgid = org.PhId;
                                        b.Orgcode = org.OCode;
                                        b.Value = set.Value;
                                        b.Issystem = 1;
                                        b.Isactive = set.Isactive;
                                        b.Bz = set.Bz;
                                        b.PersistentState = PersistentState.Added;
                                        resultSysSet.Add(b);
                                    }
                                }
                                if (DxbzCodesByValue.Count > 0)
                                {
                                    foreach (QTSysSetModel delete in DxbzCodesByValue)
                                    {
                                        delete.PersistentState = PersistentState.Deleted;
                                        resultSysSet.Add(delete);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //value新增  orglist不为空
                            if (set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    QTSysSetModel c = new QTSysSetModel();
                                    c.DicType = "DxbzCode";
                                    c.DicName = "对下补助代码维护";
                                    c.TypeName = "对下补助代码";
                                    c.Orgid = org.PhId;
                                    c.Orgcode = org.OCode;
                                    c.Value = set.Value;
                                    c.Issystem = 1;
                                    c.Isactive = set.Isactive;
                                    c.Bz = set.Bz;
                                    c.PersistentState = PersistentState.Added;
                                    resultSysSet.Add(c);
                                }
                            }
                        }
                    }
                    //删除整个value的数据
                    if (DxbzValues.Count > 0)
                    {
                        for (var i = 0; i < DxbzValues.Count; i++)
                        {
                            var d = DxbzCodes.ToList().FindAll(x => x.Value == DxbzValues[i]);
                            foreach (QTSysSetModel delete in d)
                            {
                                delete.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(delete);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有对下补助代码
                    var DxbzCodesByOrg = DxbzCodes.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    //var DxbzPhids = DxbzCodes.Select(x => x.PhId).Distinct().ToList();//所有对下补助值的phid集合(数据库)
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var DxbzCodesByOrgPhid = DxbzCodesByOrg.Find(x => x.PhId == set.PhId);
                            DxbzCodesByOrg.Remove(DxbzCodesByOrgPhid);
                            if (DxbzCodesByOrgPhid.Value != set.Value || DxbzCodesByOrgPhid.Isactive != set.Isactive || DxbzCodesByOrgPhid.Bz != set.Bz)
                            {
                                set.PersistentState = PersistentState.Modified;
                                resultSysSet.Add(set);
                            }
                        }

                    }
                    if (DxbzCodesByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in DxbzCodesByOrg)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "ZjlzCode")//直接列支代码
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "ZjlzCode"));

                var DxbzCodes = QTSysSetService.Find(dicWhere, new string[] { "Value Asc" }).Data;//所有对下补助的集合(数据库)
                var DxbzValues = DxbzCodes.Select(x => x.Value).Distinct().ToList();//所有对下补助值的集合(数据库)
                if (user.UserNo == "Admin")
                {
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        if (DxbzValues.Contains(set.Value))
                        {
                            DxbzValues.Remove(set.Value);
                            var DxbzCodesByValue = DxbzCodes.ToList().FindAll(x => x.Value == set.Value);//数据库

                            if (set.OrgList != null && set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    //如果存在就删除，最后剩下的是要删除的
                                    var DxbzCodesByValueOrg = DxbzCodesByValue.FindAll(x => x.Orgid == org.PhId);
                                    if (DxbzCodesByValueOrg.Count > 0)
                                    {
                                        DxbzCodesByValue.Remove(DxbzCodesByValueOrg[0]);
                                    }
                                    else
                                    {
                                        QTSysSetModel b = new QTSysSetModel();
                                        b.DicType = "ZjlzCode";
                                        b.DicName = "直接列支代码维护";
                                        b.TypeName = "直接列支代码";
                                        b.Orgid = org.PhId;
                                        b.Orgcode = org.OCode;
                                        b.Value = set.Value;
                                        b.Issystem = 1;
                                        b.Isactive = set.Isactive;
                                        b.Bz = set.Bz;
                                        b.PersistentState = PersistentState.Added;
                                        resultSysSet.Add(b);
                                    }
                                }
                                if (DxbzCodesByValue.Count > 0)
                                {
                                    foreach (QTSysSetModel delete in DxbzCodesByValue)
                                    {
                                        delete.PersistentState = PersistentState.Deleted;
                                        resultSysSet.Add(delete);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //value新增  orglist不为空
                            if (set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    QTSysSetModel c = new QTSysSetModel();
                                    c.DicType = "ZjlzCode";
                                    c.DicName = "直接列支代码维护";
                                    c.TypeName = "直接列支代码";
                                    c.Orgid = org.PhId;
                                    c.Orgcode = org.OCode;
                                    c.Value = set.Value;
                                    c.Issystem = 1;
                                    c.Isactive = set.Isactive;
                                    c.Bz = set.Bz;
                                    c.PersistentState = PersistentState.Added;
                                    resultSysSet.Add(c);
                                }
                            }
                        }
                    }
                    //删除整个value的数据
                    if (DxbzValues.Count > 0)
                    {
                        for (var i = 0; i < DxbzValues.Count; i++)
                        {
                            var d = DxbzCodes.ToList().FindAll(x => x.Value == DxbzValues[i]);
                            foreach (QTSysSetModel delete in d)
                            {
                                delete.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(delete);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有对下补助代码
                    var DxbzCodesByOrg = DxbzCodes.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    //var DxbzPhids = DxbzCodes.Select(x => x.PhId).Distinct().ToList();//所有对下补助值的phid集合(数据库)
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var DxbzCodesByOrgPhid = DxbzCodesByOrg.Find(x => x.PhId == set.PhId);
                            DxbzCodesByOrg.Remove(DxbzCodesByOrgPhid);
                            if (DxbzCodesByOrgPhid.Value != set.Value || DxbzCodesByOrgPhid.Isactive != set.Isactive || DxbzCodesByOrgPhid.Bz != set.Bz)
                            {
                                set.PersistentState = PersistentState.Modified;
                                resultSysSet.Add(set);
                            }
                        }

                    }
                    if (DxbzCodesByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in DxbzCodesByOrg)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "StayTime")//停留时长设置
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "StayTime"));

                var DxbzCodes = QTSysSetService.Find(dicWhere, new string[] { "Value Asc" }).Data;//所有对下补助的集合(数据库)
                var DxbzValues = DxbzCodes.Select(x => x.Value).Distinct().ToList();//所有对下补助值的集合(数据库)
                if (user.UserNo == "Admin")
                {
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        if (DxbzValues.Contains(set.Value))
                        {
                            DxbzValues.Remove(set.Value);
                            var DxbzCodesByValue = DxbzCodes.ToList().FindAll(x => x.Value == set.Value);//数据库

                            if (set.OrgList != null && set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    //如果存在就删除，最后剩下的是要删除的
                                    var DxbzCodesByValueOrg = DxbzCodesByValue.FindAll(x => x.Orgid == org.PhId);
                                    if (DxbzCodesByValueOrg.Count > 0)
                                    {
                                        DxbzCodesByValue.Remove(DxbzCodesByValueOrg[0]);
                                    }
                                    else
                                    {
                                        QTSysSetModel b = new QTSysSetModel();
                                        b.DicType = "StayTime";
                                        b.DicName = "停留时长提醒设置";
                                        b.TypeName = "停留时长设置";
                                        b.Orgid = org.PhId;
                                        b.Orgcode = org.OCode;
                                        b.Value = set.Value;
                                        b.Issystem = 1;
                                        b.Isactive = set.Isactive;
                                        b.Bz = set.Bz;
                                        b.PersistentState = PersistentState.Added;
                                        resultSysSet.Add(b);
                                    }
                                }
                                if (DxbzCodesByValue.Count > 0)
                                {
                                    foreach (QTSysSetModel delete in DxbzCodesByValue)
                                    {
                                        delete.PersistentState = PersistentState.Deleted;
                                        resultSysSet.Add(delete);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //value新增  orglist不为空
                            if (set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    QTSysSetModel c = new QTSysSetModel();
                                    c.DicType = "StayTime";
                                    c.DicName = "停留时长提醒设置";
                                    c.TypeName = "停留时长设置";
                                    c.Orgid = org.PhId;
                                    c.Orgcode = org.OCode;
                                    c.Value = set.Value;
                                    c.Issystem = 1;
                                    c.Isactive = set.Isactive;
                                    c.Bz = set.Bz;
                                    c.PersistentState = PersistentState.Added;
                                    resultSysSet.Add(c);
                                }
                            }
                        }
                    }
                    //删除整个value的数据
                    if (DxbzValues.Count > 0)
                    {
                        for (var i = 0; i < DxbzValues.Count; i++)
                        {
                            var d = DxbzCodes.ToList().FindAll(x => x.Value == DxbzValues[i]);
                            foreach (QTSysSetModel delete in d)
                            {
                                delete.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(delete);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有对下补助代码
                    var DxbzCodesByOrg = DxbzCodes.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    //var DxbzPhids = DxbzCodes.Select(x => x.PhId).Distinct().ToList();//所有对下补助值的phid集合(数据库)
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var DxbzCodesByOrgPhid = DxbzCodesByOrg.Find(x => x.PhId == set.PhId);
                            DxbzCodesByOrg.Remove(DxbzCodesByOrgPhid);
                            if (DxbzCodesByOrgPhid.Value != set.Value || DxbzCodesByOrgPhid.Isactive != set.Isactive || DxbzCodesByOrgPhid.Bz != set.Bz)
                            {
                                set.PersistentState = PersistentState.Modified;
                                resultSysSet.Add(set);
                            }
                        }

                    }
                    if (DxbzCodesByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in DxbzCodesByOrg)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "ProjectLevel")//项目级别
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "ProjectLevel"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "ProjectLevel";
                                    b.DicName = "项目级别";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("项目级别编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的项目级别编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }

                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                            //if (a != null && a.Issystem == (byte)1)
                            //{
                            //    return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                            //}
                            //if (a.PersistentState != PersistentState.Deleted)
                            //{
                            //    if (a != null && a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode)
                            //    {
                            //        set.PersistentState = PersistentState.Modified;
                            //        resultSysSet.Add(set);
                            //    }
                            //}
                            //PayMethodsByOrg.Remove(a);
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("项目级别编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的项目级别编码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                //if (PayMethods.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "ProjectProper")//项目属性
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "ProjectProper"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;

                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "ProjectProper";
                                    b.DicName = "项目属性";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("项目属性编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的项目属性编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            //if (a != null && a.Issystem == (byte)1)
                            //{
                            //    return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                            //}
                            //if (a.PersistentState != PersistentState.Deleted)
                            //{
                            //    if (a != null && a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode)
                            //    {
                            //        set.PersistentState = PersistentState.Modified;
                            //        resultSysSet.Add(set);
                            //    }
                            //}
                            //PayMethodsByOrg.Remove(a);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("项目属性编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的项目属性编码重复，请进行修改！");
                                }

                                //if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                //if (PayMethods.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "TimeLimit")//续存期限
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "TimeLimit"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;

                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);

                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);
                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }

                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);

                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "TimeLimit";
                                    b.DicName = "续存期限";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    b.PersistentState = PersistentState.Added;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }
                    }
                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("续存期限编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的续存期限编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }

                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            //if (a != null && a.Issystem == (byte)1)
                            //{
                            //    return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                            //}
                            //if (a.PersistentState != PersistentState.Deleted)
                            //{
                            //    if (a != null && a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode)
                            //    {
                            //        set.PersistentState = PersistentState.Modified;
                            //        resultSysSet.Add(set);
                            //    }
                            //}
                            //PayMethodsByOrg.Remove(a);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("续存期限编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的续存期限编码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                //if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                                //if (PayMethods.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "PayMethodTwo")//vue预算项目库的支付方式
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "PayMethodTwo"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "PayMethodTwo";
                                    b.DicName = "支付方式2";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("支付方式2编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的支付方式2编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }

                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            //if (a != null && a.Issystem == (byte)1)
                            //{
                            //    return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                            //}
                            //if(set.PersistentState != PersistentState.Deleted)
                            //{
                            //    if (a != null && a.TypeName != set.TypeName || a.Bz != set.Bz|| a.Isactive!=set.Isactive || a.TypeCode != set.TypeCode)
                            //    {
                            //        set.PersistentState = PersistentState.Modified;
                            //        resultSysSet.Add(set);
                            //    }
                            //}
                            //PayMethodsByOrg.Remove(a);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("支付方式2编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的支付方式2编码重复，请进行修改！");
                                }
                                //if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                //if (PayMethods.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "ProcurementsCatalog")//采购目录
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();

                //存入需要修改的采购目录集合
                List<ProcurementCatalogModel> procurementCatalogs = new List<ProcurementCatalogModel>();

                //所有的采购目录的集合
                IList<ProcurementCatalogModel> allCatalogs = this.ProcurementCatalogService.Find(t => t.PhId != 0).Data;

                //存原来有的采购目录编码现在没有了的集合
                IList<ProcurementCatalogModel> allCatalogsNot = new List<ProcurementCatalogModel>();

                allCatalogsNot = allCatalogs;

                //非内置的信息，用作数据验证
                var PayMethodNots = allCatalogs.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = allCatalogs.ToList().FindAll(x => x.Issystem == 1);

                if (user.UserNo == "Admin")
                {
                    allCatalogs = allCatalogs.ToList().FindAll(x => x.Issystem == 1);
                    allCatalogsNot = allCatalogsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        //获取所有采购目录编码相同的集合
                        var catalogsByCode = allCatalogs.ToList().FindAll(x => x.FCode == set.TypeCode);

                        allCatalogsNot = allCatalogsNot.ToList().FindAll(t => t.FCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var nowCatalogs = catalogsByCode.FindAll(x => x.Orgid == org.PhId);
                                if (nowCatalogs.Count > 0)
                                {
                                    ProcurementCatalogModel a = nowCatalogs[0];
                                    if (a.FName != set.TypeName || a.Isactive != set.Isactive || a.FRemark != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.FName = set.TypeName;
                                        a.FRemark = set.Bz;
                                        a.Issystem = 1;
                                        a.PersistentState = PersistentState.Modified;
                                        procurementCatalogs.Add(a);
                                    }
                                    catalogsByCode.Remove(nowCatalogs[0]);
                                }
                                else
                                {
                                    ProcurementCatalogModel b = new ProcurementCatalogModel();
                                    b.FCode = set.TypeCode;
                                    b.FName = set.TypeName;
                                    b.FRemark = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    b.PersistentState = PersistentState.Added;
                                    procurementCatalogs.Add(b);
                                }
                            }
                            if (catalogsByCode.Count > 0)
                            {
                                foreach (ProcurementCatalogModel delete in catalogsByCode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    procurementCatalogs.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (ProcurementCatalogModel z in catalogsByCode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                procurementCatalogs.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allCatalogsNot != null && allCatalogsNot.Count > 0)
                    {
                        foreach (ProcurementCatalogModel z in allCatalogsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            procurementCatalogs.Add(z);
                        }
                    }

                    //数据验证
                    if (procurementCatalogs != null && procurementCatalogs.Count > 0)
                    {
                        foreach (var pro in procurementCatalogs)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.FCode))
                                {
                                    return DCHelper.ErrorMessage("采购目录编码不能为空！");
                                }
                                if (procurementCatalogs.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的采购目录编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (procurementCatalogs.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (procurementCatalogs.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                    //与库中私有设置进行数据验证
                    //var allCatalogsSystem = allCatalogs.ToList().FindAll(t => t.Issystem == (byte)0);
                    //if(allCatalogsSystem != null && allCatalogsSystem.Count > 0)
                    //{
                    //    foreach (var pro in procurementCatalogs)
                    //    {
                    //        if (pro.PersistentState != PersistentState.Deleted && allCatalogsSystem.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode).Count > 0)
                    //        {
                    //            return DCHelper.ErrorMessage(pro.Orgcode + "该组织下的采购目录与私有编码重复，请进行修改！");
                    //        }
                    //    }
                    //}
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有采购目录
                    var catalogsByOrg = allCatalogs.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            ProcurementCatalogModel b = new ProcurementCatalogModel();
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            b.FCode = set.TypeCode;
                            b.FName = set.TypeName;
                            b.FRemark = set.Bz;
                            b.Orgid = set.Orgid;
                            b.Orgcode = set.Orgcode;
                            b.Isactive = set.Isactive;
                            b.PersistentState = PersistentState.Added;
                            procurementCatalogs.Add(b);
                        }
                        else
                        {
                            var a = catalogsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.FName != set.TypeName || a.FRemark != set.Bz || a.Isactive != set.Isactive || a.FCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    a.PersistentState = PersistentState.Modified;
                                    a.FCode = set.TypeCode;
                                    a.FName = set.TypeName;
                                    a.FRemark = set.Bz;
                                    a.Isactive = set.Isactive;
                                    procurementCatalogs.Add(a);
                                }
                                else
                                {
                                    a.PersistentState = PersistentState.Deleted;
                                    procurementCatalogs.Add(a);
                                }
                                catalogsByOrg.Remove(a);
                            }
                            //var a = catalogsByOrg.Find(x => x.FCode == set.TypeCode);

                        }
                    }
                    if (catalogsByOrg.Count > 0)
                    {
                        foreach (ProcurementCatalogModel z in catalogsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.FCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            procurementCatalogs.Add(z);
                        }
                    }
                    //数据验证
                    if (procurementCatalogs != null && procurementCatalogs.Count > 0)
                    {
                        foreach (var pro in procurementCatalogs)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {

                                if (string.IsNullOrEmpty(pro.FCode))
                                {
                                    return DCHelper.ErrorMessage("采购目录编码不能为空！");
                                }
                                if (procurementCatalogs.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的采购目录编码重复，请进行修改！");
                                }

                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }

                                //if (allCatalogs.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.FCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                            }
                        }
                    }
                }
                try
                {
                    savedresult = this.ProcurementCatalogService.Save<Int64>(procurementCatalogs, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "ProcurementsProcedures")//采购程序
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();

                //存入需要修改的采购程序集合
                List<ProcurementProceduresModel> procurementProcedures = new List<ProcurementProceduresModel>();

                //所有的采购程序的集合
                IList<ProcurementProceduresModel> allProcedures = this.ProcurementProceduresService.Find(t => t.PhId != 0).Data;

                //存原来有的采购程序编码现在没有了的集合
                IList<ProcurementProceduresModel> allProceduresNot = new List<ProcurementProceduresModel>();

                allProceduresNot = allProcedures;
                //非内置的信息，用作数据验证
                var PayMethodNots = allProcedures.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = allProcedures.ToList().FindAll(x => x.Issystem == 1);
                if (user.UserNo == "Admin")
                {
                    allProcedures = allProcedures.ToList().FindAll(x => x.Issystem == 1);
                    allProceduresNot = allProceduresNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        var proceduresByCode = allProcedures.ToList().FindAll(x => x.FCode == set.TypeCode);

                        allProceduresNot = allProceduresNot.ToList().FindAll(t => t.FCode != set.TypeCode);

                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var nowProcedures = proceduresByCode.FindAll(x => x.Orgid == org.PhId);
                                if (nowProcedures.Count > 0)
                                {
                                    ProcurementProceduresModel a = nowProcedures[0];
                                    if (a.FName != set.TypeName || a.Isactive != set.Isactive || a.FRemark != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.FName = set.TypeName;
                                        a.FRemark = set.Bz;
                                        a.Issystem = 1;
                                        a.PersistentState = PersistentState.Modified;
                                        procurementProcedures.Add(a);
                                    }
                                    proceduresByCode.Remove(nowProcedures[0]);
                                }
                                else
                                {
                                    ProcurementProceduresModel b = new ProcurementProceduresModel();
                                    b.FCode = set.TypeCode;
                                    b.FName = set.TypeName;
                                    b.FRemark = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    b.PersistentState = PersistentState.Added;
                                    procurementProcedures.Add(b);
                                }
                            }
                            if (proceduresByCode.Count > 0)
                            {
                                foreach (ProcurementProceduresModel delete in proceduresByCode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    procurementProcedures.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (ProcurementProceduresModel z in proceduresByCode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                procurementProcedures.Add(z);
                            }
                        }

                    }
                    //数据库原本有的现在没有的那些编码都要删除
                    if (allProceduresNot != null && allProceduresNot.Count > 0)
                    {
                        foreach (ProcurementProceduresModel z in allProceduresNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            procurementProcedures.Add(z);
                        }
                    }
                    //数据验证
                    if (procurementProcedures != null && procurementProcedures.Count > 0)
                    {
                        foreach (var pro in procurementProcedures)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            //判断未删除的即可
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.FCode))
                                {
                                    return DCHelper.ErrorMessage("采购程序编码不能为空！");
                                }
                                if (procurementProcedures.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的采购程序编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (procurementProcedures.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (procurementProcedures.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                            //if (procurementProcedures.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                            //{
                            //    return DCHelper.ErrorMessage(pro.Orgcode + "该组织下的采购程序编码重复，请进行修改！");
                            //}
                        }
                    }
                    //与库中私有设置进行数据验证
                    //var allProceduresSystem = allProcedures.ToList().FindAll(t => t.Issystem == (byte)0);
                    //if (allProceduresSystem != null && allProceduresSystem.Count > 0)
                    //{
                    //    foreach (var pro in procurementProcedures)
                    //    {
                    //        if (pro.PersistentState != PersistentState.Deleted && allProceduresSystem.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode).Count > 0)
                    //        {
                    //            return DCHelper.ErrorMessage(pro.Orgcode + "该组织下的采购程序与私有编码重复，请进行修改！");
                    //        }
                    //    }
                    //}
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有采购程序
                    var proceduresByOrg = allProcedures.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            ProcurementProceduresModel b = new ProcurementProceduresModel();
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            b.FCode = set.TypeCode;
                            b.FName = set.TypeName;
                            b.FRemark = set.Bz;
                            b.Orgid = set.Orgid;
                            b.Orgcode = set.Orgcode;
                            b.Isactive = set.Isactive;
                            b.PersistentState = PersistentState.Added;
                            procurementProcedures.Add(b);
                        }
                        else
                        {
                            var a = proceduresByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.FName != set.TypeName || a.FRemark != set.Bz || a.Isactive != set.Isactive || a.FCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    a.PersistentState = PersistentState.Modified;
                                    a.FCode = set.TypeCode;
                                    a.FName = set.TypeName;
                                    a.FRemark = set.Bz;
                                    a.Isactive = set.Isactive;
                                    procurementProcedures.Add(a);
                                }
                                else
                                {
                                    a.PersistentState = PersistentState.Deleted;
                                    procurementProcedures.Add(a);
                                }
                                proceduresByOrg.Remove(a);
                            }


                        }
                    }
                    if (proceduresByOrg.Count > 0)
                    {
                        foreach (ProcurementProceduresModel z in proceduresByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.FCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            procurementProcedures.Add(z);
                        }
                    }
                    //数据验证
                    if (procurementProcedures != null && procurementProcedures.Count > 0)
                    {
                        foreach (var pro in procurementProcedures)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.FCode))
                                {
                                    return DCHelper.ErrorMessage("采购程序编码不能为空！");
                                }
                                if (procurementProcedures.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的采购程序编码重复，请进行修改！");
                                }

                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }

                                //if (allProcedures.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.FCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                            }
                        }
                    }
                }
                try
                {
                    savedresult = this.ProcurementProceduresService.Save<Int64>(procurementProcedures, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "ProcurementsType")//采购类型
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();

                //存入需要修改的采购种类集合
                List<ProcurementTypeModel> procurementTypes = new List<ProcurementTypeModel>();

                //所有的采购种类的集合
                IList<ProcurementTypeModel> allTypes = this.ProcurementTypeService.Find(t => t.PhId != 0).Data;

                //存原来有的采购种类编码现在没有了的集合
                IList<ProcurementTypeModel> allTypesNot = new List<ProcurementTypeModel>();

                allTypesNot = allTypes;

                //非内置的信息，用作数据验证
                var PayMethodNots = allTypes.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = allTypes.ToList().FindAll(x => x.Issystem == 1);
                if (user.UserNo == "Admin")
                {
                    allTypes = allTypes.ToList().FindAll(x => x.Issystem == 1);
                    allTypesNot = allTypesNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        var typesByCode = allTypes.ToList().FindAll(x => x.FCode == set.TypeCode);
                        //获取先前有但现在没有的数据集合
                        allTypesNot = allTypesNot.ToList().FindAll(t => t.FCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var nowTypes = typesByCode.FindAll(x => x.Orgid == org.PhId);
                                if (nowTypes.Count > 0)
                                {
                                    ProcurementTypeModel a = nowTypes[0];
                                    if (a.FName != set.TypeName || a.Isactive != set.Isactive || a.FRemark != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.FName = set.TypeName;
                                        a.FRemark = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        procurementTypes.Add(a);
                                    }
                                    typesByCode.Remove(nowTypes[0]);
                                }
                                else
                                {
                                    ProcurementTypeModel b = new ProcurementTypeModel();
                                    b.FCode = set.TypeCode;
                                    b.FName = set.TypeName;
                                    b.FRemark = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    b.PersistentState = PersistentState.Added;
                                    procurementTypes.Add(b);
                                }
                            }
                            if (typesByCode.Count > 0)
                            {
                                foreach (ProcurementTypeModel delete in typesByCode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    procurementTypes.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            if (typesByCode != null && typesByCode.Count > 0)
                            {
                                foreach (ProcurementTypeModel z in typesByCode)
                                {
                                    z.PersistentState = PersistentState.Deleted;
                                    procurementTypes.Add(z);
                                }
                            }
                        }
                    }
                    if (allTypesNot != null && allTypesNot.Count > 0)
                    {
                        foreach (ProcurementTypeModel z in allTypesNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            procurementTypes.Add(z);
                        }
                    }
                    //数据验证
                    if (procurementTypes != null && procurementTypes.Count > 0)
                    {
                        foreach (var pro in procurementTypes)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.FCode))
                                {
                                    return DCHelper.ErrorMessage("采购类型编码不能为空！");
                                }
                                if (procurementTypes.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的采购类型编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (procurementTypes.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }

                                }
                                else
                                {
                                    if (procurementTypes.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                    ////与库中私有设置进行数据验证
                    //var allTypesSystem = allTypes.ToList().FindAll(t => t.Issystem == (byte)0);
                    //if (allTypesSystem != null && allTypesSystem.Count > 0)
                    //{
                    //    foreach (var pro in procurementTypes)
                    //    {
                    //        if (pro.PersistentState != PersistentState.Deleted && allTypesSystem.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode).Count > 0)
                    //        {
                    //            return DCHelper.ErrorMessage(pro.Orgcode + "该组织下的采购类型与私有编码重复，请进行修改！");
                    //        }
                    //    }
                    //}
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有采购种类
                    var typesByOrg = allTypes.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            ProcurementTypeModel b = new ProcurementTypeModel();
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            b.FCode = set.TypeCode;
                            b.FName = set.TypeName;
                            b.FRemark = set.Bz;
                            b.Orgid = set.Orgid;
                            b.Orgcode = set.Orgcode;
                            b.Isactive = set.Isactive;
                            b.PersistentState = PersistentState.Added;
                            procurementTypes.Add(b);
                        }
                        else
                        {
                            var a = typesByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.FName != set.TypeName || a.FRemark != set.Bz || a.Isactive != set.Isactive || a.FCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    a.PersistentState = PersistentState.Modified;
                                    a.FCode = set.TypeCode;
                                    a.FName = set.TypeName;
                                    a.FRemark = set.Bz;
                                    a.Isactive = set.Isactive;
                                    procurementTypes.Add(a);
                                }
                                else
                                {
                                    a.PersistentState = PersistentState.Deleted;
                                    procurementTypes.Add(a);
                                }
                            }
                            typesByOrg.Remove(a);
                        }
                    }
                    if (typesByOrg.Count > 0)
                    {
                        foreach (ProcurementTypeModel z in typesByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.FCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            procurementTypes.Add(z);
                        }
                    }
                    //数据验证
                    if (procurementTypes != null && procurementTypes.Count > 0)
                    {
                        foreach (var pro in procurementTypes)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.FCode))
                                {
                                    return DCHelper.ErrorMessage("采购类型编码不能为空！");
                                }
                                if (procurementTypes.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的采购类型编码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                //if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.FCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                                //if (allTypes.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                //{
                                //    return DCHelper.ErrorMessage(pro.FCode + "该编码已经被公有化，不能被私有所有！");
                                //}
                            }
                        }
                    }
                }
                try
                {
                    savedresult = this.ProcurementTypeService.Save<Int64>(procurementTypes, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "Zcgnfl")//支出功能分类
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();

                //存入需要修改的支出功能分类集合
                List<QtZcgnflModel> qtZcgnfls = new List<QtZcgnflModel>();

                //所有的支出功能分类的集合
                IList<QtZcgnflModel> allZcgnfls = this.QtZcgnflService.Find(t => t.PhId != 0).Data;

                //存原来有的支出功能分类编码现在没有了的集合
                IList<QtZcgnflModel> allZcgnflsNot = new List<QtZcgnflModel>();

                allZcgnflsNot = allZcgnfls;
                //非内置的信息，用作数据验证
                var PayMethodNots = allZcgnfls.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = allZcgnfls.ToList().FindAll(x => x.Issystem == 1);
                if (user.UserNo == "Admin")
                {
                    allZcgnfls = allZcgnfls.ToList().FindAll(x => x.Issystem == 1);
                    allZcgnflsNot = allZcgnflsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        var zcgnflsByCode = allZcgnfls.ToList().FindAll(x => x.KMDM == set.TypeCode);
                        //获取先前有但现在没有的数据集合
                        allZcgnflsNot = allZcgnflsNot.ToList().FindAll(t => t.KMDM != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var nowZcgnfls = zcgnflsByCode.FindAll(x => x.Orgid == org.PhId);

                                if (nowZcgnfls.Count > 0)
                                {
                                    QtZcgnflModel a = nowZcgnfls[0];
                                    if (a.KMMC != set.TypeName || a.Isactive != set.Isactive || a.FRemark != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.KMMC = set.TypeName;
                                        a.FRemark = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        qtZcgnfls.Add(a);
                                    }
                                    zcgnflsByCode.Remove(nowZcgnfls[0]);
                                }
                                else
                                {
                                    QtZcgnflModel b = new QtZcgnflModel();
                                    b.KMDM = set.TypeCode;
                                    b.KMMC = set.TypeName;
                                    b.FRemark = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Ocode = org.OCode;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    b.PersistentState = PersistentState.Added;
                                    qtZcgnfls.Add(b);
                                }
                            }
                            if (zcgnflsByCode.Count > 0)
                            {
                                foreach (QtZcgnflModel delete in zcgnflsByCode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    qtZcgnfls.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QtZcgnflModel z in zcgnflsByCode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                qtZcgnfls.Add(z);
                            }
                        }


                    }

                    if (allZcgnflsNot != null && allZcgnflsNot.Count > 0)
                    {
                        foreach (QtZcgnflModel z in allZcgnflsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            qtZcgnfls.Add(z);
                        }
                    }
                    //数据验证
                    if (qtZcgnfls != null && qtZcgnfls.Count > 0)
                    {
                        foreach (var pro in qtZcgnfls)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.KMDM))
                                {
                                    return DCHelper.ErrorMessage("支出功能分类编码不能为空！");
                                }
                                if (qtZcgnfls.FindAll(t => t.Orgid == pro.Orgid && t.KMDM == pro.KMDM && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的支出功能分类编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (qtZcgnfls.FindAll(t => t.KMDM == pro.KMDM && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.KMDM + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.KMDM == pro.KMDM && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.KMDM + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (qtZcgnfls.FindAll(t => t.KMDM == pro.KMDM && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.KMDM + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支出功能分类
                    var zcgnflsByOrg = allZcgnfls.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            QtZcgnflModel b = new QtZcgnflModel();
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            b.KMDM = set.TypeCode;
                            b.KMMC = set.TypeName;
                            b.FRemark = set.Bz;
                            b.Orgid = set.Orgid;
                            b.Ocode = set.Orgcode;
                            b.Isactive = set.Isactive;
                            b.PersistentState = PersistentState.Added;
                            qtZcgnfls.Add(b);
                        }
                        else
                        {
                            var a = zcgnflsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.KMMC != set.TypeName || a.FRemark != set.Bz || a.Isactive != set.Isactive || a.KMDM != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.KMMC + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.KMMC + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    a.PersistentState = PersistentState.Modified;
                                    a.KMDM = set.TypeCode;
                                    a.KMMC = set.TypeName;
                                    a.FRemark = set.Bz;
                                    a.Isactive = set.Isactive;
                                    qtZcgnfls.Add(a);
                                }
                                else
                                {
                                    a.PersistentState = PersistentState.Deleted;
                                    qtZcgnfls.Add(a);
                                }
                            }
                            zcgnflsByOrg.Remove(a);
                        }
                    }
                    if (zcgnflsByOrg.Count > 0)
                    {
                        foreach (QtZcgnflModel z in zcgnflsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.KMDM + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            qtZcgnfls.Add(z);
                        }
                    }
                }
                //数据验证
                if (qtZcgnfls != null && qtZcgnfls.Count > 0)
                {
                    foreach (var pro in qtZcgnfls)
                    {
                        var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                        if (pro.PersistentState != PersistentState.Deleted)
                        {
                            if (string.IsNullOrEmpty(pro.KMDM))
                            {
                                return DCHelper.ErrorMessage("支出功能分类编码不能为空！");
                            }
                            if (qtZcgnfls.FindAll(t => t.Orgid == pro.Orgid && t.KMDM == pro.KMDM && t.PersistentState != PersistentState.Deleted).Count > 1)
                            {
                                return DCHelper.ErrorMessage(orgname + "该组织下的支出功能分类编码重复，请进行修改！");
                            }
                            if (pro.Issystem != (byte)1)
                            {
                                if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.KMDM == pro.KMDM && t.Issystem == (byte)1).Count > 0)
                                {
                                    return DCHelper.ErrorMessage(pro.KMDM + "此编码不能同时存在私有与公有之中！");
                                }
                            }
                            //if (allZcgnfls.ToList().FindAll(t => t.KMDM == pro.KMDM && t.Issystem == (byte)1).Count > 0)
                            //{
                            //    return DCHelper.ErrorMessage(pro.KMDM + "该编码已经被公有化，不能被私有所有！");
                            //}
                        }
                        //if (qtZcgnfls.FindAll(t => t.Orgid == pro.Orgid && t.KMDM == pro.KMDM && t.PersistentState != PersistentState.Deleted).Count > 1)
                        //{
                        //    return DCHelper.ErrorMessage(pro.Ocode + "该组织下的支出功能分类编码重复，请进行修改！");
                        //}
                    }
                }
                try
                {
                    savedresult = this.QtZcgnflService.Save<Int64>(qtZcgnfls, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "TargetClasses")//绩效评价指标类别
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();

                //存入需要修改的绩效评价指标类别集合
                List<PerformEvalTargetClassModel> performEvals = new List<PerformEvalTargetClassModel>();

                //所有的绩效评价指标类别的集合
                IList<PerformEvalTargetClassModel> allPerformEvals = this.PerformEvalTargetClassService.Find(t => t.PhId != 0).Data;

                //存原来有的绩效评价指标类别编码现在没有了的集合
                IList<PerformEvalTargetClassModel> allPerformEvalsNot = new List<PerformEvalTargetClassModel>();

                allPerformEvalsNot = allPerformEvals;

                //非内置的信息，用作数据验证
                var PayMethodNots = allPerformEvals.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = allPerformEvals.ToList().FindAll(x => x.Issystem == 1);
                if (user.UserNo == "Admin")
                {
                    allPerformEvals = allPerformEvals.ToList().FindAll(x => x.Issystem == 1);
                    allPerformEvalsNot = allPerformEvalsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        var performEvalsByCode = allPerformEvals.ToList().FindAll(x => x.FCode == set.TypeCode);

                        allPerformEvalsNot = allPerformEvalsNot.ToList().FindAll(t => t.FCode != set.TypeCode);

                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var nowPerformEvals = performEvalsByCode.FindAll(x => x.Orgid == org.PhId);
                                if (nowPerformEvals.Count > 0)
                                {
                                    PerformEvalTargetClassModel a = nowPerformEvals[0];
                                    if (a.FCode != set.TypeName || a.Isactive != set.Isactive || a.FRemark != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.FName = set.TypeName;
                                        a.FRemark = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        performEvals.Add(a);
                                    }
                                    performEvalsByCode.Remove(nowPerformEvals[0]);
                                }
                                else
                                {
                                    PerformEvalTargetClassModel b = new PerformEvalTargetClassModel();
                                    b.FCode = set.TypeCode;
                                    b.FName = set.TypeName;
                                    b.FRemark = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    b.PersistentState = PersistentState.Added;
                                    performEvals.Add(b);
                                }
                            }
                            if (performEvalsByCode.Count > 0)
                            {
                                foreach (PerformEvalTargetClassModel delete in performEvalsByCode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    performEvals.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            if (performEvalsByCode != null && performEvalsByCode.Count > 0)
                            {
                                foreach (PerformEvalTargetClassModel z in performEvalsByCode)
                                {
                                    z.PersistentState = PersistentState.Deleted;
                                    performEvals.Add(z);
                                }
                            }
                        }
                    }

                    //删除原先有现在无的集合
                    if (allPerformEvals != null && allPerformEvalsNot.Count > 0)
                    {
                        foreach (PerformEvalTargetClassModel z in allPerformEvals)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            performEvals.Add(z);
                        }
                    }

                    //数据验证
                    if (performEvals != null && performEvals.Count > 0)
                    {
                        foreach (var pro in performEvals)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.FCode))
                                {
                                    return DCHelper.ErrorMessage("绩效评价指标类别编码不能为空！");
                                }
                                if (performEvals.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的绩效评价指标类别编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (performEvals.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }

                                }
                                else
                                {
                                    if (performEvals.FindAll(t => t.FCode == pro.FCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有绩效评价指标类别
                    var performEvalsByOrg = allPerformEvals.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            PerformEvalTargetClassModel b = new PerformEvalTargetClassModel();
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            b.FCode = set.TypeCode;
                            b.FName = set.TypeName;
                            b.FRemark = set.Bz;
                            b.Orgid = set.Orgid;
                            b.Orgcode = set.Orgcode;
                            b.Isactive = set.Isactive;
                            b.PersistentState = PersistentState.Added;
                            performEvals.Add(b);
                        }
                        else
                        {
                            var a = performEvalsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.FName != set.TypeName || a.FRemark != set.Bz || a.Isactive != set.Isactive || a.FCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.FName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    a.PersistentState = PersistentState.Modified;
                                    a.FCode = set.TypeCode;
                                    a.FName = set.TypeName;
                                    a.FRemark = set.Bz;
                                    a.Isactive = set.Isactive;
                                    performEvals.Add(a);
                                }
                                else
                                {
                                    a.PersistentState = PersistentState.Deleted;
                                    performEvals.Add(a);
                                }
                            }
                            performEvalsByOrg.Remove(a);
                        }
                    }
                    if (performEvalsByOrg.Count > 0)
                    {
                        foreach (PerformEvalTargetClassModel z in performEvalsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.FCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            performEvals.Add(z);
                        }
                    }
                }
                //数据验证
                if (performEvals != null && performEvals.Count > 0)
                {
                    foreach (var pro in performEvals)
                    {
                        var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                        if (pro.PersistentState != PersistentState.Deleted)
                        {
                            if (string.IsNullOrEmpty(pro.FCode))
                            {
                                return DCHelper.ErrorMessage("绩效评价指标类别编码不能为空！");
                            }
                            if (performEvals.FindAll(t => t.Orgid == pro.Orgid && t.FCode == pro.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                            {
                                return DCHelper.ErrorMessage(orgname + "该组织下的绩效评价指标类别编码重复，请进行修改！");
                            }
                            if (pro.Issystem != (byte)1)
                            {
                                if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                                {
                                    return DCHelper.ErrorMessage(pro.FCode + "此编码不能同时存在私有与公有之中！");
                                }
                            }
                            //if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                            //{
                            //    return DCHelper.ErrorMessage(pro.FCode + "该编码已经被公有化，不能被私有所有！");
                            //}

                            //if (allPerformEvals.ToList().FindAll(t => t.FCode == pro.FCode && t.Issystem == (byte)1).Count > 0)
                            //{
                            //    return DCHelper.ErrorMessage(pro.FCode + "该编码已经被公有化，不能被私有所有！");
                            //}
                        }
                    }
                }
                try
                {
                    savedresult = this.PerformEvalTargetClassService.Save<Int64>(performEvals, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "CodeRule")//编码规则
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "CodeRule"));

                var CodeRules = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data;//所有编码规则的集合(数据库)
                //var DxbzValues = DxbzCodes.Select(x => x.Value).Distinct().ToList();//所有对下补助值的集合(数据库)
                if (user.UserNo == "Admin")
                {
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (CodeRules != null && CodeRules.Count > 0)
                        {
                            var rule = CodeRules.ToList().Find(t => t.TypeCode == set.TypeCode && t.Value == set.Value);
                            if (rule != null)
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            else if (SysSet.infoData[0].DicType == "Business")//业务条线
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "Business"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }

                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "Business";
                                    b.DicName = "业务条线";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("业务条线编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的业务条线编码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                            else
                            {
                                //删除前需要判断有没有被引用
                                var qtXmDistributeList = QtXmDistributeService.Find(x => x.FBusiness == pro.TypeCode);
                                if (qtXmDistributeList != null && qtXmDistributeList.Data != null && qtXmDistributeList.Data.Count > 0)
                                {
                                    return DCHelper.ErrorMessage("存在被引用的数据！");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基本数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("业务条线编码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的业务条线编码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                            else
                            {
                                //删除前需要判断有没有被引用
                                var qtXmDistributeList = QtXmDistributeService.Find(x => x.FBusiness == pro.TypeCode);
                                if (qtXmDistributeList != null && qtXmDistributeList.Data != null && qtXmDistributeList.Data.Count > 0)
                                {
                                    return DCHelper.ErrorMessage("存在被引用的数据！");
                                }
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "TqlzProportion")//提前列支比例
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "TqlzProportion"));

                var DxbzCodes = QTSysSetService.Find(dicWhere, new string[] { "Value Asc" }).Data;//所有对下补助的集合(数据库)
                var DxbzValues = DxbzCodes.Select(x => x.Value).Distinct().ToList();//所有对下补助值的集合(数据库)
                if (user.UserNo == "Admin")
                {
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }
                        if (DxbzValues.Contains(set.Value))
                        {
                            DxbzValues.Remove(set.Value);
                            var DxbzCodesByValue = DxbzCodes.ToList().FindAll(x => x.Value == set.Value);//数据库

                            if (set.OrgList != null && set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    //如果存在就删除，最后剩下的是要删除的
                                    var DxbzCodesByValueOrg = DxbzCodesByValue.FindAll(x => x.Orgid == org.PhId);
                                    if (DxbzCodesByValueOrg.Count > 0)
                                    {
                                        DxbzCodesByValue.Remove(DxbzCodesByValueOrg[0]);
                                    }
                                    else
                                    {
                                        QTSysSetModel b = new QTSysSetModel();
                                        b.DicType = "TqlzProportion";
                                        b.DicName = "提前列支比例维护";
                                        b.TypeName = "提前列支比例";
                                        b.Orgid = org.PhId;
                                        b.Orgcode = org.OCode;
                                        b.Value = set.Value;
                                        b.Issystem = 1;
                                        b.Isactive = set.Isactive;
                                        b.Bz = set.Bz;
                                        b.PersistentState = PersistentState.Added;
                                        resultSysSet.Add(b);
                                    }
                                }
                                if (DxbzCodesByValue.Count > 0)
                                {
                                    foreach (QTSysSetModel delete in DxbzCodesByValue)
                                    {
                                        delete.PersistentState = PersistentState.Deleted;
                                        resultSysSet.Add(delete);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //value新增  orglist不为空
                            if (set.OrgList.Count > 0)
                            {
                                foreach (OrganizeModel org in set.OrgList)
                                {
                                    QTSysSetModel c = new QTSysSetModel();
                                    c.DicType = "TqlzProportion";
                                    c.DicName = "提前列支比例维护";
                                    c.TypeName = "提前列支比例";
                                    c.Orgid = org.PhId;
                                    c.Orgcode = org.OCode;
                                    c.Value = set.Value;
                                    c.Issystem = 1;
                                    c.Isactive = set.Isactive;
                                    c.Bz = set.Bz;
                                    c.PersistentState = PersistentState.Added;
                                    resultSysSet.Add(c);
                                }
                            }
                        }
                    }
                    //删除整个value的数据
                    if (DxbzValues.Count > 0)
                    {
                        for (var i = 0; i < DxbzValues.Count; i++)
                        {
                            var d = DxbzCodes.ToList().FindAll(x => x.Value == DxbzValues[i]);
                            foreach (QTSysSetModel delete in d)
                            {
                                delete.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(delete);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有对下补助代码
                    var DxbzCodesByOrg = DxbzCodes.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    //var DxbzPhids = DxbzCodes.Select(x => x.PhId).Distinct().ToList();//所有对下补助值的phid集合(数据库)
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var DxbzCodesByOrgPhid = DxbzCodesByOrg.Find(x => x.PhId == set.PhId);
                            DxbzCodesByOrg.Remove(DxbzCodesByOrgPhid);
                            if (DxbzCodesByOrgPhid.Value != set.Value || DxbzCodesByOrgPhid.Isactive != set.Isactive || DxbzCodesByOrgPhid.Bz != set.Bz)
                            {
                                set.PersistentState = PersistentState.Modified;
                                resultSysSet.Add(set);
                            }
                        }

                    }
                    if (DxbzCodesByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in DxbzCodesByOrg)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "AssociatedPerson")//关联人员
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "AssociatedPerson"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }

                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "AssociatedPerson";
                                    b.DicName = "关联人员";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("关联人员身份证号不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的关联人员身份证号重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此身份证号不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此身份证号不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此身份证号不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基础数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("关联人员身份证号不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的关联人员身份证号重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此身份证号不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "IncomeAttribution")//收入归属
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "IncomeAttribution"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }

                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "IncomeAttribution";
                                    b.DicName = "收入归属";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("收入归属代码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的收入归属代码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此收入归属代码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此收入归属代码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此收入归属代码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基础数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("收入归属代码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的收入归属代码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此收入归属代码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "ZcfxName")//支出分项名称
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "ZcfxName"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }

                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "ZcfxName";
                                    b.DicName = "支出分项名称";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("支出分项名称代码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的支出分项名称代码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此支出分项名称代码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此支出分项名称代码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此支出分项名称代码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基础数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("支出分项名称代码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的支出分项名称代码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此支出分项名称代码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else if (SysSet.infoData[0].DicType == "Costitem")//费用项目
            {
                if (string.IsNullOrEmpty(SysSet.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<string>.Eq("DicType", "Costitem"));

                var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

                //存原来有的PayMethodTwo编码现在没有了的集合
                IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

                allSysSetsNot = PayMethods;
                //非内置的信息，用作数据验证
                var PayMethodNots = PayMethods.ToList().FindAll(x => x.Issystem != 1);
                //内置的信息，用作数据验证
                var PayMethodYess = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                //var TypeCode = 0;
                //if (PayMethods.Count > 0)
                //{
                //    TypeCode = int.Parse(PayMethods[0].TypeCode);
                //}
                if (user.UserNo == "Admin")
                {
                    PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
                    allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        //通过phid获取组织集合
                        if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        }

                        //if (string.IsNullOrEmpty(set.TypeCode))
                        //{
                        //    TypeCode++;
                        //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                        //}
                        var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

                        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
                        if (set.OrgList != null && set.OrgList.Count > 0)
                        {
                            foreach (OrganizeModel org in set.OrgList)
                            {
                                //如果存在就删除，最后剩下的是要删除的
                                var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
                                if (PayMethodsByTypecodeOrg.Count > 0)
                                {
                                    QTSysSetModel a = PayMethodsByTypecodeOrg[0];
                                    if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
                                    {
                                        a.Isactive = set.Isactive;
                                        a.TypeName = set.TypeName;
                                        a.Bz = set.Bz;
                                        a.Issystem = set.Issystem;
                                        a.PersistentState = PersistentState.Modified;
                                        resultSysSet.Add(a);
                                    }
                                    PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
                                }
                                else
                                {
                                    QTSysSetModel b = new QTSysSetModel();
                                    b.DicType = "Costitem";
                                    b.DicName = "费用项目";
                                    b.TypeCode = set.TypeCode;
                                    b.TypeName = set.TypeName;
                                    b.Bz = set.Bz;
                                    b.Orgid = org.PhId;
                                    b.Orgcode = org.OCode;
                                    b.PersistentState = PersistentState.Added;
                                    b.Isactive = set.Isactive;
                                    b.Issystem = 1;
                                    resultSysSet.Add(b);
                                }
                            }
                            if (PayMethodsByTypecode.Count > 0)
                            {
                                foreach (QTSysSetModel delete in PayMethodsByTypecode)
                                {
                                    delete.PersistentState = PersistentState.Deleted;
                                    resultSysSet.Add(delete);
                                }
                            }
                        }
                        else
                        {
                            foreach (QTSysSetModel z in PayMethodsByTypecode)
                            {
                                z.PersistentState = PersistentState.Deleted;
                                resultSysSet.Add(z);
                            }
                        }


                    }

                    //删除原有的现无的数据
                    if (allSysSetsNot != null && allSysSetsNot.Count > 0)
                    {
                        foreach (QTSysSetModel z in allSysSetsNot)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }

                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("费用项目代码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的费用项目代码重复，请进行修改！");
                                }

                                if (pro.Issystem == (byte)1)
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此费用项目代码不能同时存在私有与公有之中！");
                                    }
                                    if (PayMethodNots != null && PayMethodNots.Count > 0 && PayMethodNots.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此费用项目代码不能同时存在私有与公有之中！");
                                    }
                                }
                                else
                                {
                                    if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此费用项目名称代码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SysSet.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织的所有支付方式
                    var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
                    foreach (QTSysSetModel set in SysSet.infoData)
                    {
                        if (set.PhId == 0)
                        {
                            //TypeCode++;
                            //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
                            OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(SysSet.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            resultSysSet.Add(set);
                        }
                        else
                        {
                            var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (a.Issystem == (byte)1)
                                {
                                    if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
                                    {
                                        return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                    }
                                    //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
                                }
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    set.PersistentState = PersistentState.Modified;
                                    resultSysSet.Add(set);
                                }
                                else
                                {
                                    resultSysSet.Add(set);
                                }
                                PayMethodsByOrg.Remove(a);
                            }
                        }
                    }
                    if (PayMethodsByOrg.Count > 0)
                    {
                        foreach (QTSysSetModel z in PayMethodsByOrg)
                        {
                            if (z.Issystem == (byte)1)
                            {
                                return DCHelper.ErrorMessage(z.TypeCode + "该基础数据为公有数据，你无权删除！");
                            }
                            z.PersistentState = PersistentState.Deleted;
                            resultSysSet.Add(z);
                        }
                    }
                    //数据验证
                    if (resultSysSet != null && resultSysSet.Count > 0)
                    {
                        foreach (var pro in resultSysSet)
                        {
                            var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
                            if (pro.PersistentState != PersistentState.Deleted)
                            {
                                if (string.IsNullOrEmpty(pro.TypeCode))
                                {
                                    return DCHelper.ErrorMessage("费用项目代码不能为空！");
                                }
                                if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                                {
                                    return DCHelper.ErrorMessage(orgname + "该组织下的费用项目代码重复，请进行修改！");
                                }
                                if (pro.Issystem != (byte)1)
                                {
                                    if (PayMethodYess != null && PayMethodYess.Count > 0 && PayMethodYess.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(pro.TypeCode + "此费用项目代码不能同时存在私有与公有之中！");
                                    }
                                }
                            }
                        }
                    }
                }
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            else
            {
                resultSysSet = SysSet.infoData;
                try
                {
                    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
                }
                catch (Exception ex)
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = ex.Message.ToString();
                }
            }
            #region//try.catch写入if语句
            //try
            //{
            //    //if (SysSet.infoData[0].DicType == "PayMethod")
            //    //{
            //    //    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
            //    //}
            //    //else if (SysSet.infoData[0].DicType == "DxbzCode")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "StayTime")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "ProjectLevel")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "ProjectProper")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "TimeLimit")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "PayMethodTwo")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "ProcurementsCatalog")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "ProcurementsProcedures")
            //    //{


            //    //}
            //    //else if (SysSet.infoData[0].DicType == "ProcurementsType")
            //    //{

            //    //}
            //    //else if (SysSet.infoData[0].DicType == "Zcgnfl")
            //    //{

            //    //}
            //    //else
            //    //{

            //    //}   
            //    savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
            //}
            //catch (Exception ex)
            //{
            //    savedresult.Status = ResponseStatus.Error;
            //    savedresult.Msg = ex.Message.ToString();
            //}
            #endregion
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据字典类型代码取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetSysSetList([FromUri]string DicType, [FromUri]BaseModel baseModel)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("DicType", DicType));

            var data = new List<QTSysSetModel>();
            //支付方式特殊处理
            if (DicType == "PayMethod")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "PayMethod";
                            qTSys.DicName = "支付方式";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "DxbzCode")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> valueList = data.Select(x => x.Value).Distinct().ToList();//取所有对下补助值集合
                        foreach (string value in valueList)
                        {
                            var SysSetListByvalue = data.FindAll(x => x.Value == value);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "DxbzCode";
                            qTSys.DicName = "对下补助代码维护";
                            qTSys.TypeName = "对下补助代码";
                            qTSys.Value = value;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListByvalue)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "ZjlzCode")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> valueList = data.Select(x => x.Value).Distinct().ToList();//取所有对下补助值集合
                        foreach (string value in valueList)
                        {
                            var SysSetListByvalue = data.FindAll(x => x.Value == value);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "ZjlzCode";
                            qTSys.DicName = "直接列支代码维护";
                            qTSys.TypeName = "直接列支代码";
                            qTSys.Value = value;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListByvalue)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "StayTime")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> valueList = data.Select(x => x.Value).Distinct().ToList();//取所有对下补助值集合
                        foreach (string value in valueList)
                        {
                            var SysSetListByvalue = data.FindAll(x => x.Value == value);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "StayTime";
                            qTSys.DicName = "停留时长提醒设置";
                            qTSys.TypeName = "停留时长设置";
                            qTSys.Value = value;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListByvalue)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "ProjectLevel")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "ProjectLevel";
                            qTSys.DicName = "项目级别";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "ProjectProper")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "ProjectProper";
                            qTSys.DicName = "项目属性";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "TimeLimit")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "TimeLimit";
                            qTSys.DicName = "续存期限";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "PayMethodTwo")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "PayMethodTwo";
                            qTSys.DicName = "支付方式2";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "ProcurementsCatalog")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));

                IList<ProcurementCatalogModel> allCatalogs = new List<ProcurementCatalogModel>();
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));

                    //所有的采购目录的集合
                    allCatalogs = this.ProcurementCatalogService.Find(t => t.PhId != 0 && t.Issystem == (byte)1).Data.OrderBy(t => t.FCode).ToList();

                    //data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (allCatalogs != null && allCatalogs.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = allCatalogs.Select(x => x.FCode).Distinct().ToList();//取所有采购目录的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = allCatalogs.ToList().FindAll(x => x.FCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "ProcurementsCatalog";
                            qTSys.DicName = "采购目录";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].FName;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Bz = SysSetListBytypeCode[0].FRemark;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (ProcurementCatalogModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    allCatalogs = ProcurementCatalogService.Find(dicWhere, new string[] { "FCode Asc" }).Data.ToList();
                    var data2 = new List<QTSysSetModel>();
                    foreach (ProcurementCatalogModel b in allCatalogs)
                    {
                        QTSysSetModel qTSys = new QTSysSetModel();
                        qTSys.PhId = b.PhId;
                        qTSys.DicType = "ProcurementsCatalog";
                        qTSys.DicName = "采购目录";
                        qTSys.TypeCode = b.FCode;
                        qTSys.TypeName = b.FName;
                        qTSys.Issystem = b.Issystem;
                        qTSys.Isactive = b.Isactive;
                        qTSys.Bz = b.FRemark;
                        qTSys.OrgList = new List<OrganizeModel>();
                        qTSys.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                        data2.Add(qTSys);
                    }
                    return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                }
            }
            else if (DicType == "ProcurementsProcedures")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));

                IList<ProcurementProceduresModel> allCatalogs = new List<ProcurementProceduresModel>();
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));

                    //所有的采购程序的集合
                    allCatalogs = this.ProcurementProceduresService.Find(t => t.PhId != 0 && t.Issystem == (byte)1).Data.OrderBy(t => t.FCode).ToList();

                    //data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (allCatalogs != null && allCatalogs.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = allCatalogs.Select(x => x.FCode).Distinct().ToList();//取所有采购目录的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = allCatalogs.ToList().FindAll(x => x.FCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "ProcurementsProcedures";
                            qTSys.DicName = "采购程序";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].FName;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Bz = SysSetListBytypeCode[0].FRemark;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (ProcurementProceduresModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    allCatalogs = ProcurementProceduresService.Find(dicWhere, new string[] { "FCode Asc" }).Data.ToList();
                    var data2 = new List<QTSysSetModel>();
                    foreach (ProcurementProceduresModel b in allCatalogs)
                    {
                        QTSysSetModel qTSys = new QTSysSetModel();
                        qTSys.PhId = b.PhId;
                        qTSys.DicType = "ProcurementsProcedures";
                        qTSys.DicName = "采购程序";
                        qTSys.TypeCode = b.FCode;
                        qTSys.TypeName = b.FName;
                        qTSys.Issystem = b.Issystem;
                        qTSys.Isactive = b.Isactive;
                        qTSys.Bz = b.FRemark;
                        qTSys.OrgList = new List<OrganizeModel>();
                        qTSys.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                        data2.Add(qTSys);
                    }
                    return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                }
            }
            else if (DicType == "ProcurementsType")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));

                IList<ProcurementTypeModel> allCatalogs = new List<ProcurementTypeModel>();
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));

                    //所有的采购程序的集合
                    allCatalogs = this.ProcurementTypeService.Find(t => t.PhId != 0 && t.Issystem == (byte)1).Data.OrderBy(t => t.FCode).ToList();

                    //data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (allCatalogs != null && allCatalogs.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = allCatalogs.Select(x => x.FCode).Distinct().ToList();//取所有采购目录的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = allCatalogs.ToList().FindAll(x => x.FCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "ProcurementsType";
                            qTSys.DicName = "采购种类";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].FName;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Bz = SysSetListBytypeCode[0].FRemark;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (ProcurementTypeModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    allCatalogs = ProcurementTypeService.Find(dicWhere, new string[] { "FCode Asc" }).Data.ToList();
                    var data2 = new List<QTSysSetModel>();
                    foreach (ProcurementTypeModel b in allCatalogs)
                    {
                        QTSysSetModel qTSys = new QTSysSetModel();
                        qTSys.PhId = b.PhId;
                        qTSys.DicType = "ProcurementsType";
                        qTSys.DicName = "采购种类";
                        qTSys.TypeCode = b.FCode;
                        qTSys.TypeName = b.FName;
                        qTSys.Issystem = b.Issystem;
                        qTSys.Isactive = b.Isactive;
                        qTSys.Bz = b.FRemark;
                        qTSys.OrgList = new List<OrganizeModel>();
                        qTSys.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                        data2.Add(qTSys);
                    }
                    return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                }
            }
            else if (DicType == "Zcgnfl")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));

                IList<QtZcgnflModel> allCatalogs = new List<QtZcgnflModel>();
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));

                    //所有的支出功能类别的集合
                    allCatalogs = this.QtZcgnflService.Find(t => t.PhId != 0 && t.Issystem == (byte)1).Data.OrderBy(t => t.KMDM).ToList();

                    //data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (allCatalogs != null && allCatalogs.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = allCatalogs.Select(x => x.KMDM).Distinct().ToList();//取所有采购目录的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = allCatalogs.ToList().FindAll(x => x.KMDM == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "Zcgnfl";
                            qTSys.DicName = "支出功能类别";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].KMMC;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Bz = SysSetListBytypeCode[0].FRemark;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QtZcgnflModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    allCatalogs = QtZcgnflService.Find(dicWhere, new string[] { "KMDM Asc" }).Data.ToList();
                    var data2 = new List<QTSysSetModel>();
                    foreach (QtZcgnflModel b in allCatalogs)
                    {
                        QTSysSetModel qTSys = new QTSysSetModel();
                        qTSys.PhId = b.PhId;
                        qTSys.DicType = "Zcgnfl";
                        qTSys.DicName = "支出功能类别";
                        qTSys.TypeCode = b.KMDM;
                        qTSys.TypeName = b.KMMC;
                        qTSys.Issystem = b.Issystem;
                        qTSys.Isactive = b.Isactive;
                        qTSys.Bz = b.FRemark;
                        qTSys.OrgList = new List<OrganizeModel>();
                        qTSys.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                        data2.Add(qTSys);
                    }
                    return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                }
            }
            else if (DicType == "TargetClasses")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));

                IList<PerformEvalTargetClassModel> allCatalogs = new List<PerformEvalTargetClassModel>();
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));

                    //所有的采购程序的集合
                    allCatalogs = this.PerformEvalTargetClassService.Find(t => t.PhId != 0 && t.Issystem == (byte)1).Data.OrderBy(t => t.FCode).ToList();

                    //data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (allCatalogs != null && allCatalogs.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = allCatalogs.Select(x => x.FCode).Distinct().ToList();//取所有采购目录的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = allCatalogs.ToList().FindAll(x => x.FCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "TargetClasses";
                            qTSys.DicName = "绩效评价指标类别";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].FName;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Bz = SysSetListBytypeCode[0].FRemark;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (PerformEvalTargetClassModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    allCatalogs = PerformEvalTargetClassService.Find(dicWhere, new string[] { "FCode Asc" }).Data.ToList();
                    var data2 = new List<QTSysSetModel>();
                    foreach (PerformEvalTargetClassModel b in allCatalogs)
                    {
                        QTSysSetModel qTSys = new QTSysSetModel();
                        qTSys.PhId = b.PhId;
                        qTSys.DicType = "TargetClasses";
                        qTSys.DicName = "绩效评价指标类别";
                        qTSys.TypeCode = b.FCode;
                        qTSys.TypeName = b.FName;
                        qTSys.Issystem = b.Issystem;
                        qTSys.Isactive = b.Isactive;
                        qTSys.Bz = b.FRemark;
                        qTSys.OrgList = new List<OrganizeModel>();
                        qTSys.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                        data2.Add(qTSys);
                    }
                    return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                }
            }
            else if (DicType == "Business")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "Business";
                            qTSys.DicName = "业务条线";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "TqlzProportion")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> valueList = data.Select(x => x.Value).Distinct().ToList();//取所有对下补助值集合
                        foreach (string value in valueList)
                        {
                            var SysSetListByvalue = data.FindAll(x => x.Value == value);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "TqlzProportion";
                            qTSys.DicName = "提前列支比例维护";
                            qTSys.TypeName = "提前列支比例";
                            qTSys.Value = value;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListByvalue)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "AssociatedPerson")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "AssociatedPerson";
                            qTSys.DicName = "关联人员";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "IncomeAttribution")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "IncomeAttribution";
                            qTSys.DicName = "收入归属";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "ZcfxName")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "ZcfxName";
                            qTSys.DicName = "支出分项名称";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else if (DicType == "Costitem")
            {
                if (string.IsNullOrEmpty(baseModel.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                if (user.UserNo == "Admin")
                {
                    new CreateCriteria(dicWhere)
                        .Add(ORMRestrictions<Byte>.Eq("Issystem", 1));
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    if (data.Count > 0)
                    {
                        var data2 = new List<QTSysSetModel>();
                        List<string> typeCodeList = data.Select(x => x.TypeCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var SysSetListBytypeCode = data.FindAll(x => x.TypeCode == typedm);
                            QTSysSetModel qTSys = new QTSysSetModel();
                            qTSys.DicType = "Costitem";
                            qTSys.DicName = "费用说明";
                            qTSys.TypeCode = typedm;
                            qTSys.TypeName = SysSetListBytypeCode[0].TypeName;
                            qTSys.Isactive = SysSetListBytypeCode[0].Isactive;
                            qTSys.Issystem = SysSetListBytypeCode[0].Issystem;
                            qTSys.Bz = SysSetListBytypeCode[0].Bz;
                            qTSys.OrgList = new List<OrganizeModel>();
                            foreach (QTSysSetModel a in SysSetListBytypeCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTSys.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            data2.Add(qTSys);
                        }
                        return DCHelper.ModelListToJson<QTSysSetModel>(data2, data2.Count);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        new CreateCriteria(dicWhere)
                            .Add(ORMRestrictions<Int64>.Eq("Orgid", long.Parse(baseModel.orgid)));
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
                    foreach (QTSysSetModel b in data)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
            }
            else
            {
                data = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Asc" }).Data.ToList();
            }
            return DCHelper.ModelListToJson<QTSysSetModel>(data, data.Count);
        }

        /// <summary>
        /// 根据操作员代码判断支付口令状态(1:支付口令不启用;2：支付口令为空维护;0:正常请输入支付口令)
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostPayPsd([FromBody]QTSysSetModel SysSet)
        {
            var result = "0";
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("DicType", "PayPsd"))
                .Add(ORMRestrictions<string>.Eq("TypeCode", SysSet.TypeCode));

            var data = QTSysSetService.Find(dicWhere).Data;
            if (data.Count > 0)
            {
                if (data[0].Isactive != 0)//不启用
                {
                    result = "1";
                }
                else
                {
                    if (string.IsNullOrEmpty(data[0].Value))//支付口令为空
                    {
                        result = "2";
                    }
                    else
                    {

                    }
                }
            }
            else//新增支付口令 默认为空
            {
                QTSysSetModel SysSet2 = new QTSysSetModel();
                SysSet2.DicType = "PayPsd";
                SysSet2.DicName = "支付口令";
                SysSet2.TypeCode = SysSet.TypeCode;
                SysSet2.TypeName = SysSet.TypeName;
                SysSet2.Orgid = SysSet.Orgid;
                SysSet2.Orgcode = SysSet.Orgcode;
                SysSet2.PersistentState = PersistentState.Added;
                QTSysSetService.Save<Int64>(SysSet2, "");
                result = "2";
            }

            return result;
        }

        /// <summary>
        /// 判断支付口令是否正确
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostJudgePayPsd([FromBody]QTSysSetModel SysSet)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("DicType", "PayPsd"))
                .Add(ORMRestrictions<string>.Eq("TypeCode", SysSet.TypeCode));
            var data = QTSysSetService.Find(dicWhere).Data;
            if (data.Count > 0)
            {
                if (data[0].Value.Equals(SysSet.Value))
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "error";
            }
        }

        /// <summary>
        /// 维护支付口令
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string PostSavePayPsd([FromBody]QTSysSetModel SysSet)
        {
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("DicType", "PayPsd"))
                .Add(ORMRestrictions<string>.Eq("TypeCode", SysSet.TypeCode));
            var data = QTSysSetService.Find(dicWhere).Data;
            if (data.Count > 0)
            {
                if (data[0].Value == SysSet.DEFSTR1)
                {
                    data[0].Value = SysSet.Value;
                    data[0].Isactive = SysSet.Isactive;
                    data[0].PersistentState = PersistentState.Modified;
                    savedresult = QTSysSetService.Save<Int64>(data[0], "");
                }
                else
                {
                    savedresult.Status = ResponseStatus.Error;
                    savedresult.Msg = "原密码输入不正确";
                }
            }
            else
            {
                QTSysSetModel SysSet2 = new QTSysSetModel();
                SysSet2.DicType = "PayPsd";
                SysSet2.DicName = "支付口令";
                SysSet2.TypeCode = SysSet.TypeCode;
                SysSet2.TypeName = SysSet.TypeName;
                SysSet2.Orgid = SysSet.Orgid;
                SysSet2.Orgcode = SysSet.Orgcode;
                SysSet2.Isactive = SysSet.Isactive;
                SysSet2.Value = SysSet.Value;
                SysSet2.PersistentState = PersistentState.Added;
                savedresult = QTSysSetService.Save<Int64>(SysSet2, "");
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据操作员代码取支付口令
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetPayPsd([FromUri]QTSysSetModel SysSet)
        {

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("DicType", "PayPsd"))
                .Add(ORMRestrictions<string>.Eq("TypeCode", SysSet.TypeCode));

            var data = QTSysSetService.Find(dicWhere).Data;
            if (data.Count > 0)
            {
                return DataConverterHelper.SerializeObject(data[0]);
            }
            else
            {
                return DataConverterHelper.SerializeObject(new QTSysSetModel());
            }

            //return DataConverterHelper.EntityListToJson<BankAccountModel>(bankAccountsList, bankAccountsList.Count);
            //return DCHelper.ModelListToJson<QTSysSetModel>(data, data.Count);
        }

        /// <summary>
        /// 根据操作员代码确定是否启用
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpPost]
        public string PostPayPsdIsactive([FromBody]QTSysSetModel SysSet)
        {
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("DicType", "PayPsd"))
                .Add(ORMRestrictions<string>.Eq("TypeCode", SysSet.TypeCode));

            var data = QTSysSetService.Find(dicWhere).Data;
            if (data.Count > 0)
            {
                //data[0].Value = SysSet.Value;
                data[0].Isactive = SysSet.Isactive;
                data[0].PersistentState = PersistentState.Modified;
                savedresult = QTSysSetService.Save<Int64>(data[0], "");
            }
            else
            {
                QTSysSetModel SysSet2 = new QTSysSetModel();
                SysSet2.DicType = "PayPsd";
                SysSet2.DicName = "支付口令";
                SysSet2.TypeCode = SysSet.TypeCode;
                SysSet2.TypeName = SysSet.TypeName;
                SysSet2.Orgid = SysSet.Orgid;
                SysSet2.Orgcode = SysSet.Orgcode;
                SysSet2.Isactive = SysSet.Isactive;
                //SysSet2.Value = SysSet.Value;
                SysSet2.PersistentState = PersistentState.Added;
                savedresult = QTSysSetService.Save<Int64>(SysSet2, "");
            }
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 根据操作员id确定是否启用支付加密狗验证
        /// </summary>
        /// <returns>返回Json串, 如果启用了则返回加密锁Key，否则返回空串</returns>
        [HttpPost]
        public string PostPayUsbKeyIsActive([FromBody]BaseSingleModel param)
        {
            if (string.IsNullOrEmpty(param.uid))
            {
                return DCHelper.ErrorMessage("用户ID不能为空!");
            }

            Dictionary<string, object> ret = new Dictionary<string, object>();

            bool isActive = false, isValid = false;
            string lockKey = string.Empty;
            DateTime start_dt, end_dt;
            isActive = QTSysSetService.GetPayUsbKeyIsActive(long.Parse(param.uid), out lockKey, out start_dt, out end_dt);

            ret.Add("Status", "success");
            ret.Add("IsActive", isActive);
            //ret.Add("StartDate", start_dt.ToString("yyyy-MM-dd hh:mm:ss"));
            //ret.Add("EndDate", start_dt.ToString("yyyy-MM-dd hh:mm:ss"));

            if (start_dt < end_dt)
            {
                if (end_dt > DateTime.Now)
                {
                    isValid = true;
                }
            }
            ret.Add("LockKeyIsValid", isValid);

            //无效时，设置LockKey为空
            if (isValid == false)
            {
                ret.Add("LockKey", "");
            }
            else
            {
                ret.Add("LockKey", lockKey);
            }

            return DCHelper.Message(ret);
        }

        /// <summary>
        /// 根据组织以及相关获取的组织关系数据的类型获取数据集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllRelationList([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.RelationType))
            {
                return DCHelper.ErrorMessage("关系类型不明确！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            try
            {
                Dictionary<string, object> dicWhere = new Dictionary<string, object>();
                switch (param.RelationType)
                {
                    case "1":
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "02"));
                        if (param.orgCode != null)
                        {
                            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", param.orgCode));
                        }
                        var result = BudgetAccountsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-YSKM", dicWhere);
                        return DCHelper.ModelListToJson<BudgetAccountsModel>(result.Results, (Int32)result.TotalItems);
                    case "2":
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "96"));
                        if (param.orgCode != null)
                        {
                            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", param.orgCode));
                        }
                        var result2 = SourceOfFundsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-ZJLY", dicWhere);
                        return DCHelper.ModelListToJson<SourceOfFundsModel>(result2.Results, (Int32)result2.TotalItems);
                    case "3":
                        dicWhere.Clear();
                        new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"));
                        if (param.orgCode != null)
                        {
                            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", param.orgCode));
                        }
                        var result3 = ExpenseCategoryService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX2-ZCLB", dicWhere);
                        return DCHelper.ModelListToJson<ExpenseCategoryModel>(result3.Results, (Int32)result3.TotalItems);
                    case "4":
                        var result4 = GetVc2mList(param.orgCode);
                        return DCHelper.ModelListToJson<VCorrespondenceSetting2Model>(result4, (Int32)result4.Count());
                    default:
                        FindedResult findedresultother = new FindedResult();
                        return DataConverterHelper.ResponseResultToJson(findedresultother);
                }

            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// 根据组织获取预算科目集合
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        public IList<BudgetAccountsModel> GetBudgetAccounts(string orgCode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "02"));
            if (orgCode != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", orgCode));
            }
            var result = BudgetAccountsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-YSKM", dicWhere);
            return result.Results;
        }

        /// <summary>
        /// 根据组织获取对应的资金来源
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        public IList<SourceOfFundsModel> GetSourceOfFunds(string orgCode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "96"));
            if (orgCode != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", orgCode));
            }
            var result = SourceOfFundsService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX-ZJLY", dicWhere);
            return result.Results;
        }

        /// <summary>
        /// 根据组织获取支出类别集合
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        public IList<ExpenseCategoryModel> GetExpenseCategories(string orgCode)
        {
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"));
            if (orgCode != null)
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dwdm", orgCode));
            }
            var result = ExpenseCategoryService.ServiceHelper.LoadWithPageInfinity("GQT.QT.DYGX2-ZCLB", dicWhere);
            return result.Results;
        }

        /// <summary>
        /// 根据组织编码获取对应的支出渠道
        /// </summary>
        /// <param name="dwdm">组织代码</param>
        /// <returns></returns>
        public IList<VCorrespondenceSetting2Model> GetVc2mList(String dwdm)
        {
            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];
            Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(clientJsonQuery);//查询条件转Dictionary
            CreateCriteria createCriteria = new CreateCriteria(dicWhere);
            createCriteria.Add(ORMRestrictions<string>.Eq("Dylx", "ZC"));
            createCriteria.Add(ORMRestrictions<string>.NotEq("Dydm", "NULL"));
            if (dwdm != null)
            {
                createCriteria.Add(ORMRestrictions<string>.Eq("Dwdm", dwdm));
            }

            DataStoreParam storeparam = new DataStoreParam();
            var cr2s = this.CorrespondenceSettings2Service.LoadWithPage(storeparam.PageIndex, storeparam.PageSize, dicWhere).Results as List<CorrespondenceSettings2Model>;
            var ors = this.GetOrg();
            IList<VCorrespondenceSetting2Model> vcr2s = new List<VCorrespondenceSetting2Model>();
            foreach (CorrespondenceSettings2Model cr2 in cr2s)
            {
                VCorrespondenceSetting2Model vc2 = new VCorrespondenceSetting2Model();
                vc2.PhId = cr2.PhId;
                vc2.DWDM = cr2.Dwdm;
                vc2.DYDM = cr2.Dydm;
                vc2.DYLX = cr2.Dylx;

                var or = from or1 in ors
                         where or1.OCode.Equals(cr2.Dydm)
                         select or1;
                if (or.Count() == 1)
                {
                    vc2.Dymc = or.ToList()[0].OName;
                }
                else
                {
                    vc2.Dymc = "未设置";
                }
                or = from or1 in ors
                     where or1.OCode.Equals(cr2.Dwdm)
                     select or1;
                if (or.Count() == 1)
                {
                    vc2.Dwmc = or.ToList()[0].OName;
                }
                else
                {
                    vc2.Dymc = "未设置";
                }

                vcr2s.Add(vc2);
            }
            return vcr2s;
        }

        /// <summary>
        /// 获取组织结合
        /// </summary>
        /// <returns></returns>
        public List<OrganizeModel> GetOrg()
        {
            DataStoreParam dataStoreParam = new DataStoreParam();
            var result = this.CorrespondenceSettingsService.LoadWithPageOrg(dataStoreParam);
            return result.Results as List<OrganizeModel>;
        }

        /// <summary>
        /// 根据组织过去所有基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllBasicData([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织id不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码不能为空！");
            }
            try
            {
                var result1 = GetBudgetAccounts(param.orgid).ToList();//预算科目集合
                result1 = result1.FindAll(x => x.KMLB == "1");//只取支出类预算科目
                //var result1_remove = new List<BudgetAccountsModel>();
                //取末级
                foreach (var a in result1)
                {
                    if (result1.FindAll(x => x.KMDM.IndexOf(a.KMDM) == 0).Count == 1)
                    {
                        a.DEFSTR10 = "isend";
                        //result1_remove.Add(a);
                    }
                }
                // result1 = result1.Except(result1_remove).ToList();
                var result2 = GetSourceOfFunds(param.orgid).ToList();//资金来源集合
                //取末级
                foreach (var b in result2)
                {
                    if (result2.FindAll(x => x.DM.IndexOf(b.DM) == 0).Count == 1)
                    {
                        b.isend = "isend";
                        //result1_remove.Add(a);
                    }
                }

                var result3 = GetExpenseCategories(param.orgid).ToList();//支出类别集合
                //取末级
                foreach (var c in result3)
                {
                    if (result3.FindAll(x => x.Dm.IndexOf(c.Dm) == 0).Count == 1)
                    {
                        c.isend = "isend";
                        //result1_remove.Add(a);
                    }
                }
                var result4 = GetVc2mList(param.orgCode);//支出渠道集合
                IList<QTSysSetModel> qTSysSets = new List<QTSysSetModel>();
                IList<QTSysSetModel> result5 = new List<QTSysSetModel>();
                IList<QTSysSetModel> result6 = new List<QTSysSetModel>();
                IList<QTSysSetModel> result7 = new List<QTSysSetModel>();
                IList<QTSysSetModel> result8 = new List<QTSysSetModel>();
                IList<QTSysSetModel> result9 = new List<QTSysSetModel>();
                List<QtZcgnflModel> result10 = new List<QtZcgnflModel>();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<string>.Eq("Orgcode", param.orgCode))
                    .Add(ORMRestrictions<byte>.Eq("Isactive", (byte)0));
                qTSysSets = this.QTSysSetService.Find(dic, new string[] { "DicType", "TypeCode" }).Data;
                if (qTSysSets.Count > 0)
                {
                    result5 = qTSysSets.ToList().FindAll(t => t.DicType == "PayMethod");//支付方式一的集合
                    result6 = qTSysSets.ToList().FindAll(t => t.DicType == "ProjectLevel");//项目级别集合
                    result7 = qTSysSets.ToList().FindAll(t => t.DicType == "ProjectProper");//项目属性集合
                    result8 = qTSysSets.ToList().FindAll(t => t.DicType == "TimeLimit");//续存期限集合
                    result9 = qTSysSets.ToList().FindAll(t => t.DicType == "PayMethodTwo");//支付方式二集合
                }
                result10 = this.QtZcgnflService.GetZcgnfls(param.orgid, param.orgCode).ToList();
                //取末级
                foreach (var d in result10)
                {
                    if (result10.FindAll(x => x.KMDM.IndexOf(d.KMDM) == 0).Count == 1)
                    {
                        d.isend = "isend";
                        //result1_remove.Add(a);
                    }
                }
                var data = new
                {
                    Status = "success",
                    Msg = "获取数据成功!",
                    BudgetAccounts = result1,
                    SourceOfFunds = result2,
                    ExpenseCategories = result3,
                    Vc2mList = result4,
                    PayMethods = result5,
                    ProjectLevels = result6,
                    ProjectPropers = result7,
                    TimeLimits = result8,
                    PayMethodTwos = result9,
                    Zcgnfls = result10,
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据组织以及类型获取不同的基础数据集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetSetListByOrgType([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织编码信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.DicType))
            {
                return DCHelper.ErrorMessage("基础数据类型不能为空！");
            }
            try
            {
                var result = this.QTSysSetService.GetSetListByOrgType(param.orgCode, param.DicType);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 获取申报部门集合
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetDeclareList([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.uCode))
            {
                return DCHelper.ErrorMessage("账号信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.orgCode) || string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            try
            {
                List<DeftDataModel> deftDatas = new List<DeftDataModel>();
                DeftDataModel deftData = new DeftDataModel();
                SavedResult<Int64> savedresult = new SavedResult<Int64>();
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                    .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", param.uCode));
                var orgCode = "";
                var dept = "";
                var orgName = "";
                var deptName = "";
                var orgCodeList = CorrespondenceSettingsService.Find(dicWhere);
                if (orgCodeList.Data.Count > 0)
                {
                    orgCode = orgCodeList.Data[0].Dydm;
                    dept = orgCodeList.Data[0].DefStr3;
                    orgName = CorrespondenceSettingsService.GetOrg(orgCode).OName;
                    deptName = CorrespondenceSettingsService.GetOrg(dept).OName;
                    var dicWhere1 = new Dictionary<string, object>();
                    new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("Dylx", "SB"))
                    .Add(ORMRestrictions<string>.Eq("Dwdm", orgCode));
                    var orgSbList = CorrespondenceSettings2Service.Find(dicWhere1);
                    if (orgSbList.Data.Count > 0) { }
                    else
                    {
                        orgCode = "";
                        dept = "";
                        orgName = "";
                        deptName = "";
                    }
                }
                if (orgCode.Equals(param.orgCode))
                {
                    deftData.DeftCode = dept;
                    deftData.DeftName = deptName;
                    deftDatas.Add(deftData);
                }
                else
                {
                    IList<OrganizeModel> organizes = CorrespondenceSettingsService.GetDeptByUnit(long.Parse(param.orgid), long.Parse(param.uCode));
                    if (organizes != null && organizes.Count > 0)
                    {
                        foreach (var org in organizes)
                        {
                            deftData.DeftCode = org.OCode;
                            deftData.DeftName = org.OName;
                            deftDatas.Add(deftData);
                        }
                    }
                }
                return DataConverterHelper.SerializeObject(deftDatas);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns>返回Json串</returns>
        //[HttpPost]
        //public string PostSaveBasic([FromBody]InfoBaseModel<List<QTSysSetModel>> SysSet)
        //{
        //    if(SysSet.infoData == null || SysSet.infoData.Count < 1)
        //    {
        //        return DCHelper.ErrorMessage("参数传递不正确！");
        //    }
        //    try
        //    {

        //    }
        //    catch(Exception ex)
        //    {
        //        return DCHelper.ErrorMessage(ex.Message);
        //    }
        //}

        #region //绩效相关的基础数据
        //获取绩效类型数型结构
        [HttpGet]
        public string GetTargetTypeTree([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            try
            {
                PerformEvalTargetTypeModel performEval = new PerformEvalTargetTypeModel();
                IList<PerformEvalTargetTypeModel> allPerforms = new List<PerformEvalTargetTypeModel>();
                allPerforms = this.PerformEvalTargetTypeService.Find(t => t.Orgid == long.Parse(param.orgid) && t.Orgcode == param.orgCode).Data;
                performEval.FName = "根节点";
                var result = GetTree(null, performEval, allPerforms);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取绩效类型的树结构
        /// </summary>
        /// <param name="root"></param>
        /// <param name="performs"></param>
        /// <returns></returns>
        public PerformEvalTargetTypeModel GetTree(string root, PerformEvalTargetTypeModel performs, IList<PerformEvalTargetTypeModel> allPerforms)
        {

            if (string.IsNullOrEmpty(root))
            {
                performs.Children = allPerforms.ToList().FindAll(t => t.FParentCode == null || t.FParentCode == "").OrderBy(t => t.FCode).ToList();
            }
            else
            {
                performs.Children = allPerforms.ToList().FindAll(t => t.FParentCode == root).OrderBy(t => t.FCode).ToList();
            }
            if (performs.Children != null && performs.Children.Count > 0)
            {
                foreach (var per in performs.Children)
                {
                    GetTree(per.FCode, per, allPerforms);
                }
            }
            return performs;
        }

        /// <summary>
        /// 根据指标类型获取指标明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPerformEvalTargets([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.TargetTypeCode))
            {
                return DCHelper.ErrorMessage("指标类型不能空！");
            }
            try
            {
                var result = this.PerformEvalTargetService.GetPerformEvalTargetList(param.TargetTypeCode, param.orgid, param.orgCode);
                var data = new
                {
                    Status = "success",
                    Msg = "获取数据成功!",
                    Data = result,
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 根据指标类型获取指标明细(指标类别有多层)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetPerformEvalTargets2([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.TargetTypeCode))
            {
                return DCHelper.ErrorMessage("指标类型不能空！");
            }
            try
            {
                var result = this.PerformEvalTargetService.GetPerformEvalTargetList2(param.TargetTypeCode, param.orgid, param.orgCode);
                var data = new
                {
                    Status = "success",
                    Msg = "获取数据成功!",
                    Data = result,
                };
                return DataConverterHelper.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存指标种类信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateTargetType([FromBody]BaseInfoModel<PerformEvalTargetTypeModel> param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            try
            {
                //获取该组织的所有明细数据
                var allPerformEvalTarget = this.PerformEvalTargetService.Find(t => t.Orgid == long.Parse(param.orgid)).Data;
                var result = this.PerformEvalTargetService.UpdateTargetType(param.infoData, param.orgid, param.orgCode, null, allPerformEvalTarget);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 修改指标明细数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateTargets([FromBody]BaseInfoModel<List<PerformEvalTargetModel>> param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            try
            {
                var result = this.PerformEvalTargetService.UpdateTargets(param.infoData, param.orgid, param.orgCode);
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取绩效基本数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetPerformEvals([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.PerformType))
            {
                return DCHelper.ErrorMessage("要获取的绩效数据类型不能空！");
            }
            try
            {
                object obj;
                if (param.PerformType == "1")
                {
                    obj = this.PerformEvalTypeService.GetPerformEvalTypes(param.orgid, param.orgCode);
                    var data = new
                    {
                        Status = "success",
                        Msg = "数据获取成功！",
                        Data = obj
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else if (param.PerformType == "2")
                {
                    obj = this.PerformEvalTargetClassService.GetPerformEvalTargetClasses(param.orgid, param.orgCode);
                    var data = new
                    {
                        Status = "success",
                        Msg = "数据获取成功！",
                        Data = obj
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else if (param.ProcurType == "4")
                {
                    object obj1, obj2;
                    obj1 = this.PerformEvalTypeService.GetPerformEvalTypes(param.orgid, param.orgCode);
                    obj2 = this.PerformEvalTargetClassService.GetPerformEvalTargetClasses(param.orgid, param.orgCode);
                    var data = new
                    {
                        Status = "success",
                        Msg = "数据获取成功！",
                        Types = obj1,
                        TargetClasses = obj2,
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else
                {
                    return DCHelper.ErrorMessage("要获取的绩效数据类型参数不正确！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存绩效基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string PostUpdatePerformEvals([FromBody]BaseInfoModel<object> param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.PerformType))
            {
                return DCHelper.ErrorMessage("要获取的绩效数据类型不能空！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                if (param.ProcurType == "1")
                {
                    IList<PerformEvalTargetClassModel> performEvalTargetClasses = (List<PerformEvalTargetClassModel>)param.infoData;
                    savedResult = this.PerformEvalTargetClassService.UpdatePerformEvalTargetClasses(performEvalTargetClasses, param.orgid, param.orgCode);
                }
                else if (param.ProcurType == "2")
                {
                    IList<PerformEvalTypeModel> performEvalTypes = (List<PerformEvalTypeModel>)param.infoData;
                    savedResult = this.PerformEvalTypeService.UpdatePerformEvalTypes(performEvalTypes, param.orgid, param.orgCode);
                }
                else
                {
                    return DCHelper.ErrorMessage("要获取的绩效数据类型参数传递有误！");
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion


        #region  //采购相关的基础数据接口

        /// <summary>
        /// 获取采购相关的列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAboutProcurements([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.ProcurType))
            {
                return DCHelper.ErrorMessage("要获取的采购数据类型不能空！");
            }
            try
            {
                object obj;
                if (param.ProcurType == "1")
                {
                    obj = this.ProcurementCatalogService.GetProcurementCatalogs(param.orgid, param.orgCode);
                    var data = new
                    {
                        Status = "success",
                        Msg = "数据获取成功！",
                        Data = obj
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else if (param.ProcurType == "2")
                {
                    obj = this.ProcurementProceduresService.GetProcurementProcedures(param.orgid, param.orgCode);
                    var data = new
                    {
                        Status = "success",
                        Msg = "数据获取成功！",
                        Data = obj
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else if (param.ProcurType == "3")
                {
                    obj = this.ProcurementTypeService.GetProcurementTypes(param.orgid, param.orgCode);
                    var data = new
                    {
                        Status = "success",
                        Msg = "数据获取成功！",
                        Data = obj
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else if (param.ProcurType == "4")
                {
                    object obj1, obj2, obj3;
                    obj1 = this.ProcurementCatalogService.GetProcurementCatalogs(param.orgid, param.orgCode);
                    obj2 = this.ProcurementProceduresService.GetProcurementProcedures(param.orgid, param.orgCode);
                    obj3 = this.ProcurementTypeService.GetProcurementTypes(param.orgid, param.orgCode);
                    var data = new
                    {
                        Status = "success",
                        Msg = "数据获取成功！",
                        Catalogs = obj1,
                        Procedures = obj2,
                        Types = obj3
                    };
                    return DataConverterHelper.SerializeObject(data);
                }
                else
                {
                    return DCHelper.ErrorMessage("要获取的采购数据类型参数传递有误！");
                }
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 保存采购基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateAboutProcurements([FromBody]BaseInfoModel<object> param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.ProcurType))
            {
                return DCHelper.ErrorMessage("要获取的采购数据类型不能空！");
            }
            try
            {
                SavedResult<long> savedResult = new SavedResult<long>();
                if (param.ProcurType == "1")
                {
                    IList<ProcurementCatalogModel> procurementCatalogs = (List<ProcurementCatalogModel>)param.infoData;
                    savedResult = this.ProcurementCatalogService.UpdateProcurementCatalogs(procurementCatalogs, param.orgid, param.orgCode);
                }
                else if (param.ProcurType == "2")
                {
                    IList<ProcurementProceduresModel> procurementProcedures = (List<ProcurementProceduresModel>)param.infoData;
                    savedResult = this.ProcurementProceduresService.UpdateProcurementProcedures(procurementProcedures, param.orgid, param.orgCode);
                }
                else if (param.ProcurType == "3")
                {
                    IList<ProcurementTypeModel> procurementTypes = (List<ProcurementTypeModel>)param.infoData;
                    savedResult = this.ProcurementTypeService.UpdateProcurementTypes(procurementTypes, param.orgid, param.orgCode);
                }
                else
                {
                    return DCHelper.ErrorMessage("要获取的采购数据类型参数传递有误！");
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }


        #endregion

        #region //支出功能分类
        /// <summary>
        /// 根据组织获取支出功能分裂列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetZcgnfls([FromUri]AllRelationModel param)
        {
            if (string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            if (string.IsNullOrEmpty(param.orgCode))
            {
                return DCHelper.ErrorMessage("组织信息不能空！");
            }
            try
            {
                var result = this.QtZcgnflService.GetZcgnfls(param.orgid, param.orgCode);
                var data = new
                {
                    Status = "success",
                    Msg = "数据获取成功！",
                    Data = result,
                };
                return DataConverterHelper.SerializeObject(data);
                //return "";
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        #region //组织是否启用工作流设置

        /// <summary>
        /// 不同用户启用工作流设置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateSetWorkFlowForOrgs(BaseInfoModel<List<QTIfApproveModel>> param)
        {
            if (param.infoData == null || param.infoData.Count < 1)
            {
                return DCHelper.ErrorMessage("参数传递不正确！");
            }
            try
            {
                if (string.IsNullOrEmpty(param.uid))
                {
                    return DCHelper.ErrorMessage("用户id为空！");
                }
                User2Model user = QTSysSetService.GetUser(long.Parse(param.uid));

                //获取所有组织集合
                List<OrganizeModel> allOrgs = new List<OrganizeModel>();
                allOrgs = this.QTSysSetService.GetAllOrgs();
                //获取所有未审批数据，用来判断是否可以停用工作流
                IList<GAppvalRecordModel> allAppvalRecords = new List<GAppvalRecordModel>();
                allAppvalRecords = this.GAppvalRecordService.Find(t => t.PhId >= (long)0 && t.FApproval == (byte)1).Data;
                //获取所有项目，预立项单据
                IList<ProjectMstModel> allProjectMsts = new List<ProjectMstModel>();
                allProjectMsts = this.ProjectMstService.Find(t => t.PhId >= (long)0).Data;
                //获取所有预算相关的数据
                IList<BudgetMstModel> allBudgetMsts = new List<BudgetMstModel>();
                allBudgetMsts = this.BudgetMstService.Find(t => t.PhId >= (long)0).Data;
                //获取用款计划相关数据
                IList<ExpenseMstModel> allExpenseMsts = new List<ExpenseMstModel>();
                allExpenseMsts = this.ExpenseMstService.Find(t => t.PhId >= (long)0).Data;

                //获取所有已存在的是否启用工作流的数据
                IList<QTIfApproveModel> allIfApproves = this.QTIfApproveService.Find(t => t.PhId != (long)0).Data;
                //新的要保存的集合
                List<QTIfApproveModel> newAllIfApproves = new List<QTIfApproveModel>();
                if (user.UserNo == "Admin")
                {
                    foreach (var approve in param.infoData)
                    {
                        //通过phid获取组织集合
                        if (approve.PhidList != null && approve.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        {
                            approve.OrgList = allOrgs.ToList().FindAll(t => approve.PhidList.Contains(t.PhId));
                        }
                        //该阶段所有使用工作流的对象(及所有运用该工作流的组织)
                        var approvesByCode = allIfApproves.ToList().FindAll(t => t.FCode == approve.FCode);
                        if (approve.OrgList != null && approve.OrgList.Count > 0)
                        {
                            foreach (var org in approve.OrgList)
                            {
                                //如果原集合有数据（但在传递的数据中没有），新的所有集合把这些数据加入删除标记
                                //如果原集合有数据（但在传递的数据中有），新的所有集合把这些数据加入修改标记
                                //如果原集合没有有数据（但在传递的数据中有），新的所有集合把这些数据加入新增标记
                                if (approvesByCode != null && approvesByCode.Count > 0)
                                {
                                    var approvesByCodeOrg = approvesByCode.Find(t => t.Orgcode == org.OCode && t.Orgid == org.PhId);
                                    if (approvesByCodeOrg != null)
                                    {
                                        approvesByCodeOrg.FCode = approve.FCode;
                                        approvesByCodeOrg.FName = approve.FName;
                                        approvesByCodeOrg.FIfuse = approve.FIfuse;
                                        approvesByCodeOrg.Bz = approve.Bz;
                                        approvesByCodeOrg.PersistentState = PersistentState.Modified;
                                        newAllIfApproves.Add(approvesByCodeOrg);
                                        approvesByCode.Remove(approvesByCodeOrg);
                                        //判断该组织下是否有走工作流的单据
                                        //if(approve.FIfuse == (byte)1 && allAppvalRecords != null && allAppvalRecords.Count > 0)
                                        //{
                                        //    if(allAppvalRecords.ToList().FindAll(t=>t.FBilltype == approvesByCodeOrg.FCode).Count > 0)
                                        //    {
                                        //        List<long> phids = new List<long>();
                                        //        if(approvesByCodeOrg.FCode == BillType.BeginProject)
                                        //        {
                                        //            phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == approvesByCodeOrg.FCode).Select(t => t.RefbillPhid).ToList();
                                        //            if(allProjectMsts.ToList().FindAll(t=>t.FProjStatus == 1 && phids.Contains(t.PhId)).Count > 0)
                                        //            {
                                        //                return DCHelper.ErrorMessage(org.OCode+"组织在预立项过程已存在走工作流的单据，因此不能修改工作流设置！");
                                        //            }
                                        //        }
                                        //        else if(approvesByCodeOrg.FCode == BillType.MiddleProject)
                                        //        {
                                        //            phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == approvesByCodeOrg.FCode).Select(t => t.RefbillPhid).ToList();
                                        //            if (allProjectMsts.ToList().FindAll(t => t.FProjStatus == 2 && phids.Contains(t.PhId)).Count > 0)
                                        //            {
                                        //                return DCHelper.ErrorMessage(org.OCode + "组织在立项过程已存在走工作流的单据，因此不能修改工作流设置！");
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        QTIfApproveModel qTIfApprove = new QTIfApproveModel();
                                        qTIfApprove.FCode = approve.FCode;
                                        qTIfApprove.FName = approve.FName;
                                        qTIfApprove.FIfuse = approve.FIfuse;
                                        qTIfApprove.Bz = approve.Bz;
                                        qTIfApprove.Orgid = org.PhId;
                                        qTIfApprove.Orgcode = org.OCode;
                                        qTIfApprove.PersistentState = PersistentState.Added;
                                        newAllIfApproves.Add(qTIfApprove);
                                    }
                                }
                                else
                                {
                                    QTIfApproveModel qTIfApprove = new QTIfApproveModel();
                                    qTIfApprove.FCode = approve.FCode;
                                    qTIfApprove.FName = approve.FName;
                                    qTIfApprove.FIfuse = approve.FIfuse;
                                    qTIfApprove.Bz = approve.Bz;
                                    qTIfApprove.Orgid = org.PhId;
                                    qTIfApprove.Orgcode = org.OCode;
                                    qTIfApprove.PersistentState = PersistentState.Added;
                                    newAllIfApproves.Add(qTIfApprove);
                                }
                            }
                            if (approvesByCode != null && approvesByCode.Count > 0)
                            {
                                foreach (var app in approvesByCode)
                                {
                                    app.PersistentState = PersistentState.Deleted;
                                    newAllIfApproves.Add(app);
                                }
                            }
                        }
                        else
                        {
                            if (approvesByCode != null && approvesByCode.Count > 0)
                            {
                                foreach (var app in approvesByCode)
                                {
                                    app.PersistentState = PersistentState.Deleted;
                                    newAllIfApproves.Add(app);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(param.orgid))
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    //查找该组织下的所有工作流
                    var performEvalsByOrg = allIfApproves.ToList().FindAll(x => x.Orgid == long.Parse(param.orgid));
                    foreach (QTIfApproveModel set in param.infoData)
                    {
                        //通过phid获取组织集合
                        //if (set.PhidList != null && set.PhidList.Count > 0 && allOrgs != null && allOrgs.Count > 0)
                        //{
                        //    set.OrgList = allOrgs.ToList().FindAll(t => set.PhidList.Contains(t.PhId));
                        //}
                        if (set.PhId == 0)
                        {
                            QTIfApproveModel b = new QTIfApproveModel();
                            //OrganizeModel organize = QTSysSetService.GetOrg(set.OrgList[0].PhId);
                            OrganizeModel organize = new OrganizeModel();
                            //if (set.OrgList != null && set.OrgList.Count > 0)
                            //{
                            //    organize = allOrgs.Find(t => t.PhId == long.Parse(param.orgid));
                            //}
                            //else
                            //{
                            //    return DCHelper.ErrorMessage("新增的配置项缺乏组织信息！");
                            //}
                            organize = allOrgs.Find(t => t.PhId == long.Parse(param.orgid));
                            set.Orgid = organize.PhId;
                            set.Orgcode = organize.OCode;
                            set.PersistentState = PersistentState.Added;
                            b.FCode = set.FCode;
                            b.FName = set.FName;
                            b.Bz = set.Bz;
                            b.Orgid = set.Orgid;
                            b.Orgcode = set.Orgcode;
                            b.FIfuse = set.FIfuse;
                            b.PersistentState = PersistentState.Added;
                            newAllIfApproves.Add(b);
                        }
                        else
                        {
                            var a = performEvalsByOrg.Find(x => x.PhId == set.PhId);
                            if (a != null)
                            {
                                if (set.PersistentState != PersistentState.Deleted)
                                {
                                    a.PersistentState = PersistentState.Modified;
                                    a.FCode = set.FCode;
                                    a.FName = set.FName;
                                    a.Bz = set.Bz;
                                    a.FIfuse = set.FIfuse;
                                    newAllIfApproves.Add(a);
                                }
                                else
                                {
                                    a.PersistentState = PersistentState.Deleted;
                                    newAllIfApproves.Add(a);
                                }
                            }
                            performEvalsByOrg.Remove(a);
                        }
                    }
                    if (performEvalsByOrg.Count > 0)
                    {
                        foreach (QTIfApproveModel z in performEvalsByOrg)
                        {
                            z.PersistentState = PersistentState.Deleted;
                            newAllIfApproves.Add(z);
                        }
                    }
                }
                //数据验证
                if (newAllIfApproves != null && newAllIfApproves.Count > 0)
                {
                    foreach (var newApp in newAllIfApproves)
                    {
                        if (newAllIfApproves.FindAll(t => t.Orgid == newApp.Orgid && t.FCode == newApp.FCode && t.PersistentState != PersistentState.Deleted).Count > 1)
                        {
                            return DCHelper.ErrorMessage(newApp.Orgcode + "组织下的" + newApp.FCode + "的数据存在重复！");
                        }

                        if ((newApp.PersistentState == PersistentState.Modified && (newApp.FIfuse == (byte)1 && allAppvalRecords != null && allAppvalRecords.Count > 0)) || (newApp.PersistentState == PersistentState.Deleted && (allAppvalRecords != null && allAppvalRecords.Count > 0)))
                        {
                            ////判断该组织下是否有走工作流的单据
                            //if ((newApp.FIfuse == (byte)1 && allAppvalRecords != null && allAppvalRecords.Count > 0) || (allAppvalRecords != null && allAppvalRecords.Count > 0))
                            //{

                            //}

                            if (allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Count > 0)
                            {
                                var orgname = allOrgs.Find(t => t.PhId == newApp.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == newApp.Orgid).OName;
                                List<long> phids = new List<long>();
                                if (newApp.FCode == BillType.BeginProject)
                                {
                                    phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).Distinct().ToList();
                                    if (phids != null && phids.Count > 0 && allProjectMsts.ToList().FindAll(t => t.FProjStatus == 1 && phids.Contains(t.PhId) && t.FDeclarationUnit == newApp.Orgcode).Count > 0)
                                    //if (phids != null && phids.Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(orgname + "组织在预立项过程已存在走工作流的单据，因此不能删除以及修改工作流设置！");
                                    }
                                }
                                else if (newApp.FCode == BillType.MiddleProject)
                                {
                                    phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).Distinct().ToList();
                                    if (phids != null && phids.Count > 0 && allProjectMsts.ToList().FindAll(t => t.FProjStatus == 2 && phids.Contains(t.PhId) && t.FDeclarationUnit == newApp.Orgcode).Count > 0)
                                    //if (phids != null && phids.Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(orgname + "组织在立项过程已存在走工作流的单据，因此不能删除以及修改工作流设置！");
                                    }
                                }
                                else if (newApp.FCode == BillType.MiddleBudget || newApp.FCode == BillType.MiddleAddBudget || newApp.FCode == BillType.MiddleUpdateBudget || newApp.FCode == BillType.MiddleDtlBudget)
                                {
                                    phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).Distinct().ToList();
                                    if (phids != null && phids.Count > 0 && allBudgetMsts.ToList().FindAll(t => phids.Contains(t.PhId) && t.FDeclarationUnit == newApp.Orgcode).Count > 0)
                                    //if (phids != null && phids.Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(orgname + "组织在年中调整过程已存在走工作流的单据，因此不能删除以及修改工作流设置！");
                                    }
                                }
                                else if (newApp.FCode == BillType.Expense)
                                {
                                    phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).Distinct().ToList();
                                    if (phids != null && phids.Count > 0 && allExpenseMsts.ToList().FindAll(t => phids.Contains(t.PhId) && t.FDeclarationunit == newApp.Orgcode).Count > 0)
                                    //if (phids != null && phids.Count > 0)
                                    {
                                        return DCHelper.ErrorMessage(orgname + "组织在用款计划过程已存在走工作流的单据，因此不能删除以及修改工作流设置！");
                                    }
                                }
                            }
                        }
                        #region//把删除与修改的判断写一起
                        //else if (newApp.PersistentState == PersistentState.Deleted)
                        //{
                        //    //判断该组织下是否有走工作流的单据
                        //    if (allAppvalRecords != null && allAppvalRecords.Count > 0)
                        //    {
                        //        if (allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Count > 0)
                        //        {
                        //            var orgname = allOrgs.Find(t => t.PhId == newApp.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == newApp.Orgid).OName;
                        //            List<long> phids = new List<long>();
                        //            if (newApp.FCode == BillType.BeginProject)
                        //            {
                        //                phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).ToList();
                        //                if (phids != null && phids.Count > 0 && allProjectMsts.ToList().FindAll(t => t.FProjStatus == 1 && phids.Contains(t.PhId)).Count > 0)
                        //                {
                        //                    return DCHelper.ErrorMessage(orgname + "组织在预立项过程已存在走工作流的单据，因此不能删除工作流设置！");
                        //                }
                        //            }
                        //            else if (newApp.FCode == BillType.MiddleProject)
                        //            {
                        //                phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).ToList();
                        //                if (phids != null && phids.Count > 0 && allProjectMsts.ToList().FindAll(t => t.FProjStatus == 2 && phids.Contains(t.PhId)).Count > 0)
                        //                {
                        //                    return DCHelper.ErrorMessage(orgname + "组织在立项过程已存在走工作流的单据，因此不能删除工作流设置！");
                        //                }
                        //            }
                        //            else if (newApp.FCode == BillType.MiddleBudget || newApp.FCode == BillType.MiddleAddBudget || newApp.FCode == BillType.MiddleUpdateBudget || newApp.FCode == BillType.MiddleDtlBudget)
                        //            {
                        //                phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).ToList();
                        //                if (phids != null && phids.Count > 0 && allBudgetMsts.ToList().FindAll(t => phids.Contains(t.PhId)).Count > 0)
                        //                {
                        //                    return DCHelper.ErrorMessage(orgname + "组织在年中调整过程已存在走工作流的单据，因此不能删除工作流设置！");
                        //                }
                        //            }
                        //            else if (newApp.FCode == BillType.Expense)
                        //            {
                        //                phids = allAppvalRecords.ToList().FindAll(t => t.FBilltype == newApp.FCode).Select(t => t.RefbillPhid).ToList();
                        //                if (phids != null && phids.Count > 0 && allExpenseMsts.ToList().FindAll(t => phids.Contains(t.PhId)).Count > 0)
                        //                {
                        //                    return DCHelper.ErrorMessage(orgname + "组织在用款计划过程已存在走工作流的单据，因此不能删除工作流设置！");
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                }
                var result = this.QTIfApproveService.Save<long>(newAllIfApproves, "");
                return DataConverterHelper.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 不同用户获取所有工作流设置
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetAllWorkFlows([FromUri]BaseListModel baseModel)
        {
            if (string.IsNullOrEmpty(baseModel.uid))
            {
                return DCHelper.ErrorMessage("用户id为空！");
            }
            try
            {
                //用户信息
                User2Model user = QTSysSetService.GetUser(long.Parse(baseModel.uid));
                //返回的集合
                List<QTIfApproveModel> approveModels = new List<QTIfApproveModel>();
                if (user.UserNo == "Admin")
                {
                    var data = this.QTIfApproveService.Find(t => t.PhId != 0).Data.ToList();
                    if (data != null && data.Count > 0)
                    {
                        List<string> typeCodeList = data.Select(x => x.FCode).Distinct().ToList();//取所有支付方式的code集合
                        foreach (string typedm in typeCodeList)
                        {
                            var approveByCode = data.FindAll(x => x.FCode == typedm);
                            QTIfApproveModel qTIfApprove = new QTIfApproveModel();
                            qTIfApprove.FCode = typedm;
                            qTIfApprove.FName = approveByCode[0].FName;
                            qTIfApprove.FIfuse = approveByCode[0].FIfuse;
                            qTIfApprove.Bz = approveByCode[0].Bz;
                            qTIfApprove.OrgList = new List<OrganizeModel>();
                            foreach (QTIfApproveModel a in approveByCode)
                            {
                                if (a.Orgid != 0)
                                {
                                    qTIfApprove.OrgList.Add(QTSysSetService.GetOrg(a.Orgid));
                                }
                            }
                            approveModels.Add(qTIfApprove);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(baseModel.orgid))
                    {
                        approveModels = this.QTIfApproveService.Find(t => t.Orgid == long.Parse(baseModel.orgid)).Data.ToList();
                    }
                    else
                    {
                        return DCHelper.ErrorMessage("组织id为空！");
                    }
                    foreach (QTIfApproveModel b in approveModels)
                    {
                        b.OrgList = new List<OrganizeModel>();
                        b.OrgList.Add(QTSysSetService.GetOrg(b.Orgid));
                    }
                }
                var newData = new
                {
                    Status = "success",
                    Msg = "获取数据成功!",
                    Data = approveModels,
                };
                return DataConverterHelper.SerializeObject(newData);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 获取过程是否启用工作流
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetWorkFlow([FromUri]BaseListModel param)
        {
            if (string.IsNullOrEmpty(param.orgCode) || string.IsNullOrEmpty(param.orgid))
            {
                return DCHelper.ErrorMessage("组织信息不能为空！");
            }
            if (string.IsNullOrEmpty(param.workType))
            {
                return DCHelper.ErrorMessage("工作流信息不能为空！");
            }
            try
            {
                int flam = 0;
                if (param.workType == "006")
                {
                    var result = this.QTIfApproveService.Find(t => t.Orgcode == param.orgCode && t.Orgid == long.Parse(param.orgid) && (t.FCode == BillType.MiddleBudget || t.FCode == BillType.MiddleAddBudget || t.FCode == BillType.MiddleDtlBudget || t.FCode == BillType.MiddleUpdateBudget)).Data;
                    if (result != null && result.Count > 0)
                    {
                        foreach (var res in result)
                        {
                            if (res.FIfuse == (byte)1)
                            {
                                flam = 0;
                            }
                            else
                            {
                                flam = 1;
                            }
                        }
                    }
                }
                else
                {
                    var result = this.QTIfApproveService.Find(t => t.Orgcode == param.orgCode && t.Orgid == long.Parse(param.orgid) && t.FCode == param.workType).Data;
                    if (result != null && result.Count > 0)
                    {
                        if (result[0].FIfuse == (byte)1)
                        {
                            flam = 0;
                        }
                        else
                        {
                            flam = 1;
                        }
                    }
                }
                var Data = new
                {
                    Status = "success",
                    Msg = "获取数据成功!",
                    Data = flam,
                };
                return DataConverterHelper.SerializeObject(Data);
            }
            catch (Exception ex)
            {
                return DCHelper.ErrorMessage(ex.Message);
            }
        }
        #endregion

        #region//编码设置相关接口
        /// <summary>
        /// 根据组织获取编码数据
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetCodingRule([FromUri]string orgid)
        {
            var dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "CodingRule"))
                .Add(ORMRestrictions<string>.Eq("Dydm", orgid));
            var Query = CorrespondenceSettingsService.Find(dic, new string[] { "DefInt1" }).Data;
            if (Query != null && Query.Count > 0)
            {

            }
            else
            {
                //初始化
                var nameList = new List<string>() { "年度", "月份", "日期", "组织编码", "部门编码", "四位流水号" };
                var codeList = new List<string>() { "Year", "Month", "Date", "OrgCode", "DeptCode", "Num" };
                OrganizeModel organize = QTSysSetService.GetOrg(long.Parse(orgid));
                for (var i = 0; i < nameList.Count; i++)
                {
                    CorrespondenceSettingsModel a = new CorrespondenceSettingsModel();
                    a.Dwdm = organize.OCode;
                    a.Dydm = orgid;
                    a.Dylx = "CodingRule";
                    a.DefStr1 = nameList[i];
                    a.DefStr2 = codeList[i];
                    a.DefInt1 = i;//排序
                    a.DefInt2 = 0;//是否启用（1：启用）
                    Query.Add(a);
                }

            }
            var result = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功!",
                data = Query
            };
            return DataConverterHelper.SerializeObject(result);
        }

        /// <summary>
        /// 编码数据保存
        /// </summary>
        /// <param name="coding"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostCodingRule([FromBody]CodingResultModel coding)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (coding.list != null)
            {
                try
                {
                    var data = new List<CorrespondenceSettingsModel>();
                    var dic = new Dictionary<string, object>();
                    new CreateCriteria(dic)
                        .Add(ORMRestrictions<string>.Eq("Dylx", "CodingRule"))
                        .Add(ORMRestrictions<string>.Eq("Dydm", coding.orgid));
                    var Query = CorrespondenceSettingsService.Find(dic, new string[] { "DefInt1" }).Data;
                    if (Query != null && Query.Count > 0)
                    {
                        foreach (var a in Query)
                        {
                            a.PersistentState = PersistentState.Deleted;
                            data.Add(a);
                        }
                    }
                    foreach (var item in coding.list)
                    {
                        item.PersistentState = PersistentState.Added;
                        data.Add(item);
                    }
                    savedResult = CorrespondenceSettingsService.Save<long>(data, "");
                }
                catch (Exception ex)
                {
                    return DCHelper.ErrorMessage(ex.Message);
                }
                return DataConverterHelper.SerializeObject(savedResult);
            }
            else
            {
                return DCHelper.ErrorMessage("数据列表获取失败");
            }

            #region MyRegion
            //if (SysSet.infoData == null || SysSet.infoData.Count <= 0)
            //{
            //    return DCHelper.ErrorMessage("数据传递不正确！");
            //}
            //SavedResult<Int64> savedresult = new SavedResult<Int64>();
            //List<QTSysSetModel> resultSysSet = new List<QTSysSetModel>();

            ////获取所有组织集合
            //List<OrganizeModel> allOrgs = new List<OrganizeModel>();
            //allOrgs = this.QTSysSetService.GetAllOrgs();
            //if (SysSet.infoData[0].DicType == "PayMethod")//编码的标识类型
            //{
            //    if (string.IsNullOrEmpty(SysSet.uid))
            //    {
            //        return DCHelper.ErrorMessage("用户id为空！");
            //    }
            //    User2Model user = QTSysSetService.GetUser(long.Parse(SysSet.uid));

            //    Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            //    new CreateCriteria(dicWhere)
            //        .Add(ORMRestrictions<string>.Eq("DicType", "PayMethod"));

            //    var PayMethods = QTSysSetService.Find(dicWhere, new string[] { "TypeCode Desc" }).Data;

            //    //存原来有的PayMethodTwo编码现在没有了的集合
            //    IList<QTSysSetModel> allSysSetsNot = new List<QTSysSetModel>();

            //    allSysSetsNot = PayMethods;
            //    //var TypeCode = 0;
            //    //if (PayMethods.Count > 0)
            //    //{
            //    //    TypeCode = int.Parse(PayMethods[0].TypeCode);
            //    //}
            //    if (user.UserNo == "Admin")
            //    {
            //        PayMethods = PayMethods.ToList().FindAll(x => x.Issystem == 1);
            //        allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.Issystem == 1);
            //        foreach (QTSysSetModel set in SysSet.infoData)
            //        {
            //            //if (string.IsNullOrEmpty(set.TypeCode))
            //            //{
            //            //    TypeCode++;
            //            //    set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
            //            //}
            //            var PayMethodsByTypecode = PayMethods.ToList().FindAll(x => x.TypeCode == set.TypeCode);

            //            allSysSetsNot = allSysSetsNot.ToList().FindAll(t => t.TypeCode != set.TypeCode);
            //            if (set.OrgList.Count > 0)
            //            {
            //                foreach (OrganizeModel org in set.OrgList)
            //                {
            //                    //如果存在就删除，最后剩下的是要删除的
            //                    var PayMethodsByTypecodeOrg = PayMethodsByTypecode.FindAll(x => x.Orgid == org.PhId);
            //                    if (PayMethodsByTypecodeOrg.Count > 0)
            //                    {
            //                        QTSysSetModel a = PayMethodsByTypecodeOrg[0];
            //                        if (a.TypeName != set.TypeName || a.Isactive != set.Isactive || a.Bz != set.Bz)
            //                        {
            //                            a.Isactive = set.Isactive;
            //                            a.TypeName = set.TypeName;
            //                            a.Bz = set.Bz;
            //                            a.Issystem = set.Issystem;
            //                            a.PersistentState = PersistentState.Modified;
            //                            resultSysSet.Add(a);
            //                        }
            //                        PayMethodsByTypecode.Remove(PayMethodsByTypecodeOrg[0]);
            //                    }
            //                    else
            //                    {
            //                        QTSysSetModel b = new QTSysSetModel();
            //                        b.DicType = "PayMethod";
            //                        b.DicName = "支付方式";
            //                        b.TypeCode = set.TypeCode;
            //                        b.TypeName = set.TypeName;
            //                        b.Bz = set.Bz;
            //                        b.Orgid = org.PhId;
            //                        b.Orgcode = org.OCode;
            //                        b.PersistentState = PersistentState.Added;
            //                        b.Isactive = set.Isactive;
            //                        b.Issystem = 1;
            //                        resultSysSet.Add(b);
            //                    }
            //                }
            //                if (PayMethodsByTypecode.Count > 0)
            //                {
            //                    foreach (QTSysSetModel delete in PayMethodsByTypecode)
            //                    {
            //                        delete.PersistentState = PersistentState.Deleted;
            //                        resultSysSet.Add(delete);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                foreach (QTSysSetModel z in PayMethodsByTypecode)
            //                {
            //                    z.PersistentState = PersistentState.Deleted;
            //                    resultSysSet.Add(z);
            //                }
            //            }


            //        }

            //        //删除原有的现无 的数据
            //        if (allSysSetsNot != null && allSysSetsNot.Count > 0)
            //        {
            //            foreach (QTSysSetModel z in allSysSetsNot)
            //            {
            //                z.PersistentState = PersistentState.Deleted;
            //                resultSysSet.Add(z);
            //            }
            //        }

            //        //数据验证
            //        if (resultSysSet != null && resultSysSet.Count > 0)
            //        {
            //            foreach (var pro in resultSysSet)
            //            {
            //                var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
            //                if (pro.PersistentState != PersistentState.Deleted)
            //                {
            //                    if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
            //                    {
            //                        return DCHelper.ErrorMessage(orgname + "该组织下的支付方式编码重复，请进行修改！");
            //                    }

            //                    if (pro.Issystem == (byte)1)
            //                    {
            //                        if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)1).Count > 0)
            //                        {
            //                            return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (resultSysSet.FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem != (byte)0).Count > 0)
            //                        {
            //                            return DCHelper.ErrorMessage(pro.TypeCode + "此编码不能同时存在私有与公有之中！");
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(SysSet.orgid))
            //        {
            //            return DCHelper.ErrorMessage("组织id为空！");
            //        }
            //        //查找该组织的所有支付方式
            //        var PayMethodsByOrg = PayMethods.ToList().FindAll(x => x.Orgid == long.Parse(SysSet.orgid));
            //        foreach (QTSysSetModel set in SysSet.infoData)
            //        {
            //            if (set.PhId == 0)
            //            {
            //                //TypeCode++;
            //                //set.TypeCode = ("000" + TypeCode).Substring(("000" + TypeCode).Length - 3);
            //                OrganizeModel organize = QTSysSetService.GetOrg(set.OrgList[0].PhId);
            //                set.Orgid = organize.PhId;
            //                set.Orgcode = organize.OCode;
            //                set.PersistentState = PersistentState.Added;
            //                resultSysSet.Add(set);
            //            }
            //            else
            //            {
            //                var a = PayMethodsByOrg.Find(x => x.PhId == set.PhId);
            //                if (a != null)
            //                {
            //                    if (a.Issystem == (byte)1)
            //                    {
            //                        if (set.PersistentState == PersistentState.Deleted || (a.TypeName != set.TypeName || a.Bz != set.Bz || a.Isactive != set.Isactive || a.TypeCode != set.TypeCode))
            //                        {
            //                            return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
            //                        }
            //                        //return DCHelper.ErrorMessage(a.TypeName + "为公有设置，不能进行修改或者删除！");
            //                    }
            //                    if (set.PersistentState != PersistentState.Deleted)
            //                    {
            //                        set.PersistentState = PersistentState.Modified;
            //                        resultSysSet.Add(set);
            //                    }
            //                    else
            //                    {
            //                        resultSysSet.Add(set);
            //                    }
            //                    PayMethodsByOrg.Remove(a);
            //                }
            //            }
            //        }
            //        if (PayMethodsByOrg.Count > 0)
            //        {
            //            foreach (QTSysSetModel z in PayMethodsByOrg)
            //            {
            //                if (z.Issystem == (byte)1)
            //                {
            //                    return DCHelper.ErrorMessage(z.TypeCode + "该基本数据为公有数据，你无权删除！");
            //                }
            //                z.PersistentState = PersistentState.Deleted;
            //                resultSysSet.Add(z);
            //            }
            //        }
            //        //数据验证
            //        if (resultSysSet != null && resultSysSet.Count > 0)
            //        {
            //            foreach (var pro in resultSysSet)
            //            {
            //                var orgname = allOrgs.Find(t => t.PhId == pro.Orgid) == null ? "" : allOrgs.Find(t => t.PhId == pro.Orgid).OName;
            //                if (pro.PersistentState != PersistentState.Deleted)
            //                {
            //                    if (resultSysSet.FindAll(t => t.Orgid == pro.Orgid && t.TypeCode == pro.TypeCode && t.PersistentState != PersistentState.Deleted).Count > 1)
            //                    {
            //                        return DCHelper.ErrorMessage(orgname + "该组织下的支付方式编码重复，请进行修改！");
            //                    }

            //                    if (PayMethods.ToList().FindAll(t => t.TypeCode == pro.TypeCode && t.Issystem == (byte)1).Count > 0)
            //                    {
            //                        return DCHelper.ErrorMessage(pro.TypeCode + "该编码已经被公有化，不能被私有所有！");
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    try
            //    {
            //        savedresult = QTSysSetService.Save<Int64>(resultSysSet, "");
            //    }
            //    catch (Exception ex)
            //    {
            //        savedresult.Status = ResponseStatus.Error;
            //        savedresult.Msg = ex.Message.ToString();
            //    }
            //}
            //return "";
            #endregion

        }
        #endregion


        /// <summary>
        /// 根据组织代码和数据类型取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetQtByOrg([FromUri]string orgCode, [FromUri]string DicType)
        {
            var result = QTSysSetService.Find(x => x.DicType == DicType && x.Orgcode == orgCode && x.Isactive == 0).Data.ToList();
            if (result != null && result.Count > 0)
            {
                return DCHelper.ModelListToJson<QTSysSetModel>(result, result.Count);
            }
            else
            {

                return DCHelper.ModelListToJson<QTSysSetModel>(new List<QTSysSetModel>(), 0);
            }
        }
    }
}
