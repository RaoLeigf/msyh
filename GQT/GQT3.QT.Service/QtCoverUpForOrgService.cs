#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtCoverUpForOrgService
    * 文 件 名：			QtCoverUpForOrgService.cs
    * 创建时间：			2019/10/29 
    * 作    者：			王冠冠    
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

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.WebApi.GQT3.QT.Model.Response;
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtCoverUpForOrg服务组装处理类
	/// </summary>
    public partial class QtCoverUpForOrgService : EntServiceBase<QtCoverUpForOrgModel>, IQtCoverUpForOrgService
    {
		#region 类变量及属性
		/// <summary>
        /// QtCoverUpForOrg业务外观处理对象
        /// </summary>
		IQtCoverUpForOrgFacade QtCoverUpForOrgFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtCoverUpForOrgFacade;
            }
        }

        IQtCoverUpDataFacade QtCoverUpDataFacade { get; set; }

        IOrganizationFacade OrganizationFacade { get; set; }
        #endregion

        #region 实现 IQtCoverUpForOrgService 业务添加的成员

        /// <summary>
        /// 根据所选的过程编码，返回所有组织的套打数据
        /// </summary>
        /// <param name="processCode">过程编码</param>
        /// <param name="processName">过程名称</param>
        /// <returns></returns>
        public IList<QtCoverUpForOrgModel> GetQtCoverUpForOrgs(string processCode, string processName)
        {
            //返回的套打数据
            IList<QtCoverUpForOrgModel> qtCoverUpForOrgs = new List<QtCoverUpForOrgModel>();
            //内置的套打模板
            IList<QtCoverUpDataModel> allCoverUpDatas = new List<QtCoverUpDataModel>();
            allCoverUpDatas = this.QtCoverUpDataFacade.GetQtCoverUpDataList();

            //该过程的所有套打配置数据
            var qtCoverUpForOrgList = this.QtCoverUpForOrgFacade.Find(t => t.ProcessCode == processCode).Data;

            //获取所有组织信息为后续做准备
            IList<OrganizeModel> allOrg = this.OrganizationFacade.Find(t => t.PhId > 0).Data;
            if(allOrg == null || allOrg.Count <= 0)
            {
                throw new Exception("组织信息查询失败！");
            }

            if (qtCoverUpForOrgList != null && qtCoverUpForOrgList.Count > 0)
            {
                //根据内置模板获取对应打印格式数据
                foreach(var coverUp in allCoverUpDatas)
                {
                    var coverUpByOrg = qtCoverUpForOrgList.ToList().FindAll(t => t.TempLateId == coverUp.PhId && t.TempLateCode == coverUp.EnCode);
                    if(coverUpByOrg != null && coverUpByOrg.Count > 0)
                    {
                        var phids = coverUpByOrg.Select(t => t.OrgId).ToList();

                        QtCoverUpForOrgModel qtCoverUpForOrg = new QtCoverUpForOrgModel();
                        qtCoverUpForOrg.TempLateId = coverUp.PhId;
                        qtCoverUpForOrg.TempLateCode = coverUp.EnCode;
                        qtCoverUpForOrg.TempLateName = coverUp.Name;
                        qtCoverUpForOrg.SortCode = coverUp.SortCode;
                        qtCoverUpForOrg.ProcessCode = processCode;
                        qtCoverUpForOrg.ProcessName = processName;
                        qtCoverUpForOrg.EnabledMark = coverUpByOrg.Find(t => t.EnabledMark == (byte)0) == null ? (byte)1 : (byte)0;
                        qtCoverUpForOrg.OrgList = allOrg.ToList().FindAll(t => phids.Contains(t.PhId));

                        qtCoverUpForOrgs.Add(qtCoverUpForOrg);
                    }
                }
            }

            return qtCoverUpForOrgs;
        }

        /// <summary>
        /// 修改所有组织的套打数据
        /// </summary>
        /// <param name="allCoverUps">套打数据集合</param>
        /// <returns></returns>
        public SavedResult<long> UpdateCoverUpList(List<AllCoverUpModel> allCoverUps)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            //现获取原有的所有组织套打数据
            IList<QtCoverUpForOrgModel> oldAllCoverUpForOrgs = new List<QtCoverUpForOrgModel>();
            oldAllCoverUpForOrgs = this.QtCoverUpForOrgFacade.Find(t => t.PhId > 0).Data;

            //内置的套打模板
            IList<QtCoverUpDataModel> allCoverUpDatas = new List<QtCoverUpDataModel>();
            allCoverUpDatas = this.QtCoverUpDataFacade.GetQtCoverUpDataList();

            IList<QtCoverUpForOrgModel> newAllCoverUpForOrgs = new List<QtCoverUpForOrgModel>();
            if (allCoverUps != null && allCoverUps.Count > 0)
            {
                //分阶段
                foreach(var cover in allCoverUps)
                {
                    //各阶段的原数据
                    IList<QtCoverUpForOrgModel> oldAllCoverUpByPro = new List<QtCoverUpForOrgModel>();
                    if (oldAllCoverUpForOrgs != null && oldAllCoverUpForOrgs.Count > 0)
                    {
                        oldAllCoverUpByPro = oldAllCoverUpForOrgs.ToList().FindAll(t => t.ProcessCode == cover.ProcessCode);
                    }

                    if(cover.QtCoverUpForOrgs != null && cover.QtCoverUpForOrgs.Count > 0)
                    {
                        foreach(var co in cover.QtCoverUpForOrgs)
                        {
                            //各阶段下的各模块原数据
                            IList<QtCoverUpForOrgModel> oldAllCoverUpByProTem = new List<QtCoverUpForOrgModel>();
                            if(oldAllCoverUpByPro != null && oldAllCoverUpByPro.Count > 0)
                            {
                                oldAllCoverUpByProTem = oldAllCoverUpByPro.ToList().FindAll(t => t.TempLateId == co.TempLateId);
                            }
                            if (cover.QtCoverUpForOrgs.ToList().FindAll(t=>t.TempLateId == co.TempLateId).Count > 1)
                            {
                                throw new Exception("每个内置配置项只能配置一行！");
                            }
                            //新的启用或停用的组织集合
                            if(co.OrgList != null && co.OrgList.Count > 0)
                            {                              
                                foreach(var org in co.OrgList)
                                {
                                    QtCoverUpForOrgModel qtCoverUpForOrg = new QtCoverUpForOrgModel();
                                    QtCoverUpForOrgModel oldCoverUpForOrg = new QtCoverUpForOrgModel();
                                    if (oldAllCoverUpByProTem != null && oldAllCoverUpByProTem.Count > 0)
                                    {
                                        oldCoverUpForOrg = oldAllCoverUpByProTem.ToList().Find(t => t.OrgId == org.PhId);
                                    }
                                    else
                                    {
                                        oldCoverUpForOrg = null;
                                    }
                                    if(oldCoverUpForOrg != null)
                                    {
                                        qtCoverUpForOrg = oldCoverUpForOrg;
                                        qtCoverUpForOrg.TempLateId = co.TempLateId;
                                        qtCoverUpForOrg.TempLateCode = co.TempLateCode;
                                        qtCoverUpForOrg.TempLateName = co.TempLateName;
                                        qtCoverUpForOrg.ProcessCode = co.ProcessCode;
                                        qtCoverUpForOrg.ProcessName = co.ProcessName;
                                        qtCoverUpForOrg.SortCode = co.SortCode;
                                        qtCoverUpForOrg.EnabledMark = co.EnabledMark;
                                        qtCoverUpForOrg.Description = co.Description;
                                        qtCoverUpForOrg.OrgId = org.PhId;
                                        qtCoverUpForOrg.OrgCode = org.OCode;
                                        qtCoverUpForOrg.PersistentState = PersistentState.Modified;
                                        oldAllCoverUpByPro.Remove(oldCoverUpForOrg);
                                    }
                                    else
                                    {
                                        qtCoverUpForOrg.TempLateId = co.TempLateId;
                                        qtCoverUpForOrg.TempLateCode = co.TempLateCode;
                                        qtCoverUpForOrg.TempLateName = co.TempLateName;
                                        qtCoverUpForOrg.ProcessCode = co.ProcessCode;
                                        qtCoverUpForOrg.ProcessName = co.ProcessName;
                                        qtCoverUpForOrg.SortCode = co.SortCode;
                                        qtCoverUpForOrg.EnabledMark = co.EnabledMark;
                                        qtCoverUpForOrg.Description = co.Description;
                                        qtCoverUpForOrg.OrgId = org.PhId;
                                        qtCoverUpForOrg.OrgCode = org.OCode;
                                        qtCoverUpForOrg.PersistentState = PersistentState.Added;
                                    }
                                    newAllCoverUpForOrgs.Add(qtCoverUpForOrg);
                                }
                            }


                        }
                        
                    }

                    //删除原有的但为新增修改的数据
                    if(oldAllCoverUpByPro != null && oldAllCoverUpByPro.Count > 0)
                    {
                        foreach(var oldCover in oldAllCoverUpByPro)
                        {
                            oldCover.PersistentState = PersistentState.Deleted;
                            newAllCoverUpForOrgs.Add(oldCover);
                        }
                    }

                    //验证数据的准确性
                    if(newAllCoverUpForOrgs != null && newAllCoverUpForOrgs.Count > 0)
                    {
                        var newAllCoverUpByPro = newAllCoverUpForOrgs.ToList().FindAll(t => t.ProcessCode == cover.ProcessCode);
                        if(newAllCoverUpByPro != null && newAllCoverUpByPro.Count > 0)
                        {
                            //先个模板类型赋值，用来验证数据
                            foreach (var newCover in newAllCoverUpByPro)
                            {
                                newCover.TypeNumber = allCoverUpDatas.ToList().Find(t => t.PhId == newCover.TempLateId) == null ? 0 : allCoverUpDatas.ToList().Find(t => t.PhId == newCover.TempLateId).TypeNumber;
                            }
                            foreach (var newCover in newAllCoverUpByPro)
                            {
                                if(newAllCoverUpByPro.ToList().FindAll(t=>t.OrgId == newCover.OrgId && t.TypeNumber == newCover.TypeNumber && t.PersistentState != PersistentState.Deleted && t.EnabledMark==(byte)0).Count > 1)
                                {
                                    throw new Exception("同一组织同一过程下只能启用一种套打格式模板！");
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }

                savedResult = this.QtCoverUpForOrgFacade.Save<long>(newAllCoverUpForOrgs);
            }


            return savedResult; 
        }

        /// <summary>
        /// 根据过程组织获取对应的打印格式
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="processCode">过程编码</param>
        /// <returns></returns>
        public IList<QtCoverUpForOrgModel> GetCoverUpByOrg(long orgId, string processCode)
        {
            //内置的套打模板
            IList<QtCoverUpDataModel> allCoverUpDatas = new List<QtCoverUpDataModel>();
            allCoverUpDatas = this.QtCoverUpDataFacade.GetQtCoverUpDataList();

            IList<QtCoverUpForOrgModel> qtCoverUpForOrgs = new List<QtCoverUpForOrgModel>();
            IList<QtCoverUpForOrgModel> newCoverUpForOrgs = new List<QtCoverUpForOrgModel>();
            qtCoverUpForOrgs = this.QtCoverUpForOrgFacade.Find(t => t.OrgId == orgId && t.ProcessCode == processCode && t.EnabledMark == (byte)0).Data;
            if(qtCoverUpForOrgs != null && qtCoverUpForOrgs.Count > 0)
            {
                foreach (var coverUp in qtCoverUpForOrgs)
                {
                    coverUp.TypeNumber = allCoverUpDatas.ToList().Find(t => t.PhId == coverUp.TempLateId) == null ? 0 : allCoverUpDatas.ToList().Find(t => t.PhId == coverUp.TempLateId).TypeNumber;
                    coverUp.TypeName = allCoverUpDatas.ToList().Find(t => t.PhId == coverUp.TempLateId) == null ? "" : allCoverUpDatas.ToList().Find(t => t.PhId == coverUp.TempLateId).TypeName;
                }
                newCoverUpForOrgs.Add(qtCoverUpForOrgs.ToList().Find(t => t.TypeNumber == 1));
                newCoverUpForOrgs.Add(qtCoverUpForOrgs.ToList().Find(t => t.TypeNumber == 2));
            }
            return newCoverUpForOrgs;
        }
        #endregion
    }
}

