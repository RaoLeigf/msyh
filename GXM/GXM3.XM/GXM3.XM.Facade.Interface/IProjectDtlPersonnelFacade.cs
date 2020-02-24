#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade.Interface
    * 类 名 称：			IProjectDtlPersonnelFacade
    * 文 件 名：			IProjectDtlPersonnelFacade.cs
    * 创建时间：			2020/1/6 
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade.Interface
{
	/// <summary>
	/// ProjectDtlPersonnel业务组装层接口
	/// </summary>
    public partial interface IProjectDtlPersonnelFacade : IEntFacadeBase<ProjectDtlPersonnelModel>
    {
        #region IProjectDtlPersonnelFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectDtlPersonnelModel> ExampleMethod<ProjectDtlPersonnelModel>(string param)


        /// <summary>
        /// 保存人员分摊数据
        /// </summary>
        /// <param name="projectDtlPersonnels">人员分摊数据集合</param>
        /// <param name="phid">主键id</param>
        /// <returns></returns>
        SavedResult<long> SaveMSYHPersonnels(List<ProjectDtlPersonnelModel> projectDtlPersonnels, long phid);
        #endregion
    }
}
