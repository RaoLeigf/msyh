#region Summary
/**************************************************************************************
    * 类 名 称：        IUserFacade
    * 命名空间：        GQT3.QT.Facade.Interface
    * 文 件 名：        IUserFacade.cs
    * 创建时间：        2018/9/13 
    * 作    者：        夏华军    
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
	/// User业务组装层接口
	/// </summary>
    public partial interface IUserFacade : IEntFacadeBase<User2Model>
    {
		#region IUserFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<UserModel> ExampleMethod<UserModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="userEntity"></param>
		/// <param name="userEntities"></param>
		/// <param name="organizationEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveUser(User2Model userEntity, List<User2Model> userEntities, List<OrganizeModel> organizationEntities);

        /// <summary>
        /// 通过代码找名称
        /// </summary>
        /// <param name="Dm"></param>
        /// <returns></returns>
        string FindMcByDm(string Dm);
        #endregion
    }
}
