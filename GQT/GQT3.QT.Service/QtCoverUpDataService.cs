#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtCoverUpDataService
    * 文 件 名：			QtCoverUpDataService.cs
    * 创建时间：			2019/10/29 
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
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GQT3.QT.Service.Interface;
using GQT3.QT.Facade.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtCoverUpData服务组装处理类
	/// </summary>
    public partial class QtCoverUpDataService : EntServiceBase<QtCoverUpDataModel>, IQtCoverUpDataService
    {
		#region 类变量及属性
		/// <summary>
        /// QtCoverUpData业务外观处理对象
        /// </summary>
		IQtCoverUpDataFacade QtCoverUpDataFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtCoverUpDataFacade;
            }
        }
		#endregion

		#region 实现 IQtCoverUpDataService 业务添加的成员

        /// <summary>
        /// 获取内置的启用的内置套打格式数据
        /// </summary>
        /// <returns></returns>
        public IList<QtCoverUpDataModel> GetQtCoverUpDataList()
        {
            //IList<QtCoverUpDataModel> qtCoverUpDatas = new List<QtCoverUpDataModel>();
            //qtCoverUpDatas = this.QtCoverUpDataFacade.Find(t => t.IsSystem == (byte)1 && t.EnabledMark == (byte)0).Data;
            //if(qtCoverUpDatas == null)
            //{
            //    throw new Exception("套打格式的内置数据为空，请联系管理员！");
            //}
            return this.QtCoverUpDataFacade.GetQtCoverUpDataList();
        }
        #endregion
    }
}

