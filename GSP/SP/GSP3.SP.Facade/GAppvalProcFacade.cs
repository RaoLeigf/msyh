#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Facade
    * 类 名 称：			GAppvalProcFacade
    * 文 件 名：			GAppvalProcFacade.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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

using GSP3.SP.Facade.Interface;
using GSP3.SP.Rule.Interface;
using GSP3.SP.Model.Domain;
using Enterprise3.WebApi.GSP3.SP.Model.Common;
using System.Data;
using GSP3.SP.Model.Extra;
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;
using GBK3.BK.Rule.Interface;
using GGK3.GK.Rule.Interface;
using GSP3.SP.Model.Enums;

namespace GSP3.SP.Facade
{
	/// <summary>
	/// GAppvalProc业务组装处理类
	/// </summary>
    public partial class GAppvalProcFacade : EntFacadeBase<GAppvalProcModel>, IGAppvalProcFacade
    {
		#region 类变量及属性
		/// <summary>
        /// GAppvalProc业务逻辑处理对象
        /// </summary>
		IGAppvalProcRule GAppvalProcRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IGAppvalProcRule;
            }
        }
		/// <summary>
        /// GAppvalProcConds业务逻辑处理对象
        /// </summary>
		IGAppvalProcCondsRule GAppvalProcCondsRule { get; set; }

        IGAppvalProc4PostRule GAppvalProc4PostRule { get; set; }

        IPaymentMstRule PaymentMstRule { get; set; }

        IGKPaymentMstRule GKPaymentMstRule { get; set; }

        IGAppvalRecordRule GAppvalRecordRule { get; set; }
        #endregion

        #region 实现 IGAppvalProcFacade 业务添加的成员

        /// <summary>
        /// 根据组织id，单据类型，审批类型获取所有的审批流程
        /// </summary>
        /// <param name="orgids">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        public List<GAppvalProcModel> GetAppvalProc(List<long> orgids, string bType, long splx_phid) {
            List<GAppvalProcModel> procModels = new List<GAppvalProcModel>();

            if (orgids == null || orgids.Count == 0 || string.IsNullOrEmpty(bType))
            {
                return procModels;
            }

            string orgid = "";
            foreach (long id in orgids) {
                orgid = orgid + id + ",";
            }

            SqlDao sqlDao = new SqlDao();
            DataTable dataTable = sqlDao.GetAppvalProc(orgid.Substring(0,orgid.Length-1),bType, splx_phid);
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return procModels;
            }
            else {
                procModels = DCHelper.DataTable2List<GAppvalProcModel>(dataTable).ToList();
            }

            return procModels;
        }

        /// <summary>
        /// 根据组织id，单据类型，审批类型,单据主键获取所有的符合条件的审批流程
        /// </summary>
        /// <param name="orgids">组织id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="splx_phid">审批类型id</param>
        /// <param name="bPhIds">主键结合</param>
        /// <returns></returns>
        public List<GAppvalProcModel> GetAppvalProcList(List<long> orgids, string bType, long splx_phid, List<long> bPhIds)
        {
            List<GAppvalProcModel> procModels = new List<GAppvalProcModel>();
            List<GAppvalProcModel> procModelList = new List<GAppvalProcModel>();

            if (orgids == null || orgids.Count == 0 || string.IsNullOrEmpty(bType))
            {
                return procModels;
            }

            string orgid = "";
            foreach (long id in orgids)
            {
                orgid = orgid + id + ",";
            }

            SqlDao sqlDao = new SqlDao();
            DataTable dataTable = sqlDao.GetAppvalProc(orgid.Substring(0, orgid.Length - 1), bType, splx_phid);
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return procModels;
            }
            else
            {
                procModels = DCHelper.DataTable2List<GAppvalProcModel>(dataTable).ToList();
            }
            if(bPhIds.Count > 0)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                new CreateCriteria(dic)
                    .Add(ORMRestrictions<List<long>>.In("PhId", bPhIds));
                if(bType == BillType.FundsPay)
                {
                    var payment = this.PaymentMstRule.Find(dic);
                    if(payment.Count > 0)
                    {
                        decimal minAmount = payment.OrderBy(t=>t.FAmountTotal).ToList()[0].FAmountTotal;
                        decimal maxAmount = payment.OrderByDescending(t => t.FAmountTotal).ToList()[0].FAmountTotal;
                        foreach (var proc in procModels)
                        {
                            if(RetainProc(proc.PhId, minAmount, maxAmount))
                            {
                                procModelList.Add(proc);
                            }
                        }
                    }
                    else
                    {
                        procModelList = procModels;
                    }
                }
                else if(bType == BillType.PayMent)
                {
                    var gPayment = this.GKPaymentMstRule.Find(dic);
                    if (gPayment.Count > 0)
                    {
                        decimal minAmount = gPayment.OrderBy(t => t.FAmountTotal).ToList()[0].FAmountTotal;
                        decimal maxAmount = gPayment.OrderByDescending(t => t.FAmountTotal).ToList()[0].FAmountTotal;
                        foreach (var proc in procModels)
                        {
                            if (RetainProc(proc.PhId, minAmount, maxAmount))
                            {
                                procModelList.Add(proc);
                            }
                        }
                    }
                    else
                    {
                        procModelList = procModels;
                    }
                }
                else
                {
                    procModelList = procModels;
                }

            }
            else
            {
                procModelList = procModels;
            }
            return procModelList;
        }

        #region//新的获取批量审批流程的接口
        //public List<GAppvalProcModel> GetNewAppvalProcList(List<long> orgids, string bType, long splx_phid, List<long> bPhIds)
        //{
        //    List<GAppvalProcModel> procModels = new List<GAppvalProcModel>();
        //    List<GAppvalProcModel> procModelList = new List<GAppvalProcModel>();

        //    if (orgids == null || orgids.Count == 0 || string.IsNullOrEmpty(bType))
        //    {
        //        return procModels;
        //    }

        //    string orgid = "";
        //    foreach (long id in orgids)
        //    {
        //        orgid = orgid + id + ",";
        //    }

        //    SqlDao sqlDao = new SqlDao();
        //    DataTable dataTable = sqlDao.GetAppvalProc(orgid.Substring(0, orgid.Length - 1), bType, splx_phid);
        //    if (dataTable == null || dataTable.Rows.Count == 0)
        //    {
        //        return procModels;
        //    }
        //    else
        //    {
        //        procModels = DCHelper.DataTable2List<GAppvalProcModel>(dataTable).ToList();
        //    }
        //    if (bPhIds.Count > 0)
        //    {
        //        Dictionary<string, object> dic = new Dictionary<string, object>();
        //        new CreateCriteria(dic)
        //            .Add(ORMRestrictions<List<long>>.In("PhId", bPhIds));
        //        if (bType == BillType.FundsPay)
        //        {
        //            var payment = this.PaymentMstRule.Find(dic);
        //            if (payment.Count > 0)
        //            {
        //                decimal minAmount = payment.OrderBy(t => t.FAmountTotal).ToList()[0].FAmountTotal;
        //                decimal maxAmount = payment.OrderByDescending(t => t.FAmountTotal).ToList()[0].FAmountTotal;
        //                foreach (var proc in procModels)
        //                {
        //                    if (RetainProc(proc.PhId, minAmount, maxAmount))
        //                    {
        //                        procModelList.Add(proc);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                procModelList = procModels;
        //            }
        //        }
        //        else if (bType == BillType.PayMent)
        //        {
        //            var gPayment = this.GKPaymentMstRule.Find(dic);
        //            if (gPayment.Count > 0)
        //            {
        //                decimal minAmount = gPayment.OrderBy(t => t.FAmountTotal).ToList()[0].FAmountTotal;
        //                decimal maxAmount = gPayment.OrderByDescending(t => t.FAmountTotal).ToList()[0].FAmountTotal;
        //                foreach (var proc in procModels)
        //                {
        //                    if (RetainProc(proc.PhId, minAmount, maxAmount))
        //                    {
        //                        procModelList.Add(proc);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                procModelList = procModels;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        procModelList = procModels;
        //    }

        //    return procModelList;
        //}
        #endregion

        /// <summary>
        /// 通过流程与金额判断是否符合条件
        /// </summary>
        /// <param name="procPhid">流程主键</param>
        /// <param name="minAmount">最小金额</param>
        /// <param name="maxAmout">最大金额</param>
        /// <returns></returns>
        public bool RetainProc(long procPhid, decimal minAmount, decimal maxAmout)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<long>.Eq("ProcPhid", procPhid));
            var procConds = this.GAppvalProcCondsRule.Find(dic);
            string operator1, operator2;
            decimal operand1, operand2;
            if (procConds.Count == 1)
            {
                operator1 = procConds[0].FOperator;
                operand1 = decimal.Parse(procConds[0].FOperand2);
                if (operator1.Equals("<="))
                {
                    if(maxAmout <= operand1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (minAmount >= operand1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (procConds.Count == 2)
            {
                operator1 = procConds[0].FOperator;
                operand1 = decimal.Parse(procConds[0].FOperand2);
                operator2 = procConds[1].FOperator;
                operand2 = decimal.Parse(procConds[1].FOperand2);
                if (procConds[0].FConnector.Equals("and") || procConds[0].FConnector.Equals("and"))
                {
                    if(operator1.Equals("<=") && operator2.Equals(">=") && operand1 >= operand2)
                    {
                        if(minAmount >= operand2 && maxAmout <= operand1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }else if (operator2.Equals("<=") && operator1.Equals(">=") && operand1 <= operand2)
                    {
                        if (minAmount >= operand1 && maxAmout <= operand2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        throw new Exception("流程条件参数保存有误，请联系管理员！");
                    }
                }
                else
                {
                    throw new Exception("流程条件参数保存有误，请联系管理员！");
                }
            }
            else
            {
                return true;
            }
        }



        /// <summary>
        /// 根据审批类型，单据类型，审批流程编码获取审批流程
        /// </summary>
        /// <param name="approvalTypeId">审批类型id</param>
        /// <param name="bType">单据类型</param>
        /// <param name="procCode">审批流程编码</param>
        /// <returns></returns>
        public GAppvalProcModel GetAppvalProc(long approvalTypeId, string bType, string procCode) {
            GAppvalProcModel procModel = null;

            SqlDao sqlDao = new SqlDao();

            DataTable dataTable = sqlDao.GetAppvalProc(approvalTypeId, bType, procCode);
            if (dataTable == null || dataTable.Rows.Count == 0)
                procModel = new GAppvalProcModel();
            else {
                IList<GAppvalProcModel> procModels = DCHelper.DataTable2List<GAppvalProcModel>(dataTable);
                List<Organize> organizes = new List<Organize>();
                foreach (GAppvalProcModel model in procModels) {
                    Organize organize = new Organize();
                    organize.OrgId = model.OrgPhid;
                    organize.OrgCode = model.OrgCode;
                    organize.OrgName = model.OrgName;
                    organizes.Add(organize);
                }
                procModel = procModels[0];
                procModel.Organizes = organizes;
            }

            return procModel;
        }


        /// <summary>
        /// 判断审批流程是否被引用
        /// </summary>
        /// <param name="splx_phid">审批类型id</param>
        /// <returns></returns>
        public bool ProcIsUsed(long splx_phid) {
            bool symbol = false;
            if (splx_phid == 0)
                return symbol;

            SqlDao sqlDao = new SqlDao();
            DataTable dataTable = sqlDao.ProcIsUsed(splx_phid);
            if (dataTable != null && dataTable.Rows.Count > 0) {
                symbol = true;
            }

            return symbol;
        }

        /// <summary>
        /// 批量删除审批流程
        /// </summary>
        /// <param name="procModels"></param>
        /// <returns></returns>
        public int DeleteAppvalProc(IList<GAppvalProcModel> procModels) {
            int result = 0;
            if (procModels != null && procModels.Count > 0)
            {
                List<GAppvalProcModel> models = procModels.ToList();
                List<long> procIds = models.Select(t => t.PhId).ToList();
                //加入判断，若审批流在审批中心已经被调用，则不能删除
                if(procIds != null && procIds.Count > 0)
                {
                    var records = this.GAppvalRecordRule.Find(t => procIds.Contains(t.PhId));
                    if(records != null && records.Count > 0)
                    {
                        throw new Exception("审批流已被调用，无法进行更改！");
                    }
                }


                //删除审批流程与审批岗位的对应关系
                Dictionary<string, object> where = new Dictionary<string, object>();
                new CreateCriteria(where)
                    .Add(ORMRestrictions<List<long>>.In("ProcPhid", procIds));
                result += GAppvalProc4PostRule.Delete(where);

                //删除审批流程条件
                Dictionary<string, object> where2 = new Dictionary<string, object>();
                new CreateCriteria(where2)
                    .Add(ORMRestrictions<List<long>>.In("ProcPhid", procIds));
                result += GAppvalProcCondsRule.Delete(where2);

                //删除审批流程
                Dictionary<string, object> where3 = new Dictionary<string, object>();
                new CreateCriteria(where3)
                    .Add(ORMRestrictions<List<long>>.In("PhId", procIds));
                result += GAppvalProcRule.Delete(where3);
            }
            return result;
        }

        /// <summary>
        /// 保存审批流程
        /// </summary>
        /// <param name="procModel"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveAppvalProc(GAppvalProcModel procModel) {

            if (procModel.Proc4PostModels == null || procModel.Proc4PostModels.Count == 0)
            {
                throw new Exception("审批流程与审批岗位的对应关系为空！");
            }

            //判断审批流程编码，名称是否重复
            IList<GAppvalProcModel> findedResults = GAppvalProcRule.Find(t => (t.FCode == procModel.FCode || t.FName == procModel.FName) && t.SPLXPhid == procModel.SPLXPhid && t.OrgPhid == procModel.OrgPhid);
            if (findedResults != null && findedResults.Count > 0)
            {
                throw new Exception("流程代码或流程名称重复！");
            }

            //保存审批流程主表
            if(procModel.Ucode == "Admin")
            {
                procModel.IsSystem = (byte)1;
            }
            else
            {
                procModel.IsSystem = (byte)0;
            }
            procModel.PersistentState = PersistentState.Added;
            SavedResult<Int64> savedResult = GAppvalProcRule.Save<Int64>(procModel);

            //保存审批流程和岗位的对应关系
            List<GAppvalProc4PostModel> proc4PostModels = procModel.Proc4PostModels;
            foreach (GAppvalProc4PostModel model in proc4PostModels)
            {
                model.ProcPhid = savedResult.KeyCodes[0];
                model.PersistentState = PersistentState.Added;
            }
            GAppvalProc4PostRule.Save<Int64>(proc4PostModels);

            //保存审批流程条件
            if (procModel.CondsModels != null && procModel.CondsModels.Count > 0)
            {
                foreach (GAppvalProcCondsModel conds in procModel.CondsModels)
                {
                    conds.ProcPhid = savedResult.KeyCodes[0];
                    conds.PersistentState = PersistentState.Added;
                }
                GAppvalProcCondsRule.Save<Int64>(procModel.CondsModels);
            }

            return savedResult;
        }
        #endregion
    }
}

