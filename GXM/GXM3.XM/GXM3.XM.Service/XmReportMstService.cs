#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Service
    * 类 名 称：			XmReportMstService
    * 文 件 名：			XmReportMstService.cs
    * 创建时间：			2020/1/17 
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

using GXM3.XM.Service.Interface;
using GXM3.XM.Facade.Interface;
using GXM3.XM.Model.Domain;
using GQT3.QT.Facade.Interface;

namespace GXM3.XM.Service
{
	/// <summary>
	/// XmReportMst服务组装处理类
	/// </summary>
    public partial class XmReportMstService : EntServiceBase<XmReportMstModel>, IXmReportMstService
    {
		#region 类变量及属性
		/// <summary>
        /// XmReportMst业务外观处理对象
        /// </summary>
		IXmReportMstFacade XmReportMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IXmReportMstFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IXmReportDtlFacade XmReportDtlFacade { get; set; }

        IProjectMstFacade ProjectMstFacade { get; set; }

        IQTSysSetFacade QTSysSetFacade { get; set; }

        IProjectDtlBudgetDtlFacade ProjectDtlBudgetDtlFacade { get; set; }
        #endregion

        #region 实现 IXmReportMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="xmReportMstEntity"></param>
        /// <param name="xmReportDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveXmReportMst(XmReportMstModel xmReportMstEntity, List<XmReportDtlModel> xmReportDtlEntities)
        {
			return XmReportMstFacade.SaveXmReportMst(xmReportMstEntity, xmReportDtlEntities);
        }

        /// <summary>
        /// 通过外键值获取XmReportDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<XmReportDtlModel> FindXmReportDtlByForeignKey<TValType>(TValType id)
        {
            return XmReportDtlFacade.FindByForeignKey(id);
        }


        /// <summary>
        /// 通过单据主键获取签报单信息
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <returns></returns>
        public XmReportMstModel GetMSYHProjectReport(long phid)
        {
            XmReportMstModel xmReportMst = new XmReportMstModel();
            //先查询是否已存在签报单数据
            var reports = this.XmReportMstFacade.Find(t => t.XmPhid == phid).Data;
            if(reports != null && reports.Count > 0)
            {
                if(reports.Count == 1)
                {
                    xmReportMst = reports[0];
                    //获取单据编码名称
                    var projects = this.ProjectMstFacade.Find(t => t.PhId == phid).Data;
                    if(projects != null && projects.Count > 0)
                    {
                        xmReportMst.FProjCode = projects[0].FProjCode;
                        xmReportMst.FProjName = projects[0].FProjName;
                        xmReportMst.FProjStatus = projects[0].FProjStatus;
                        //获取业务条线的数据
                        var businesses = this.QTSysSetFacade.Find(x => x.DicType == "Business" && x.Orgcode == projects[0].FDeclarationUnit && x.TypeCode == projects[0].FBusinessCode).Data;
                        if (businesses != null && businesses.Count > 0)
                        {
                            //业务条线转名称
                            xmReportMst.FBusinessName = businesses[0].TypeName;
                        }
                        //获取明细数据集合
                        xmReportMst.XmReportDtls = this.XmReportDtlFacade.Find(t => t.MstPhid == xmReportMst.PhId).Data;
                    }
                    else
                    {
                        throw new Exception("单据信息查询失败！");
                    }
                }
                else
                {
                    throw new Exception("该单据存在多条签报单，请联系管理员进行处理！");
                }
            }
            else
            {
                //获取单据编码名称
                var projects = this.ProjectMstFacade.Find(t => t.PhId == phid).Data;
                if (projects != null && projects.Count > 0)
                {
                    xmReportMst.FProjCode = projects[0].FProjCode;
                    xmReportMst.FProjName = projects[0].FProjName;
                    xmReportMst.FProjStatus = projects[0].FProjStatus;
                    //获取业务条线的数据
                    var businesses = this.QTSysSetFacade.Find(x => x.DicType == "Business" && x.Orgcode == projects[0].FDeclarationUnit && x.TypeCode == projects[0].FBusinessCode).Data;
                    if (businesses != null && businesses.Count > 0)
                    {
                        //业务条线转名称
                        xmReportMst.FBusinessName = businesses[0].TypeName;
                    }
                    //获取明细数据集合(项目明细转签报明细)
                    var projectDtls = this.ProjectDtlBudgetDtlFacade.Find(t => t.MstPhid == projects[0].PhId).Data;
                    if(projectDtls != null && projectDtls.Count > 0)
                    {
                        IList<XmReportDtlModel> xmReportDtls = new List<XmReportDtlModel>();
                        foreach (var dtl in projectDtls)
                        {
                            XmReportDtlModel xmReportDtl = new XmReportDtlModel();
                            xmReportDtl.XmName = dtl.FName;
                            xmReportDtl.XmPhid = dtl.PhId;
                            xmReportDtl.FAmount = dtl.FAmount;
                            xmReportDtls.Add(xmReportDtl);
                        }
                        xmReportMst.XmReportDtls = xmReportDtls;
                    }
                }
                else
                {
                    throw new Exception("单据信息查询失败！");
                }
            }
            return xmReportMst;
        }
        #endregion
    }
}

