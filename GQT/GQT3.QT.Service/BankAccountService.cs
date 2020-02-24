#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			BankAccountService
    * 文 件 名：			BankAccountService.cs
    * 创建时间：			2019/5/28 
    * 作    者：			刘杭    
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
using GGK3.GK.Facade.Interface;
using Enterprise3.Common.Base.Criterion;
using GGK3.GK.Model.Domain;

namespace GQT3.QT.Service
{
	/// <summary>
	/// BankAccount服务组装处理类
	/// </summary>
    public partial class BankAccountService : EntServiceBase<BankAccountModel>, IBankAccountService
    {
		#region 类变量及属性
		/// <summary>
        /// BankAccount业务外观处理对象
        /// </summary>
		IBankAccountFacade BankAccountFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IBankAccountFacade;
            }
        }

        private IOrganizationFacade OrganizationFacade { get; set; }

        private IGKPaymentDtlFacade GKPaymentDtlFacade { get; set; }
        #endregion

        #region 实现 IBankAccountService 业务添加的成员

        /// <summary>
        /// 增加组织名称传给前端
        /// </summary>
        /// <param name="bankAccounts"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public List<BankAccountModel> GetOrgName(List<BankAccountModel> bankAccounts, long OrgId)
        {
            OrganizeModel organize = OrganizationFacade.Find(OrgId).Data;
            foreach(BankAccountModel bankAccount in bankAccounts)
            {
                bankAccount.OrgName = organize.OName;
            }
            return bankAccounts;
        }

        /// <summary>
        /// 判断是否被引用（返回被引用的银行账户主键list）
        /// </summary>
        /// <param name="phids"></param>
        /// <returns></returns>
        public List<long> judgeIfUse(List<long> phids)
        {
            List<long> usePhids = new List<long>();//被引用的银行账户主键
            Dictionary<string, object> dic = new Dictionary<string, object>();
            new CreateCriteria(dic)
                .Add(ORMRestrictions<List<long>>.In("BankPhid", phids));
            IList<GKPaymentDtlModel> gKPaymentDtls = GKPaymentDtlFacade.Find(dic).Data;
            if (gKPaymentDtls.Count > 0)
            {
                foreach(GKPaymentDtlModel dtl in gKPaymentDtls)
                {
                    if (!usePhids.Contains(dtl.BankPhid))
                    {
                        usePhids.Add(dtl.BankPhid);
                    }
                }
            }
            return usePhids;
        }
        #endregion
    }
}

