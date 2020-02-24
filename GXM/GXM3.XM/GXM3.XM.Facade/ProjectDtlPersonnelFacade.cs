#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Facade
    * 类 名 称：			ProjectDtlPersonnelFacade
    * 文 件 名：			ProjectDtlPersonnelFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GXM3.XM.Facade.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade
{
	/// <summary>
	/// ProjectDtlPersonnel业务组装处理类
	/// </summary>
    public partial class ProjectDtlPersonnelFacade : EntFacadeBase<ProjectDtlPersonnelModel>, IProjectDtlPersonnelFacade
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectDtlPersonnel业务逻辑处理对象
        /// </summary>
		IProjectDtlPersonnelRule ProjectDtlPersonnelRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectDtlPersonnelRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<ProjectDtlPersonnelModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<ProjectDtlPersonnelModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<ProjectDtlPersonnelModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<ProjectDtlPersonnelModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        #endregion

        #region 实现 IProjectDtlPersonnelFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlPersonnelModel> ExampleMethod<ProjectDtlPersonnelModel>(string param)
        //{
        //    //编写代码
        //}


        /// <summary>
        /// 保存人员分摊数据
        /// </summary>
        /// <param name="projectDtlPersonnels">人员分摊数据集合</param>
        /// <param name="phid">主键id</param>
        /// <returns></returns>
        public SavedResult<long> SaveMSYHPersonnels(List<ProjectDtlPersonnelModel> projectDtlPersonnels, long phid)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            savedResult = this.ProjectDtlPersonnelRule.Save(projectDtlPersonnels, phid);
            return savedResult;
        }
        #endregion
    }
}

