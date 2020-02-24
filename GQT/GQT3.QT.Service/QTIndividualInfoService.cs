#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service
    * 类 名 称：			QTIndividualInfoService
    * 文 件 名：			QTIndividualInfoService.cs
    * 创建时间：			2019/5/14 
    * 作    者：			董泉伟    
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

namespace GQT3.QT.Service
{
	/// <summary>
	/// QTIndividualInfo服务组装处理类
	/// </summary>
    public partial class QTIndividualInfoService : EntServiceBase<QTIndividualInfoModel>, IQTIndividualInfoService
    {
		#region 类变量及属性
		/// <summary>
        /// QTIndividualInfo业务外观处理对象
        /// </summary>
		IQTIndividualInfoFacade QTIndividualInfoFacade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IQTIndividualInfoFacade;
            }
        }
        #endregion

        #region 实现 IQTIndividualInfoService 业务添加的成员

        private IOrganizationFacade OrganizationFacade { get; set; }

        #endregion

        #region  绑定自定义表单跟金额设置关系
        /// <summary>
        /// 保存自定义表单跟金额关联设置
        /// </summary>
        /// <param name="templePhid"></param>
        /// <param name="phid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public int SaveTemple(long templePhid, string bustype, long phid)
        {
            int ret = QTIndividualInfoFacade.SaveTemple(templePhid, bustype,phid);
            return ret;
        }

        /// <summary>
        /// 删除自定义表单跟金额关联设置
        /// individualui_phid：自定义界面方案主键（传0的时候，执行删除；已存在相同业务类型和业务基础数据主。键的时候执行修改）
        ///buscode：业务类型
        /// basedata：业务基础数据主键（如：合同类型，供应商类型）
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public int DeleteTemple(string bustype, long phid)
        {
            int ret = QTIndividualInfoFacade.DeleteTemple(bustype,phid);
            return ret;
        }
        /// <summary>
        /// 根据组织代码串得到组织
        /// </summary>
        /// <param name="OrgStr"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetUseOrg(string OrgStr)
        {
            if (!string.IsNullOrEmpty(OrgStr))
            {
                var OrgCodeList = OrgStr.Split(',').ToList();
                var dicWhere = new Dictionary<string, object>();
                new CreateCriteria(dicWhere)
                    .Add(ORMRestrictions<List<string>>.In("OCode", OrgCodeList));
                var result = OrganizationFacade.Find(dicWhere, new string[] { "OCode Asc" }).Data;
                return result;
            }
            else
            {
                return new List<OrganizeModel>();
            }
        }

        /// <summary>
        /// 根据组织代码串得到组织(非)
        /// </summary>
        /// <param name="OrgStr"></param>
        /// <returns></returns>
        public IList<OrganizeModel> GetNoUseOrg(string OrgStr)
        {
            var dicWhere = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(OrgStr))
            {
                var OrgCodeList = OrgStr.Split(',').ToList();
                new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<List<string>>.NotIn("OCode", OrgCodeList));
            }
            new CreateCriteria(dicWhere)
               .Add(ORMRestrictions<string>.Eq("IfCorp", "Y"));
            var result = OrganizationFacade.Find(dicWhere, new string[] { "OCode Asc" }).Data;
            return result;
          
        }
        #endregion
    }
}

