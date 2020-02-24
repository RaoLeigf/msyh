#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QTControlSetService
    * 文 件 名：			QTControlSetService.cs
    * 创建时间：			2019/4/3 
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
	/// QTControlSet服务组装处理类
	/// </summary>
    public partial class QTControlSetService : EntServiceBase<QTControlSetModel>, IQTControlSetService
    {
		#region 类变量及属性
		/// <summary>
        /// QTControlSet业务外观处理对象
        /// </summary>
		IQTControlSetFacade QTControlSetFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQTControlSetFacade;
            }
        }
        private ICorrespondenceSettings2Facade CorrespondenceSettings2Facade { get; set; }
        #endregion

        #region 实现 IQTControlSetService 业务添加的成员

        /// <summary>
        /// 根据主键更新数据
        /// </summary>
        /// <param name="SetPhId"></param>
        /// <returns></returns>
        public SavedResult<long> UpdateOrg(string SetPhId)
        {
            string ControlOrgName = "";
            var result = new SavedResult<long>();
            var dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere).
                            Add(ORMRestrictions<string>.Eq("Dylx", "button")).
                            Add(ORMRestrictions<string>.Eq("Dwdm", SetPhId));
            IList<CorrespondenceSettings2Model> correspondenceSettings2 =CorrespondenceSettings2Facade.Find(dicWhere).Data;

            if (correspondenceSettings2.Count > 0)
            {
                for (var i = 0; i < correspondenceSettings2.Count; i++)
                {
                    ControlOrgName = ControlOrgName + correspondenceSettings2[i].DefStr2 + ";";
                }
            }
            QTControlSetModel qTControlSet = base.Find(long.Parse(SetPhId)).Data;
            qTControlSet.ControlOrgName = ControlOrgName;
            qTControlSet.PersistentState = PersistentState.Modified;
            result=base.Save<long>(qTControlSet,"");
            return result;
        }

        #endregion
    }
}

