#region Summary
/**************************************************************************************
    * 类 名 称：        QTProductUserService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        QTProductUserService.cs
    * 创建时间：        2018/12/12 
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

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SUP.Common.Base;
using NG3.Data.Service;
using System.Data;

namespace GQT3.QT.Service
{
    /// <summary>
    /// QTProductUser服务组装处理类
    /// </summary>
    public partial class QTProductUserService : EntServiceBase<QTProductUserModel>, IQTProductUserService
    {
        #region 类变量及属性
        /// <summary>
        /// QTProductUser业务外观处理对象
        /// </summary>
        IQTProductUserFacade QTProductUserFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQTProductUserFacade;
            }
        }

        private IQTProductFacade QTProductFacade { get; set; }
        private IQTProductUserDygxFacade QTProductUserDygxFacade { get; set; }

        #endregion

        #region 实现 IQTProductUserService 业务添加的成员

        /// <summary>
        /// 保存时获取产品主键
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public SavedResult<Int64> Save2(IList<QTProductUserModel> entities)
        {
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            try
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    if (entities[i].ProductBZ != "")
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("ProductBZ", entities[i].ProductBZ));
                        QTProductModel qTProductModel = QTProductFacade.Find(dic).Data[0];
                        entities[i].ProductPhid = qTProductModel.PhId;
                    }
                }
                savedresult = base.Save<Int64>(entities, "");
            }
            catch (Exception ex)
            {
                savedresult.Status = ResponseStatus.Error;
                savedresult.Msg = ex.Message.ToString();
            }
            return savedresult;
        }

        /// <summary>
        /// 获取产品用户通过产品标识和G6账号
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <param name="UserNo"></param>
        /// <returns></returns>
        public QTProductUserModel getUserByProduct(string ProductBZ, string UserNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ))
                .Add(ORMRestrictions<string>.Eq("Fg3userno", UserNo));
            var Dygx = QTProductUserDygxFacade.Find(dic).Data;
            if (Dygx.Count > 0)
            {
                Dictionary<string, object> dic2 = new Dictionary<string, object>();
                new CreateCriteria(dic2).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ))
                    .Add(ORMRestrictions<string>.Eq("ProductUserCode", Dygx[0].ProductUserCode));
                var ProductUser = base.Find(dic2).Data;
                if (ProductUser.Count > 0)
                {
                    Dictionary<string, object> dic3 = new Dictionary<string, object>();
                    new CreateCriteria(dic3).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ));
                    IList<QTProductModel> qTProductModelList = QTProductFacade.Find(dic3).Data;
                    if (qTProductModelList.Count > 0)
                    {
                        QTProductModel qTProductModel = qTProductModelList[0];
                        string conn;
                        string sqltext = "SELECT " + qTProductModel.FSqlUserTablePwd + " FROM " + qTProductModel.FSqlUserTable + " WHERE " + qTProductModel.FSqlUserTableCode + "='" + ProductUser[0].ProductUserCode+"'";
                        if (qTProductModel.FSqlType == "oracle")
                        {
                            conn = "ConnectType=ORACLEClient;Data Source=" + qTProductModel.FSqlServer + ";User ID=" + qTProductModel.FSqlDataName + ";Password=" + qTProductModel.FSqlDataPwd + ";";
                        }
                        else
                        {
                            conn = "ConnectType=SqlClient;Server=" + qTProductModel.FSqlServer + ";Database=" + qTProductModel.FSqlSource + ";User ID =" + qTProductModel.FSqlDataName + "; Password=" + qTProductModel.FSqlDataPwd + ";";
                        }
                        DbHelper.Open(conn);
                        DataTable dataTable = DbHelper.GetDataTable(conn, sqltext);
                        DbHelper.Close(conn);
                        if (dataTable.Rows.Count > 0)
                        {
                            ProductUser[0].ProductUserPwd = dataTable.Rows[0][qTProductModel.FSqlUserTablePwd].ToString();
                        }
                    }
                    
                    return ProductUser[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        public SavedResult<Int64> SaveImportData(string fileExtension,string filePath, string clear,string ProductBZ,long ProductPhid)
        {
            IList<QTProductUserModel> qTProductUserList = new List<QTProductUserModel>();

            if ("1".Equals(clear))
            {
                try
                {
                    Dictionary<string, object> dicwhere = new Dictionary<string, object>();
                    new CreateCriteria(dicwhere).Add(ORMRestrictions<string>.Eq("ProductBZ", ProductBZ));
                    base.Delete(dicwhere);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    if (".xls".Equals(fileExtension))
                    {
                        HSSFWorkbook workbook = new HSSFWorkbook(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int rowCount = sheet.LastRowNum;
                        for (int i = 1; i <= rowCount; i++)
                        {
                            QTProductUserModel qTProductUser = new QTProductUserModel();
                            IRow row = sheet.GetRow(i);
                            ICell cell1 = row.GetCell(0);
                            ICell cell2 = row.GetCell(1);
                            ICell cell3 = row.GetCell(2);
                            cell1.SetCellType(CellType.String);
                            cell2.SetCellType(CellType.String);
                            cell3.SetCellType(CellType.String);
                            string ProductUserCode = cell1.StringCellValue;
                            string ProductUserPwd = cell2.StringCellValue;
                            string ProductUserName = cell3.StringCellValue;
                            qTProductUser.ProductUserCode = ProductUserCode;
                            qTProductUser.ProductUserPwd = ProductUserPwd;
                            qTProductUser.ProductUserName = ProductUserName;
                            qTProductUser.ProductBZ = ProductBZ;
                            qTProductUser.ProductPhid = ProductPhid;
                            qTProductUser.PersistentState = PersistentState.Added;
                            qTProductUserList.Add(qTProductUser);
                        }
                    }
                    else if (".xlsx".Equals(fileExtension))
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        int rowCount = sheet.LastRowNum;
                        for (int i = 1; i <= rowCount; i++)
                        {
                            QTProductUserModel qTProductUser = new QTProductUserModel();
                            IRow row = sheet.GetRow(i);
                            ICell cell1 = row.GetCell(0);
                            ICell cell2 = row.GetCell(1);
                            ICell cell3 = row.GetCell(2);
                            cell1.SetCellType(CellType.String);
                            cell2.SetCellType(CellType.String);
                            cell3.SetCellType(CellType.String);
                            string ProductUserCode = cell1.StringCellValue;
                            string ProductUserPwd = cell2.StringCellValue;
                            string ProductUserName = cell3.StringCellValue;
                            qTProductUser.ProductUserCode = ProductUserCode;
                            qTProductUser.ProductUserPwd = ProductUserPwd;
                            qTProductUser.ProductUserName = ProductUserName;
                            qTProductUser.ProductBZ = ProductBZ;
                            qTProductUser.ProductPhid = ProductPhid;
                            qTProductUser.PersistentState = PersistentState.Added;
                            qTProductUserList.Add(qTProductUser);
                        }
                    }
                    
                    
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return base.Save<Int64>(qTProductUserList, "");
        }


        #endregion
    }
}

