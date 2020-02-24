#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesFacade
    * 命名空间：        GQT3.QT.Facade
    * 文 件 名：        ExamplesFacade.cs
    * 创建时间：        2018/8/17 9:54:19 
    * 作    者：        丰立新    
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
using Enterprise3.Common.BizBase;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GQT3.QT.Model;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Facade.Interface;

namespace GQT3.QT.Facade
{
    /// <summary>
    /// Examples业务组装处理类
    /// </summary>
    public partial class ExamplesFacade : EntFacadeBase<ExamplesModel>, IExamplesFacade
    {
        IExamplesRule ExamplesRule
        {
            get
            {
                if (CurrentRule == null)
                    throw new ArgumentNullException(Resources.InitializeObject);

                return CurrentRule as IExamplesRule;
            }
        }

        #region 重载方法
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<ExamplesModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params object[] sorts)
        {
            PagedResult<ExamplesModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ExamplesModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ExamplesModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<ExamplesModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params object[] sorts)
        {
            PagedResult<ExamplesModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ExamplesModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ExamplesModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 保存数据
        /// <para>
        /// 此方法已经包含对DataHandling方法和DataCheck方法的调用
        /// </para>
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回保存对象实体</returns>
        public override SavedResult Save(ExamplesModel entity)
        {
            SavedResult savedResult;

            try
            {
                savedResult = base.Save(entity);

                #region 若单据保存未涉及到用户编码获取[执行过GetBillNo(true)]，请把下面这段代码注释掉
                if (ExamplesRule.ReqBillNoOrIdCommitEntity != null)
                {
                    ExamplesRule.CommitBillNo();//提交单据用户编码，此操作将单据用户编码永久占用
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region 若单据保存未涉及到用户编码获取[执行过GetBillNo(true)]，请把下面这段代码注释掉
                if (ExamplesRule.ReqBillNoOrIdCommitEntity != null)
                {
                    ExamplesRule.RollbackBillNo();//回滚单据用户编码，此操作将单据用户编码至回初始状态
                }
                #endregion

                throw ex;//截获异常后要再次抛出
            }

            return savedResult;
        }

        /// <summary>
        /// 数据集合数据
        /// <para>
        /// 此方法已经包含对DataHandling方法和DataCheck方法的调用
        /// </para>
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>返回保存对象实体</returns>
        public override SavedResult Save(List<ExamplesModel> entities)
        {
            SavedResult savedResult;

            try
            {
                savedResult = base.Save(entities);

                #region 若单据保存未涉及到用户编码获取[执行过GetBillNo(true)]，请把下面这段代码注释掉     
                if (ExamplesRule.ReqBillNoOrIdCommitEntity != null)
                {
                    ExamplesRule.CommitBillNo();//提交单据用户编码，此操作将单据用户编码永久占用
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region 若单据保存未涉及到用户编码获取[执行过GetBillNo(true)]，请把下面这段代码注释掉
                if (ExamplesRule.ReqBillNoOrIdCommitEntity != null)
                {
                    ExamplesRule.RollbackBillNo();//回滚单据用户编码，此操作将单据用户编码至回初始状态
                }
                #endregion

                throw ex;//截获异常后要再次抛出
            }

            return savedResult;
        }
        #endregion

        #region 实现 IExamplesFacade 业务添加的成员

        /// <summary>
        /// 获取所有有效的组织信息
        /// </summary>
        /// <returns>组织集合</returns>
        public IList<ExamplesModel> GetAllEffectiveOrgs()
        {
            return ExamplesRule.GetAllEffectiveOrgs();
        }

        /// <summary>
        /// 获取指定组织所有有效的部门信息
        /// </summary>
        /// <param name="ocode">组织编码（目前通过编码设置关系）</param>
        /// <returns></returns>
        public IList<ExamplesModel> GetAllEffectiveDepts(string ocode)
        {
            return ExamplesRule.GetAllEffectiveDepts(ocode);
        }


        #endregion
    }
}

