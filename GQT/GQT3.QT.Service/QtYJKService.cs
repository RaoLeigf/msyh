#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtYJKService
    * 文 件 名：			QtYJKService.cs
    * 创建时间：			2019/4/15 
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
	/// QtYJK服务组装处理类
	/// </summary>
    public partial class QtYJKService : EntServiceBase<QtYJKModel>, IQtYJKService
    {
		#region 类变量及属性
		/// <summary>
        /// QtYJK业务外观处理对象
        /// </summary>
		IQtYJKFacade QtYJKFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtYJKFacade;
            }
        }
		#endregion

		#region 实现 IQtYJKService 业务添加的成员

        /// <summary>
        /// 更新意见库
        /// </summary>
        /// <param name="DeleteYJPhids"></param>
        /// <param name="Changedatas"></param>
        /// <param name="Insertdatas"></param>
        /// <returns></returns>
        public SavedResult<Int64> Update1(List<long> DeleteYJPhids, List<QtYJKModel> Changedatas, List<QtYJKModel> Insertdatas)
        {
            SavedResult<Int64> savedresult = new SavedResult<Int64>();
            List<QtYJKModel> qtYJKs = new List<QtYJKModel>();
            if (Changedatas.Count > 0)
            {
                for (var i = 0; i < Changedatas.Count; i++)
                {
                    QtYJKModel qtYJK = QtYJKFacade.Find(Changedatas[i].PhId).Data;
                    qtYJK.Text = Changedatas[i].Text;
                    qtYJK.PersistentState = PersistentState.Modified;
                    qtYJKs.Add(qtYJK);
                }
            }
            if (Insertdatas.Count > 0)
            {
                for (var j = 0; j < Insertdatas.Count; j++)
                {
                    Insertdatas[j].PersistentState = PersistentState.Added;
                    qtYJKs.Add(Insertdatas[j]);
                }
            }
            if (DeleteYJPhids.Count > 0)
            {
                for (var x = 0; x < DeleteYJPhids.Count; x++)
                {
                    QtYJKFacade.FacadeHelper.Delete(DeleteYJPhids[x]);
                }
            }
            savedresult = QtYJKFacade.Save<Int64>(qtYJKs,"");
            return savedresult;
        }

        #endregion
    }
}

