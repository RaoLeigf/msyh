#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Service
    * 类 名 称：			YsAccountMstService
    * 文 件 名：			YsAccountMstService.cs
    * 创建时间：			2019/9/23 
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

using GYS3.YS.Service.Interface;
using GYS3.YS.Facade.Interface;
using GYS3.YS.Model.Domain;
using Enterprise3.Common.Model;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Helpers;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using GData3.Common.Utils;
using NPOI.SS.Util;
using GYS3.YS.Model.Extra;
using System.IO;
using Newtonsoft.Json;
using Enterprise3.WebApi.GYS3.YS.Model.Response;

namespace GYS3.YS.Service
{
	/// <summary>
	/// YsAccountMst服务组装处理类
	/// </summary>
    public partial class YsAccountMstService : EntServiceBase<YsAccountMstModel>, IYsAccountMstService
    {
		#region 类变量及属性
		/// <summary>
        /// YsAccountMst业务外观处理对象
        /// </summary>
		IYsAccountMstFacade YsAccountMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IYsAccountMstFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IYsAccountFacade YsAccountFacade { get; set; }
		#endregion

		#region 实现 IYsAccountMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysAccountMstEntity"></param>
		/// <param name="ysAccountEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveYsAccountMst(YsAccountMstModel ysAccountMstEntity, List<YsAccountModel> ysAccountEntities)
        {
			return YsAccountMstFacade.SaveYsAccountMst(ysAccountMstEntity, ysAccountEntities);
        }

        /// <summary>
        /// 通过外键值获取YsAccount明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<YsAccountModel> FindYsAccountByForeignKey<TValType>(TValType id)
        {
            return YsAccountFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 保存预决算报表
        /// </summary>
        /// <param name="ysAccountMst">对象</param>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <param name="uid">用户id</param>
        /// <param name="verify">用来判断年初，年中，年末（1、年初，2、年中，3、年末）</param>
        /// <returns></returns>
        public SavedResult<long> SaveYsAccount(YsAccountMstModel ysAccountMst, long orgId, string orgCode, string year, long uid, string verify)
        {
            return this.YsAccountMstFacade.SaveYsAccount(ysAccountMst, orgId, orgCode, year, uid, verify);
        }

        /// <summary>
        /// 修改预决算说明书
        /// </summary>
        /// <param name="ysAccountMst">主表对象</param>
        /// <returns></returns>
        public SavedResult<long> UpdateAccountMst(YsAccountMstModel ysAccountMst)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            if (ysAccountMst.PhId == 0)
            {
                throw new Exception("预决算数据还未保存，不能先保存预算说明书！");
            }
            else
            {
                string propertyName = "";
                string description = "";
                if (ysAccountMst.DescriptionStart != null && !"".Equals(ysAccountMst.DescriptionStart))
                {
                    propertyName = "DescriptionStart";
                    description = ysAccountMst.DescriptionStart;
                }
                else if (ysAccountMst.DescriptionMiddle != null && !"".Equals(ysAccountMst.DescriptionMiddle))
                {
                    propertyName = "DescriptionMiddle";
                    description = ysAccountMst.DescriptionMiddle;
                }
                else if (ysAccountMst.DescriptionEnd != null && !"".Equals(ysAccountMst.DescriptionEnd))
                {
                    propertyName = "DescriptionEnd";
                    description = ysAccountMst.DescriptionEnd;
                }

                if (!"".Equals(propertyName) && !"".Equals(description))
                {
                    List<PropertyColumnMapperInfo> mapperInfos = new List<PropertyColumnMapperInfo>();
                    mapperInfos.Add(new PropertyColumnMapperInfo
                    {
                        PropertyName = "PhId",
                        Value = ysAccountMst.PhId
                    });
                    mapperInfos.Add(new PropertyColumnMapperInfo
                    {
                        PropertyName = propertyName,
                        Value = description
                    });
                    savedResult = this.YsAccountMstFacade.FacadeHelper.Update<Int64>(mapperInfos);
                }
            }
            return savedResult;
        }

        /// <summary>
        /// 根据组织获取各个上报组织的数量
        /// </summary>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">对应年份</param>
        /// <returns></returns>
        public Dictionary<string, object> GetYsAccountNum(string orgCode, string year)
        {
            return this.YsAccountMstFacade.GetYsAccountNum(orgCode, year);
        }

        /// <summary>
        /// 根据组织获取该组织以及其下级的所有汇总信息
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <param name="chooseOwn">是否包含本级</param>
        /// <param name="verify">用来判断年初年中年末</param>
        /// <returns></returns>
        public YsAccountMstModel GetAllYsAccountList(long orgId, string orgCode, string year, int chooseOwn, string verify)
        {
            YsAccountMstModel ysAccountMst = new YsAccountMstModel();
            //判断组织是否保存过预决算
            if(this.YsAccountMstFacade.Find(t => t.Orgid == orgId && t.Uyear == year).Data != null && this.YsAccountMstFacade.Find(t => t.Orgid == orgId && t.Uyear == year).Data.Count > 0)
            {
                ysAccountMst = this.YsAccountMstFacade.Find(t => t.Orgid == orgId && t.Uyear == year).Data[0];
            }
            else
            {
                ysAccountMst.Orgcode = orgCode;
                ysAccountMst.Orgid = orgId;
                ysAccountMst.Uyear = year;
            }
            //初始的所有科目列表
            ysAccountMst.YsAccounts = this.YsAccountMstFacade.GetYsAccountsBySubject(orgCode, orgId, year);
            //已上报的明细
            List<YsAccountModel> ysAccounts = this.YsAccountMstFacade.GetAllYsAccountList(orgCode, year, chooseOwn, verify);
            if (ysAccountMst.YsAccounts != null && ysAccountMst.YsAccounts.Count > 0)
            {
                if(ysAccounts != null && ysAccounts.Count > 0)
                {               
                    foreach(var acc in ysAccountMst.YsAccounts)
                    {
                        acc.FINALACCOUNTSTOTAL = ysAccounts.FindAll(t=>t.SUBJECTCODE == acc.SUBJECTCODE).Sum(t => t.FINALACCOUNTSTOTAL);
                        acc.BUDGETTOTAL = ysAccounts.FindAll(t => t.SUBJECTCODE == acc.SUBJECTCODE).Sum(t => t.BUDGETTOTAL);
                        acc.ADJUSTMENT = ysAccounts.FindAll(t => t.SUBJECTCODE == acc.SUBJECTCODE).Sum(t => t.ADJUSTMENT);
                        acc.APPROVEDBUDGETTOTAL = ysAccounts.FindAll(t => t.SUBJECTCODE == acc.SUBJECTCODE).Sum(t => t.APPROVEDBUDGETTOTAL);
                        acc.ThisaccountsTotal = ysAccounts.FindAll(t => t.SUBJECTCODE == acc.SUBJECTCODE).Sum(t => t.ThisaccountsTotal);
                    }
                }
            }
            return ysAccountMst;
        }

        /// <summary>
        /// 给所选组织的本级与下级组织打上审批记号
        /// </summary>
        /// <param name="orgCode">组织编码</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public IList<OrganizeModel> GetOrganizeVerifyList(string orgCode, string year)
        {
            return this.YsAccountMstFacade.GetOrganizeVerifyList(orgCode, year);
        }
        #endregion

        #region//与年中，年末决算相关

        /// <summary>
        /// 得到年初上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public YsAccountMstModel GetBegineAccounts(long orgId, string orgCode, string year)
        {
            return this.YsAccountFacade.GetBegineAccounts(orgId, orgCode, year);
        }

        /// <summary>
        /// 得到年中上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public YsAccountMstModel GetMiddleAccounts(long orgId, string orgCode, string year)
        {
            return this.YsAccountFacade.GetMiddleAccounts(orgId, orgCode, year);
        }

        /// <summary>
        /// 得到年末上报的数据（单个组织）
        /// </summary>
        /// <param name="orgId">组织id</param>
        /// <param name="orgCode">组织code</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public YsAccountMstModel GetEndAccounts(long orgId, string orgCode, string year)
        {
            return this.YsAccountFacade.GetEndAccounts(orgId, orgCode, year);
        }
        #endregion

        #region
        /// <summary>
        /// 导出年初预算报表
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="title">标题</param>
        /// <param name="userModel">用户对象</param>
        /// <param name="organizeModel">组织对象</param>
        /// <returns></returns>
        public String GetBeginExcel(YsAccountModel[] datas, String[] title, User2Model userModel, OrganizeModel organizeModel)
        {
            //long orgid = 0;
            //if (datas.Length > 0)
            //{
            //    orgid = datas[0].OrgId;
            //}
            //var orgs = this.SysOrganizeFacade.Find(t => t.PhId == orgid).Data;
            //if (orgs.Count != 1)
            //{
            //    throw new Exception("组织对应不明确");
            //}
            //var org = orgs[0];
            var org = organizeModel;
            var Incom_Head = ConfigHelper.GetString("Incom").First().ToString();
            var Expenditure_Head = ConfigHelper.GetString("Expenditure").First().ToString();

            String[] pname = { "SUBJECTCODE", "SUBJECTNAME", "FINALACCOUNTSTOTAL", "BUDGETTOTAL", "DESCRIPTION" };
            //datas = this.GetBaseReport("", "547181121000001", "2018");
            ICellStyle[,] cellstyle = new ICellStyle[datas.Length, pname.Length];
            Dictionary<int, int> columnWidth = new Dictionary<int, int>();
            columnWidth.Add(0, 18 * 256);
            columnWidth.Add(1, 28 * 256);
            columnWidth.Add(2, 26 * 256);
            columnWidth.Add(3, 26 * 256);
            columnWidth.Add(4, 28 * 256);
            HSSFWorkbook workbook = new HSSFWorkbook();
            foreach (var t in datas.Where(t => !(t.SUBJECTCODE.StartsWith(Incom_Head) || (t.SUBJECTCODE.StartsWith(Expenditure_Head)))))
            {
                t.SUBJECTCODE = "";
                t.Layers = -1;
            }
            foreach (var t in datas.Where(t => (t.SUBJECTCODE.StartsWith(Incom_Head) || t.SUBJECTCODE.StartsWith(Expenditure_Head)) && (t.Layers > 0)))
            {
                t.SUBJECTCODE = "    " + t.SUBJECTCODE;
                t.SUBJECTNAME = "    " + t.SUBJECTCODE;
            }
            //i是行，j是列
            ICellStyle moneyStyle = null;
            for (int i = 0; i < datas.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (((pname[j].Equals("SUBJECTCODE") || pname[j].Equals("SUBJECTNAME")) && datas[i].Layers >= 0) || pname[j].Equals("DESCRIPTION"))
                    {
                        cellstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 70, 12, true);
                        continue;
                    }
                    if ((pname[j].Equals("SUBJECTCODE") || pname[j].Equals("SUBJECTNAME")) && datas[i].Layers < 0)
                    {
                        cellstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                        continue;
                    }
                    moneyStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 70, 12, true);
                    IDataFormat dataFormat = workbook.CreateDataFormat();
                    moneyStyle.DataFormat = dataFormat.GetFormat("#,###0.00");
                    cellstyle[i, j] = moneyStyle;
                }
            }

            if (datas != null && datas.Count() > 0)
            {
                foreach (var da in datas)
                {
                    if (da.SUBJECTCODE.StartsWith("4B") || da.SUBJECTCODE.StartsWith("5B") || da.SUBJECTCODE.StartsWith("5Q"))
                    {
                        da.SUBJECTCODE = " ";
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            //获得表体数据
            ISheet Body = ExcelHelper.OutExcel<YsAccountModel>(datas, pname, 18 * 20, 10, cellstyle, new CellRangeAddress[0], workbook, columnWidth);


            BudgetExcelHead[] heads = new BudgetExcelHead[6];
            heads[0] = new BudgetExcelHead { SUBJECTCODE = "工会经费收支预算表" };
            heads[1] = new BudgetExcelHead();
            heads[2] = new BudgetExcelHead();
            heads[3] = new BudgetExcelHead { SUBJECTCODE = "编制单位：（公章）" + org.OName + "                  年度： " + DateTime.Now.Year + "年             编报日期：" + DateTime.Now.Year + " 年" + DateTime.Now.Month + " 月" + DateTime.Now.Day + " 日          单位：元" };
            heads[4] = new BudgetExcelHead { SUBJECTCODE = "科        目", FINALACCOUNTSTOTAL = "上年决算数", BUDGETTOTAL = "本年预算数", DESCRIPTION = "说明" };
            heads[5] = new BudgetExcelHead { SUBJECTCODE = "编    码", SUBJECTNAME = "名    称" };
            //获得表头数据
            ICellStyle[,] headstyle = new ICellStyle[heads.Length, pname.Length];
            for (int i = 0; i < heads.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (i == 0 || i == 1 || i == 2)
                    {
                        headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 24, false);
                        continue;
                    }
                    if (i == 3)
                    {
                        headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, false);
                        continue;
                    }
                    headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                }
            }
            CellRangeAddress[] headRang = new CellRangeAddress[6];
            headRang[0] = new CellRangeAddress(0, 1, 0, 4);
            headRang[1] = new CellRangeAddress(3, 3, 0, 4);
            headRang[2] = new CellRangeAddress(4, 4, 0, 1);
            headRang[3] = new CellRangeAddress(4, 5, 3, 3);
            headRang[4] = new CellRangeAddress(4, 5, 2, 2);
            headRang[5] = new CellRangeAddress(4, 5, 4, 4);
            ISheet Head = ExcelHelper.OutExcel<BudgetExcelHead>(heads, pname, 18 * 20, 10, headstyle, new CellRangeAddress[0], workbook, columnWidth);

            BudgetExcelHead[] ends = new BudgetExcelHead[1];
            ends[0] = new BudgetExcelHead { SUBJECTCODE = "工会主席： " + userModel.UserName + "             经费审查委员会主任：" + userModel.UserName + "             财务负责人：" + userModel.UserName + "             制表 ：" + userModel.UserName };
            ICellStyle[,] Endstyle = new ICellStyle[ends.Length, pname.Length];
            for (int i = 0; i < ends.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (i == 0)
                    {
                        Endstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, false);
                        continue;
                    }
                    Endstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                }
            }
            CellRangeAddress[] endRang = new CellRangeAddress[1];
            endRang[0] = new CellRangeAddress(0, 0, 0, 4);
            ISheet End = ExcelHelper.OutExcel<BudgetExcelHead>(ends, pname, 18 * 20, 10, Endstyle, new CellRangeAddress[0], workbook, columnWidth);
            End.AddMergedRegion(endRang[0]);
            //将表头表体表尾合并
            ExcelHelper.Splice_Sheet(Head, Body, End, workbook, 18 * 20, 10, columnWidth, headRang, "工会经费收支预算表");
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\Begin";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();
            return JsonConvert.SerializeObject(new { path = path, filename = filename });
        }

        /// <summary>
        /// 导出年中调整报表
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="title">标题</param>
        /// <param name="userModel">用户对象</param>
        /// <param name="organizeModel">组织对象</param>
        /// <returns></returns>
        public String GetMiddleExcel(YsAccountModel[] datas, String[] title, User2Model userModel, OrganizeModel organizeModel)
        {
            //long orgid = 0;
            //if (datas.Length > 0)
            //{
            //    orgid = datas[0].OrgId;
            //}
            //var orgs = this.SysOrganizeFacade.Find(t => t.PhId == orgid).Data;
            //if (orgs.Count != 1)
            //{
            //    throw new Exception("组织对应不明确");
            //}
            //var org = orgs[0];

            var org = organizeModel;

            var Incom_Head = ConfigHelper.GetString("Incom").First().ToString();
            var Expenditure_Head = ConfigHelper.GetString("Expenditure").First().ToString();
            String[] pname = { "SUBJECTCODE", "SUBJECTNAME", "BUDGETTOTAL", "ADJUSTMENT", "APPROVEDBUDGETTOTAL", "BudgetComplete", "DESCRIPTIONMIDDLE" };
            //String[] pname = { "SubjectCode", "k_name", "BudgetTotal", "Adjustment", "ApprovedBudgetTotal", "Description" };
            //datas = this.GetBaseReport("", "547181121000001", "2018");
            ICellStyle[,] cellstyle = new ICellStyle[datas.Length, pname.Length];
            Dictionary<int, int> columnWidth = new Dictionary<int, int>();
            columnWidth.Add(0, 18 * 256);
            columnWidth.Add(1, 28 * 256);
            columnWidth.Add(2, 26 * 256);
            columnWidth.Add(3, 26 * 256);
            columnWidth.Add(4, 26 * 256);
            columnWidth.Add(5, 28 * 256);
            columnWidth.Add(6, 28 * 256);
            HSSFWorkbook workbook = new HSSFWorkbook();
            foreach (var t in datas.Where(t => !(t.SUBJECTCODE.StartsWith(Incom_Head) || (t.SUBJECTCODE.StartsWith(Expenditure_Head)))))
            {
                t.SUBJECTCODE = "";
                t.Layers = -1;
            }
            foreach (var t in datas.Where(t => (t.SUBJECTCODE.StartsWith(Incom_Head) || t.SUBJECTCODE.StartsWith(Expenditure_Head)) && (t.Layers > 0)))
            {
                t.SUBJECTCODE = "    " + t.SUBJECTCODE;
                t.SUBJECTNAME = "    " + t.SUBJECTNAME;
            }
            //i是行，j是列
            ICellStyle moneyStyle = null;
            for (int i = 0; i < datas.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (((pname[j].Equals("SUBJECTCODE") || pname[j].Equals("SUBJECTNAME")) && datas[i].Layers >= 0) || pname[j].Equals("DESCRIPTIONMIDDLE"))
                    {
                        cellstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 70, 12, true);
                        continue;
                    }
                    if ((pname[j].Equals("SUBJECTCODE") || pname[j].Equals("SUBJECTNAME")) && datas[i].Layers < 0)
                    {
                        cellstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                        continue;
                    }
                    moneyStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 70, 12, true);
                    IDataFormat dataFormat = workbook.CreateDataFormat();
                    moneyStyle.DataFormat = dataFormat.GetFormat("#,###0.00");
                    cellstyle[i, j] = moneyStyle;
                }
            }

            if (datas != null && datas.Count() > 0)
            {
                foreach (var da in datas)
                {
                    if (da.SUBJECTCODE.StartsWith("4B") || da.SUBJECTCODE.StartsWith("5B") || da.SUBJECTCODE.StartsWith("5Q"))
                    {
                        da.SUBJECTCODE = " ";
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            //获得表体数据
            ISheet Body = ExcelHelper.OutExcel<YsAccountModel>(datas, pname, 18 * 20, 10, cellstyle, new CellRangeAddress[0], workbook, columnWidth);


            BudgetExcelHead[] heads = new BudgetExcelHead[6];
            heads[0] = new BudgetExcelHead { SUBJECTCODE = "工会经费收支预算表(调整)" };
            heads[1] = new BudgetExcelHead();
            heads[2] = new BudgetExcelHead();
            heads[3] = new BudgetExcelHead { SUBJECTCODE = "编制单位：" + org.OName + "                  年度：" + DateTime.Now.Year + " 年            编报日期：" + DateTime.Now.Year + " 年" + DateTime.Now.Month + " 月" + DateTime.Now.Day + " 日          单位：元" };
            heads[4] = new BudgetExcelHead { SUBJECTCODE = "科        目", BUDGETTOTAL = "年初核定预算数", ADJUSTMENT = "预算调整数", APPROVEDBUDGETTOTAL = "调整后预算数", BudgetComplete ="与年初预算对比%", DESCRIPTIONMIDDLE = "说明" };
            heads[5] = new BudgetExcelHead { SUBJECTCODE = "编    码", SUBJECTNAME = "名    称" };
            //获得表头数据
            ICellStyle[,] headstyle = new ICellStyle[heads.Length, pname.Length];
            for (int i = 0; i < heads.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (i == 0 || i == 1 || i == 2)
                    {
                        headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 24, false);
                        continue;
                    }
                    if (i == 3)
                    {
                        headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, false);
                        continue;
                    }
                    headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                }
            }
            CellRangeAddress[] headRang = new CellRangeAddress[8];
            headRang[0] = new CellRangeAddress(0, 1, 0, 6);
            headRang[1] = new CellRangeAddress(3, 3, 0, 6);
            headRang[2] = new CellRangeAddress(4, 4, 0, 1);
            headRang[3] = new CellRangeAddress(4, 5, 3, 3);
            headRang[4] = new CellRangeAddress(4, 5, 2, 2);
            headRang[5] = new CellRangeAddress(4, 5, 4, 4);
            headRang[6] = new CellRangeAddress(4, 5, 5, 5);
            headRang[7] = new CellRangeAddress(4, 5, 6, 6);
            ISheet Head = ExcelHelper.OutExcel<BudgetExcelHead>(heads, pname, 18 * 20, 10, headstyle, new CellRangeAddress[0], workbook, columnWidth);

            //表尾数据
            BudgetExcelHead[] ends = new BudgetExcelHead[1];
            ends[0] = new BudgetExcelHead { SUBJECTCODE = "工会主席： " + userModel.UserName + "             经费审查委员会主任：" + userModel.UserName + "              财务负责人：" + userModel.UserName + "               制表人 ：" + userModel.UserName };
            ICellStyle[,] Endstyle = new ICellStyle[ends.Length, pname.Length];
            for (int i = 0; i < ends.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (i == 0)
                    {
                        Endstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, false);
                        continue;
                    }
                    Endstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                }
            }
            CellRangeAddress[] endRang = new CellRangeAddress[1];
            endRang[0] = new CellRangeAddress(0, 0, 0, 4);
            ISheet End = ExcelHelper.OutExcel<BudgetExcelHead>(ends, pname, 18 * 20, 10, Endstyle, new CellRangeAddress[0], workbook, columnWidth);
            End.AddMergedRegion(endRang[0]);
            //ISheet End = workbook.CreateSheet();
            //将表头表体合并
            ExcelHelper.Splice_Sheet(Head, Body, End, workbook, 18 * 20, 10, columnWidth, headRang, "工会经费收支预算表(调整)");
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\Middle";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { path = path, filename = filename });
        }


        /// <summary>
        /// 导出年末决算报表
        /// </summary>
        /// <param name="datas">数据集合</param>
        /// <param name="title">标题</param>
        /// <param name="userModel">用户对象</param>
        /// <param name="organizeModel">组织对象</param>
        /// <returns></returns>
        public String GetEndExcel(YsAccountModel[] datas, String[] title, User2Model userModel, OrganizeModel organizeModel)
        {
            //long orgid = 0;
            //if (datas.Length > 0)
            //{
            //    orgid = datas[0].OrgId;
            //}
            //var orgs = this.SysOrganizeFacade.Find(t => t.PhId == orgid).Data;
            //if (orgs.Count != 1)
            //{
            //    throw new Exception("组织对应不明确");
            //}
            //var org = orgs[0];

            var org = organizeModel;
            //获取制表人的信息
            //if (uid == "" || uid == null)
            //{
            //    throw new Exception("制表人对应不明确");
            //}
            //var user = this.SysUserFacade.Find(t => t.PhId == long.Parse(uid)).Data;
            //if (user.Count != 1)
            //{
            //    throw new Exception("制表人对应不明确");
            //}

            var Incom_Head = ConfigHelper.GetString("Incom").First().ToString();
            var Expenditure_Head = ConfigHelper.GetString("Expenditure").First().ToString();
            String[] pname = { "SUBJECTCODE", "SUBJECTNAME", "APPROVEDBUDGETTOTAL", "ThisaccountsTotal", "COMPLETE", "DESCRIPTIONEND" };
            //datas = this.GetBaseReport("", "547181121000001", "2018");
            ICellStyle[,] cellstyle = new ICellStyle[datas.Length, pname.Length];
            Dictionary<int, int> columnWidth = new Dictionary<int, int>();
            columnWidth.Add(0, 18 * 256);
            columnWidth.Add(1, 28 * 256);
            columnWidth.Add(2, 26 * 256);
            columnWidth.Add(3, 26 * 256);
            columnWidth.Add(4, 26 * 256);
            columnWidth.Add(5, 28 * 256);
            HSSFWorkbook workbook = new HSSFWorkbook();
            foreach (var t in datas.Where(t => !(t.SUBJECTCODE.StartsWith(Incom_Head) || (t.SUBJECTCODE.StartsWith(Expenditure_Head)))))
            {
                t.SUBJECTCODE = "";
                t.Layers = -1;
            }
            foreach (var t in datas.Where(t => (t.SUBJECTCODE.StartsWith(Incom_Head) || t.SUBJECTCODE.StartsWith(Expenditure_Head)) && (t.Layers > 0)))
            {
                t.SUBJECTCODE = "    " + t.SUBJECTCODE;
                t.SUBJECTNAME = "    " + t.SUBJECTNAME;
            }
            //i是行，j是列
            ICellStyle moneyStyle = null;
            for (int i = 0; i < datas.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (((pname[j].Equals("SUBJECTCODE") || pname[j].Equals("SUBJECTNAME")) && datas[i].Layers >= 0) || pname[j].Equals("DESCRIPTIONEND"))
                    {
                        cellstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 70, 12, true);
                        continue;
                    }
                    if ((pname[j].Equals("SUBJECTCODE") || pname[j].Equals("SUBJECTNAME")) && datas[i].Layers < 0)
                    {
                        cellstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                        continue;
                    }
                    cellstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 70, 12, true);
                    moneyStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 70, 12, true);
                    IDataFormat dataFormat = workbook.CreateDataFormat();
                    moneyStyle.DataFormat = dataFormat.GetFormat("#,###0.00");
                    cellstyle[i, j] = moneyStyle;
                }
            }
            if(datas != null && datas.Count() > 0)
            {
                foreach (var da in datas)
                {
                    if(da.SUBJECTCODE.StartsWith("4B") || da.SUBJECTCODE.StartsWith("5B") || da.SUBJECTCODE.StartsWith("5Q"))
                    {
                        da.SUBJECTCODE = " ";
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //获得表体数据
            ISheet Body = ExcelHelper.OutExcel<YsAccountModel>(datas, pname, 18 * 20, 10, cellstyle, new CellRangeAddress[0], workbook, columnWidth);


            BudgetExcelHead[] heads = new BudgetExcelHead[6];
            heads[0] = new BudgetExcelHead { SUBJECTCODE = "工会经费收支决算表" };
            heads[1] = new BudgetExcelHead();
            heads[2] = new BudgetExcelHead();
            heads[3] = new BudgetExcelHead { SUBJECTCODE = "编制单位：" + org.OName + "                  年度：" + DateTime.Now.Year + "年             编报日期：" + DateTime.Now.Year + " 年" + DateTime.Now.Month + " 月" + DateTime.Now.Day + " 日          单位：元" };
            heads[4] = new BudgetExcelHead { SUBJECTCODE = "科        目", APPROVEDBUDGETTOTAL = "核定预算数", ThisaccountsTotal = "决算数", COMPLETE = "完成决算(%)", DESCRIPTIONEND = "说明" };
            heads[5] = new BudgetExcelHead { SUBJECTCODE = "编    码", SUBJECTNAME = "名    称" };
            //获得表头数据
            ICellStyle[,] headstyle = new ICellStyle[heads.Length, pname.Length];
            for (int i = 0; i < heads.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (i == 0 || i == 1 || i == 2)
                    {
                        headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 24, false);
                        continue;
                    }
                    if (i == 3)
                    {
                        headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, false);
                        continue;
                    }
                    headstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                }
            }
            CellRangeAddress[] headRang = new CellRangeAddress[7];
            headRang[0] = new CellRangeAddress(0, 1, 0, 5);
            headRang[1] = new CellRangeAddress(3, 3, 0, 5);
            headRang[2] = new CellRangeAddress(4, 4, 0, 1);
            headRang[3] = new CellRangeAddress(4, 5, 3, 3);
            headRang[4] = new CellRangeAddress(4, 5, 2, 2);
            headRang[5] = new CellRangeAddress(4, 5, 4, 4);
            headRang[6] = new CellRangeAddress(4, 5, 5, 5);
            ISheet Head = ExcelHelper.OutExcel<BudgetExcelHead>(heads, pname, 18 * 20, 10, headstyle, new CellRangeAddress[0], workbook, columnWidth);

            //表尾数据
            BudgetExcelHead[] ends = new BudgetExcelHead[1];
            ends[0] = new BudgetExcelHead { SUBJECTCODE = "工会主席： " + userModel.UserName + "            经费审查委员会主任：" + userModel.UserName + "              财务负责人：" + userModel.UserName + "             制表 ：" + userModel.UserName };
            ICellStyle[,] Endstyle = new ICellStyle[ends.Length, pname.Length];
            for (int i = 0; i < ends.Length; i++)
            {
                for (int j = 0; j < pname.Length; j++)
                {
                    if (i == 0)
                    {
                        Endstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, false);
                        continue;
                    }
                    Endstyle[i, j] = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 70, 12, true);
                }
            }
            CellRangeAddress[] endRang = new CellRangeAddress[1];
            endRang[0] = new CellRangeAddress(0, 0, 0, 4);
            ISheet End = ExcelHelper.OutExcel<BudgetExcelHead>(ends, pname, 18 * 20, 10, Endstyle, new CellRangeAddress[0], workbook, columnWidth);
            End.AddMergedRegion(endRang[0]);
            //ISheet End = workbook.CreateSheet();
            //将表头表体表尾合并
            ExcelHelper.Splice_Sheet(Head, Body, End, workbook, 18 * 20, 10, columnWidth, headRang, "工会经费收支决算表");
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\End";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { path = path, filename = filename });
        }
        #endregion

        #region//导出项目预算调整表

        public string ExportBudgetAdjustExcel(List<BudgetAdjustAnalyseModel> budgetAdjustAnalyses, OrganizeModel organize, User2Model user, IList<QtTableCustomizeModel> qtTableCustomizes)
        {

            //行索引
            int rowNumber = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("项目预算调整分析表（包含网报）");
            if(qtTableCustomizes != null && qtTableCustomizes.Count > 0)
            {
                qtTableCustomizes = qtTableCustomizes.ToList().FindAll(t => t.EnabledMark == (byte)0).OrderBy(t => t.ColumnSort).ToList();
                sheet.DefaultRowHeight = 18 * 20;
                sheet.DefaultColumnWidth = 12;
                for(int j = 0; j< qtTableCustomizes.Count; j++)
                {
                    sheet.SetColumnWidth(j, 4800);
                }
                //sheet.SetColumnWidth(0, 4800);
                //sheet.SetColumnWidth(1, 4800);
                //sheet.SetColumnWidth(2, 4800);
                //sheet.SetColumnWidth(3, 4800);
                //sheet.SetColumnWidth(4, 4800);
                //sheet.SetColumnWidth(5, 4800);
                //sheet.SetColumnWidth(6, 4800);
                //sheet.SetColumnWidth(7, 4800);
                //sheet.SetColumnWidth(8, 4800);
                //sheet.SetColumnWidth(9, 4800);
                //sheet.SetColumnWidth(10, 4800);
                //sheet.SetColumnWidth(11, 4800);
                //sheet.SetColumnWidth(12, 4800);
                //sheet.SetColumnWidth(13, 4800);
                //sheet.SetColumnWidth(14, 4800);
                //sheet.SetColumnWidth(15, 4800);
                //sheet.SetColumnWidth(16, 4800);
                //sheet.SetColumnWidth(17, 4800);
                //sheet.SetColumnWidth(18, 4800);

                //合并单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, qtTableCustomizes.Count-1));


                //标题单元格样式
                ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                //表头单元格样式
                ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                //内容单元格样式
                ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                //数字内容格式
                ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                //标题
                IRow row = sheet.CreateRow(rowNumber);
                row.Height = 20 * 20;
                ICell cell = row.CreateCell(0);
                cell.SetCellValue("项目预算调整分析表");
                cell.CellStyle = cellTitleStyle;
                rowNumber++;

                //表头
                row = sheet.CreateRow(rowNumber);

                for (int j = 0; j < qtTableCustomizes.Count; j++)
                {
                    cell = row.CreateCell(j);
                    cell.SetCellValue(qtTableCustomizes[j].ColumnName);
                    cell.CellStyle = cellHeadStyle;
                }

                //cell = row.CreateCell(0);
                //cell.SetCellValue("单位名称");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(1);
                //cell.SetCellValue("预算部门");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(2);
                //cell.SetCellValue("申报部门");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(3);
                //cell.SetCellValue("序号");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(4);
                //cell.SetCellValue("项目名称");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(5);
                //cell.SetCellValue("项目明细");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(6);
                //cell.SetCellValue("预算科目");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(7);
                //cell.SetCellValue("核算单位");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(8);
                //cell.SetCellValue("核定年初预算数");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(9);
                //cell.SetCellValue("调增数");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(10);
                //cell.SetCellValue("调减数");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(11);
                //cell.SetCellValue("调整后预算数");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(12);
                //cell.SetCellValue("核定预算数");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(13);
                //cell.SetCellValue("借款申请占用金额");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(14);
                //cell.SetCellValue("报销申请占用金额");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(15);
                //cell.SetCellValue("已用额度");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(16);
                //cell.SetCellValue("未用额度");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(17);
                //cell.SetCellValue("（含在途）执行率（%）");
                //cell.CellStyle = cellHeadStyle;
                //cell = row.CreateCell(18);
                //cell.SetCellValue("执行率（%）");
                //cell.CellStyle = cellHeadStyle;
                rowNumber++;

                double sum = 0;
                if (budgetAdjustAnalyses != null && budgetAdjustAnalyses.Count > 0)
                {
                    //budgetAdjustAnalyses = budgetAdjustAnalyses.OrderBy(t => t.FDeclarationUnit).OrderBy(t => t.FBudgetDept).OrderBy(t => t.FDeclarationDept).OrderBy(t => t.FProjCode).OrderBy(t => t.FDtlCode).ToList();
                    //表格内容
                    for (int i = 1; i <= budgetAdjustAnalyses.Count; i++)
                    {
                        //先开行
                        row = sheet.CreateRow(rowNumber);


                        for (int j = 0; j < qtTableCustomizes.Count; j++)
                        {
                            //在开列
                            cell = row.CreateCell(j);
                            //合并单元格
                            if (i > 1)
                            {
                                //第一列
                                if (qtTableCustomizes[j].ColumnCode == "FDeclarationUnit_EXName" && budgetAdjustAnalyses[i - 1].FDeclarationUnit_EXName.Equals(budgetAdjustAnalyses[i - 2].FDeclarationUnit_EXName))
                                {
                                    sheet.AddMergedRegion(new CellRangeAddress(j, j, rowNumber - 1, rowNumber));
                                }
                                //第二列
                                if (qtTableCustomizes[j].ColumnCode == "FBudgetDept_EXName" && budgetAdjustAnalyses[i - 1].FBudgetDept_EXName.Equals(budgetAdjustAnalyses[i - 2].FBudgetDept_EXName))
                                {
                                    sheet.AddMergedRegion(new CellRangeAddress(j, j, rowNumber - 1, rowNumber));
                                }
                                //第三列
                                if (qtTableCustomizes[j].ColumnCode == "FDeclarationDept_EXName" && budgetAdjustAnalyses[i - 1].FDeclarationDept_EXName.Equals(budgetAdjustAnalyses[i - 2].FDeclarationDept_EXName))
                                {
                                    sheet.AddMergedRegion(new CellRangeAddress(j, j, rowNumber - 1, rowNumber));
                                }
                                //第五列
                                if (qtTableCustomizes[j].ColumnCode == "FProjName" && budgetAdjustAnalyses[i - 1].FProjName.Equals(budgetAdjustAnalyses[i - 2].FProjName))
                                {
                                    sheet.AddMergedRegion(new CellRangeAddress(j, j, rowNumber - 1, rowNumber));
                                }
                            }
                            //每行
                            cell = row.CreateCell(0);
                            cell.SetCellValue(budgetAdjustAnalyses[i - 1].FDeclarationUnit_EXName);
                            cell.CellStyle = cellStyle2;
                            //cell = row.CreateCell(1);
                            //cell.SetCellValue(budgetAdjustAnalyses[i - 1].FBudgetDept_EXName);
                            //cell.CellStyle = cellStyle2;
                            //cell = row.CreateCell(2);
                            //cell.SetCellValue(budgetAdjustAnalyses[i - 1].FDeclarationDept_EXName);
                            //cell.CellStyle = cellStyle2;
                            //cell = row.CreateCell(3);
                            //cell.SetCellValue(i);
                            //cell.CellStyle = cellStyle2;
                            //cell = row.CreateCell(4);
                            //cell.SetCellValue(budgetAdjustAnalyses[i - 1].FProjName);
                            //cell.CellStyle = cellStyle2;

                            //cell = row.CreateCell(5);
                            //cell.SetCellValue(budgetAdjustAnalyses[i - 1].FDtlName);
                            //cell.CellStyle = cellStyle2;
                            //cell = row.CreateCell(6);
                            //cell.SetCellValue(budgetAdjustAnalyses[i - 1].FBudgetAccounts_EXName);
                            //cell.CellStyle = cellStyle2;
                            //cell = row.CreateCell(7);
                            //cell.SetCellValue(budgetAdjustAnalyses[i - 1].FAccountOrgName);
                            //cell.CellStyle = cellStyle2;
                            //cell = row.CreateCell(8);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FAmount);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(9);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FZAmount);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(10);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FJAmount);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(11);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FAmountEdit);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(12);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FAmountAfterEdit);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(13);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FLoanAmount);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(14);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FSubmitAmount);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(15);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FUseAmount);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(6);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FNoUseAmount);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(17);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FZTRate);
                            //cell.CellStyle = cellStyle4;
                            //cell = row.CreateCell(18);
                            //cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FRate);
                            //cell.CellStyle = cellStyle4;
                            rowNumber++;
                            sum += (double)budgetAdjustAnalyses[i - 1].FAmount;
                        }

                        
                    }
                }

                //合并单元格
                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 14));
                //土地部分
                row = sheet.CreateRow(rowNumber);
                cell = row.CreateCell(0);
                cell.SetCellValue("项目金额合计：" + sum);
                cell.CellStyle = cellHeadStyle;
                rowNumber++;

            }
            else
            {
                sheet.DefaultRowHeight = 18 * 20;
                sheet.DefaultColumnWidth = 12;
                sheet.SetColumnWidth(0, 4800);
                sheet.SetColumnWidth(1, 4800);
                sheet.SetColumnWidth(2, 4800);
                sheet.SetColumnWidth(3, 4800);
                sheet.SetColumnWidth(4, 4800);
                sheet.SetColumnWidth(5, 4800);
                sheet.SetColumnWidth(6, 4800);
                sheet.SetColumnWidth(7, 4800);
                sheet.SetColumnWidth(8, 4800);
                sheet.SetColumnWidth(9, 4800);
                sheet.SetColumnWidth(10, 4800);
                sheet.SetColumnWidth(11, 4800);
                sheet.SetColumnWidth(12, 4800);
                sheet.SetColumnWidth(13, 4800);
                sheet.SetColumnWidth(14, 4800);
                sheet.SetColumnWidth(15, 4800);
                sheet.SetColumnWidth(16, 4800);
                sheet.SetColumnWidth(17, 4800);
                sheet.SetColumnWidth(18, 4800);

                //合并单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 18));


                //标题单元格样式
                ICellStyle cellTitleStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 500, 14, false);
                ICellStyle cellTitleStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellTitleStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 400, 12, false);
                //表头单元格样式
                ICellStyle cellHeadStyle = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                //内容单元格样式
                ICellStyle cellStyle1 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle2 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, 400, 12, false);
                ICellStyle cellStyle3 = ExcelHelper.CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);
                //数字内容格式
                ICellStyle cellStyle4 = ExcelHelper.CreateStyle2(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, 400, 12, false);

                //标题
                IRow row = sheet.CreateRow(rowNumber);
                row.Height = 20 * 20;
                ICell cell = row.CreateCell(0);
                cell.SetCellValue("项目预算调整分析表");
                cell.CellStyle = cellTitleStyle;
                rowNumber++;

                //表头
                row = sheet.CreateRow(rowNumber);
                cell = row.CreateCell(0);
                cell.SetCellValue("单位名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(1);
                cell.SetCellValue("预算部门");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(2);
                cell.SetCellValue("申报部门");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(3);
                cell.SetCellValue("序号");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(4);
                cell.SetCellValue("项目名称");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(5);
                cell.SetCellValue("项目明细");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(6);
                cell.SetCellValue("预算科目");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(7);
                cell.SetCellValue("核算单位");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(8);
                cell.SetCellValue("核定年初预算数");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(9);
                cell.SetCellValue("调增数");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(10);
                cell.SetCellValue("调减数");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(11);
                cell.SetCellValue("调整后预算数");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(12);
                cell.SetCellValue("核定预算数");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(13);
                cell.SetCellValue("借款申请占用金额");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(14);
                cell.SetCellValue("报销申请占用金额");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(15);
                cell.SetCellValue("已用额度");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(16);
                cell.SetCellValue("未用额度");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(17);
                cell.SetCellValue("（含在途）执行率（%）");
                cell.CellStyle = cellHeadStyle;
                cell = row.CreateCell(18);
                cell.SetCellValue("执行率（%）");
                cell.CellStyle = cellHeadStyle;
                rowNumber++;

                double sum = 0, sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0, sum5 = 0, sum6 = 0, sum7 = 0, sum8 = 0, sum9 = 0, sum10 = 0;
                if (budgetAdjustAnalyses != null && budgetAdjustAnalyses.Count > 0)
                {
                    //budgetAdjustAnalyses = budgetAdjustAnalyses.OrderBy(t => t.FDeclarationUnit).OrderBy(t => t.FBudgetDept).OrderBy(t => t.FDeclarationDept).OrderBy(t => t.FProjCode).OrderBy(t => t.FDtlCode).ToList();
                    //表格内容
                    for (int i = 1; i <= budgetAdjustAnalyses.Count; i++)
                    {
                        row = sheet.CreateRow(rowNumber);
                        //合并单元格
                        if (i > 1)
                        {
                            //第一列
                            if(budgetAdjustAnalyses[i - 1].FDeclarationUnit_EXName.Equals(budgetAdjustAnalyses[i - 2].FDeclarationUnit_EXName))
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(0, 0, rowNumber-1, rowNumber));
                            }
                            //第二列
                            if (budgetAdjustAnalyses[i - 1].FBudgetDept_EXName.Equals(budgetAdjustAnalyses[i - 2].FBudgetDept_EXName))
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(1, 1, rowNumber - 1, rowNumber));
                            }
                            //第三列
                            if (budgetAdjustAnalyses[i - 1].FDeclarationDept_EXName.Equals(budgetAdjustAnalyses[i - 2].FDeclarationDept_EXName))
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(2, 2, rowNumber - 1, rowNumber));
                            }
                            //第五列
                            if (budgetAdjustAnalyses[i - 1].FProjName.Equals(budgetAdjustAnalyses[i - 2].FProjName))
                            {
                                sheet.AddMergedRegion(new CellRangeAddress(4, 4, rowNumber - 1, rowNumber));
                            }
                        }
                        //每行
                        cell = row.CreateCell(0);
                        cell.SetCellValue(budgetAdjustAnalyses[i - 1].FDeclarationUnit_EXName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(1);
                        cell.SetCellValue(budgetAdjustAnalyses[i - 1].FBudgetDept_EXName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(2);
                        cell.SetCellValue(budgetAdjustAnalyses[i - 1].FDeclarationDept_EXName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(3);
                        cell.SetCellValue(i);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(4);
                        cell.SetCellValue(budgetAdjustAnalyses[i - 1].FProjName);
                        cell.CellStyle = cellStyle2;

                        cell = row.CreateCell(5);
                        cell.SetCellValue(budgetAdjustAnalyses[i - 1].FDtlName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(6);
                        cell.SetCellValue(budgetAdjustAnalyses[i - 1].FBudgetAccounts_EXName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(7);
                        cell.SetCellValue(budgetAdjustAnalyses[i - 1].FAccountOrgName);
                        cell.CellStyle = cellStyle2;
                        cell = row.CreateCell(8);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FAmount);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(9);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FZAmount);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(10);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FJAmount);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(11);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FAmountEdit);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(12);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FAmountAfterEdit);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(13);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FLoanAmount);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(14);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FSubmitAmount);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(15);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FUseAmount);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(6);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FNoUseAmount);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(17);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FZTRate);
                        cell.CellStyle = cellStyle4;
                        cell = row.CreateCell(18);
                        cell.SetCellValue((double)budgetAdjustAnalyses[i - 1].FRate);
                        cell.CellStyle = cellStyle4;
                        rowNumber++;
                        sum += (double)budgetAdjustAnalyses[i - 1].FAmount;
                    }
                }

                //合并单元格
                sheet.AddMergedRegion(new CellRangeAddress(rowNumber, rowNumber, 0, 14));
                //土地部分
                row = sheet.CreateRow(rowNumber);
                cell = row.CreateCell(0);
                cell.SetCellValue("项目金额合计：" + sum);
                cell.CellStyle = cellHeadStyle;
                rowNumber++;
              
            }
            

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var buf = ms.ToArray();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\YProjectMst";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
                fs.Close();
            }
            workbook = null;
            ms.Close();
            ms.Dispose();

            return JsonConvert.SerializeObject(new { path = "YProjectMst", filename = filename });
        }
        #endregion
    }
}

