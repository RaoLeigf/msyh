#region Summary
/**************************************************************************************
    * 类 名 称：        IQTProductUserDygxService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IQTProductUserDygxService.cs
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QTProductUserDygx服务组装层接口
	/// </summary>
    public partial interface IQTProductUserDygxService : IEntServiceBase<QTProductUserDygxModel>
    {
        #region IQTProductUserDygxService 业务添加的成员

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="User"></param>
        /// <param name="adddata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        CommonResult Save2(User2Model User, List<QTProductUserDygxModel> adddata, List<string> deletedata);


        /// <summary>
        /// 判断是否设置过该产品的对应账号
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        CommonResult JudgeByProduct(string ProductBZ);

        /// <summary>
        /// 删除后设置对应关系
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        CommonResult SaveByProductIfDelete(string ProductBZ);

        /// <summary>
        /// 不删除设置对应关系
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        CommonResult SaveByProductNoDelete(string ProductBZ);

        /// <summary>
        /// 没有该产品对应关系设置对应关系
        /// </summary>
        /// <param name="ProductBZ"></param>
        /// <returns></returns>
        CommonResult SaveByProduct(string ProductBZ);
        #endregion
    }
}
