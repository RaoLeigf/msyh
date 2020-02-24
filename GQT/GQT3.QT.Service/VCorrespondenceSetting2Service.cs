#region Summary
/**************************************************************************************
    * 类 名 称：        VCorrespondenceSetting2Service
    * 命名空间：        GQT3.QT.Service
    * 文 件 名：        VCorrespondenceSetting2Service.cs
    * 创建时间：        2018/9/13 
    * 作    者：        李长敏琛    
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
using Enterprise3.NHORM.Dac;
using SUP.Common.Base;
using GQT3.QT.Facade;
using Enterprise3.Common.Base.Criterion;

namespace GQT3.QT.Service
{
	/// <summary>
	/// VCorrespondenceSetting2服务组装处理类
	/// </summary>
    public partial class VCorrespondenceSetting2Service : EntServiceBase<VCorrespondenceSetting2Model>, IVCorrespondenceSetting2Service
    {
		#region 类变量及属性
		/// <summary>
        /// VCorrespondenceSetting2业务外观处理对象
        /// </summary>
		IVCorrespondenceSetting2Facade VCorrespondenceSetting2Facade
        {
            get
            {          
                if (CurrentFacade == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentFacade as IVCorrespondenceSetting2Facade;
            }
        }
        #endregion
        IOrganizationFacade Organization
        {
            get;set;
        }
        #region 实现 IVCorrespondenceSetting2Service 业务添加的成员

        public List<OrganizeModel> GetOrange()
        {
            IOrganizationFacade OrganizationFacade =this.Organization;
            //var t = new EntDacBase<OrganizeModel>();
            //var t = new OrganizationDac();
            //Dictionary<string, object> dicWhere = DataConverterHelper.ConvertToDic(null);
            //CreateCriteria createCriteria = new CreateCriteria(dicWhere);
            //createCriteria.Add(ORMRestrictions<string>.Eq("OrgType", "Y"));
            //List<OrganizeModel> oli = t.Find(dicWhere) as List<OrganizeModel>;
            //var result=new EntDacBase<OrganizeModel>().LoadWithPage(0, Int32.MaxValue, DataConverterHelper.ConvertToDic(null));
            var result = OrganizationFacade.LoadWithPage(0, Int32.MaxValue, DataConverterHelper.ConvertToDic(null));
            //return new EntDacBase<OrganizeModel>().LoadWithPage(0, Int32.MaxValue, DataConverterHelper.ConvertToDic(null)).Results as List<OrganizeModel>;
            return null;
        }
        #endregion
    }
}

