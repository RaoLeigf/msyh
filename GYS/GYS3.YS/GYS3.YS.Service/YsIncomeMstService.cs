#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Service
    * 类 名 称：			YsIncomeMstService
    * 文 件 名：			YsIncomeMstService.cs
    * 创建时间：			2019/12/31 
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
using SUP.Common.Base;
using GQT3.QT.Model.Domain;
using GQT3.QT.Facade.Interface;

namespace GYS3.YS.Service
{
	/// <summary>
	/// YsIncomeMst服务组装处理类
	/// </summary>
    public partial class YsIncomeMstService : EntServiceBase<YsIncomeMstModel>, IYsIncomeMstService
    {
		#region 类变量及属性
		/// <summary>
        /// YsIncomeMst业务外观处理对象
        /// </summary>
		IYsIncomeMstFacade YsIncomeMstFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IYsIncomeMstFacade;
            }
        }

		/// <summary>
        /// OrderDetails业务外观处理对象
        /// </summary>
		private IYsIncomeDtlFacade YsIncomeDtlFacade { get; set; }

        private IOrganizationFacade OrganizationFacade { get; set; }
		#endregion

		#region 实现 IYsIncomeMstService 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="ysIncomeMstEntity"></param>
		/// <param name="ysIncomeDtlEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveYsIncomeMst(YsIncomeMstModel ysIncomeMstEntity, List<YsIncomeDtlModel> ysIncomeDtlEntities)
        {
			return YsIncomeMstFacade.SaveYsIncomeMst(ysIncomeMstEntity, ysIncomeDtlEntities);
        }

        /// <summary>
        /// 通过外键值获取YsIncomeDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        public FindedResults<YsIncomeDtlModel> FindYsIncomeDtlByForeignKey<TValType>(TValType id)
        {
            return YsIncomeDtlFacade.FindByForeignKey(id);
        }

        /// <summary>
        /// 保存单个组织的收入预算
        /// </summary>
        /// <param name="ysIncomeMst">收入预算对象</param>
        /// <param name="ysIncomeDtls">收入预算子表集合</param>
        /// <returns></returns>
        public SavedResult<long> SaveYsIncome(YsIncomeMstModel ysIncomeMst, IList<YsIncomeDtlModel> ysIncomeDtls)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            //先保存主表数据
            //savedResult = this.YsIncomeMstFacade.Save<long>(ysIncomeMst, "");
            //if(savedResult.KeyCodes != null && savedResult.KeyCodes.Count > 0)
            //{
            //    //保存子表数据
            //    this.YsIncomeDtlFacade.Save<long>(ysIncomeDtls, savedResult.KeyCodes[0].ToString());
            //}

            savedResult = this.YsIncomeMstFacade.SaveYsIncomeMst(ysIncomeMst, ysIncomeDtls.ToList());
            return savedResult;
        }

        /// <summary>
        /// 删除收入预算数据
        /// </summary>
        /// <param name="phid">收入预算主键</param>
        /// <returns></returns>
        public DeletedResult SaveDelete(long phid)
        {
            DeletedResult deletedResult = new DeletedResult();
            var ysIncomes = this.YsIncomeMstFacade.Find(t=>t.PhId == phid).Data;
            if(ysIncomes != null && ysIncomes.Count == 1)
            {
                //未上报，未送审，未生成预算的数据才能删除
                if(ysIncomes[0].FApproval == (byte)0 && ysIncomes[0].FIsbudget==(byte)0&& ysIncomes[0].FIsreport == (byte)0)
                {
                    ysIncomes[0].PersistentState = PersistentState.Deleted;
                    deletedResult = this.YsIncomeMstFacade.Delete<long>(ysIncomes[0].PhId);//删除主从表数据
                }
                else
                {
                    throw new Exception("只有未上报，未送审以及未生成预算的收入预算才能删除！");
                }
            }
            else
            {
                throw new Exception("传递的收入预算主键有误！");
            }
            return deletedResult;
        }

        /// <summary>
        /// 根据收入预算主键生成预算
        /// </summary>
        /// <param name="phid">收入预算主键</param>
        /// <param name="userId">人员主键</param>
        /// <returns></returns>
        public SavedResult<long> SaveBudget(long phid, long userId)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            var ysIncomes = this.YsIncomeMstFacade.Find(t => t.PhId == phid).Data;
            if (ysIncomes != null && ysIncomes.Count == 1)
            {
                //审批通过而且未生成预算的数据才能生成预算
                if (ysIncomes[0].FApproval == (byte)9 && ysIncomes[0].FIsbudget == (byte)0)
                {
                    ysIncomes[0].FIsbudget = 1;
                    ysIncomes[0].FBudgeter = userId;
                    ysIncomes[0].FBudgettime = DateTime.Now;
                    ysIncomes[0].PersistentState = PersistentState.Modified;
                    savedResult = this.YsIncomeMstFacade.Save<long>(ysIncomes[0]);
                }
                else
                {
                    throw new Exception("只有审批通过而且未生成预算的收入预算才能删除！");
                }
            }
            else
            {
                throw new Exception("传递的收入预算主键有误！");
            }
            return savedResult;
        }

        /// <summary>
        /// 获取当前所有组织信息
        /// </summary>
        /// <returns></returns>
        public IList<OrganizeModel> GetAllOrganize()
        {
            IList<OrganizeModel> organizes = new List<OrganizeModel>();
            organizes = this.OrganizationFacade.Find(t => t.PhId != 0).Data;
            return organizes;
        }
        #endregion
    }
}

