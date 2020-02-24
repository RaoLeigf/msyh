#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtVenueService
    * 文 件 名：			QtVenueService.cs
    * 创建时间：			2019/11/27 
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

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtVenue服务组装处理类
	/// </summary>
    public partial class QtVenueService : EntServiceBase<QtVenueModel>, IQtVenueService
    {
		#region 类变量及属性
		/// <summary>
        /// QtVenue业务外观处理对象
        /// </summary>
		IQtVenueFacade QtVenueFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtVenueFacade;
            }
        }
        #endregion

        #region 实现 IQtVenueService 业务添加的成员
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="updatedata"></param>
        /// <param name="deletedata"></param>
        /// <returns></returns>
        public SavedResult<Int64> Save2(List<QtVenueModel> updatedata, List<string> deletedata)
        {
            List<QtVenueModel> data = new List<QtVenueModel>();
            if (updatedata.Count > 0)
            {
                for(var i=0;i< updatedata.Count; i++)
                {
                    if (updatedata[i].PhId == 0)
                    {
                        updatedata[i].PersistentState = PersistentState.Added;
                        data.Add(updatedata[i]);
                    }
                    else
                    {
                        var Model = QtVenueFacade.Find(updatedata[i].PhId).Data;
                        Model.Dm = updatedata[i].Dm;
                        Model.Mc = updatedata[i].Mc;
                        Model.Bz = updatedata[i].Bz;
                        Model.PersistentState = PersistentState.Modified;
                        data.Add(Model);
                    }
                    
                }
            }
            if (deletedata.Count > 0)
            {
                for(var j=0;j< deletedata.Count; j++)
                {
                    QtVenueModel delete = QtVenueFacade.Find(long.Parse(deletedata[j])).Data;
                    delete.PersistentState = PersistentState.Deleted;
                    data.Add(delete);
                }
            }
            var result = QtVenueFacade.Save<Int64>(data, "");
            return result;
        }

        #endregion
    }
}

