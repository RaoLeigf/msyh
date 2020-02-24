#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QTMemoService
    * 文 件 名：			QTMemoService.cs
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
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;
using SUP.Common.Base;
using System.Collections;
using System.Data;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QTMemo服务组装处理类
	/// </summary>
    public partial class QTMemoService : EntServiceBase<QTMemoModel>, IQTMemoService
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

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="adddata"></param>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        public CommonResult Save2(List<QTMemoModel> adddata, List<QTMemoModel> updatedata, List<string> deletedata)
        {
            CommonResult result = new CommonResult();
            List<QTMemoModel> data = new List<QTMemoModel>();
            if (adddata != null && adddata.Count > 0)
            {
                for (var i = 0; i < adddata.Count; i++)
                {
                    QTMemoModel qTMemo = adddata[i];
                    if (string.IsNullOrEmpty(qTMemo.MenoStatus))
                    {
                        qTMemo.MenoStatus = "0";
                    }
                    qTMemo.PersistentState = PersistentState.Added;
                    data.Add(qTMemo);
                    //base.Save<Int64>(qTMemo, "");
                }

            }
            if (updatedata != null && updatedata.Count > 0)
            {
                for (var j = 0; j < updatedata.Count; j++)
                {
                    QTMemoModel qTMemo2 = updatedata[j];
                    QTMemoModel qTMemo3 = base.Find(qTMemo2.PhId).Data;
                    qTMemo3.MenoStatus = qTMemo2.MenoStatus;
                    qTMemo3.MenoName = qTMemo2.MenoName;
                    qTMemo3.MenoRemind = qTMemo2.MenoRemind;
                    qTMemo3.BZ = qTMemo2.BZ;
                    qTMemo3.PersistentState = PersistentState.Modified;
                    data.Add(qTMemo3);
                    //base.Save<Int64>(qTMemo3, "");
                }

            }
            base.Save<Int64>(data, "");
            if (deletedata != null && deletedata.Count > 0)
            {
                for (var x = 0; x < deletedata.Count; x++)
                {
                    base.Delete(deletedata[x]);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取页面上的按钮信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="orgid">组织id</param>
        /// <param name="userType">框架类型：System，</param>
        /// <param name="rightname">页面权限标识</param>
        /// <returns></returns>
        public Hashtable GetFormRights(Int64 userid, Int64 orgid, string userType, string rightname)
        {
          return  this.QTMemoFacade.GetFormRights(userid, orgid, userType, rightname);
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
        public DataTable GetLoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, bool rightFlag, bool lazyLoadFlag, string treeFilter)
        {
            return this.QTMemoFacade.GetLoadMenu(product, suite, isusbuser, usertype, orgID, userID, nodeid, rightFlag, lazyLoadFlag, treeFilter);

        }
        #endregion
    }
}

