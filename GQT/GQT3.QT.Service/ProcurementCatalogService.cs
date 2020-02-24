#region Summary
/**************************************************************************************
    * 类 名 称：        ProcurementCatalogService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        ProcurementCatalogService.cs
    * 创建时间：        2018/10/16 
    * 作    者：        李长敏琛    
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
using GXM3.XM.Facade.Interface;
using GXM3.XM.Model.Domain;
using SUP.Common.Base;
using Enterprise3.Common.Base.Criterion;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GQT3.QT.Service
{
	/// <summary>
	/// ProcurementCatalog服务组装处理类
	/// </summary>
    public partial class ProcurementCatalogService : EntServiceBase<ProcurementCatalogModel>, IProcurementCatalogService
    {
		#region 类变量及属性
		/// <summary>
        /// ProcurementCatalog业务外观处理对象
        /// </summary>
		IProcurementCatalogFacade ProcurementCatalogFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IProcurementCatalogFacade;
            }
        }

        //引用XM的IProjectDtlPurchaseDtlFacade
        //IProjectDtlPurchaseDtlFacade ProjectDtlPurchaseDtlFacade
        //{
        //    get; set;
        //}

        #endregion

        #region 实现 IProcurementCatalogService 业务添加的成员
        //public IList<ProjectDtlPurchaseDtlModel> getQuoteByProjectDtlPurchaseDtl(string ProcurementCatalogPhId)
        //{
        //    Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic("");//查询条件转Dictionary
        //    CreateCriteria createCriteria = new CreateCriteria(dicWhere);
        //    //createCriteria.Add(ORMRestrictions<string>.Eq("FCatalogCode", ProcurementCatalogPhId));
        //    FindedResults<ProjectDtlPurchaseDtlModel> results = this.ProjectDtlPurchaseDtlFacade.Find(dicWhere);
        //    return null;
        //}

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public SavedResult<Int64> SaveImportData(string fileExtension, string filePath, string clear)
        {
            IList<ProcurementCatalogModel> procurementCatalogList = new List<ProcurementCatalogModel>();

            if ("1".Equals(clear))
            {
                Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                dicwhere.Add("1", "1");
                base.Delete(dicwhere);
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (".xls".Equals(fileExtension))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = sheet.LastRowNum;
                    for (int i = 1; i <= rowCount; i++)
                    {
                        ProcurementCatalogModel procurementCatalog = new ProcurementCatalogModel();
                        IRow row = sheet.GetRow(i);
                        ICell cell1 = row.GetCell(0);
                        ICell cell2 = row.GetCell(1);
                        //ICell cell3 = row.GetCell(2);
                        cell1.SetCellType(CellType.String);
                        cell2.SetCellType(CellType.String);
                        //cell3.SetCellType(CellType.String);
                        string FCode = cell1.StringCellValue;
                        string FName = cell2.StringCellValue;
                        //string FRemark = cell3.StringCellValue;
                        procurementCatalog.FCode = FCode;
                        procurementCatalog.FName = FName;
                        //procurementCatalog.FRemark = FRemark;
                        procurementCatalog.PersistentState = PersistentState.Added;
                        procurementCatalogList.Add(procurementCatalog);
                    }
                }
                else if (".xlsx".Equals(fileExtension))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = sheet.LastRowNum;
                    for (int i = 1; i <= rowCount; i++)
                    {
                        ProcurementCatalogModel procurementCatalog = new ProcurementCatalogModel();
                        IRow row = sheet.GetRow(i);
                        ICell cell1 = row.GetCell(0);
                        ICell cell2 = row.GetCell(1);
                        //ICell cell3 = row.GetCell(2);
                        cell1.SetCellType(CellType.String);
                        cell2.SetCellType(CellType.String);
                        //cell3.SetCellType(CellType.String);
                        string FCode = cell1.StringCellValue;
                        string FName = cell2.StringCellValue;
                        //string FRemark = cell3.StringCellValue;
                        procurementCatalog.FCode = FCode;
                        procurementCatalog.FName = FName;
                        //procurementCatalog.FRemark = FRemark;
                        procurementCatalog.PersistentState = PersistentState.Added;
                        procurementCatalogList.Add(procurementCatalog);
                    }
                }
            }

            return base.Save<Int64>(procurementCatalogList,"");
        }


        /// <summary>
        /// 根据组织获取所有采购目录集合
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public IList<ProcurementCatalogModel> GetProcurementCatalogs(string orgId, string orgCode)
        {
            IList<ProcurementCatalogModel> procurementCatalogs = new List<ProcurementCatalogModel>();
            procurementCatalogs = this.ProcurementCatalogFacade.Find(t => t.Orgid == long.Parse(orgId) && t.Orgcode == orgCode).Data.OrderBy(t => t.FCode).ToList();
            return procurementCatalogs;
        }

        /// <summary>
        /// 保存采购目录集合
        /// </summary>
        /// <param name="procurementCatalogs">集合对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <returns></returns>
        public SavedResult<long> UpdateProcurementCatalogs(IList<ProcurementCatalogModel> procurementCatalogs, string orgId, string orgCode)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if(procurementCatalogs != null && procurementCatalogs.Count > 0)
            {
                foreach(var pro in procurementCatalogs)
                {
                    pro.Orgcode = orgCode;
                    pro.Orgid = long.Parse(orgId);
                    if(pro.PhId == 0)
                    {
                        pro.PersistentState = PersistentState.Added;
                    }
                    else
                    {
                        if(pro.PersistentState != PersistentState.Deleted)
                        {
                            pro.PersistentState = PersistentState.Modified;
                        }
                    }
                }
                savedResult = this.ProcurementCatalogFacade.Save<long>(procurementCatalogs);
            }
            return savedResult;
        }
        #endregion
    }
}

