#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade.Interface
    * 类 名 称：			IQTIndividualInfoFacade
    * 文 件 名：			IQTIndividualInfoFacade.cs
    * 创建时间：			2019/5/14 
    * 作    者：			董泉伟    
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

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// QTIndividualInfo业务组装层接口
	/// </summary>
    public partial interface IQTIndividualInfoFacade : IEntFacadeBase<QTIndividualInfoModel>
    {
        #region IQTIndividualInfoFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QTIndividualInfoModel> ExampleMethod<QTIndividualInfoModel>(string param)

        /// <summary>
        /// 保存自定义表单跟金额关联设置
        /// </summary>
        /// <param name="templePhid"></param>
        /// <param name="phid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        int SaveTemple(long templePhid, string bustype, long phid);
        /// <summary>
        /// 删除自定义表单跟金额关联设置
        /// </summary>
        /// <param name="phid"></param>
        ///  <param name="bustype"></param>
        /// <returns></returns>
        int DeleteTemple(string bustype, long phid);

        #endregion
    }
}
