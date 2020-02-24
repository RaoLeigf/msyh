#region Summary
/**************************************************************************************
    * 类 名 称：        MethodAroundMstFacade
    * 命名空间：        NG3.Addin.Facade
    * 文 件 名：        MethodAroundMstFacade.cs
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
using NG3.Addin.Core;
using NG3.Addin.Core.Interceptor;
using Enterprise3.Common.Model;
using Enterprise3.Common.Model.Enums;

namespace NG3.Addin.Facade
{
	/// <summary>
	/// MethodAroundMst业务组装处理类
	/// </summary>
    public partial class MethodAroundMstFacade : EntFacadeBase<MethodAroundMstModel>, IMethodAroundMstFacade
    {

        private IAddinAssemblyRule AssemblyRule;
        private IAddinExpressionRule ExpRule;
        private IAddinExpressionVarRule ExpVarRule;
        private IAddinSqlRule SqlRule;
        private IAddinServiceRule serviceRule; //服务的ruler

        private InterceptorExecutor executor;
        #region 类变量及属性
        /// <summary>
        /// MethodAroundMst业务逻辑处理对象
        /// </summary>
        IMethodAroundMstRule MethodAroundMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("BaseLanguage", "InitializeObjectFail");

                return CurrentRule as IMethodAroundMstRule;
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
        public override PagedResult<MethodAroundMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<MethodAroundMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			helpdac.CodeToName<MethodAroundMstModel>(pageResult.Results, "TargetServiceId", "TargetServiceName", "addinservice", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValType"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public override FindedResult<MethodAroundMstModel> Find<TValType>(TValType id)
        {

            var result =  base.Find<TValType>(id);

            RichHelpDac helpdac = new RichHelpDac();

            helpdac.CodeToName<MethodAroundMstModel>(result.Data, "TargetServiceId", "TargetServiceName", "addinservice");

            return result;
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
        public override PagedResult<MethodAroundMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<MethodAroundMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<MethodAroundMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<MethodAroundMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

    
        #endregion
       




        #region 实现 IMethodAroundMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public List<MethodAroundMstModel> ExampleMethod<MethodAroundMstModel>(string param)
        //{
        //    //编写代码
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeletedResult DeleteMethodAround(long id)
        {
            var result = MethodAroundMstRule.Delete<Int64>(id);

            AssemblyRule.DeleteByForeignKey<Int64>(id);
            SqlRule.DeleteByForeignKey<Int64>(id);
            ExpRule.DeleteByForeignKey<Int64>(id);
            ExpVarRule.DeleteByForeignKey<Int64>(id);

            DeletedResult dresult = new DeletedResult();
            dresult.DelRows = result;
            dresult.Status = "success"; //返回状态

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
                .Add(ORMRestrictions<Int32>.Eq("AssemblyCatalog", (int)EnumCatalog.Interceptor));

            var assemblyModels = AssemblyRule.Find<AddinAssemblyModel>(dic,new string[] { "Phid asc"});
            return assemblyModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinExpressionModel> FindAddinExpressionByMstPhid(long id)
        {          
            var ExpModels = ExpRule.FindByForeignKey<Int64>(id);
            return ExpModels;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AddinExpressionVarModel> FindAddinExpressionVarByMstPhid(long id)
        {
            var ExpVarModels = ExpVarRule.FindByForeignKey<Int64>(id);
            return ExpVarModels;
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
                .Add(ORMRestrictions<Int32>.Eq("SqlCatalog", (int)EnumCatalog.Interceptor));


            var sqlModels = SqlRule.Find<AddinSqlModel>(dic,new string[] {"Phid asc"});
            return sqlModels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FindedResult<MethodAroundMstModel> GetMethodAroundMst(long id)
        {

            var methodModel = MethodAroundMstRule.Find<Int64>(id);
            if(methodModel!=null)
            {
                var serviceId = methodModel.TargetServiceId;
                var serviceModel = serviceRule.Find<Int64>(serviceId);
                if(serviceModel!=null)
                {
                    methodModel.TargetAssemblyName = serviceModel.TargetAssemblyName;
                    methodModel.TargetClassName = serviceModel.TargetClassName;
                    methodModel.TargetMethodName = serviceModel.TargetMethodName;
                    methodModel.TargetServiceName = serviceModel.ServiceName;
                }
            }
            //返回的结果集
            var result = new FindedResult<MethodAroundMstModel>();
            result.Data = methodModel;
            result.Status = "success";

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mstModels"></param>
        /// <param name="sqlModels"></param>
        /// <param name="varModels"></param>
        /// <param name="expModels"></param>
        /// <param name="assemblyModels"></param>
        /// <returns></returns>
        public SavedResult<long> SaveMethodAround(List<MethodAroundMstModel> mstModels, List<AddinSqlModel> sqlModels, List<AddinExpressionVarModel> varModels, List<AddinExpressionModel> expModels, List<AddinAssemblyModel> assemblyModels)
        {

            //返回主表的
            var result = MethodAroundMstRule.Save<Int64>(mstModels);

            expModels.ForEach(data => {
                data.MstPhid = result.KeyCodes[0];
            });
            ExpRule.Save<Int64>(expModels);

            sqlModels.ForEach(data => {
                data.MstPhid = result.KeyCodes[0];
                data.SqlCatalog = Model.Enums.EnumCatalog.Interceptor;
            });
            SqlRule.Save<Int64>(sqlModels);

            varModels.ForEach(data => { data.MstPhid = result.KeyCodes[0]; });
            ExpVarRule.Save<Int64>(varModels);


            assemblyModels.ForEach(data => {
                data.MstPhid = result.KeyCodes[0];
                data.AssemblyCatalog = Model.Enums.EnumCatalog.Interceptor;
            });
            AssemblyRule.Save<Int64>(assemblyModels);

            //重新加载配置
            executor.ReloadConfigure();

            return result;
        }

        /// <summary>
        /// 系统表达式当前支持的函数
        /// </summary>
        /// <returns></returns>
        public IList<SupportFunctionBizModel> GetSupportFunctions()
        {
            return ExpressionUtils.GetSupportFunctions();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeployConfigure(long id)
        {
            var pmaps = new List<PropertyColumnMapperInfo>();
            pmaps.Add(new PropertyColumnMapperInfo
            {
                PropertyName = "Phid",
                Value = id
            });
            pmaps.Add(new PropertyColumnMapperInfo
            {
                PropertyName = "DeployFlag",
                Value = 1
            });

            var data = MethodAroundMstRule.Update(pmaps);
            return executor.DeployConfigure(id);
        }
        /// 0<summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UndeployConfigure(long id)
        {
            var pmaps = new List<PropertyColumnMapperInfo>();
            pmaps.Add(new PropertyColumnMapperInfo
            {
                PropertyName = "Phid",
                Value = id
            });
            pmaps.Add(new PropertyColumnMapperInfo
            {
                PropertyName = "DeployFlag",
                Value = 0
            });          
            var data = MethodAroundMstRule.Update(pmaps);
           
            return executor.UnDeployConfigure(id);
        }

        /// <summary>
        /// 获取功能注入列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public PagedResult<MethodAroundMstModel> GetMethodAroundMstList(int pageIndex, int pageSize, Dictionary<string, object> dic)
        {
            var data = MethodAroundMstRule.LoadWithPage(pageIndex, pageSize, dic);

            if(data.Results!= null)
            {
                data.Results.ToList().ForEach(p => {
                    var serviceId = p.TargetServiceId;
                    var serviceModel = serviceRule.Find<Int64>(serviceId);
                    if (serviceModel != null)
                    {
                        p.TargetAssemblyName = serviceModel.TargetAssemblyName;
                        p.TargetClassName = serviceModel.TargetClassName;
                        p.TargetMethodName = serviceModel.TargetMethodName;
                        p.TargetServiceName = serviceModel.ServiceName;
                    }

                });
            }
            return data;
        }

        #endregion
    }
}

