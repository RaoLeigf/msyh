using System;
using System.Collections.Generic;
using System.Linq;
using SUP.Common.DataEntity;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using SUP.Common.Base;

namespace GQT3.QT.Service
{
    /// <summary>
    /// 系统设置处理
    /// </summary>
    public partial class SysSettingService : EntServiceBase<QTMemoModel>, ISysSettingService
    {
        #region 类变量及属性
        /// <summary>
        /// QTMemo业务外观处理对象
        /// </summary>
        IQTMemoFacade QTMemoFacade
        {
            get
            {
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQTMemoFacade;
            }
        }
        #endregion

        #region 实现 IQTMemoService 业务添加的成员

        public void GetFormRights(string userid, string orgid)
        {
            //this.QTMemoFacade.GetFormRights(userid, orgid);
        }

        /// <summary>
        ///加载菜单权限
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
        public IList<TreeJSONBase> GetLoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter)
        {
            //return this.QTMemoFacade.GetLoadMenu(product, suite, isusbuser, usertype, orgID, userID, nodeid, rightFlag, lazyLoadFlag, treeFilter);
            return null;
        }

        #endregion
    }
}
