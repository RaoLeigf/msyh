#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QtXmDistributeService
    * 文 件 名：			QtXmDistributeService.cs
    * 创建时间：			2020/1/6 
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
using Enterprise3.Common.Base.Criterion;
using SUP.Common.Base;

namespace GQT3.QT.Service
{
	/// <summary>
	/// QtXmDistribute服务组装处理类
	/// </summary>
    public partial class QtXmDistributeService : EntServiceBase<QtXmDistributeModel>, IQtXmDistributeService
    {
		#region 类变量及属性
		/// <summary>
        /// QtXmDistribute业务外观处理对象
        /// </summary>
		IQtXmDistributeFacade QtXmDistributeFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQtXmDistributeFacade;
            }
        }

        IQtSysCodeSeqFacade QtSysCodeSeqFacade;
        #endregion

        #region 实现 IQtXmDistributeService 业务添加的成员


        /// <summary>
        /// 获取最大项目库编码
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string CreateOrGetMaxProjCode(string year)
        {
            string projCode = "";
            QtSysCodeSeqModel seqM = null;

            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("FYear", year))
                .Add(ORMRestrictions<string>.Eq("FCode", "ProjCode")).Add(ORMRestrictions<string>.Eq("FTname", "z_qtxmdistribute"));

            FindedResults<QtSysCodeSeqModel> sysCodeSeqResult = QtSysCodeSeqFacade.Find(dicWhere);
            // FindedResults<QtSysCodeSeqModel> sysCodeSeqResult = QtSysCodeSeqFacade.Find(t => t.FYear == year && t.FCode== "ProjCode" && t.FTname== "xm3_projectmst");

            if (sysCodeSeqResult.Status == ResponseStatus.Success)
            {
                //插入或更新项目代码编码序号                    
                if (sysCodeSeqResult.Data.Count > 0)
                {
                    seqM = sysCodeSeqResult.Data[0];
                    if (string.IsNullOrWhiteSpace(seqM.FSeqNO))
                    {
                        projCode = year + string.Format("{0:D8}", 1);
                    }
                    else
                    {
                        var max = Int64.Parse(seqM.FSeqNO.Substring(4));
                        max = max + 1;
                        projCode = year + string.Format("{0:D8}", max);
                    }

                    seqM.FSeqNO = projCode; //更新代码，访问一次后就加1，后续不还原，一直累加
                    seqM.PersistentState = PersistentState.Modified;
                }
                else
                {
                    //系统编码不存在 
                    projCode = year + string.Format("{0:D8}", 1);

                    seqM = new QtSysCodeSeqModel
                    {
                        FYear = year,
                        FCode = "ProjCode",
                        FName = "项目分发编码序号",
                        FTname = "z_qtxmdistribute",
                        FSeqNO = projCode,
                        PersistentState = SUP.Common.Base.PersistentState.Added
                    };
                }
                SavedResult<Int64> saveResult = QtSysCodeSeqFacade.Save<Int64>(seqM);
            }
            else
            {
                projCode = year + string.Format("{0:D8}", 1);
            }

            return projCode;
        }

        #endregion
    }
}

