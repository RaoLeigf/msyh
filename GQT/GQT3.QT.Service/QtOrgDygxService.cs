#region Summary
/**************************************************************************************
    * 类 名 称：        QtOrgDygxService
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        QtOrgDygxService.cs
    * 创建时间：        2019/2/14 
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
using SUP.Common.Base;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtOrgDygx服务组装处理类
	/// </summary>
    public partial class QtOrgDygxService : EntServiceBase<QtOrgDygxModel>, IQtOrgDygxService
    {
		#region 类变量及属性
		/// <summary>
        /// QtOrgDygx业务外观处理对象
        /// </summary>
		IQtOrgDygxFacade QtOrgDygxFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtOrgDygxFacade;
            }
        }

        private IOrganizationFacade OrganizationFacade { get; set; }

        #endregion

        #region 实现 IQtOrgDygxService 业务添加的成员


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        public CommonResult Save2( List<QtOrgDygxModel> adddata, List<QtOrgDygxModel> updatedata, List<string> deletedata)
        {
            CommonResult result = new CommonResult();
            if (adddata != null && adddata.Count > 0)
            {
                for (var i = 0; i < adddata.Count; i++)
                {
                    QtOrgDygxModel qtOrgDygx = adddata[i];
                    qtOrgDygx.PersistentState= PersistentState.Added;
                    base.Save<Int64>(qtOrgDygx,"");
                }
                
            }
            if (updatedata != null && updatedata.Count > 0)
            {
                for (var j = 0; j < updatedata.Count; j++)
                {
                    QtOrgDygxModel qtOrgDygx2 = updatedata[j];
                    QtOrgDygxModel qtOrgDygx3 = base.Find(qtOrgDygx2.PhId).Data;
                    qtOrgDygx3.Xmorg = qtOrgDygx2.Xmorg;
                    qtOrgDygx3.Oldorg = qtOrgDygx2.Oldorg;
                    qtOrgDygx3.Oldbudget = qtOrgDygx2.Oldbudget;
                    qtOrgDygx3.PersistentState = PersistentState.Modified;
                    base.Save<Int64>(qtOrgDygx3, "");
                }

            }
            if (deletedata != null && deletedata.Count > 0)
            {
                for (var x = 0; x < deletedata.Count; x++)
                {
                    base.Delete(deletedata[x]);
                }
            }
            return result;
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public SavedResult<Int64> ImportData(string fileExtension, string filePath)
        {
            SavedResult<Int64> result = new SavedResult<Int64>();
            IList<QtOrgDygxModel> qtOrgDygxes = new List<QtOrgDygxModel>();
            List<string> xmorglist=new List<string>();//数据库的所有对象的项目库组织代码的list

            string message = "";//存储重复的项目库组织代码

            Dictionary<string, object> dicwhere = new Dictionary<string, object>();
            new CreateCriteria(dicwhere)
                .Add(ORMRestrictions<System.Int64>.NotEq("PhId", 0));
            IList<QtOrgDygxModel> data = QtOrgDygxFacade.Find(dicwhere).Data;//数据库的所有数据
            for(int j=0;j< data.Count; j++)
            {
                xmorglist.Add(data[j].Xmorg);
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
                        QtOrgDygxModel qtOrgDygx = new QtOrgDygxModel();
                        IRow row = sheet.GetRow(i);
                        ICell cell1 = row.GetCell(0);
                        ICell cell2 = row.GetCell(1);
                        cell1.SetCellType(CellType.String);
                        cell2.SetCellType(CellType.String);
                        string Xmorg = cell1.StringCellValue;
                        string Oldorg = cell2.StringCellValue;
                        if (Xmorg != "" && Oldorg != "")
                        {
                            if (xmorglist.Contains(Xmorg))
                            {
                                message += Xmorg + "/";
                            }
                            else
                            {
                                Dictionary<string, object> dicwhere2 = new Dictionary<string, object>();
                                new CreateCriteria(dicwhere2)
                                    .Add(ORMRestrictions<System.String>.Eq("OCode", Xmorg));
                                OrganizeModel Org = OrganizationFacade.Find(dicwhere2).Data[0];
                                
                                qtOrgDygx.ParentOrgId = Org.ParentOrgId;
                                qtOrgDygx.IfCorp = Org.IfCorp;
                                qtOrgDygx.Xmorg = Xmorg;
                                qtOrgDygx.Oldorg = Oldorg;
                                qtOrgDygx.PersistentState = PersistentState.Added;
                                qtOrgDygxes.Add(qtOrgDygx);
                            }
                        }
                    }
                }
                else if (".xlsx".Equals(fileExtension))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);
                    int rowCount = sheet.LastRowNum;
                    for (int i = 1; i <= rowCount; i++)
                    {
                        QtOrgDygxModel qtOrgDygx = new QtOrgDygxModel();
                        IRow row = sheet.GetRow(i);
                        ICell cell1 = row.GetCell(0);
                        ICell cell2 = row.GetCell(1);
                        cell1.SetCellType(CellType.String);
                        cell2.SetCellType(CellType.String);
                        string Xmorg = cell1.StringCellValue;
                        string Oldorg = cell2.StringCellValue;
                        if (Xmorg != "" && Oldorg != "")
                        {
                            if (xmorglist.Contains(Xmorg))
                            {
                                message += Xmorg + "/";
                            }
                            else
                            {
                                Dictionary<string, object> dicwhere2 = new Dictionary<string, object>();
                                new CreateCriteria(dicwhere2)
                                    .Add(ORMRestrictions<System.String>.Eq("OCode", Xmorg));
                                OrganizeModel Org = OrganizationFacade.Find(dicwhere2).Data[0];

                                qtOrgDygx.ParentOrgId = Org.ParentOrgId;
                                qtOrgDygx.IfCorp = Org.IfCorp;

                                qtOrgDygx.Xmorg = Xmorg;
                                qtOrgDygx.Oldorg = Oldorg;
                                qtOrgDygx.PersistentState = PersistentState.Added;
                                qtOrgDygxes.Add(qtOrgDygx);
                            }
                        }
                    }
                }
                
            }
            if (message.Length==0)
            {
                result=base.Save<Int64>(qtOrgDygxes, "");
            }
            else
            {
                result.Status = ResponseStatus.Error;
                result.Msg = "导入失败，重复的项目库组织代码："+message;
            }

            return result;
        }
        #endregion
    }
}

