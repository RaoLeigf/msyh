using Enterprise3.Common.Model.Results;
using Enterprise3.WebApi.ApiControllerBase;
using Enterprise3.WebApi.GQT3.QT.Model.Request;
using Enterprise3.WebApi.GQT3.QT.Model.Response;
using GQT3.QT.Model.Domain;
using GQT3.QT.Service.Interface;
using GXM3.XM.Service.Interface;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class QtXmDistributeApiController : ApiBase
    {
        IQtXmDistributeService QtXmDistributeService { get; set; }
        ICorrespondenceSettingsService CorrespondenceSettingsService { get; set; }
        IQTSysSetService QTSysSetService { get; set; }

        IProjectMstService ProjectMstService;
        /// <summary>
        /// 构造函数
        /// </summary>
        public QtXmDistributeApiController()
        {
            QtXmDistributeService = base.GetObject<IQtXmDistributeService>("GQT3.QT.Service.QtXmDistribute");
            CorrespondenceSettingsService = base.GetObject<ICorrespondenceSettingsService>("GQT3.QT.Service.CorrespondenceSettings");
            QTSysSetService = base.GetObject<IQTSysSetService>("GQT3.QT.Service.QTSysSet");
            ProjectMstService = base.GetObject<IProjectMstService>("GXM3.XM.Service.ProjectMst");
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <param name="OrgPhid"></param>
        /// <param name="FProjname">项目名称(模糊查询)</param>
        /// <param name="FProjcodeOrder">项目编号排序(0 不排序 1正序 2倒序)</param>
        /// <param name="FBusinessOrder">业务条线排序(0 不排序 1正序 2倒序)</param>
        /// <param name="Sort">0 先排编号  1 先排业条线</param>
        /// <returns></returns>
        [HttpGet]
        public string GetQtXmDistributeList([FromUri]long OrgPhid,
            [FromUri]string FProjname,
            [FromUri]int FProjcodeOrder,
            [FromUri]int FBusinessOrder,
            [FromUri]int Sort)
        {
            
            if (OrgPhid == 0)
            {
                return DCHelper.ErrorMessage("组织ID为空！");
            }
            List<XmDistributeModel> result = new List<XmDistributeModel>();

            if (FProjname == null)
            {
                FProjname = string.Empty;
            }
           
            var syssets = QTSysSetService.Find(x => x.DicType == "Business" && x.Orgid == OrgPhid).Data.ToList();
           
            //有权限修改的项目数据
            var data1 = QtXmDistributeService.Find(x => x.Distributeorgid == OrgPhid && x.FProjname.Contains(FProjname), "FProjcode").Data.ToList();
            var orgList = new List<long>();
            if (data1 != null && data1.Count > 0)
            {
                var data2 = new List<QtXmDistributeModel>();
                var FProjcodeList1 = data1.Select(x => x.FProjcode).Distinct().ToList();
                foreach (var code in FProjcodeList1)
                {
                    data2 = data1.FindAll(x => x.FProjcode == code);
                    XmDistributeModel a = new XmDistributeModel
                    {
                        PhId= data2[0].PhId,
                        Orgcode= data2[0].Orgcode,
                        CurOrgId = data2.First(p => p.FProjcode == code).CurOrgId,
                        CanFF = true,
                        FProjcode = code,
                        FProjname = data2[0].FProjname,
                        FBusiness = data2[0].FBusiness,
                        IfUse = data2.Find(p => p.Orgid == OrgPhid).IfUse
                };
                    if (!string.IsNullOrEmpty(a.FBusiness) && syssets != null)
                    {
                        if (syssets.Find(x => x.TypeCode == a.FBusiness) != null)
                        {
                            a.FBusiness_EXName = syssets.Find(x => x.TypeCode == a.FBusiness).TypeName;
                        }
                    }
                    a.EnableOrgList = data2.OrderBy(x => x.Orgcode).Select(x => x.Orgid).ToList();
                    if (a.EnableOrgList != null && a.EnableOrgList.Count > 0)
                    {
                        a.EnableOrgList2 = new List<object>();
                        foreach (var o in a.EnableOrgList)
                        {
                            if (o == OrgPhid)
                            {
                                a.EnableOrgList2.Add(new { phid = o, disabled = true });
                            }
                            else
                            {
                                var disabled = data2.Find(x => x.Orgid == o).Isactive == 0 ? false : true;
                                a.EnableOrgList2.Add(new { phid = o, disabled = disabled });
                            }
                        }
                        //a.EnableOrgStr = CorrespondenceSettingsService.GetOrgStr(a.EnableOrgList);
                        orgList.AddRange(a.EnableOrgList);
                    }
                    if (data2.FindAll(x => x.Isactive == 1).Count > 0)
                    {
                        a.CanUpdate = false;
                    }
                    else
                    {
                        a.CanUpdate = true;
                    }
                    
                    result.Add(a);
                }
            }
            
            //取没分发权限的
            var data3 = QtXmDistributeService.Find(x => x.Distributeorgid != OrgPhid && x.Orgid == OrgPhid && x.FProjname.Contains(FProjname)).Data.ToList();

            if (data3 != null && data3.Count > 0)
            {
                var data4 = new List<QtXmDistributeModel>();
                var FProjcodeList2 = data3.Select(x => x.FProjcode).Distinct().ToList();
                foreach (var code in FProjcodeList2)
                {
                    data4 = QtXmDistributeService.Find(x => x.FProjcode == code).Data.ToList();
                    XmDistributeModel b = new XmDistributeModel
                    {
                        PhId = data4[0].PhId,
                        Orgcode = data4[0].Orgcode,
                        CurOrgId = data3.First(p => p.FProjcode == code).CurOrgId,
                        CanFF = false,
                        FProjcode = code,
                        FProjname = data4[0].FProjname,
                        FBusiness = data4[0].FBusiness,
                        IfUse = data3.Find(p => p.Orgid == OrgPhid).IfUse
                    };
                    if (!string.IsNullOrEmpty(b.FBusiness) && syssets != null)
                    {
                        if (syssets.Find(x => x.TypeCode == b.FBusiness) != null)
                        {
                            b.FBusiness_EXName = syssets.Find(x => x.TypeCode == b.FBusiness).TypeName;
                        }
                    }
                    b.EnableOrgList = new List<long>();
                    b.EnableOrgList.Add(OrgPhid);
                    /*b.EnableOrgList = data4.OrderBy(x => x.Orgcode).Select(x => x.Orgid).ToList();
                    if (b.EnableOrgList != null && b.EnableOrgList.Count > 0)
                    {
                        b.EnableOrgList2 = new List<object>();
                        foreach (var o in b.EnableOrgList)
                        {
                            var disabled = data4.Find(x => x.Orgid == o).Isactive == 0 ? false : true;
                            b.EnableOrgList2.Add(new { phid = o, disabled = disabled });
                        }
                        //b.EnableOrgList = data4.OrderBy(x => x.Orgcode).Select(x => x.Orgid).ToList();
                        //b.EnableOrgStr = CorrespondenceSettingsService.GetOrgStr(b.EnableOrgList);
                        orgList.AddRange(b.EnableOrgList);
                    }*/

                    //b.CanUpdate = false;
                    b.CanUpdate = data4[0].Isactive == 1 ? false : true;

                    result.Add(b);
                }
            }
            orgList = orgList.Distinct().ToList();
            var EnableOrgInfo = CorrespondenceSettingsService.GetOrgInfo(orgList);
            foreach(var r in result)
            {
                r.EnableOrgStr = string.Join(",", EnableOrgInfo.Where(p => r.EnableOrgList.Contains(p.PhId))
                    .OrderBy(p => p.OCode)
                    .Select(p => p.OName)); 
            }
            //先排编号
            if (Sort == 0)
            {
                if (FProjcodeOrder == 1)
                {
                    result = result.OrderBy(x => x.FProjcode).ToList();
                }
                else if (FProjcodeOrder == 2)
                {
                    result = result.OrderByDescending(x => x.FProjcode).ToList();
                }

                if (FBusinessOrder == 1)
                {
                    result = result.OrderBy(x => x.FBusiness).ToList();
                }
                else if (FBusinessOrder == 2)
                {
                    result = result.OrderByDescending(x => x.FBusiness).ToList();
                }
            }
            //先排业务条线
            else if (Sort == 1)
            {
                if (FBusinessOrder == 1)
                {
                    result = result.OrderBy(x => x.FBusiness).ToList();
                }
                else if (FBusinessOrder == 2)
                {
                    result = result.OrderByDescending(x => x.FBusiness).ToList();
                }

                if (FProjcodeOrder == 1)
                {
                    result = result.OrderBy(x => x.FProjcode).ToList();
                }
                else if (FProjcodeOrder == 2)
                {
                    result = result.OrderByDescending(x => x.FProjcode).ToList();
                }
            }
            

            //result = result.OrderBy(x => x.FProjcode).ToList();
            return DCHelper.ModelListToJson<XmDistributeModel>(result, (Int32)result.Count); ;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostAdd([FromBody]XmDistributeAddModel data)
        {
            if (string.IsNullOrEmpty(data.Year))
            {
                return DCHelper.ErrorMessage("年度为空！");
            }
            if (string.IsNullOrEmpty(data.Orgcode))
            {
                return DCHelper.ErrorMessage("组织编码为空！");
            }
            if (data.Orgid == 0)
            {
                return DCHelper.ErrorMessage("组织id为空！");
            }
            if (data.userid == 0)
            {
                return DCHelper.ErrorMessage("操作员id为空！");
            }

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            List<QtXmDistributeModel> modelList = new List<QtXmDistributeModel>();
            if (data.data != null && data.data.Count > 0)
            {
                data.data.Reverse();
                for (var i = 0; i < data.data.Count; i++)
                {
                    QtXmDistributeModel model = new QtXmDistributeModel
                    {
                        FProjcode = QtXmDistributeService.CreateOrGetMaxProjCode(data.Year),
                        FProjname = data.data[i].FProjname,
                        FBusiness = data.data[i].FBusiness,
                        Orgid = data.Orgid,
                        Orgcode = data.Orgcode,
                        Distributeorgid = data.Orgid,
                        Distributeuserid = data.userid,
                        PersistentState = PersistentState.Added,
                        //IfUse = data.IfUse ? (byte)1 : (byte)0
                        IfUse = data.data[i].IfUse
                    };
                    modelList.Add(model);
                }
            }
            savedresult = QtXmDistributeService.Save<Int64>(modelList, "");
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 项目分发
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostXmFF([FromBody]XmDistributeModel data)
        {
            //改成取新建时默认的那条数据（即分发组织分发给自己的单据）
            var selectdata = QtXmDistributeService.Find(x => x.FProjcode == data.FProjcode && x.Distributeorgid==x.Orgid).Data.ToList();
            if (selectdata==null ||selectdata.Count == 0 || selectdata.Count>1)
            {
                return DCHelper.ErrorMessage("该项目数据有问题！");
            }
            //判断是否有权限操作
            var orgList = CorrespondenceSettingsService.GetAuthOrgList(data.userid)?.Select(p => p.PhId)?.ToList() ?? new List<long>();

            //全部删除情况
            if (data.EnableOrgList == null
                || data.EnableOrgList.Count == 0)
            {
                return DCHelper.ErrorMessage("默认组织不能被删除！");
            }

            //没有任何权限
            if (orgList == null
                || orgList.Count() == 0)
            {
                return DCHelper.ErrorMessage("没有权限修改！");
            }
            //没有修改的权限
            if (data.EnableOrgList.Except(orgList).Count() > 0)
            {
                return DCHelper.ErrorMessage("没有权限修改！");
            }
            //默认组织不能删除
            if (!data.EnableOrgList.Exists(p => p == selectdata.First().Distributeorgid))
            {
                return DCHelper.ErrorMessage("默认组织不能被删除");
            }
            var orglist = new List<Int64>();
            var rundata = new List<QtXmDistributeModel>();
            if (selectdata != null && selectdata.Count > 0)
            {
                //既然能选到 数据库必有数据
                var data1 = QtXmDistributeService.Find(x => x.FProjcode == data.FProjcode).Data.ToList();
                orglist = (data1 != null && data1.Count > 0) ? data1.Select(x => x.Orgid).ToList() : new List<long>();
            }
            var AddOrg = (data.EnableOrgList != null && data.EnableOrgList.Count > 0) ? data.EnableOrgList.Except(orglist).ToList() : null;
            var deleteOrg = (data.EnableOrgList != null && data.EnableOrgList.Count > 0) ? orglist.Except(data.EnableOrgList).ToList() : orglist;

            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            if (deleteOrg != null && deleteOrg.Count > 0)
            {
                rundata = QtXmDistributeService.Find(x => x.FProjcode == data.FProjcode && deleteOrg.Contains(x.Orgid)).Data.ToList();
                foreach (var b in rundata)
                {
                    b.PersistentState = PersistentState.Deleted;
                }
            }
            if (AddOrg != null && AddOrg.Count > 0)
            {
                //取存在这个业务代码的所有组织
                if (!string.IsNullOrEmpty(selectdata[0].FBusiness))
                {
                    var syssetOrgs = QTSysSetService.Find(x => x.DicType == "Business" && x.TypeCode == selectdata[0].FBusiness).Data.Select(x => x.Orgid).ToList();
                    var ExceptOrgs = AddOrg.Except(syssetOrgs).ToList();
                    if (ExceptOrgs.Count > 0)
                    {
                        string orgStr = CorrespondenceSettingsService.GetOrgStr(ExceptOrgs);
                        return DCHelper.ErrorMessage(orgStr + "  这些组织不存在该业务条线");
                    }
                }

                var orglist1 = CorrespondenceSettingsService.GetOrgCodeList(AddOrg);
                foreach (var a in AddOrg)
                {
                    QtXmDistributeModel model = new QtXmDistributeModel();
                    model.FProjcode = selectdata[0].FProjcode;//data.FProjcode;
                    model.FProjname = selectdata[0].FProjname;//data.FProjname;
                    model.FBusiness = selectdata[0].FBusiness;//data.FBusiness;
                    model.Orgid = a;
                    model.Orgcode = orglist1.Find(x => x.PhId == a).OCode;
                    model.Distributeorgid = selectdata[0].Distributeorgid;//data.orgid;
                    model.Distributeuserid = selectdata[0].Distributeuserid;//data.userid;
                    model.IfUse = (byte)1;
                    model.PersistentState = PersistentState.Added;
                    rundata.Add(model);
                }
            }

            savedresult = QtXmDistributeService.Save<Int64>(rundata, "");
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdate([FromBody]XmDistributeModel data)
        {
            /*var rundata = QtXmDistributeService.Find(x => x.FProjcode == data.FProjcode).Data.ToList();
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            foreach (var a in rundata)
            {
                if(a.Distributeorgid == data.orgid)
                {
                    a.IfUse = data.IfUse;
                }
                a.FProjname = data.FProjname;
                a.FBusiness = data.FBusiness;
                a.PersistentState = PersistentState.Modified;
            }*/
            var rundata = QtXmDistributeService.Find(data.PhId).Data;

            var proMst = ProjectMstService.Find(p => p.FProjCode.StartsWith(rundata.FProjcode) && p.FDeclarationUnit == rundata.Orgcode);
            if (proMst != null && proMst.Data != null && proMst.Data.Count > 0)
            {
                return DataConverterHelper.SerializeObject(new
                {
                    Ststus = ResponseStatus.Error,
                    Msg = "项目被引用，无法修改！"
                });
            }
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            rundata.IfUse = data.IfUse;
            rundata.FProjname = data.FProjname;
            rundata.FBusiness = data.FBusiness;
            rundata.PersistentState = PersistentState.Modified;
            savedresult = QtXmDistributeService.Save<Int64>(rundata, "");
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 修改启用状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostUpdateUse([FromBody]XmDistributeModel data)
        {
            var rundata = QtXmDistributeService.Find(data.PhId).Data;

            var proMst = ProjectMstService.Find(p => p.FProjCode.StartsWith(rundata.FProjcode) && p.FDeclarationUnit == rundata.Orgcode);
            if (proMst != null && proMst.Data != null && proMst.Data.Count > 0)
            {
                return DataConverterHelper.SerializeObject(new
                {
                    Ststus = ResponseStatus.Error,
                    Msg = "项目被引用，无法停用！"
                });
            }
            //var rundata = QtXmDistributeService.Find(x => x.FProjcode == data.FProjcode && x.Orgid == data.CurOrgId).Data?.First();


            rundata.PersistentState = PersistentState.Modified;
            rundata.IfUse = data.IfUse;
            var savedresult = QtXmDistributeService.Save<Int64>(rundata, "");

            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string PostDelete([FromBody]XmDistributeModel data)
        {

            var rundata = QtXmDistributeService.Find(data.PhId).Data;
            var proMst = ProjectMstService.Find(p => p.FProjCode.StartsWith(rundata.FProjcode) && p.FDeclarationUnit== rundata.Orgcode);
            if(proMst != null && proMst.Data != null && proMst.Data.Count > 0)
            {
                return DataConverterHelper.SerializeObject(new
                {
                    Ststus = ResponseStatus.Error,
                    Msg = "项目被引用，无法删除！"
                });
            }


            /*var rundata = QtXmDistributeService.Find(x => x.FProjcode == data.FProjcode).Data.ToList();
            
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            foreach (var a in rundata)
            {
                a.PersistentState = PersistentState.Deleted;
            }*/
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            rundata.PersistentState= PersistentState.Deleted;
            savedresult = QtXmDistributeService.Save<Int64>(rundata, "");
            return DataConverterHelper.SerializeObject(savedresult);
        }

        /// <summary>
        /// 取列表数据
        /// </summary>
        /// <returns>返回Json串</returns>
        [HttpGet]
        public string GetQtXmDistributeByOrg([FromUri]long OrgPhid,
            [FromUri]int pageIndex = 0,
            [FromUri]int pageSize = 0,
            [FromUri]string search = "")
        {
            if (OrgPhid == 0)
            {
                return DCHelper.ErrorMessage("组织ID为空！");
            }
            var data = new List<QtXmDistributeModel>();
            try
            {
                var syssets = QTSysSetService.Find(x => x.DicType == "Business" && x.Orgid == OrgPhid).Data.ToList();
                data = QtXmDistributeService.Find(x => x.Orgid == OrgPhid && x.IfUse == 1, "FProjcode desc").Data.ToList();
                if (data != null && data.Count > 0)
                {
                    foreach (var i in data)
                    {
                        if (!string.IsNullOrEmpty(i.FBusiness) && syssets != null)
                        {
                            var sysset = syssets.Find(x => x.TypeCode == i.FBusiness);
                            if (sysset != null)
                            {
                                i.FBusiness_EXName = sysset.TypeName;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return DCHelper.ErrorMessage(e.Message);
            }

            
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                data = data.Where(p => p.FProjcode.Contains(search)
                    || p.FProjname.Contains(search)
                    || (p.FBusiness_EXName??string.Empty).Contains(search)).ToList();
            }
            var count = data.Count;
            if (pageIndex != 0 && pageSize != 0)
            {
                data = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            
            var result = new
            {
                Status = ResponseStatus.Success,
                Msg = "获取成功!",
                data = data,
                Count = count
            };
            return DataConverterHelper.SerializeObject(result);
        }
    }
}
