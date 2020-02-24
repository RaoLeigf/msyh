#region Summary
/**************************************************************************************
    * 类 名 称：        ExtendFunctionMstFacade
    * 命名空间：        NG3.Addin.Facade
    * 文 件 名：        ExtendFunctionMstFacade.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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

using NG3.Addin.Facade.Interface;
using NG3.Addin.Rule.Interface;
using NG3.Addin.Model.Domain;
using NG3.Addin.Model.Domain.BusinessModel;
using Enterprise3.Common.Base.Criterion;
using NG3.Addin.Model.Enums;
using NG3.Addin.Core.Extend;

namespace NG3.Addin.Facade
{
	/// <summary>
	/// ExtendFunctionMst业务组装处理类
	/// </summary>
    public partial class ExtendFunctionMstFacade : EntFacadeBase<ExtendFunctionMstModel>, IExtendFunctionMstFacade
    {

        private IAddinAssemblyRule assemblyRule;
        private IAddinSqlRule sqlRule;
        private ExtendExecutor executor;

        #region 类变量及属性
        /// <summary>
        /// ExtendFunctionMst业务逻辑处理对象
        /// </summary>
        IExtendFunctionMstRule ExtendFunctionMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentRule as IExtendFunctionMstRule;
            }
        }
       
        #endregion

        #region 重载方法
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<ExtendFunctionMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<ExtendFunctionMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<ExtendFunctionMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<ExtendFunctionMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
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
        public override PagedResult<ExtendFunctionMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<ExtendFunctionMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<ExtendFunctionMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<ExtendFunctionMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

    
       

        #endregion

        #region 实现 IExtendFunctionMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<ExtendFunctionMstModel> ExampleMethod<ExtendFunctionMstModel>(string param)
        //{
        //    //编写代码
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult DeleteExtendFunc(long id)
        {
            var result = ExtendFunctionMstRule.Delete<Int64>(id);

            assemblyRule.DeleteByForeignKey<Int64>(id);
            sqlRule.DeleteByForeignKey<Int64>(id);


            DeletedResult dresult = new DeletedResult();
            dresult.DelRows = result;
            dresult.Status = "success"; //返回状态

            //刷新缓存配置
            executor.RefreshConfigure();

            return dresult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinAssemblyModel> FindAddinAssemblyByMstPhid(long id)
        {
            var dic = new Dictionary<string, object>();

            new CreateCriteria(dic)
                .Add(ORMRestrictions<Int64>.Eq("MstPhid", id))
                .Add(ORMRestrictions<Int32>.Eq("AssemblyCatalog", (int)EnumCatalog.Extend));

            var assemblyModels = assemblyRule.Find<AddinAssemblyModel>(dic, new string[] { "Phid asc" });
            return assemblyModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinSqlModel> FindAddinSqlByMstPhid(long id)
        {
            var dic = new Dictionary<string, object>();

            new CreateCriteria(dic)
                .Add(ORMRestrictions<Int64>.Eq("MstPhid", id))
                .Add(ORMRestrictions<Int32>.Eq("SqlCatalog", (int)EnumCatalog.Extend));


            var sqlModels = sqlRule.Find<AddinSqlModel>(dic, new string[] { "Phid asc" });
            return sqlModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<ExtendFuncUrlBizModel> GetUrl(long id)
        {
            IList<ExtendFuncUrlBizModel> datas = new List<ExtendFuncUrlBizModel>();
            var data = ExtendFunctionMstRule.Find<Int64>(id);

            datas.Add(new ExtendFuncUrlBizModel { Phid = data.Phid, Url = data.Url });


            return datas;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mstModels"></param>
        /// <param name="sqlModels"></param>
        /// <param name="assemblyModels"></param>
        /// <returns></returns>
        public SavedResult<long> SaveExtendFunc(List<ExtendFunctionMstModel> mstModels, List<AddinSqlModel> sqlModels, List<AddinAssemblyModel> assemblyModels)
        {
            var result = ExtendFunctionMstRule.Save<Int64>(mstModels);

            sqlModels.ForEach(data => {
                data.MstPhid = result.KeyCodes[0];
                data.SqlCatalog = Model.Enums.EnumCatalog.Extend;
            });

            sqlRule.Save<Int64>(sqlModels);

            assemblyModels.ForEach(data => {
                data.MstPhid = result.KeyCodes[0];
                data.AssemblyCatalog = Model.Enums.EnumCatalog.Extend;
            });

            assemblyRule.Save<Int64>(assemblyModels);

            //刷新缓存配置
            executor.RefreshConfigure();

            return result;
        }
        #endregion
    }
}

