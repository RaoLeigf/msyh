#region Summary
/**************************************************************************************
    * 类 名 称：        CorrespondenceSettings2Service
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        CorrespondenceSettings2Service.cs
    * 创建时间：        2018/9/6 
    * 作    者：        刘杭    
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

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GYS3.YS.Facade.Interface;
using GXM3.XM.Facade.Interface;
using GQT3.QT.Model.Domain;
using GYS3.YS.Model.Domain;
using GXM3.XM.Model.Domain;
using GQT3.QT.Model;
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// CorrespondenceSettings2服务组装处理类
	/// </summary>
    public partial class CorrespondenceSettings2Service : EntServiceBase<CorrespondenceSettings2Model>, ICorrespondenceSettings2Service
    {
		#region 类变量及属性
		/// <summary>
        /// CorrespondenceSettings2业务外观处理对象
        /// </summary>
		ICorrespondenceSettings2Facade CorrespondenceSettings2Facade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as ICorrespondenceSettings2Facade;
            }
        }

        /// <summary>
        /// BudgetProcessCtrlFacade业务外观处理对象
        /// </summary>
        private IBudgetProcessCtrlFacade BudgetProcessCtrlFacade { get; set; }
        /// <summary>
        /// OrganizationFacade业务外观处理对象
        /// </summary>
        private IOrganizationFacade OrganizationFacade { get; set; }
        /// <summary>
        /// ProjectMstFacade业务外观处理对象
        /// </summary>
        private IProjectMstFacade ProjectMstFacade { get; set; }

        /// <summary>
        /// OrgRelatitem2Facade业务外观处理对象
        /// </summary>
        private IOrgRelatitem2Facade OrgRelatitem2Facade { get; set; }

        #endregion

        #region 实现 ICorrespondenceSettings2Service 业务添加的成员


        /// <summary>
        /// 是否为申报单位的设置
        /// </summary>
        /// <returns>返回Json串</returns>
        public CommonResult<CorrespondenceSettings2Model> UpdateIfSBOrg(List<OrganizeModel> models,List<CorrespondenceSettings2Model> DeleteData,List<OrganizeModel> InsertData)
        {
            //末级组织保存对应关系
            string OrgCode;
            string OrgPhId;
            var selectresult = new PagedResult<CorrespondenceSettings2Model>();
            CommonResult<CorrespondenceSettings2Model> result = new CommonResult<CorrespondenceSettings2Model>();
            if (models != null && models.Count>0)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    var dicWhere = new Dictionary<string, object>();
                    OrganizeModel organize = models[i];
                    OrgCode = organize.OCode;
                    OrgPhId = organize.PhId.ToString();
                    new CreateCriteria(dicWhere).
                            Add(ORMRestrictions<string>.Eq("Dwdm", OrgCode)).
                            Add(ORMRestrictions<string>.Eq("Dydm", OrgCode)).
                            Add(ORMRestrictions<string>.Eq("Dylx", "ZC")).
                            Add(ORMRestrictions<string>.Eq("DefStr2", OrgPhId));
                    selectresult = base.ServiceHelper.LoadWithPageInfinity("GQT3.QT.DYGX2_All", dicWhere);
                    if (selectresult.TotalItems == 0)
                    {
                        try
                        {
                            CorrespondenceSettings2Model correspondence = new CorrespondenceSettings2Model();
                            correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                            correspondence.Dwdm = OrgCode;
                            correspondence.Dydm = OrgCode;
                            correspondence.Dylx = "ZC";
                            correspondence.DefStr2 = OrgPhId;
                            SavedResult<Int64> savedResult = base.Save<Int64>(correspondence,"");
                        }
                        catch (Exception e)
                        {
                            result.Status = ResponseStatus.Error;
                            result.Msg = "设置失败，请重新设置！";
                            return result;
                        }
                    }
                }
            }

            long Deleteid;  //主表主键
            if (DeleteData != null && DeleteData.Count>0)
            {
                for (int i = 0; i < DeleteData.Count; i++)
                {
                    CorrespondenceSettings2Model delete = DeleteData[i];
                    Deleteid = delete.PhId;
                    
                    try
                    {
                        //判断当前组织是否在项目中被使用，如果使用则不能删除
                        Dictionary<string, object> where1 = new Dictionary<string, object>();
                        new CreateCriteria(where1).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", delete.Dwdm));
                        FindedResults<ProjectMstModel> findedResults = ProjectMstFacade.Find(where1);
                        if (findedResults.Data.Count > 0)
                        {
                            result.Status = ResponseStatus.Error;
                            result.Msg = "设置失败，组织已被引用！";
                            return result;
                        }
                        else {
                            Dictionary<string, object> where2 = new Dictionary<string, object>();
                            new CreateCriteria(where2).Add(ORMRestrictions<string>.Eq("FOcode", delete.Dwdm));
                            DeletedResult deletedResult = BudgetProcessCtrlFacade.Delete(where2);
                        }

                        base.Delete<System.Int64>(Deleteid);
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                        return result;
                    }
                }
            }

            if (InsertData != null && InsertData.Count>0)
            {
                for (int i = 0; i < InsertData.Count; i++)
                {
                    OrganizeModel insertorganize = InsertData[i];
                    var insertOrgCode = insertorganize.OCode;
                    var insertOrgPhId = insertorganize.PhId.ToString();
                    try
                    {
                        CorrespondenceSettings2Model correspondence2 = new CorrespondenceSettings2Model();
                        correspondence2.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence2.Dwdm = insertOrgCode;
                        correspondence2.Dylx = "SB";
                        correspondence2.DefStr2 = insertOrgPhId;
                        SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence2,"");

                        //向预算进度控制表添加相应的记录
                        Dictionary<string, object> where1 = new Dictionary<string, object>();
                        new CreateCriteria(where1).Add(ORMRestrictions<string>.Eq("FOcode", insertorganize.OCode));
                        FindedResults<BudgetProcessCtrlModel> findedResults = BudgetProcessCtrlFacade.Find(where1);
                        //判断进度表中是否已经存在
                        if (findedResults.Data.Count == 0) {
                            List<BudgetProcessCtrlModel> budgetProcessList = new List<BudgetProcessCtrlModel>();
                            Dictionary<string, object> where2 = new Dictionary<string, object>();
                            Dictionary<string, object> where3 = new Dictionary<string, object>();
                            Dictionary<string, object> where4 = new Dictionary<string, object>();
                            new CreateCriteria(where2).Add(ORMRestrictions<Int64>.Eq("ParentOrgId", insertorganize.PhId));
                            new CreateCriteria(where3).Add(ORMRestrictions<string>.Eq("OCode", insertorganize.OCode));
                            new CreateCriteria(where4).Add(ORMRestrictions.Or(where2,where3));
                            List<OrganizeModel> organizeList = OrganizationFacade.Find(where4).Data.ToList();
                            //获得组织对象,前端传过来的组织对象可能没有组织名称
                            OrganizeModel organizeModel = organizeList.Find(t => t.IfCorp == "Y");
                            if (organizeList.Count > 1)
                            {
                                for (int j = 0; j < organizeList.Count; j++)
                                {
                                    if ("N".Equals(organizeList[j].IfCorp))
                                    {
                                        BudgetProcessCtrlModel budgetProcessCtrlModel = new BudgetProcessCtrlModel();
                                        budgetProcessCtrlModel.PersistentState = PersistentState.Added;
                                        budgetProcessCtrlModel.FOcode = organizeModel.OCode;
                                        budgetProcessCtrlModel.FOname = organizeModel.OName;
                                        budgetProcessCtrlModel.FDeptCode = organizeList[j].OCode;
                                        budgetProcessCtrlModel.FDeptName = organizeList[j].OName;
                                        budgetProcessCtrlModel.FProcessStatus = "1";
                                        budgetProcessList.Add(budgetProcessCtrlModel);
                                    }
                                }
                            }
                            else {
                                BudgetProcessCtrlModel budgetProcessCtrlModel = new BudgetProcessCtrlModel();
                                budgetProcessCtrlModel.PersistentState = PersistentState.Added;
                                budgetProcessCtrlModel.FOcode = organizeModel.OCode;
                                budgetProcessCtrlModel.FOname = organizeModel.OName;
                                budgetProcessCtrlModel.FProcessStatus = "1";
                                budgetProcessList.Add(budgetProcessCtrlModel);
                            }
                            BudgetProcessCtrlFacade.Save<Int64>(budgetProcessList);
                        }
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                        return result;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// 支出类别关系的改变
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <param name="OrgPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        public CommonResult<CorrespondenceSettings2Model> UpdateZCLB(string OrgCode, string OrgPhId, List<CorrespondenceSettings2Model> mydelete, List<ExpenseCategoryModel> myinsert)
        {
            CommonResult<CorrespondenceSettings2Model> result = new CommonResult<CorrespondenceSettings2Model>();
            if (mydelete != null && mydelete.Count>0)
            {
                for (int i = 0; i < mydelete.Count; i++)
                {
                    CorrespondenceSettings2Model delete = mydelete[i];
                    var Deleteid = delete.PhId;
                    try
                    {
                        base.Delete<System.Int64>(Deleteid);
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            if (myinsert != null && myinsert.Count>0)
            {
                for (int i = 0; i < myinsert.Count; i++)
                {
                    ExpenseCategoryModel insert = myinsert[i];
                    var Dwdm = OrgPhId;
                    var Dydm = insert.Dm;
                    var Dylx = "08";
                    var DefStr1 = OrgCode;
                    try
                    {
                        CorrespondenceSettings2Model correspondence = new CorrespondenceSettings2Model();
                        correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence.Dwdm = Dwdm;
                        correspondence.Dydm = Dydm;
                        correspondence.Dylx = Dylx;
                        correspondence.DefStr1 = DefStr1;
                        correspondence.DefStr2 = "0";
                        SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence,"");
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 判断是否是末级组织
        /// </summary>
        /// <param name="ParentOrg"></param>
        /// <returns></returns>
        public PagedResult<OrgRelatitem2Model> LoadWithPageIsend(string ParentOrg)
        {
            //CommonResult<OrgRelatitem2Model> result = new CommonResult<OrgRelatitem2Model>();

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<string>.Eq("ParentOrg", ParentOrg));

            var result = OrgRelatitem2Facade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetAllorgrelatitem", dicWhere);
            return result;
        }

        /// <summary>
        /// 保存项目类型对应项目预算明细显示格式设置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResult<CorrespondenceSettings2Model> UpdateYSDtlGS(string data)
        {
            if (data.EndsWith("|"))
            {
                data = data.Substring(0, data.Length - 1);
            }
            String[] Items = data.Split('|');

            CommonResult<CorrespondenceSettings2Model> result = new CommonResult<CorrespondenceSettings2Model>();
            foreach (String item in Items)
            {
                String[] attrs = item.Split(':');
                try
                {
                    CorrespondenceSettings2Model correspondence = this.CorrespondenceSettings2Facade.Find(Int64.Parse(attrs[0])).Data;
                    correspondence.PersistentState = SUP.Common.Base.PersistentState.Modified;
                    if(attrs[1]=="undefined")
                    {
                        attrs[1] = "";
                    }
                    correspondence.DefStr2 = attrs[1];
                    if (attrs[2] == "undefined")
                    {
                        attrs[2] = "";
                    }
                    correspondence.DefStr3 = attrs[2];
                    SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence,"");
                }
                catch (Exception e)
                {
                    result.Status = ResponseStatus.Error;
                    result.Msg = "设置失败，请重新设置！";
                }
            }
            return result;
        }



        /// <summary>
        /// 根据按钮主键取对应关系列表基础数据详细(得到的PhId为对应关系的主键)（没有对应关系的数据）(按钮权限)
        /// </summary>
        /// <returns>返回Json串</returns>
        public string GetOrgListNoDYGXdtl(string Dwdm)
        {
            List<string> list = new List<string>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "button"))
                .Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));
            var result = CorrespondenceSettings2Facade.FacadeHelper.LoadWithPageInfinity("GQT3.QT.DYGX2_All", dicWhere);
            new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("IfCorp", "Y"))
                   .Add(ORMRestrictions<string>.Eq("IsActive", "1"));
            for (var i = 0; i < result.TotalItems; i++)
            {
                list.Add(result.Results[i].Dydm);
            }
            if (list.Count > 0)
            {
                new CreateCriteria(dicWhere2)
                    .Add(ORMRestrictions<List<string>>.NotIn("OCode", list));
            }
            var result2 = OrganizationFacade.FacadeHelper.LoadWithPageInfinity("GQT.QT.GetOrganize", dicWhere2, false, new string[] { "OCode Asc" });
            return DataConverterHelper.EntityListToJson<OrganizeModel>(result2.Results, (Int32)result2.TotalItems);
        }


        /// <summary>
        /// 页面功能控制
        /// </summary>
        /// <param name="Setcode"></param>
        /// <param name="SetPhId"></param>
        /// <param name="mydelete"></param>
        /// <param name="myinsert"></param>
        /// <returns></returns>
        public CommonResult UpdateControlSet(string Setcode, string SetPhId, List<CorrespondenceSettings2Model> mydelete, List<OrganizeModel> myinsert)
        {
            CommonResult result = new CommonResult();
            if (mydelete != null && mydelete.Count > 0)
            {
                for (int i = 0; i < mydelete.Count; i++)
                {
                    CorrespondenceSettings2Model delete = mydelete[i];
                    var Deleteid = delete.PhId;
                    try
                    {
                        base.Delete<System.Int64>(Deleteid);
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }
                }
            }
            if (myinsert != null && myinsert.Count > 0)
            {
                for (int i = 0; i < myinsert.Count; i++)
                {
                    OrganizeModel insert = myinsert[i];
                    var Dwdm = SetPhId;
                    var Dydm = insert.OCode;
                    var Dylx = "button";
                    var DefStr1 = Setcode;
                    var DefStr2 = insert.OName;
                    try
                    {
                        CorrespondenceSettings2Model correspondence = new CorrespondenceSettings2Model();
                        correspondence.PersistentState = SUP.Common.Base.PersistentState.Added;
                        correspondence.Dwdm = Dwdm;
                        correspondence.Dydm = Dydm;
                        correspondence.Dylx = Dylx;
                        correspondence.DefStr1 = DefStr1;
                        correspondence.DefStr2 = DefStr2;
                        SavedResult<Int64> savedResult2 = base.Save<Int64>(correspondence, "");
                    }
                    catch (Exception e)
                    {
                        result.Status = ResponseStatus.Error;
                        result.Msg = "设置失败，请重新设置！";
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 根据操作员取申报单位
        /// </summary>
        /// <param name="USERID"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetSBUnit(long USERID)
        {
            return OrganizationFacade.GetSBUnit(USERID);
        }

        /// <summary>
        /// 根据项目代码获取对应归口部门
        /// </summary>
        /// <param name="Dwdm"></param>
        /// <returns></returns>
        public List<OrganizeModel> GetBMListDYGXdtl(string Dwdm)
        {
            List<OrganizeModel> result = new List<OrganizeModel>();
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).
                    Add(ORMRestrictions<string>.Eq("Dylx", "GKBM")).
                    Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));

            IList<CorrespondenceSettings2Model> correspondenceSettings2s = CorrespondenceSettings2Facade.Find(dicWhere,new string[] { "Dydm Asc" }).Data;
            
            if (correspondenceSettings2s.Count > 0)
            {
                var dicWhereOrg = new Dictionary<string, object>();
                new CreateCriteria(dicWhereOrg).
                        Add(ORMRestrictions<string>.Eq("IfCorp", "N")).
                        Add(ORMRestrictions<string>.Eq("IsActive", "1"));
                IList<OrganizeModel> OrgList = OrganizationFacade.Find(dicWhereOrg).Data;
                foreach (CorrespondenceSettings2Model a in correspondenceSettings2s)
                {
                    OrganizeModel b = new OrganizeModel();
                    b.PhId = a.PhId;
                    b.OCode = a.Dydm;
                    b.OName = OrgList.ToList().Find(x => x.OCode == a.Dydm).OName;
                    result.Add(b);
                }
            }
            
            return result;
        }

        /// <summary>
        /// 根据项目代码获取对应归口部门（没有对应关系的）
        /// </summary>
        /// <param name="Dwdm"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetBMListNoDYGXdtl(string Dwdm,string OrgId)
        {
            //取出设置了对应关系的部门
            var dicWhereDYGX = new Dictionary<string, object>();
            new CreateCriteria(dicWhereDYGX).
                    Add(ORMRestrictions<string>.Eq("Dylx", "GKBM")).
                    Add(ORMRestrictions<string>.Eq("Dwdm", Dwdm));

            IList<CorrespondenceSettings2Model> correspondenceSettings2s = CorrespondenceSettings2Facade.Find(dicWhereDYGX).Data;
            List<string> OrgDYGX=correspondenceSettings2s.ToList().Select(x => x.Dydm).Distinct().ToList();

            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("IfCorp", "N"))
                .Add(ORMRestrictions<string>.Eq("IsActive", "1"))
                .Add(ORMRestrictions<List<string>>.NotIn("OCode", OrgDYGX));
            if (!string.IsNullOrEmpty(OrgId))
            {
                new CreateCriteria(dicWhere).Add(ORMRestrictions<Int64>.Eq("ParentOrgId", long.Parse(OrgId)));
            }
            var result = OrganizationFacade.Find(dicWhere,new string[] { "OCode" }).Data;
            if (result.Count > 0)
            {
                foreach (var a in result)
                {
                    a.PhId = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// 归口项目对应部门设置关系的改变
        /// </summary>
        /// <returns></returns>
        public SavedResult<Int64> UpdateGKXM(List<long> mydelete, List<CorrespondenceSettings2Model> myinsert)
        {
            List<CorrespondenceSettings2Model> data = new List<CorrespondenceSettings2Model>();
            if (mydelete.Count > 0)
            {
                foreach (var deletephid in mydelete)
                {
                    CorrespondenceSettings2Model a = CorrespondenceSettings2Facade.Find(deletephid).Data;
                    a.PersistentState = PersistentState.Deleted;
                    data.Add(a);
                }
            }
            if (myinsert.Count > 0)
            {
                foreach (var insert in myinsert)
                {
                    insert.PersistentState = PersistentState.Added;
                    data.Add(insert);
                }
            }
            SavedResult<Int64> result=CorrespondenceSettings2Facade.Save<Int64>(data, "");
            return result;
        }

        /// <summary>
        /// 归口项目对应部门设置关系的改变
        /// </summary>
        /// <returns></returns>
        public SavedResult<Int64> SaveJXJLset(List<CorrespondenceSettings2Model> updateinfo, List<string> deleteinfo)
        {
            List<CorrespondenceSettings2Model> savedata = new List<CorrespondenceSettings2Model>();
            if (updateinfo.Count > 0)
            {
                foreach (CorrespondenceSettings2Model a in updateinfo)
                {
                    if (a.PhId == 0)
                    {
                        CorrespondenceSettings2Model data = a;
                        data.PersistentState = PersistentState.Added;
                        savedata.Add(data);
                    }
                    else
                    {
                        CorrespondenceSettings2Model data = CorrespondenceSettings2Facade.Find(a.PhId).Data;
                        data.DefStr1 = a.DefStr1;
                        data.DefStr2 = a.DefStr2;
                        data.DefStr3 = a.DefStr3;
                        data.PersistentState = PersistentState.Modified;
                        savedata.Add(data);
                    }
                }
            }
            if (deleteinfo.Count > 0)
            {
                foreach (string b in deleteinfo)
                {
                    if (!string.IsNullOrEmpty(b))
                    {
                        CorrespondenceSettings2Model data = CorrespondenceSettings2Facade.Find(long.Parse(b)).Data;
                        data.PersistentState = PersistentState.Deleted;
                        savedata.Add(data);
                    }
                }
            }
            SavedResult<Int64> result = CorrespondenceSettings2Facade.Save<Int64>(savedata, "");
            return result;
        }
        #endregion
    }
}

