#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade
    * 类 名 称：			QTMemoFacade
    * 文 件 名：			QTMemoFacade.cs
    * 创建时间：			2019/5/15 
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using SUP.Frame.Rule;
using SUP.Common.Base;
using System.Collections;
using System.Data;

namespace GQT3.QT.Facade
{
	/// <summary>
	/// QTMemo业务组装处理类
	/// </summary>
    public partial class QTMemoFacade : EntFacadeBase<QTMemoModel>, IQTMemoFacade
    {
		#region 类变量及属性
		/// <summary>
        /// QTMemo业务逻辑处理对象
        /// </summary>
		IQTMemoRule QTMemoRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IQTMemoRule;
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
        public override PagedResult<QTMemoModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<QTMemoModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<QTMemoModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<QTMemoModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
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
        public override PagedResult<QTMemoModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<QTMemoModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<QTMemoModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<QTMemoModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            #endregion

            return pageResult;
        }

        #endregion

        #region 实现 IQTMemoFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<QTMemoModel> ExampleMethod<QTMemoModel>(string param)
        //{
        //    //编写代码
        //}


        /// <summary>
        /// 获取页面上的按钮信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="orgid">组织id</param>
        /// <param name="userType">框架类型：System，</param>
        /// <param name="rightname">页面权限标识</param>
        /// <returns></returns>
        public Hashtable GetFormRights(Int64 userid, Int64 orgid,string userType,string rightname)
        {
            var funcRight = new Enterprise3.Rights.AnalyticEngine.FuncRightControl();
            //获取窗体的功能权限（目前没有考虑硬件加密）
            // <param name="uCode">账套号</param>
            // <param name="orgId">组织Id</param>
            // <param name="userId">用户Id</param>
            // <param name="userType">用户类型（SYSTEM为系统管理员）</param>
            // <param name="rightname">页面权限标识</param>
            // <param name="funcName">功能权限名</param>
            // var ht = funcRight.GetFormRights(string.Empty, orgid, userid, userType, rightname, string.Empty);

            //DataTable dt = funcRight.GetFunc(rightname); //获取当前页面中的所有功能权限

            //var dtRights = funcRight.GetUserInOrgRights(orgid, userid, string.Empty);

            //Console.WriteLine(ht.Count.ToString());

            //return ht;


            var ht = new Hashtable();

            DataTable dt = funcRight.GetFunc(rightname); //获取当前页面中的所有功能权限


            if (dt == null) return ht;

            userType = string.IsNullOrWhiteSpace(userType) ? "" : userType;

            var dtRights = funcRight.GetUserInOrgRights(orgid, userid, string.Empty);

            if (dtRights == null)
            {

                return ht;
            }

            foreach (DataRow dr in dt.Rows)
            {

                ht[dr["buttonname"].ToString()] = funcRight.HasRightEx(orgid, userid, Convert.ToInt64(dr["rightkey"]), dtRights, false);
            }

            return ht;

        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="product">产品标识</param>
        /// <param name="suite">模块标识:GXM</param>
        /// <param name="isusbuser"></param>
        /// <param name="usertype">框架类型：System，</param>
        /// <param name="orgID">组织id</param>
        /// <param name="userID">用户id</param>
        /// <param name="nodeid">节点ID,懒加载使用:root</param>
        /// <param name="rightFlag">是否控制权限的开关</param>
        /// <param name="lazyLoadFlag">是否懒加载的开关</param>
        /// <param name="treeFilter">按指定SQL语句构建系统功能树</param>
        /// <returns></returns>
        public DataTable GetLoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter)
        {
            MainTreeRule rule = new MainTreeRule();
            //return rule.LoadMenu(product, suite, isusbuser, usertype, orgID, userID, nodeid, rightFlag, lazyLoadFlag, treeFilter);
            //string filter = string.Empty;

            //是否懒加载，懒加载只获取前两层数据
            if (lazyLoadFlag)
            {

                return rule.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.LazyLevel, rightFlag, treeFilter);

            }
            else
            {
                return rule.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.TopLevel, rightFlag, treeFilter);
            }

        }
        #endregion
    }
}

