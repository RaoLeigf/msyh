#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtTableCustomizeService
    * 文 件 名：			QtTableCustomizeService.cs
    * 创建时间：			2019/11/26 
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
	/// QtTableCustomize服务组装处理类
	/// </summary>
    public partial class QtTableCustomizeService : EntServiceBase<QtTableCustomizeModel>, IQtTableCustomizeService
    {
		#region 类变量及属性
		/// <summary>
        /// QtTableCustomize业务外观处理对象
        /// </summary>
		IQtTableCustomizeFacade QtTableCustomizeFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtTableCustomizeFacade;
            }
        }
		#endregion

		#region 实现 IQtTableCustomizeService 业务添加的成员

        /// <summary>
        /// 修改列表自定义集合
        /// </summary>
        /// <param name="qtTableCustomizes">列表自定义集合</param>
        /// <returns></returns>
        public SavedResult<long> UpdateQtTableCustomizes(List<QtTableCustomizeModel> qtTableCustomizes)
        {
            SavedResult<long> savedResult = new SavedResult<long>();
            savedResult = this.QtTableCustomizeFacade.Save<long>(qtTableCustomizes);
            return savedResult;
        }

        /// <summary>
        /// 根据用户与表编码获取该表格所有列是否显示的数据
        /// </summary>
        /// <param name="uid">用户账号</param>
        /// <param name="tableCode">表格编码</param>
        /// <returns></returns>
        public IList<QtTableCustomizeModel> GetQtTableCustomizes(long uid, string tableCode)
        {
            IList<QtTableCustomizeModel> qtTableCustomizes = new List<QtTableCustomizeModel>();
            qtTableCustomizes = this.QtTableCustomizeFacade.Find(t => t.TableCode == tableCode).Data.OrderBy(t => t.ColumnSort).ToList();
            return qtTableCustomizes;
        }

        #endregion
    }
}

