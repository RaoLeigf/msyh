#region Summary
/**************************************************************************************
    * 类 名 称：        ExamplesService
    * 命名空间：        GYS3.YS.Service
    * 文 件 名：        ExamplesService.cs
    * 创建时间：        2018/8/17 9:53:13 
    * 作    者：        丰立新    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Service;

using GYS3.YS.Model;
using GYS3.YS.Facade.Interface;
using GYS3.YS.Service.Interface;

namespace GYS3.YS.Service
{
    /// <summary>
    /// Examples服务组装处理类
    /// </summary>
    public partial class ExamplesService : EntServiceBase<ExamplesModel>, IExamplesService
    {
        IExamplesFacade ExamplesFacad
        {
            get
            {
                if (CurrentFacade == null)
                    throw new ArgumentNullException(Resources.InitializeObject);

                return CurrentFacade as IExamplesFacade;
            }
        }

        #region 重载方法

        #endregion

        #region 实现 IExamplesFacade 业务添加的成员

        /// <summary>
        /// 获取所有有效的组织信息
        /// </summary>
        /// <returns>组织集合</returns>
        public IList<ExamplesModel> GetAllEffectiveOrgs()
        {
            return ExamplesFacad.GetAllEffectiveOrgs();
        }

        /// <summary>
        /// 获取指定组织所有有效的部门信息
        /// </summary>
        /// <param name="ocode">组织编码（目前通过编码设置关系）</param>
        /// <returns></returns>
        public IList<ExamplesModel> GetAllEffectiveDepts(string ocode)
        {
            return ExamplesFacad.GetAllEffectiveDepts(ocode);
        }

        #endregion
    }
}