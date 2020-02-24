#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IBankAccountService
    * 文 件 名：			IBankAccountService.cs
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// BankAccount服务组装层接口
	/// </summary>
    public partial interface IBankAccountService : IEntServiceBase<BankAccountModel>
    {
        #region IBankAccountService 业务添加的成员

        /// <summary>
        /// 增加组织名称传给前端
        /// </summary>
        /// <param name="bankAccounts"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        List<BankAccountModel> GetOrgName(List<BankAccountModel> bankAccounts, long OrgId);

        /// <summary>
        /// 判断是否被引用（返回被引用的银行账户主键list）
        /// </summary>
        /// <param name="phids"></param>
        /// <returns></returns>
        List<long> judgeIfUse(List<long> phids);
        #endregion
    }
}
