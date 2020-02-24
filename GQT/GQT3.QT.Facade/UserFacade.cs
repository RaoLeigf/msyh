#region Summary
/**************************************************************************************
    * 类 名 称：        UserFacade
    * 命名空间：        GQT3.QT.Facade
    * 文 件 名：        UserFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Facade
{
	/// <summary>
	/// User业务组装处理类
	/// </summary>
    public partial class UserFacade : EntFacadeBase<User2Model>, IUserFacade
    {
		#region 类变量及属性
		/// <summary>
        /// User业务逻辑处理对象
        /// </summary>
		IUserRule UserRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IUserRule;
            }
        }
		/// <summary>
        /// User业务逻辑处理对象
        /// </summary>
		//IUserRule UserRule { get; set; }
		/// <summary>
        /// Organization业务逻辑处理对象
        /// </summary>
		//IOrganizationRule OrganizationRule { get; set; }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<User2Model> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<User2Model> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<UserModel>(findedResults.Data, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<UserModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			UserRule.RuleHelper.DeleteByForeignKey(id);
			//OrganizationRule.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			UserRule.RuleHelper.DeleteByForeignKey(ids);
			//OrganizationRule.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IUserFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<UserModel> ExampleMethod<UserModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="userEntity"></param>
		/// <param name="userEntities"></param>
		/// <param name="organizationEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveUser(User2Model userEntity, List<User2Model> userEntities, List<OrganizeModel> organizationEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(userEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (userEntities.Count > 0)
				{
					UserRule.Save(userEntities, savedResult.KeyCodes[0]);
				}
				if (organizationEntities.Count > 0)
				{
					//OrganizationRule.Save(organizationEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        /// <summary>
        /// 通过代码找名称
        /// </summary>
        /// <param name="Dm"></param>
        /// <returns></returns>
        public string FindMcByDm(string Dm)
        {
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                   .Add(ORMRestrictions<string>.Eq("UserNo", Dm));
            IList<User2Model> users = UserRule.Find(dicWhere);
            if (users.Count > 0)
            {
                return users[0].UserName;
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}

